using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Egil.RazorComponents.Testing
{
    public delegate void Test(TestContext context);

    public class Fixture : FragmentBase
    {
        private Test _setup = NoopTestMethod;
        private Test _test = NoopTestMethod;
        private IReadOnlyCollection<Test> _tests = Array.Empty<Test>();

        [Parameter] public Test Setup { get => _setup; set => _setup = value ?? NoopTestMethod; }
        [Parameter] public Test Test { get => _test; set => _test = value ?? NoopTestMethod; }
        [Parameter] public IReadOnlyCollection<Test> Tests { get => _tests; set => _tests = value ?? Array.Empty<Test>(); }

        private static void NoopTestMethod(TestContext context) { }
    }

    public abstract class FragmentBase : IComponent
    {
        [Parameter] public RenderFragment ChildContent { get; set; } = default!;

        public void Attach(RenderHandle renderHandle) { }

        public virtual Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);
            if (ChildContent is null) throw new InvalidOperationException($"No {nameof(ChildContent)} specified.");
            return Task.CompletedTask;
        }
    }

    public class ComponentUnderTest : FragmentBase
    {
        public override Task SetParametersAsync(ParameterView parameters)
        {
            var result = base.SetParametersAsync(parameters);
            return result;
        }
    }

    public class Fragment : FragmentBase
    {
        [Parameter] public string Id { get; set; } = string.Empty;
    }
}
