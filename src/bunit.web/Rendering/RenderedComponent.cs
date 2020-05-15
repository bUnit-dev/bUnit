using System;
using System.Collections.Generic;
using System.Linq;
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
		public void Render() => SetParametersAndRender(ParameterView.Empty);

		/// <inheritdoc/>
		public void SetParametersAndRender(ParameterView parameters)
		{
			InvokeAsync(() =>
			{
				Instance.SetParametersAsync(parameters);
			});
		}

		/// <inheritdoc/>
		public void SetParametersAndRender(params ComponentParameter[] parameters)
		{
			SetParametersAndRender(ToParameterView(parameters));
		}

		/// <inheritdoc/>
		public void SetParametersAndRender(Action<ComponentParameterBuilder<TComponent>> parameterBuilder)
		{
			if (parameterBuilder is null)
				throw new ArgumentNullException(nameof(parameterBuilder));

			var builder = new ComponentParameterBuilder<TComponent>();
			parameterBuilder(builder);

			SetParametersAndRender(ToParameterView(builder.Build()));
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
						throw new InvalidOperationException($"You cannot provide a new cascading value through the {nameof(SetParametersAndRender)} method.");
					paramDict.Add(param.Name!, param.Value);
				}
				parameterView = ParameterView.FromDictionary(paramDict);
			}
			return parameterView;
		}
	}
}
