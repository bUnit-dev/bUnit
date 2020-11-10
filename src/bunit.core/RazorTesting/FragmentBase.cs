using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Bunit.RazorTesting
{
	/// <summary>
	/// Represents a component that can be used to capture a render fragment.
	/// </summary>
	public abstract class FragmentBase : IComponent
	{
		/// <summary>
		/// Gets or sets the child content of the fragment.
		/// </summary>
		[Parameter] public RenderFragment ChildContent { get; set; } = default!;

		/// <inheritdoc />
		public void Attach(RenderHandle renderHandle)
		{
			// Since this component just captures a render fragment for testing,
			// the renderHandler is not used for anything in this component.
		}

		/// <inheritdoc />
		public virtual Task SetParametersAsync(ParameterView parameters)
		{
			parameters.SetParameterProperties(this);
			if (ChildContent is null)
				throw new InvalidOperationException($"No {nameof(ChildContent)} specified in test component.");

			return Task.CompletedTask;
		}
	}
}
