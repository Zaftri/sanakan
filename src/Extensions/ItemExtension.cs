﻿#pragma warning disable 1591

using System.Collections.Generic;
using Sanakan.Database.Models;

namespace Sanakan.Extensions
{
    public static class ItemExtension
    {
        public static string Desc(this ItemType type)
        {
            switch (type)
            {
                case ItemType.AffectionRecoveryGreat:
                    return "Poprawia relacje z kartą w dużym stopniu.";
                case ItemType.AffectionRecoveryBig:
                    return "Poprawia relacje z kartą w znacznym stopniu.";
                case ItemType.AffectionRecoveryNormal:
                    return "Poprawia relacje z kartą.";
                case ItemType.BetterIncreaseUpgradeCnt:
                    return "Może zwiększyć znacznie liczbę ulepszeń karty, tylko kto by chciał twoją krew?";
                case ItemType.IncreaseUpgradeCnt:
                    return "Dodaje dodatkowy punkt ulepszenia do karty.";
                case ItemType.DereReRoll:
                    return "Pozwala zmienić charakter karty.";
                case ItemType.CardParamsReRoll:
                    return "Pozwala wylosować na nowo parametry karty.";
                case ItemType.RandomBoosterPackSingleE:
                    return "Dodaje nowy pakiet z dwiema losowymi kartami.\n\nWykluczone jakości to: SS, S i A.";
                case ItemType.RandomTitleBoosterPackSingleE:
                    return "Dodaje nowy pakiet z dwiema losowymi, niewymienialnymi kartami z tytułu podanego przez kupującego.\n\nWykluczone jakości to: SS i S.";
                case ItemType.AffectionRecoverySmall:
                    return "Poprawia odrobinę relacje z kartą.";
                case ItemType.RandomNormalBoosterPackB:
                    return "Dodaje nowy pakiet z trzema losowymi kartami, w tym jedną o gwarantowanej jakości B.\n\nWykluczone jakości to: SS.";
                case ItemType.RandomNormalBoosterPackA:
                    return "Dodaje nowy pakiet z trzema losowymi kartami, w tym jedną o gwarantowanej jakości A.\n\nWykluczone jakości to: SS.";
                case ItemType.RandomNormalBoosterPackS:
                    return "Dodaje nowy pakiet z trzema losowymi kartami, w tym jedną o gwarantowanej jakości S.\n\nWykluczone jakości to: SS.";
                case ItemType.RandomNormalBoosterPackSS:
                    return "Dodaje nowy pakiet z trzema losowymi kartami, w tym jedną o gwarantowanej jakości SS.";

                default:
                    return "Brak opisu.";
            }
        }

        public static string Name(this ItemType type)
        {
            switch (type)
            {
                case ItemType.AffectionRecoveryGreat:
                    return "Wielka fontanna czekolady";
                case ItemType.AffectionRecoveryBig:
                    return "Tort czekoladowy";
                case ItemType.AffectionRecoveryNormal:
                    return "Ciasto truskawkowe";
                case ItemType.BetterIncreaseUpgradeCnt:
                    return "Kropla twojej krwi";
                case ItemType.IncreaseUpgradeCnt:
                    return "Pierścionek zaręczynowy";
                case ItemType.DereReRoll:
                    return "Bukiet kwiatów";
                case ItemType.CardParamsReRoll:
                    return "Naszyjnik z diamentem";
                case ItemType.RandomBoosterPackSingleE:
                    return "Tani pakiet losowych kart";
                case ItemType.RandomTitleBoosterPackSingleE:
                    return "Pakiet losowych kart z tytułu";
                case ItemType.AffectionRecoverySmall:
                    return "Banan w czekoladzie";
                case ItemType.RandomNormalBoosterPackB:
                    return "Fioletowy pakiet losowych kart";
                case ItemType.RandomNormalBoosterPackA:
                    return "Pomarańczowy pakiet losowych kart";
                case ItemType.RandomNormalBoosterPackS:
                    return "Złoty pakiet losowych kart";
                case ItemType.RandomNormalBoosterPackSS:
                    return "Różowy pakiet losowych kart";

                default:
                    return "Brak";
            }
        }

