using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleApp.Data
{
    public interface ITodoService
    {
        void Add(Todo todo);
        void Complete(int todoId);
        Task<IReadOnlyList<Todo>> GetAll();
    }
}