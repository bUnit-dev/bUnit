#nullable enable
using AngleSharp.Dom;
using System.CodeDom.Compiler;

namespace Bunit.Web.AngleSharp;

/// <summary>
/// Represents a wrapper around an <typeparamref name="TElement"/>.
/// </summary>
[GeneratedCodeAttribute("Bunit.Web.AngleSharp", "1.0.0.0")]
internal interface IElementWrapper<out TElement> where TElement : class, IElement
{
	/// <summary>
	/// Gets the wrapped element.
	/// </summary>
	TElement WrappedElement { get; }
}
#nullable restore
