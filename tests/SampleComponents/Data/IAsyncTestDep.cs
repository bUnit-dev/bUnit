﻿using System.Threading.Tasks;

namespace Bunit.SampleComponents.Data
{
    public interface IAsyncTestDep
    {
        Task<string> GetData();
    }
}
