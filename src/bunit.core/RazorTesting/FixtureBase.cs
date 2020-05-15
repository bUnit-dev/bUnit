using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

namespace Bunit.RazorTesting
{
	/// <summary>
	/// Represents a single fixture in a Razor based test. Used to
	/// define the <see cref="ComponentUnderTest"/> and any <see cref="Fragment"/>'s
	/// you might need during testing, and assert against them in the Test methods.
	/// </summary>
	public abstract class FixtureBase<TFixture> : RazorTestBase
	{
		/// <inheritdoc/>
		public override string? DisplayName => Description ?? Test?.Method.Name ?? TestAsync?.Method.Name;

		/// <summary>
		/// Gets or sets the child content of the fragment.
		/// </summary>
		[Parameter] public RenderFragment ChildContent { get; set; } = default!;

		/// <summary>
		/// Gets or sets the setup action to perform before the <see cref="Test"/> action or
		/// <see cref="TestAsync"/> action are invoked.
		/// </summary>
		[Parameter] public Action<TFixture>? Setup { get; set; }

		/// <summary>
		/// Gets or sets the asynchronous setup action to perform before the <see cref="Test"/> action or
		/// <see cref="TestAsync"/> action are invoked.
		/// </summary>
		[Parameter] public Func<TFixture, Task>? SetupAsync { get; set; }

		/// <summary>
		/// Gets or sets the test action to invoke, after the <see cref="Setup"/> and <see cref="SetupAsync"/> actions has
		/// invoked (if provided).
		/// If this is set, then <see cref="TestAsync"/> cannot also be set.
		/// </summary>
		[Parameter] public Action<TFixture>? Test { get; set; }

		/// <summary>
		/// Gets or sets the test action to invoke, after the <see cref="Setup"/> and <see cref="SetupAsync"/> actions has
		/// invoked (if provided).
		/// If this is set, then <see cref="Test"/> cannot also be set.
		/// </summary>
		[Parameter] public Func<TFixture, Task>? TestAsync { get; set; }

		/// <summary>
		/// Obsolete. Methods assigned to this parameter will not be invoked.
		/// </summary>
		[Obsolete("This feature has been removed since it caused confusion about the state of the fixture being passed to the test methods. Methods assigned to this parameter will not be invoked.")]
		[Parameter]
		public IReadOnlyCollection<Action<TFixture>>? Tests { get; set; }

		/// <summary>
		/// Obsolete. Methods assigned to this parameter will not be invoked.
		/// </summary>
		[Obsolete("This feature has been removed since it caused confusion about the state of the fixture being passed to the test methods. Methods assigned to this parameter will not be invoked.")]
		[Parameter]
		public IReadOnlyCollection<Func<TFixture, Task>>? TestsAsync { get; set; }

		/// <inheritdoc/>
		public override Task SetParametersAsync(ParameterView parameters)
		{
			ChildContent = parameters.GetValueOrDefault<RenderFragment>(nameof(ChildContent));
			Setup = parameters.GetValueOrDefault<Action<TFixture>>(nameof(Setup));
			SetupAsync = parameters.GetValueOrDefault<Func<TFixture, Task>>(nameof(SetupAsync));
			Test = parameters.GetValueOrDefault<Action<TFixture>>(nameof(Test));
			TestAsync = parameters.GetValueOrDefault<Func<TFixture, Task>>(nameof(TestAsync));

#pragma warning disable CS0618 // Type or member is obsolete
			if (parameters.TryGetValue<IReadOnlyCollection<Action<TFixture>>>(nameof(Tests), out var tests))
				Tests = tests;
			if (parameters.TryGetValue<IReadOnlyCollection<Func<TFixture, Task>>>(nameof(TestsAsync), out var asyncTests))
				TestsAsync = asyncTests;
#pragma warning restore CS0618 // Type or member is obsolete

			return base.SetParametersAsync(parameters);
		}


		/// <inheritdoc/>
		[SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "Validating component parameters")]
		public override void Validate()
		{
			base.Validate();
			if (ChildContent is null)
				throw new ArgumentException($"No '{nameof(ChildContent)}' specified in the {GetType().Name} component.", nameof(ChildContent));
			if (Test is null && TestAsync is null)
				throw new ArgumentException($"No test action provided via the '{nameof(Test)}' or '{nameof(TestAsync)}' parameters to the {GetType().Name} component.", nameof(Test));
			if (Test is { } && TestAsync is { })
				throw new ArgumentException($"Only one of the '{nameof(Test)}' or '{nameof(TestAsync)}' actions can be provided to the {GetType().Name} component at the same time.", nameof(Test));
#pragma warning disable CS0618 // Type or member is obsolete
			if (Tests is { })
				throw new ArgumentException($"The use of the '{nameof(Tests)}' parameter has been obsoleted, and any methods assigned to it will not longer be invoked.");
			if (TestsAsync is { })
				throw new ArgumentException($"The use of the '{nameof(TestsAsync)}' parameter has been obsoleted, and any methods assigned to it will not longer be invoked.");
#pragma warning restore CS0618 // Type or member is obsolete
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
		}
	}
}
