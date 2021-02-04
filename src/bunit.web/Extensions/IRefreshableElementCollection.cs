using System.Collections.Generic;
using AngleSharp.Dom;

namespace Bunit
{
	/// <summary>
	/// Represents a <see cref="IElement"/> collection, which queries and finds its
	/// elements in an <see cref="IRenderedFragmentBase"/>, based on a CSS selector.
	/// The collection can be refreshed either manually or automatically.
	/// </summary>
	/// <typeparam name="T">The type of <see cref="IElement"/> in the collection.</typeparam>
	public interface IRefreshableElementCollection<out T> : IReadOnlyList<T>
	    where T : IElement
	{
		/// <summary>
		/// Gets or sets a value indicating whether the collection automatically refreshes when the
		/// <see cref="IRenderedFragmentBase"/> changes.
		/// </summary>
		bool EnableAutoRefresh { get; set; }

		/// <summary>
		/// Trigger a refresh of the elements in the collection, by querying the rendered fragments DOM tree.
		/// </summary>
		void Refresh();
	}
}
