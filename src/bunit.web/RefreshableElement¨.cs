using AngleSharp.Dom;

namespace Bunit;

/// <summary>
/// Represents a <see cref="IElement"/> that gets refreshed when the <see cref="IRenderedFragment"/> changes.
/// </summary>
public class RefreshableElement
{
	private readonly IRenderedFragment renderedFragment;
	private readonly string cssSelector;

	/// <summary>
	/// Initializes a new instance of the <see cref="RefreshableElement"/> class.
	/// </summary>
	public RefreshableElement(IRenderedFragment renderedFragment, string cssSelector)
	{
		this.renderedFragment = renderedFragment ?? throw new ArgumentNullException(nameof(renderedFragment));
		this.cssSelector = cssSelector ?? throw new ArgumentNullException(nameof(cssSelector));
	}

	/// <summary>
	/// Gets the latest state of the element.
	/// </summary>
	public IElement Unwrap()
	{
		return renderedFragment.Nodes.QuerySelector(cssSelector) ?? throw new ElementNotFoundException(cssSelector);
	}
}
