using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit.Rendering;
using Microsoft.AspNetCore.Components;

namespace Bunit
{
	/// <summary>
	/// A collection for <see cref="ComponentParameter" />
	/// </summary>
	public class ComponentParameterCollection
	{
		private HashSet<ComponentParameter>? _parameters;

		/// <summary>
		/// The internal collection of parameters
		/// </summary>
		protected virtual HashSet<ComponentParameter> Parameters
		{
			get
			{
				if (_parameters is null)
					_parameters = new HashSet<ComponentParameter>();
				return _parameters;
			}
		}

		/// <summary>
		/// Gets the number of <see cref="ComponentParameter"/> in the collection.
		/// </summary>
		public int Count => _parameters?.Count ?? 0;

		/// <summary>
		/// Adds a <paramref name="parameter"/> to the collection.
		/// </summary>
		/// <param name="parameter">Parameter to add to the collection.</param>
		public void Add(ComponentParameter parameter)
		{
			if (parameter.Name is null && parameter.Value is null)
				throw new ArgumentException("A component parameter without a name and value is not valid.");

			Parameters.Add(parameter);
		}

		/// <summary>
		/// Checks if the <paramref name="parameter"/> is in the collection.
		/// </summary>
		/// <param name="parameter">Parameter to check with.</param>
		/// <returns>True if <paramref name="parameter"/> is in the collection, false otherwise.</returns>
		public bool Contains(ComponentParameter parameter) => _parameters?.Contains(parameter) ?? false;

		/// <summary>
		/// Creates a <see cref="RenderFragment"/> that will render a
		/// component of type <typeparamref name="TComponent"/> with
		/// the parameters in the collection passed to it.
		/// </summary>
		/// <typeparam name="TComponent">Type of component to render.</typeparam>
		public RenderFragment ToComponentRenderFragment<TComponent>() where TComponent : IComponent
		{
			return builder =>
			{
				builder.OpenComponent<TComponent>(0);
				builder.CloseComponent();
			};
		}
		//	var parametersList = parameters as IReadOnlyList<ComponentParameter> ?? parameters.ToArray();
		//	var cascadingParams = new Queue<ComponentParameter>(parametersList.Where(x => x.IsCascadingValue));

		//	if (cascadingParams.Count > 0)
		//		return CreateCascadingValueRenderFragment(cascadingParams, parametersList);
		//	else
		//		return CreateComponentRenderFragment(parametersList);

		//	static RenderFragment CreateCascadingValueRenderFragment(Queue<ComponentParameter> cascadingParams, IReadOnlyList<ComponentParameter> parameters)
		//	{
		//		var cp = cascadingParams.Dequeue();
		//		var cascadingValueType = GetCascadingValueType(cp);
		//		return builder =>
		//		{
		//			builder.OpenComponent(0, cascadingValueType);
		//			if (cp.Name is { })
		//				builder.AddAttribute(1, nameof(CascadingValue<object>.Name), cp.Name);

		//			builder.AddAttribute(2, nameof(CascadingValue<object>.Value), cp.Value);
		//			builder.AddAttribute(3, nameof(CascadingValue<object>.IsFixed), true);

		//			if (cascadingParams.Count > 0)
		//				builder.AddAttribute(4, nameof(CascadingValue<object>.ChildContent), CreateCascadingValueRenderFragment(cascadingParams, parameters));
		//			else
		//				builder.AddAttribute(4, nameof(CascadingValue<object>.ChildContent), CreateComponentRenderFragment(parameters));

		//			builder.CloseComponent();
		//		};
		//	}

		//	static RenderFragment CreateComponentRenderFragment(IReadOnlyList<ComponentParameter> parameters)
		//	{
		//		return builder =>
		//		{
		//			builder.OpenComponent(0, typeof(TComponent));

		//			for (var i = 0; i < parameters.Count; i++)
		//			{
		//				var para = parameters[i];
		//				if (!para.IsCascadingValue && para.Name is { })
		//					builder.AddAttribute(i + 1, para.Name, para.Value);
		//			}

		//			builder.CloseComponent();
		//		};
		//	}
		//}

		//private static readonly Type CascadingValueType = typeof(CascadingValue<>);

		//private static Type GetCascadingValueType(ComponentParameter parameter)
		//{
		//	if (parameter.Value is null)
		//		throw new InvalidOperationException("Cannot get the type of a null object");
		//	var cascadingValueType = parameter.Value.GetType();
		//	return CascadingValueType.MakeGenericType(cascadingValueType);
		//}
	}

}
