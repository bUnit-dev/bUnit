using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Bunit.RazorTesting
{
	/// <summary>
	/// Creates an instance of the <see cref="FragmentContainer"/>, which is used
	/// when a fragment is rendered inside a test contexts render tree.
	/// It is primarily used to be able to find the starting point to return.
	/// </summary>
	public sealed class FragmentContainer : ComponentBase
	{
		/// <summary>
		/// Gets or sets the content to wrap.
		/// </summary>
		[Parameter] public RenderFragment? ChildContent { get; set; }

		/// <inheritdoc/>
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder?.AddContent(0, ChildContent);
		}

		/// <summary>
		/// Wraps the <paramref name="wrappingTarget"/> in a <see cref="FragmentContainer"/>.
		/// </summary>
		public static RenderFragment Wrap(RenderFragment wrappingTarget)
		{
			return builder =>
			{
				builder.OpenComponent<FragmentContainer>(0);
				builder.AddAttribute(1, nameof(ChildContent), wrappingTarget);
				builder.CloseComponent();
			};
		}
	}
}
