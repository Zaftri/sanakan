﻿#pragma warning disable 1591

using System;
using System.Threading.Tasks;

namespace Sanakan.Services.Executor
{
    public interface IExecutable
    {
        string GetName();
        Task<bool> ExecuteAsync(IServiceProvider provider);
    }
}
