using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Bunit.RazorTesting
{
	/// <summary>
	/// Represents a component used to define tests in Razor files.
	/// </summary>
	public abstract class RazorTestBase : TestContextBase, IComponent
	{
		/// <summary>
		/// Gets the name of the test, which is displayed by the test runner/explorer.
		/// </summary>
		public abstract string? DisplayName { get; }

		/// <summary>
		/// Gets a value indicating whether the tests is running or not.
		/// </summary>
		public bool IsRunning { get; private set; }

		/// <summary>
		/// Gets or sets a description for the test that will be displayed in the test explorer.
		/// </summary>
		[Parameter] public virtual string? Description { get; set; }

		/// <summary>
		/// Gets or sets a reason for skipping the test. If not set (null), the test will not be skipped.
		/// </summary>
		[Parameter] public virtual string? Skip { get; set; }

		/// <summary>
		/// Gets or sets the timeout of the test, in milliseconds; if zero or negative, means the test case has no timeout.
		/// </summary>
		[Parameter] public virtual TimeSpan? Timeout { get; set; }

		/// <summary>
		/// Run the test logic of the <see cref="RazorTestBase"/>.
		/// </summary>
		/// <exception cref="InvalidOperationException">Thrown when called and <see cref="IsRunning"/> is true.</exception>
		public async Task RunTestAsync()
		{
			if (IsRunning)
				throw new InvalidOperationException("The razor test is already running.");

			IsRunning = true;

			try
			{
				await RunAsync().ConfigureAwait(false);
			}
			finally
			{
				IsRunning = false;
			}
		}

		/// <inheritdoc/>
		public virtual Task SetParametersAsync(ParameterView parameters)
		{
			Skip = parameters.GetValueOrDefault<string>(nameof(Skip));
			Description = parameters.GetValueOrDefault<string>(nameof(Description));
			Timeout = parameters.GetValueOrDefault<TimeSpan>(nameof(Timeout));
			return Task.CompletedTask;
		}

		/// <summary>
		/// Validates the test and throws an exception if the test does not have received all
		/// input it needs to run.
		/// </summary>
		public virtual void Validate() { }

		/// <inheritdoc/>
		void IComponent.Attach(RenderHandle renderHandle)
		{
			// Since this component just captures a render fragments and test settings for testing,
			// the renderHandler is not used for anything in this component.
		}

		/// <summary>
		/// Implements the logic for running the test.
		/// </summary>
		protected abstract Task RunAsync();

		/// <summary>
		/// Try to run the <see cref="Action{T}"/>.
		/// </summary>
		protected static void TryRun<T>(Action<T> action, T input)
		{
			if (action is null)
				throw new ArgumentNullException(nameof(action));

			action(input);
		}

		/// <summary>
		/// Try to run the <see cref="Func{T, Task}"/>.
		/// </summary>
		protected static Task TryRunAsync<T>(Func<T, Task> action, T input)
		{
			if (action is null)
				throw new ArgumentNullException(nameof(action));

			return action(input);
		}
	}
}
