using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egil.RazorComponents.Testing.Library.SampleApp.Data
{
    public class TodoService
    {
        public Task<Todo[]> Get()
        {
            return Task.FromResult(Array.Empty<Todo>());
        }

        public void Add(Todo todo) { }
        public void Update(Todo todo) { }
        public void Delete(Todo todo) { }
    }
}
