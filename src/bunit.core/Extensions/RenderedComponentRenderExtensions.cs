using Bunit.Rendering;
using System.Runtime.ExceptionServices;

namespace Bunit;

/// <summary>
/// Re-render extension methods, optionally with new parameters, for <see cref="IRenderedComponentBase{TComponent}"/>.
/// </summary>
public static class RenderedComponentRenderExtensions
{
	/// <summary>
	/// Render the component under test again.
	/// </summary>
	/// <param name="renderedComponent">The rendered component to re-render.</param>
	/// <typeparam name="TComponent">The type of the component.</typeparam>
	public static void Render<TComponent>(this IRenderedComponentBase<TComponent> renderedComponent)
		where TComponent : IComponent
		=> SetParametersAndRender(renderedComponent, ParameterView.Empty);

	/// <summary>
	/// Render the component under test again with the provided <paramref name="parameters"/>.
	/// </summary>
	/// <param name="renderedComponent">The rendered component to re-render with new parameters.</param>
	/// <param name="parameters">Parameters to pass to the component upon rendered.</param>
	/// <typeparam name="TComponent">The type of the component.</typeparam>
	public static void SetParametersAndRender<TComponent>(this IRenderedComponentBase<TComponent> renderedComponent, ParameterView parameters)
		where TComponent : IComponent
	{
		if (renderedComponent is null)
			throw new ArgumentNullException(nameof(renderedComponent));

		var renderer = renderedComponent.Services.GetRequiredService<TestRenderer>();

		try
		{
			renderer.SetDirectParametersAsync(renderedComponent, parameters).GetAwaiter().GetResult();
		}
		catch (AggregateException e) when (e.InnerExceptions.Count == 1)
		{
			ExceptionDispatchInfo.Capture(e.InnerExceptions[0]).Throw();
		}
	}

	/// <summary>
	/// Render the component under test again with the provided <paramref name="parameters"/>.
	/// </summary>
	/// <param name="renderedComponent">The rendered component to re-render with new parameters.</param>
	/// <param name="parameters">Parameters to pass to the component upon rendered.</param>
	/// <typeparam name="TComponent">The type of the component.</typeparam>
	public static void SetParametersAndRender<TComponent>(this IRenderedComponentBase<TComponent> renderedComponent, params ComponentParameter[] parameters)
		where TComponent : IComponent
	{
		if (renderedComponent is null)
			throw new ArgumentNullException(nameof(renderedComponent));
		if (parameters is null)
			throw new ArgumentNullException(nameof(parameters));

		SetParametersAndRender(renderedComponent, ToParameterView(parameters));
	}

	/// <summary>
	/// Render the component under test again with the provided parameters from the <paramref name="parameterBuilder"/>.
	/// </summary>
	/// <param name="renderedComponent">The rendered component to re-render with new parameters.</param>
	/// <param name="parameterBuilder">An action that receives a <see cref="ComponentParameterCollectionBuilder{TComponent}"/>.</param>
	/// <typeparam name="TComponent">The type of the component.</typeparam>
	public static void SetParametersAndRender<TComponent>(this IRenderedComponentBase<TComponent> renderedComponent, Action<ComponentParameterCollectionBuilder<TComponent>> parameterBuilder)
		where TComponent : IComponent
	{
		if (renderedComponent is null)
			throw new ArgumentNullException(nameof(renderedComponent));
		if (parameterBuilder is null)
			throw new ArgumentNullException(nameof(parameterBuilder));

		var builder = new ComponentParameterCollectionBuilder<TComponent>(parameterBuilder);
		SetParametersAndRender(renderedComponent, ToParameterView(builder.Build()));
	}

	private static ParameterView ToParameterView(IReadOnlyCollection<ComponentParameter> parameters)
	{
		var parameterView = ParameterView.Empty;

		if (parameters.Count > 0)
		{
			var paramDict = new Dictionary<string, object?>(StringComparer.Ordinal);

			foreach (var param in parameters)
			{
				if (param.IsCascadingValue)
					throw new InvalidOperationException($"You cannot provide a new cascading value through the {nameof(SetParametersAndRender)} method.");
				if (param.Name is null)
					throw new InvalidOperationException("A parameters name is required.");

				paramDict.Add(param.Name, param.Value);
			}

			// Nullable is disabled to get around the issue with different annotations
			// between .netstandard2.1 and net6.
#nullable disable
			parameterView = ParameterView.FromDictionary(paramDict);
#nullable restore
		}

		return parameterView;
	}
}
