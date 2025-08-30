using AngleSharp.Dom;
using Bunit.Web.AngleSharp;

namespace Bunit;

/// <summary>
/// Internal extensions for working with wrapped elements.
/// </summary>
internal static class ElementFactoryAccessor
{
	/// <summary>
	/// Attempts to get the component accessor from an element factory.
	/// </summary>
	/// <param name="element">The element to get the component from.</param>
	/// <returns>The component accessor if available, null otherwise.</returns>
	internal static IComponentAccessor? GetComponentAccessor(this IElement element)
	{
		var factory = element.GetElementFactory();
		return factory as IComponentAccessor;
	}

	private static IElementWrapperFactory? GetElementFactory(this IElement element)
	{
		return element is IElementWrapper<IElement> wrapper ? wrapper.Factory : null;
	}
}
