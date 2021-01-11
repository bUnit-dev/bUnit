using System;
using System.Collections.Generic;
using AngleSharp.Diffing.Core;
using AngleSharp.Dom;

namespace Bunit
{
	/// <summary>
	/// Represents a rendered fragment.
	/// </summary>
	public interface IRenderedFragment : IRenderedFragmentBase
	{
		/// <summary>
		/// An event that is raised after the markup of the <see cref="IRenderedFragmentBase"/> is updated.
		/// </summary>
		event EventHandler OnMarkupUpdated;

		/// <summary>
		/// Gets the HTML markup from the rendered fragment/component.
		/// </summary>
		string Markup { get; }

		/// <summary>
		/// Gets the AngleSharp <see cref="INodeList"/> based
		/// on the HTML markup from the rendered fragment/component.
		/// </summary>
		INodeList Nodes { get; }

		/// <summary>
		/// Performs a comparison of the markup produced by the initial rendering of the
		/// fragment or component under test with the current rendering of the fragment
		/// or component under test.
		/// </summary>
		/// <returns>A list of differences found.</returns>
		IReadOnlyList<IDiff> GetChangesSinceFirstRender();

		/// <summary>
		/// Performs a comparison of the markup produced by the rendering of the
		/// fragment or component under test at the time the <see cref="SaveSnapshot"/> was called
		/// with the current rendering of the fragment or component under test.
		/// </summary>
		/// <returns>A list of differences found.</returns>
		IReadOnlyList<IDiff> GetChangesSinceSnapshot();

		/// <summary>
		/// Saves the markup from the current rendering of the fragment or component under test.
		/// Use the method <see cref="GetChangesSinceSnapshot"/> later to get the difference between
		/// the snapshot and the rendered markup at that time.
		/// </summary>
		void SaveSnapshot();
	}
}
