using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Bunit.RazorTesting
{
	/// <summary>
	/// Represents a component used to define tests in Razor files.
	/// </summary>
	public abstract class RazorTest : TestContextBase, ITestContext, IComponent
	{
		/// <summary>
		/// Gets whether the tests is running or not.
		/// </summary>
		public bool IsRunning { get; private set; }

		/// <summary>
		/// A description or name for the test that will be displayed if the test fails.
		/// </summary>
		[Parameter] public string? Description { get; set; }

		/// <summary>
		/// Gets or sets a reason for skipping the test. If not set (null), the test will not be skipped.
		/// </summary>
		[Parameter] public string? Skip { get; set; }

		/// <summary>
		/// Run the test logic of the <see cref="RazorTest"/>.
		/// </summary>
		/// <exception cref="InvalidOperationException">Thrown when called and <see cref="IsRunning"/> is true.</exception>
		/// <returns></returns>
		public async Task RunTest()
		{
			if (IsRunning)
				throw new InvalidOperationException("The fixture test is already running.");

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
		public override string ToString() => $"{Description ?? "[no description]"}";

		/// <inheritdoc/>
		public virtual Task SetParametersAsync(ParameterView parameters)
		{
			Skip = parameters.GetValueOrDefault<string>(nameof(Skip));
			Description = parameters.GetValueOrDefault<string>(nameof(Description));
			return Task.CompletedTask;
		}

		/// <inheritdoc/>
		void IComponent.Attach(RenderHandle renderHandle) { }

		/// <summary>
		/// Implements the logic for running the test.
		/// </summary>
		protected abstract Task Run();

		/// <summary>
		/// Try to run the <see cref="Action{T}"/>.
		/// </summary>
		protected void TryRun<T>(Action<T> action, T input)
		{
			try
			{
				action(input);
			}
			catch (Exception ex)
			{
				throw new RazorTestFailedException(Description ?? $"{action.Method.Name} failed:", ex);
			}
		}

		/// <summary>
		/// Try to run the <see cref="Func{T, Task}"/>.
		/// </summary>
		protected async Task TryRunAsync<T>(Func<T, Task> action, T input)
		{
			try
			{
				await action(input).ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				throw new RazorTestFailedException(Description ?? $"{action.Method.Name} failed:", ex);
			}
		}
	}
}
