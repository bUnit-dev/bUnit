using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

namespace Bunit.RazorTesting
{
	/// <summary>
	/// Represents a component used to define tests in Razor files.
	/// </summary>
	public abstract class RazorTestBase : TestContextBase, ITestContext, IComponent
	{
		/// <summary>
		/// Gets the name of the test, which is displayed by the test runner/explorer.
		/// </summary>
		public abstract string? DisplayName { get; }

		/// <summary>
		/// Gets whether the tests is running or not.
		/// </summary>
		public bool IsRunning { get; private set; }

		/// <summary>
		/// A description or name for the test that will be displayed if the test fails.
		/// </summary>
		[Parameter] public virtual string? Description { get; set; }

		/// <summary>
		/// Gets or sets a reason for skipping the test. If not set (null), the test will not be skipped.
		/// </summary>
		[Parameter] public virtual string? Skip { get; set; }

		/// <summary>
		/// Gets or sets the timeout of the test, in milliseconds; if zero or negative, means the test case has no timeout.
		/// </summary>
		[Parameter] public virtual int Timeout { get; set; } = 0;

		/// <summary>
		/// Run the test logic of the <see cref="RazorTestBase"/>.
		/// </summary>
		/// <exception cref="InvalidOperationException">Thrown when called and <see cref="IsRunning"/> is true.</exception>
		/// <returns></returns>
		public async Task RunTest()
		{
			if (IsRunning)
				throw new InvalidOperationException("The razor test is already running.");

			IsRunning = true;

			try
			{
				await Run().ConfigureAwait(false);
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
			Timeout = parameters.GetValueOrDefault<int>(nameof(Timeout));
			return Task.CompletedTask;
		}

		/// <summary>
		/// Validates the test and throws an exception if the test does not have received all
		/// input it needs to run.
		/// </summary>
		public virtual void Validate() { }

		/// <inheritdoc/>
		void IComponent.Attach(RenderHandle renderHandle) { }

		/// <summary>
		/// Implements the logic for running the test.
		/// </summary>
		protected abstract Task Run();

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
		protected static async Task TryRunAsync<T>(Func<T, Task> action, T input)
		{
			if (action is null)
				throw new ArgumentNullException(nameof(action));
			await action(input).ConfigureAwait(false);
		}
	}
}
