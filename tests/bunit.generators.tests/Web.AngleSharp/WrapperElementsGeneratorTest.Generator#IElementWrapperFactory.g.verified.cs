//HintName: IElementWrapperFactory.g.cs
#nullable enable
using AngleSharp.Dom;
using System;
using System.CodeDom.Compiler;

namespace Bunit.Web.AngleSharp;

/// <summary>
/// Represents an <see cref="IElement"/> factory, used by a <see cref="WrapperBase{TElement}"/>.
/// </summary>
[GeneratedCodeAttribute("Bunit.Web.AngleSharp", "1.0.0.0")]
internal interface IElementWrapperFactory
{
	/// <summary>
	/// A method that returns the latest version of the element to wrap.
	/// </summary>
	/// <remarks>
	/// This method should throw if the element is not found or is not of the correct type (<typeparamref name="TElement"/>).
	/// </remarks>
	TElement GetElement<TElement>() where TElement : class, IElement;

	/// <summary>
	/// Subscribe to updates to the wrapped elements.
	/// </summary>
	Action? OnElementReplaced { get; set; }
}
#nullable restore
