using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Egil.RazorComponents.Testing
{
    /// <summary>
    /// Represents a single fixture in a Razor based test. Used to
    /// define the <see cref="ComponentUnderTest"/> and any <see cref="Fragment"/>'s
    /// you might need during testing, and assert against them in the Test methods.
    /// </summary>
    public class Fixture : FragmentBase
    {
        private Action _setup = NoopTestMethod;
        private Action _test = NoopTestMethod;
        private IReadOnlyCollection<Action> _tests = Array.Empty<Action>();

        /// <summary>
        /// A description or name for the test that will be displayed if the test fails.
        /// </summary>
        [Parameter] public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the setup action to perform before the <see cref="Test"/> action
        /// and <see cref="Tests"/> actions are invoked.
        /// </summary>
        [Parameter] public Action Setup { get => _setup; set => _setup = value ?? NoopTestMethod; }

        /// <summary>
        /// Gets or sets the first test action to invoke, after the <see cref="Setup"/> action has
        /// executed (if provided).
        /// 
        /// Use this to assert against the <see cref="ComponentUnderTest"/> and <see cref="Fragment"/>'s
        /// defined in the <see cref="Fixture"/>.
        /// </summary>
        [Parameter] public Action Test { get => _test; set => _test = value ?? NoopTestMethod; }

        /// <summary>
        /// Gets or sets the test actions to invoke, one at the time, in the order they are placed 
        /// into the collection, after the <see cref="Setup"/> action and the <see cref="Test"/> action has
        /// executed (if provided).
        /// 
        /// Use this to assert against the <see cref="ComponentUnderTest"/> and <see cref="Fragment"/>'s
        /// defined in the <see cref="Fixture"/>.
        /// </summary>
        [Parameter] public IReadOnlyCollection<Action> Tests { get => _tests; set => _tests = value ?? Array.Empty<Action>(); }

        private static void NoopTestMethod() { }
    }
}
