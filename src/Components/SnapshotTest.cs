using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Egil.RazorComponents.Testing
{
    /// <summary>
    /// A component used to create snapshot tests.
    /// Snapshot tests takes two sets of child components, a <see cref="TestInput"/> component
    /// for declaring the test input, and an <see cref="ExpectedOutput"/> component for
    /// declaring the expected output, the test input should produce.
    /// </summary>
    public class SnapshotTest : FragmentBase
    {
        private Action _setup = NoopTestMethod;

        /// <summary>
        /// A description or name for the test that will be displayed if the test fails.
        /// </summary>
        [Parameter] public string? Description { get; set; }

        /// <summary>
        /// A method to be called <see cref="TestInput"/> component and <see cref="ExpectedOutput"/> component
        /// is rendered. Use to e.g. setup services that the test input needs to render.
        /// </summary>
        [Parameter] public Action Setup { get => _setup; set => _setup = value ?? NoopTestMethod; }

        private static void NoopTestMethod() { }
    }

    /// <summary>
    /// Represents the test input in a snapshot test (<see cref="SnapshotTest"/>).
    /// </summary>
    public class TestInput : FragmentBase { }

    /// <summary>
    /// Represents the expected output in a snapshot test (<see cref="SnapshotTest"/>).
    /// </summary>
    public class ExpectedOutput : FragmentBase { }
}
