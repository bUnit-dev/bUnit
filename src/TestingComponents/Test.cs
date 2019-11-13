using System;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Components;
using Xunit.Sdk;
using System.Runtime.CompilerServices;

namespace Egil.RazorComponents.Testing
{
    public class Test<TSubject> : TestingComponentBase where TSubject : IComponent
    {
        [Parameter] public string Description { get; set; } = string.Empty;

        [Parameter] public Action<TestContext<TSubject>>? TestMethod { get; set; }

        [Parameter] public RenderFragment? ChildContent { get; set; }

        public override void ExecuteTest()
        {
            if (RenderContext is null) throw new InvalidOperationException("No TestRenderContext attached to test component.");
            if (TestMethod is null) throw new InvalidOperationException($"Parameter {nameof(TestMethod)} cannot be null.");
            if (ChildContent is null) throw new InvalidOperationException($"Parameter {nameof(ChildContent)} cannot be null.");

            RenderContext.RenderComponentUnderTest(ChildContent);
            var (id, component) = RenderContext.GetComponents<TSubject>().Single();
            var context = new TestContext<TSubject>(id, component, RenderContext);
            TestMethod(context);
        }
    }
}