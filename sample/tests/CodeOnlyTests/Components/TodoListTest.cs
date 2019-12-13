using AngleSharp.Dom;
using Egil.RazorComponents.Testing.EventDispatchExtensions;
using Egil.RazorComponents.Testing.SampleApp.Components;
using Egil.RazorComponents.Testing.SampleApp.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Egil.RazorComponents.Testing.SampleApp.CodeOnlyTests
{
    public class TodoListTest : ComponentTestFixture
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
            // arrange
            Services.AddMockJsRuntime();

            // act
            var cut = RenderComponent<TodoList>();

            // assert
            cut.ShouldBe(GetExpectedHtml());
        }

        [Fact(DisplayName = "Task list with custom label renders correctly")]
        public void Test002()
        {
            // arrange
            Services.AddMockJsRuntime();
            var label = "hello world";

            // act
            var cut = RenderComponent<TodoList>((nameof(TodoList.Label), label));

            // assert
            cut.ShouldBe(GetExpectedHtml(label: label));
        }

        [Fact(DisplayName = "Task list renders Items using ItemTemplate")]
        public void Test003()
        {
            // arrange
            Services.AddMockJsRuntime();
            RenderFragment<Todo> itemTemplate = todo => builder => builder.AddMarkupContent(0, $"<li>{todo.Id}</li>");
            var items = new[] { new Todo { Id = 42 }, new Todo { Id = 1337 } };

            // act
            var cut = RenderComponent<TodoList>(
                (nameof(TodoList.Items), items),
                (nameof(TodoList.ItemsTemplate), itemTemplate)
            );

            // assert
            var expectedHtml = GetExpectedHtml(itemsHtml: $"<li>{items[0].Id}</li><li>{items[1].Id}</li>");
            cut.ShouldBe(expectedHtml);
        }

        [Fact(DisplayName = "After first render, the new task input field has focus")]
        public void Test004()
        {
            // arrange
            var jsRtMock = Services.AddMockJsRuntime();

            // act
            var cut = RenderComponent<TodoList>();

            // assert that there is a call to document.body.focus.call with a single argument,
            // a reference to the input element.
            jsRtMock.VerifyInvoke("document.body.focus.call")
                .Arguments.Single().ShouldBeElementReferenceTo(cut.Find("input"));
        }

        [Fact(DisplayName = "The new task input field does not receive focus explicitly after second and later renders")]
        public void Test0041()
        {
            // arrange
            var jsRtMock = Services.AddMockJsRuntime();

            // act 
            var cut = RenderComponent<TodoList>(); // first render
            cut.Render(); // second render

            // assert
            jsRtMock.VerifyInvoke("document.body.focus.call", calledTimes: 1);
        }


        [Fact(DisplayName = "When a text entered in the add task input field and the form is submitted, " +
                            "the OnAddingTodo is raised with a new Todo containing the entered text")]
        public void Test005()
        {
            var jsRtMock = Services.AddMockJsRuntime();
            var taskValue = "HELLO WORLD TASK";
            var createdTask = default(Todo);
            var cut = RenderComponent<TodoList>(
                EventCallback<Todo>(nameof(TodoList.OnAddingTodo), task => createdTask = task)
            );

            cut.Find("input").Change(taskValue);
            cut.Find("form").Submit();

            createdTask.ShouldNotBeNull().Text.ShouldBe(taskValue);
        }

        [Fact(DisplayName = "When add task form is submitted with no text OnAddingTodo is not called")]
        public void Test006()
        {
            var jsRtMock = Services.AddMockJsRuntime();
            var createdTask = default(Todo);
            var cut = RenderComponent<TodoList>(
                EventCallback<Todo>(nameof(TodoList.OnAddingTodo), task => createdTask = task)
            );

            cut.Find("form").Submit();

            createdTask.ShouldBeNull();
        }
    }
}
