#nullable enable
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using AngleSharp.Dom;

namespace Bunit.Web.AngleSharp;

/// <summary>
/// Represents a wrapper <see cref="IElement"/>.
/// </summary>
[DebuggerNonUserCode]
[GeneratedCodeAttribute("Bunit.Web.AngleSharp", "1.0.0.0")]
internal abstract class WrapperBase<TElement> : IElementWrapper<TElement>
	where TElement : class, IElement
{
	private readonly IElementWrapperFactory elementFactory;
	private TElement? element;

	/// <summary>
	/// Gets the wrapped element.
	/// </summary>
	[DebuggerNonUserCode]
	public TElement WrappedElement
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (element is null)
				element = elementFactory.GetElement<TElement>();

			return element;
		}
	}

	/// <summary>
	/// Creates an instance of the <see cref="WrapperBase{T}"/> class.
	/// </summary>
	protected WrapperBase(
		TElement element,
		IElementWrapperFactory elementFactory)
	{
		this.element = element;
		this.elementFactory = elementFactory;
		elementFactory.OnElementReplaced = () => this.element = null;
	}

	/// <inheritdoc/>
	public override bool Equals(object? obj)
		=> WrappedElement.Equals(obj);

	/// <inheritdoc/>
	public override int GetHashCode() => WrappedElement.GetHashCode();

	/// <inheritdoc/>
	public static bool operator ==(WrapperBase<TElement> x, TElement y)
	{
		if (x is null)
			return y is null;
		return x.WrappedElement == y;
	}

	/// <inheritdoc/>
	public static bool operator !=(WrapperBase<TElement> x, TElement y)
	{
		return !(x == y);
	}

	/// <inheritdoc/>
	public static bool operator ==(TElement x, WrapperBase<TElement> y)
	{
		if (y is null)
			return x is null;
		return x == y.WrappedElement;
	}

	/// <inheritdoc/>
	public static bool operator !=(TElement x, WrapperBase<TElement> y)
	{
		return !(x == y);
	}
}
#nullable restore
