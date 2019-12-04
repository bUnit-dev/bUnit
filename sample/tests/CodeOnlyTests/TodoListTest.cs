using Egil.RazorComponents.Testing.Library.SampleApp.Components;
using Egil.RazorComponents.Testing.Library.SampleApp.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Egil.RazorComponents.Testing.Library.SampleApp.CodeOnlyTests
{
    public class TodoListTest : ComponentFixtureBase
    {
        private string GetExpectedHtml(string label = "Task description", string itemsHtml = "") =>
            $@"<form>
                   <div class=""input-group"">
                       <input value="""" type=""text"" class=""form-control"" 
                               placeholder=""{label}"" aria-label=""{label}"" />
                       <div class=""input-group-append"">
                           <button class=""btn btn-secondary"" type=""submit"">Add task</button>
                       </div>
                   </div>
               </form>
               <ol class=""list-group"">{itemsHtml}</ol>";

        [Fact(DisplayName = "Task list renders as empty with default html when no items is provided")]
        public void Test001()
        {
            TestHost.AddMockJsRuntime();
            var cut = TestHost.AddComponent<TodoList>();

            cut.ShouldBe(GetExpectedHtml());
        }

        [Fact(DisplayName = "Task list with custom label renders correctly")]
        public void Test002()
        {
            TestHost.AddMockJsRuntime();
            var label = "hello world";
            var cut = TestHost.AddComponent<TodoList>((nameof(TodoList.Label), label));

            cut.ShouldBe(GetExpectedHtml(label: label));
        }

        [Fact(DisplayName = "Task list renders Items using ItemTemplate")]
        public void Test003()
        {
            // arrange
            TestHost.AddMockJsRuntime();
            var items = new[] { new Todo { Id = 42 }, new Todo { Id = 1337 } };

            // act
            var cut = TestHost.AddComponent<TodoList>(
                (nameof(TodoList.Items), items),
                (nameof(TodoList.ItemsTemplate), GetItemTemplate())
            );

            // assert
            var expectedHtml = GetExpectedHtml(itemsHtml: $"<li>{items[0].Id}</li><li>{items[1].Id}</li>");
            cut.ShouldBe(expectedHtml);

            RenderFragment<Todo> GetItemTemplate()
            { 
                return todo => builder => builder.AddMarkupContent(0, $"<li>{todo.Id}</li>");
            }
        }
    }
}
