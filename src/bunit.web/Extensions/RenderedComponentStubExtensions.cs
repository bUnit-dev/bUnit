#if NET5_0_OR_GREATER
using System.Linq.Expressions;
using Bunit.TestDoubles;

namespace Bunit;

/// <summary>
/// Extension methods for using component doubles.
/// </summary>
public static class RenderedComponentStubExtensions
{
	/// <summary>
	/// Invokes the event callback from the stub.
	/// </summary>
	public static Task InvokeEventCallback<TComponent>(this IRenderedComponent<Stub<TComponent>> component,
		Expression<Func<TComponent, EventCallback>> selector) where TComponent : IComponent
	{
		if (component is null)
			throw new ArgumentNullException(nameof(component));

		return component.Instance.InvokeEventCallback(selector);
	}

	/// <summary>
	/// Invokes the event callback from the stub.
	/// </summary>
	public static Task InvokeEventCallback<TComponent, T>(this IRenderedComponent<Stub<TComponent>> component,
		Expression<Func<TComponent, EventCallback<T>>> selector, T value) where TComponent : IComponent
	{
		if (component is null)
			throw new ArgumentNullException(nameof(component));

		return component.Instance.InvokeEventCallback(selector, value);
	}

	/// <summary>
	/// Gets the parameters that was passed to the <typeparamref name="TComponent"/>
	/// </summary>
	public static T GetParameter<TComponent, T>(this IRenderedComponent<Stub<TComponent>> component,
		Expression<Func<TComponent, T>> selector) where TComponent : IComponent
	{
		if (component is null)
			throw new ArgumentNullException(nameof(component));

		return component.Instance.Parameters.Get(selector);
	}
}
#endif
