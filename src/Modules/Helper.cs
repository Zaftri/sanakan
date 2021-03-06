﻿#pragma warning disable 1591

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Sanakan.Extensions;
using Sanakan.Preconditions;
using Sanakan.Services.Commands;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Sanakan.Modules
{
    [Name("Ogólne")]
    public class Helper : SanakanModuleBase<SocketCommandContext>
    {
        private Services.Helper _helper;

        public Helper(Services.Helper helper)
        {
            _helper = helper;
        }

        [Command("pomoc", RunMode = RunMode.Async)]
        [Alias("h", "help")]
        [Summary("wyświetla listę poleceń")]
        [Remarks("odcinki"), RequireCommandChannel]
        public async Task GiveHelpAsync([Summary("nazwa polecenia(opcjonalne)")][Remainder]string command = null)
        {
            if (command != null)
            {
                try
                {
                    await ReplyAsync(_helper.GiveHelpAboutPublicCmd(command));
                }
                catch (Exception ex)
                {
                    await ReplyAsync("", embed: ex.Message.ToEmbedMessage(EMType.Error).Build());
                }

                return;
            }

            await ReplyAsync(_helper.GivePublicHelp());
        }

        [Command("ktoto", RunMode = RunMode.Async)]
        [Alias("whois")]
        [Summary("wyświetla informacje o użytkowniku")]
        [Remarks("Dzida"), RequireCommandChannel]
        public async Task GiveUserInfoAsync([Summary("nazwa użytkownika(opcjonalne)")]SocketUser user = null)
        {
            var usr = (user ?? Context.User) as SocketGuildUser;
            if (usr == null)
            {
                await ReplyAsync("", embed: "Polecenie działa tylko z poziomu serwera.".ToEmbedMessage(EMType.Info).Build());
                return;
            }

            await ReplyAsync("", embed: _helper.GetInfoAboutUser(usr));
        }

        [Command("ping", RunMode = RunMode.Async)]
        [Summary("sprawdza opóźnienie między botem a serwerem")]
        [Remarks(""), RequireCommandChannel]
        public async Task GivePingAsync()
        {
            int latency = Context.Client.Latency;

            EMType type = EMType.Error;
            if (latency < 400) type = EMType.Warning;
            if (latency < 200) type = EMType.Success;

            await ReplyAsync("", embed: $"Pong! `{latency}ms`".ToEmbedMessage(type).Build());
        }

        [Command("serwerinfo", RunMode = RunMode.Async)]
        [Alias("serverinfo", "sinfo")]
        [Summary("wyświetla informacje o serwerze")]
        [Remarks(""), RequireCommandChannel]
        public async Task GiveServerInfoAsync()
        {
            if (Context.Guild == null)
            {
                await ReplyAsync("", embed: "Polecenie działa tylko z poziomu serwera.".ToEmbedMessage(EMType.Info).Build());
                return;
            }

            await ReplyAsync("", embed: _helper.GetInfoAboutServer(Context.Guild));
        }

        [Command("awatar", RunMode = RunMode.Async)]
        [Alias("avatar", "pfp")]
        [Summary("wyświetla awatar użytkownika")]
        [Remarks("Dzida"), RequireCommandChannel]
        public async Task ShowUserAvatarAsync([Summary("nazwa użytkownika(opcjonalne)")]SocketUser user = null)
        {
            var usr = (user ?? Context.User);
            var embed = new EmbedBuilder
            {
                ImageUrl = usr.GetAvatarUrl() ?? "https://i.imgur.com/xVIMQiB.jpg",
                Author = new EmbedAuthorBuilder().WithUser(usr),
                Color = EMType.Info.Color(),
            };

            await ReplyAsync("", embed: embed.Build());
        }

        [Command("info", RunMode = RunMode.Async)]
        [Summary("wyświetla informacje o bocie")]
        [Remarks(""), RequireCommandChannel]
        public async Task GiveBotInfoAsync()
        {
            using (var proc = System.Diagnostics.Process.GetCurrentProcess())
            {
                string info = $"**Sanakan ({typeof(Sanakan).Assembly.GetName().Version})**:\n"
                    + $"**Czas działania**: `{(DateTime.Now - proc.StartTime).ToString(@"d'd 'hh\:mm\:ss")}`";

                await ReplyAsync(info);
            }
        }

        [Command("zgłoś", RunMode = RunMode.Async)]
        [Alias("raport", "report", "zgłos", "zglos", "zgloś")]
        [Summary("zgłasza wiadomośc użytkownika")]
        [Remarks("63312335634561 Tak nie wolno!"), RequireUserRole]
        public async Task ReportUserAsync([Summary("id wiadomości")]ulong messageId, [Summary("powód")][Remainder]string reason)
        {
            using (var db = new Database.GuildConfigContext(Config))
            {
                var config = await db.GetCachedGuildFullConfigAsync(Context.Guild.Id);
                if (config == null)
                {
                    await ReplyAsync("", embed: "Serwer nie jest jeszcze skonfigurowany.".ToEmbedMessage(EMType.Bot).Build());
                    return;
                }

                var raportCh = Context.Guild.GetTextChannel(config.RaportChannel);
                if (raportCh == null)
                {
                    await ReplyAsync("", embed: "Serwer nie ma skonfigurowanych raportów.".ToEmbedMessage(EMType.Bot).Build());
                    return;
                }

                var repMsg = await Context.Channel.GetMessageAsync(messageId);
                if (repMsg == null) repMsg = await _helper.FindMessageInGuildAsync(Context.Guild, messageId);

                if (repMsg == null)
                {
                    await ReplyAsync("", embed: "Nie odnaleziono wiadomości.".ToEmbedMessage(EMType.Error).Build());
                    return;
                }

                if (repMsg.Author.IsBot || repMsg.Author.IsWebhook)
                {
                    await ReplyAsync("", embed: "Raportować bota? Bezsensu.".ToEmbedMessage(EMType.Bot).Build());
                    return;
                }

                if ((DateTime.Now - repMsg.CreatedAt.DateTime.ToLocalTime()).TotalHours > 3)
                {
                    await ReplyAsync("", embed: "Można raportować tylko wiadomośći, które nie są starsze jak 3h.".ToEmbedMessage(EMType.Bot).Build());
                    return;
                }

                await Context.Message.DeleteAsync();
                await ReplyAsync("", embed: "Wysłano zgłoszenie.".ToEmbedMessage(EMType.Success).Build());

                string userName = $"{Context.User.Username}({Context.User.Id})";
                var sendMsg = await raportCh.SendMessageAsync("", embed: "prep".ToEmbedMessage().Build());
                await sendMsg.ModifyAsync(x => x.Embed = _helper.BuildRaportInfo(repMsg, userName, reason, sendMsg.Id));

                var rConfig = await db.GetGuildConfigOrCreateAsync(Context.Guild.Id);
                rConfig.Raports.Add(new Database.Models.Configuration.Raport { User = repMsg.Author.Id, Message = sendMsg.Id });
                await db.SaveChangesAsync();
            }
        }
    }
}