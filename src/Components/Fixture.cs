using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Egil.RazorComponents.Testing
{
    public class Fixture : FragmentBase
    {
        private Test _setup = NoopTestMethod;
        private Test _test = NoopTestMethod;
        private IReadOnlyCollection<Test> _tests = Array.Empty<Test>();

        [Parameter] public Test Setup { get => _setup; set => _setup = value ?? NoopTestMethod; }
        [Parameter] public Test Test { get => _test; set => _test = value ?? NoopTestMethod; }
        [Parameter] public IReadOnlyCollection<Test> Tests { get => _tests; set => _tests = value ?? Array.Empty<Test>(); }

        private static void NoopTestMethod(IRazorTestContext _) { }
    }
}
