using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp.Data
{
    public class TodoService : ITodoService
    {
        private readonly List<Todo> _todos = new List<Todo>();

        public Task<IReadOnlyList<Todo>> GetAll()
        {
            return Task.FromResult((IReadOnlyList<Todo>)_todos);
        }

        public void Add(Todo todo)
        {
            if(todo is null) throw new ArgumentNullException(nameof(todo));
            todo.Id = _todos.Count;
            _todos.Add(todo);
        }

        public void Complete(int todoId)
        {
            _todos.RemoveAll(x => x.Id == todoId);
        }
    }
}
