using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egil.RazorComponents.Testing.SampleComponents.Data
{
    public class AsyncNameDep : IAsyncTestDep
    {
        private TaskCompletionSource<string> _completionSource = new TaskCompletionSource<string>();

        public Task<string> GetData() => _completionSource.Task;

        public void SetResult(string name)
        {
            var prevSource = _completionSource;
            _completionSource = new TaskCompletionSource<string>();
            prevSource.SetResult(name);
        }
    }
}