        public static bool IsBoosterPack(this ItemType type)
        {
            switch (type)
            {
                case ItemType.RandomBoosterPackSingleE:
                case ItemType.RandomTitleBoosterPackSingleE:
                case ItemType.RandomNormalBoosterPackB:
                case ItemType.RandomNormalBoosterPackA:
                case ItemType.RandomNormalBoosterPackS:
                case ItemType.RandomNormalBoosterPackSS:
                    return true;

                default:
                    return false;
            }
        }

        public static int Count(this ItemType type)
        {
            switch (type)
            {
                case ItemType.RandomNormalBoosterPackB:
                case ItemType.RandomNormalBoosterPackA:
                case ItemType.RandomNormalBoosterPackS:
                case ItemType.RandomNormalBoosterPackSS:
                    return 3;

                default:
                    return 2;
            }
        }

        public static Rarity MinRarity(this ItemType type)
        {
            switch (type)
            {
                case ItemType.RandomNormalBoosterPackSS:
                    return Rarity.SS;

                case ItemType.RandomNormalBoosterPackS:
                    return Rarity.S;

                case ItemType.RandomNormalBoosterPackA:
                    return Rarity.A;

                case ItemType.RandomNormalBoosterPackB:
                    return Rarity.B;

                default:
                    return Rarity.E;
            }
        }

        public static bool IsTradable(this ItemType type)
        {
            switch (type)
            {
                case ItemType.RandomTitleBoosterPackSingleE:
                    return false;

                default:
                    return true;
            }
        }

        public static CardSource GetSource(this ItemType type)
        {
            switch (type)
            {
                case ItemType.RandomBoosterPackSingleE:
                case ItemType.RandomNormalBoosterPackB:
                case ItemType.RandomNormalBoosterPackA:
                case ItemType.RandomNormalBoosterPackS:
                case ItemType.RandomNormalBoosterPackSS:
                case ItemType.RandomTitleBoosterPackSingleE:
                    return CardSource.Shop;

                default:
                    return CardSource.Other;
            }
        }

        public static List<RarityExcluded> RarityExcluded(this ItemType type)
        {
            var ex = new List<RarityExcluded>();

            switch (type)
            {
                case ItemType.RandomTitleBoosterPackSingleE:
                    ex.Add(new RarityExcluded { Rarity = Rarity.SS });
                    ex.Add(new RarityExcluded { Rarity = Rarity.S });
                    break;

                case ItemType.RandomNormalBoosterPackB:
                case ItemType.RandomNormalBoosterPackA:
                case ItemType.RandomNormalBoosterPackS:
                    ex.Add(new RarityExcluded { Rarity = Rarity.SS });
                    break;

                case ItemType.RandomBoosterPackSingleE:
                    ex.Add(new RarityExcluded { Rarity = Rarity.SS });
                    ex.Add(new RarityExcluded { Rarity = Rarity.S });
                    ex.Add(new RarityExcluded { Rarity = Rarity.A });
                    break;

                default:
                    break;
            }

            return ex;
        }

        public static Item ToItem(this ItemType type, long count = 1)
        {
            return new Item
            {
                Name = type.Name(),
                Count = count,
                Type = type,
            };
        }

        public static BoosterPack ToBoosterPack(this ItemType type)
        {
            if (!type.IsBoosterPack())
                return null;

            return new BoosterPack
            {
                Name = type.Name(),
                CardCnt = type.Count(),
                MinRarity = type.MinRarity(),
                CardSourceFromPack = type.GetSource(),
                IsCardFromPackTradable = type.IsTradable(),
                RarityExcludedFromPack = type.RarityExcluded(),
            };
        }
    }
}
