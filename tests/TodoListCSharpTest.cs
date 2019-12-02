using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using Egil.RazorComponents.Testing.SampleComponents;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Diagnostics.CodeAnalysis;
using Shouldly;
using Xunit;

namespace Egil.RazorComponents.Testing
{
    public class TodoListCSharpTest : IDisposable
    {
        private readonly TestHost testHost = new TestHost();

        public void Dispose() => testHost.Dispose();

        static RenderFragment<Todo> CreateTodoItemTemplate()
        {
            return todo => builder =>
            {
                builder.OpenComponent<TodoItem>(0);
                builder.AddAttribute(1, nameof(TodoItem.Todo), todo);
                builder.CloseComponent();
            };
        }

        [Fact]
        public void ListStartsOutAsEmpty()
        {
            var labelText = "Type your todo here";
            var cut = testHost.AddComponent<TodoList>(
                (nameof(TodoList.Label), labelText),
                (nameof(TodoList.ItemsTemplate), CreateTodoItemTemplate())
            );
            var expected = $@"<div class=""input-group mb-3 p-3"">
                                <input value="""" type=""text"" class=""form-control"" placeholder=""{labelText}"" aria-label=""{labelText}"" />
                                <div class=""input-group-append"">
                                    <button class=""btn btn-secondary"" type=""button"">Add task</button>
                                </div>
                            </div>
                            <ol class=""list-group""></ol>";

            cut.ShouldBe(expected);
        }

        [Fact]
        public void AddingTaskToListWorks()
        {
            // arrange
            var newItemText = "Add new todo to todo list";
            var cut = testHost.AddComponent<TodoList>((nameof(TodoList.ItemsTemplate), CreateTodoItemTemplate()));

            // act
            cut.Find("input").Change(newItemText);
            cut.Find("button").Click();

            // assert
            cut.FindAll(".list-group .list-group-item label").Count().ShouldBe(1);
            cut.Find(".list-group .list-group-item label").TextContent.ShouldBe(newItemText);
            cut.Find<IHtmlInputElement>(".list-group .list-group-item input").IsChecked.ShouldBeFalse();
        }
    }
}
