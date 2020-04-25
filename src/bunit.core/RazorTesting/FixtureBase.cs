using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Bunit.RazorTesting;
using Bunit.Rendering;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace Bunit
{
	/// <summary>
	/// Represents a single fixture in a Razor based test. Used to
	/// define the <see cref="ComponentUnderTest"/> and any <see cref="Fragment"/>'s
	/// you might need during testing, and assert against them in the Test methods.
	/// </summary>
	public abstract class FixtureBase<TFixture> : RazorTest
	{
		/// <summary>
		/// Gets or sets the child content of the fragment.
		/// </summary>
		[Parameter] public RenderFragment ChildContent { get; set; } = default!;

		/// <summary>
		/// Gets or sets the setup action to perform before the <see cref="Test"/> action,
		/// <see cref="TestAsync"/> action and <see cref="Tests"/> and <see cref="TestsAsync"/> actions are invoked.
		/// </summary>
		[Parameter] public Action<TFixture>? Setup { private get; set; }

		/// <summary>
		/// Gets or sets the asynchronous setup action to perform before the <see cref="Test"/> action,
		/// <see cref="TestAsync"/> action and <see cref="Tests"/> and <see cref="TestsAsync"/> actions are invoked.
		/// </summary>
		[Parameter] public Func<TFixture, Task>? SetupAsync { private get; set; }

		/// <summary>
		/// Gets or sets the first test action to invoke, after the <see cref="Setup"/> action has
		/// executed (if provided).
		/// 
		/// Use this to assert against the <see cref="ComponentUnderTest"/> and <see cref="Fragment"/>'s
		/// defined in the fixture.
		/// </summary>
		[Parameter] public Action<TFixture>? Test { private get; set; }

		/// <summary>
		/// Gets or sets the first test action to invoke, after the <see cref="SetupAsync"/> action has
		/// executed (if provided).
		/// 
		/// Use this to assert against the <see cref="ComponentUnderTest"/> and <see cref="Fragment"/>'s
		/// defined in the fixture.
		/// </summary>
		[Parameter] public Func<TFixture, Task>? TestAsync { private get; set; }

		/// <summary>
		/// Gets or sets the test actions to invoke, one at the time, in the order they are placed 
		/// into the collection, after the <see cref="Setup"/> action and the <see cref="Test"/> action has
		/// executed (if provided).
		/// 
		/// Use this to assert against the <see cref="ComponentUnderTest"/> and <see cref="Fragment"/>'s
		/// defined in the fixture.
		/// </summary>
		[Parameter] public IReadOnlyCollection<Action<TFixture>>? Tests { private get; set; }

		/// <summary>
		/// Gets or sets the test actions to invoke, one at the time, in the order they are placed 
		/// into the collection, after the <see cref="SetupAsync"/> action and the <see cref="TestAsync"/> action has
		/// executed (if provided).
		/// 
		/// Use this to assert against the <see cref="ComponentUnderTest"/> and <see cref="Fragment"/>'s
		/// defined in the fixture.
		/// </summary>
		[Parameter] public IReadOnlyCollection<Func<TFixture, Task>>? TestsAsync { private get; set; }

		/// <inheritdoc/>
		public override Task SetParametersAsync(ParameterView parameters)
		{
			ChildContent = parameters.GetValueOrDefault<RenderFragment>(nameof(ChildContent));
			Setup = parameters.GetValueOrDefault<Action<TFixture>>(nameof(Setup));
			SetupAsync = parameters.GetValueOrDefault<Func<TFixture, Task>>(nameof(SetupAsync));
			Test = parameters.GetValueOrDefault<Action<TFixture>>(nameof(Test));
			TestAsync = parameters.GetValueOrDefault<Func<TFixture, Task>>(nameof(TestAsync));
			Tests = parameters.GetValueOrDefault<IReadOnlyCollection<Action<TFixture>>>(nameof(Tests), Array.Empty<Action<TFixture>>());
			TestsAsync = parameters.GetValueOrDefault<IReadOnlyCollection<Func<TFixture, Task>>>(nameof(TestsAsync), Array.Empty<Func<TFixture, Task>>());

			return base.SetParametersAsync(parameters);
		}

		/// <inheritdoc/>
		public override void Validate()
		{
			base.Validate();
			if (ChildContent is null)
				throw new ArgumentException($"No {nameof(ChildContent)} specified in the {GetType().Name} component.");
			if (Test is null && TestAsync is null && Tests?.Count == 0 && TestsAsync?.Count == 0)
				throw new ArgumentException($"No test/assertions provided to the {GetType().Name} component.");
		}

		/// <inheritdoc/>
		protected virtual async Task Run(TFixture self)
		{
			Validate();
			if (Setup is { })
				TryRun(Setup, self);

			if (SetupAsync is { })
				await TryRunAsync(SetupAsync, self).ConfigureAwait(false);

			if (Test is { })
				TryRun(Test, self);

			if (TestAsync is { })
				await TryRunAsync(TestAsync, self).ConfigureAwait(false);

			foreach (var test in Tests ?? Array.Empty<Action<TFixture>>())
				TryRun(test, self);

			foreach (var test in TestsAsync ?? Array.Empty<Func<TFixture, Task>>())
				await TryRunAsync(test, self).ConfigureAwait(false);
		}
	}
}
