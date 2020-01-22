using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Egil.RazorComponents.Testing.Asserting;
using Egil.RazorComponents.Testing.SampleApp.Data;
using Egil.RazorComponents.Testing.EventDispatchExtensions;
using Egil.RazorComponents.Testing.SampleApp.Pages;
using Microsoft.Extensions.DependencyInjection;

namespace Egil.RazorComponents.Testing.SampleApp.CodeOnlyTests.Pages
{
    public class TodosTest : ComponentTestFixture
    {
        public TodosTest()
        {
            Services.AddMockJsRuntime();
        }

        [Fact(DisplayName = "Renders Todos provided by todo service")]
        public void Test001()
        {
            // arrange
            var todos = new[] { new Todo { Id = 1, Text = "First" }, new Todo { Id = 2, Text = "Second" } };

            // setup ITodoService
            var getTask = new TaskCompletionSource<IReadOnlyList<Todo>>();
            var todoSrv = new Mock<ITodoService>();
            todoSrv.Setup(x => x.GetAll()).Returns(getTask.Task);
            Services.AddSingleton(todoSrv.Object);

            // act
            var page = RenderComponent<Todos>();
            WaitForNextRender(() => getTask.SetResult(todos));

            // assert            
            page.FindAll("li")
                .ShouldAllBe(
                    (elm, idx) => elm.Id.ShouldBe($"todo-{todos[idx].Id}"),
                    (elm, idx) => elm.Id.ShouldBe($"todo-{todos[idx].Id}")
                );
        }

        [Fact(DisplayName = "When a todo is marked as completed, the todo service is invoked")]
        public void Test002()
        {
            // arrange
            var todos = new[] { new Todo { Id = 1, Text = "First" } };
            var todoSrv = new Mock<ITodoService>();
            todoSrv.Setup(x => x.GetAll()).Returns(Task.FromResult<IReadOnlyList<Todo>>(todos));
            Services.AddSingleton(todoSrv.Object);

            // act
            var page = RenderComponent<Todos>();
            page.Find("#todo-1").Click();

            // assert
            todoSrv.Verify(srv => srv.Complete(todos[0].Id), Times.Once);
        }

        [Fact(DisplayName = "When a new todo is added, the todo service is invoked")]
        public void Test003()
        {
            // arrange
            var todoSrv = new Mock<ITodoService>();
            Services.AddSingleton(todoSrv.Object);
            var page = RenderComponent<Todos>();

            // act
            page.Find("input").Change("TODO");
            page.Find("form").Submit();

            // assert
            todoSrv.Verify(srv => srv.Add(It.IsAny<Todo>()), Times.Once);
        }
    }
}
