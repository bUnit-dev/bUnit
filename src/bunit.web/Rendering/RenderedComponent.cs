using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Bunit.Rendering
{
	/// <inheritdoc/>
	internal class RenderedComponent<TComponent> : RenderedFragment, IRenderedComponent<TComponent> where TComponent : IComponent
	{
		/// <inheritdoc/>
		public TComponent Instance { get; }

		public RenderedComponent(IServiceProvider services, int componentId, TComponent component) : base(services, componentId)
		{
			Instance = component;
		}

		/// <inheritdoc/>
		public Task InvokeAsync(Action callback) => Renderer.Dispatcher.InvokeAsync(callback);

		/// <inheritdoc/>
		public Task InvokeAsync(Func<Task> callback) => Renderer.Dispatcher.InvokeAsync(callback);

		/// <inheritdoc/>
		public Task Render() => SetParametersAndRenderAsync(ParameterView.Empty);

		/// <inheritdoc/>
		public Task SetParametersAndRenderAsync(ParameterView parameters)
		{
			return InvokeAsync(() => Instance.SetParametersAsync(parameters));
		}

		/// <inheritdoc/>
		public Task SetParametersAndRenderAsync(params ComponentParameter[] parameters)
		{
			return SetParametersAndRenderAsync(ToParameterView(parameters));
		}

		/// <inheritdoc/>
		public Task SetParametersAndRenderAsync(Action<ComponentParameterBuilder<TComponent>> parameterBuilder)
		{
			if (parameterBuilder is null)
				throw new ArgumentNullException(nameof(parameterBuilder));

			var builder = new ComponentParameterBuilder<TComponent>();
			parameterBuilder(builder);

			return SetParametersAndRenderAsync(ToParameterView(builder.Build()));
		}

		private static ParameterView ToParameterView(IReadOnlyList<ComponentParameter> parameters)
		{
			var parameterView = ParameterView.Empty;
			if (parameters.Any())
			{
				var paramDict = new Dictionary<string, object?>();
				foreach (var param in parameters)
				{
					if (param.IsCascadingValue)
						throw new InvalidOperationException($"You cannot provide a new cascading value through the {nameof(SetParametersAndRenderAsync)} method.");
					paramDict.Add(param.Name!, param.Value);
				}
				parameterView = ParameterView.FromDictionary(paramDict);
			}
			return parameterView;
		}
	}
}
