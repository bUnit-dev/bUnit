using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunit.SampleComponents.Data
{
    public interface IAsyncTestDep
    {
        Task<string> GetData();
    }
}
