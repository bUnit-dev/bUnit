using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace Bunit
{
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

			var result = renderedComponent.InvokeAsync(() => renderedComponent.Instance.SetParametersAsync(parameters));

			if (!result.IsCompleted)
			{
				result.GetAwaiter().GetResult();
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
				var paramDict = new Dictionary<string, object>(StringComparer.Ordinal);

				foreach (var param in parameters)
				{
					if (param.IsCascadingValue)
						throw new InvalidOperationException($"You cannot provide a new cascading value through the {nameof(SetParametersAndRender)} method.");
					if (param.Name is null)
						throw new InvalidOperationException($"A parameters name is required.");

					// BANG: it should technically be allowed to pass null
					paramDict.Add(param.Name, param.Value!);
				}

				parameterView = ParameterView.FromDictionary(paramDict);
			}

			return parameterView;
		}
	}
}
