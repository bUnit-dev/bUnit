using System;
using System.Collections.Generic;
using Bunit.Rendering;
using Microsoft.AspNetCore.Components;

namespace Bunit
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
		public void SetParametersAndRender(params ComponentParameter[] parameters)
		{
			var parameterView = ParameterView.Empty;
			if (parameters.Length > 0)
			{
				var paramDict = new Dictionary<string, object?>(parameters.Length);
				foreach (var param in parameters)
				{
					if (param.IsCascadingValue)
						throw new InvalidOperationException($"You cannot provide a new cascading value through the {nameof(SetParametersAndRender)} method.");

					paramDict.Add(param.Name!, param.Value);
				}
				parameterView = ParameterView.FromDictionary(paramDict);
			}
			SetParametersAndRender(parameterView);
		}

		/// <inheritdoc/>
		public void SetParametersAndRender(ParameterView parameters)
		{
			Renderer.InvokeAsync(() =>
			{
				Instance.SetParametersAsync(parameters);
			});
		}
	}
}
