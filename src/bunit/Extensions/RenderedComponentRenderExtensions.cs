using Bunit.Rendering;
using System.Runtime.ExceptionServices;

namespace Bunit;

/// <summary>
/// Re-render extension methods, optionally with new parameters, for <see cref="RenderedComponent{TComponent}"/>.
/// </summary>
public static class RenderedComponentRenderExtensions
{
	/// <summary>
	/// Render the component under test again with the provided <paramref name="parameters"/>.
	/// </summary>
	/// <param name="renderedComponent">The rendered component to re-render with new parameters.</param>
	/// <param name="parameters">Parameters to pass to the component upon rendered.</param>
	/// <typeparam name="TComponent">The type of the component.</typeparam>
	public static void Render<TComponent>(this RenderedComponent<TComponent> renderedComponent, ParameterView parameters)
		where TComponent : IComponent
	{
		ArgumentNullException.ThrowIfNull(renderedComponent);

		var renderer = renderedComponent.Services.GetRequiredService<BunitRenderer>();

		var result = renderer.EnableUnblockedRendering(() => renderer.SetDirectParametersAsync(renderedComponent, parameters));

		if (result.IsFaulted && result.Exception is not null)
		{
			if (result.Exception.InnerExceptions.Count == 1)
			{
				ExceptionDispatchInfo.Capture(result.Exception.InnerExceptions[0]).Throw();
			}
			else
			{
				ExceptionDispatchInfo.Capture(result.Exception).Throw();
			}
		}
	}

	/// <summary>
	/// Render the component under test again with the provided parameters from the <paramref name="parameterBuilder"/>.
	/// </summary>
	/// <param name="renderedComponent">The rendered component to re-render with new parameters.</param>
	/// <param name="parameterBuilder">An action that receives a <see cref="ComponentParameterCollectionBuilder{TComponent}"/>.</param>
	/// <typeparam name="TComponent">The type of the component.</typeparam>
	public static void Render<TComponent>(this RenderedComponent<TComponent> renderedComponent, Action<ComponentParameterCollectionBuilder<TComponent>>? parameterBuilder = null)
		where TComponent : IComponent
	{
		ArgumentNullException.ThrowIfNull(renderedComponent);

		var builder = new ComponentParameterCollectionBuilder<TComponent>(parameterBuilder);
		Render(renderedComponent, ToParameterView(builder.Build()));
	}

	private static ParameterView ToParameterView(ComponentParameterCollection parameters)
	{
		if (parameters.Count == 0)
		{
			return ParameterView.Empty;
		}

		var paramDict = new Dictionary<string, object?>(parameters.Count, StringComparer.Ordinal);

		foreach (var param in parameters)
		{
			if (param.IsCascadingValue)
				throw new InvalidOperationException($"You cannot provide a new cascading value through the {nameof(Render)} method.");
			if (param.Name is null)
				throw new InvalidOperationException("A parameter name is required.");

			paramDict.Add(param.Name, param.Value);
		}

		return ParameterView.FromDictionary(paramDict);
	}
}
