using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Bunit
{
	/// <summary>
	/// A collection for <see cref="ComponentParameter" />.
	/// </summary>
	public class ComponentParameterCollection : ICollection<ComponentParameter>, IReadOnlyCollection<ComponentParameter>
	{
		private static readonly MethodInfo CreateTemplateWrapperMethod = GetCreateTemplateWrapperMethod();
		private static readonly Type CascadingValueType = typeof(CascadingValue<>);
		private List<ComponentParameter>? parameters;

		/// <summary>
		/// Gets the number of <see cref="ComponentParameter"/> in the collection.
		/// </summary>
		public int Count => parameters?.Count ?? 0;

		/// <inheritdoc />
		public bool IsReadOnly { get; }

		/// <summary>
		/// Adds a <paramref name="item"/> to the collection.
		/// </summary>
		/// <param name="item">Parameter to add to the collection.</param>
		public void Add(ComponentParameter item)
		{
			if (item.Name is null && item.Value is null)
				throw new ArgumentException("A component parameter without a name and value is not valid.", nameof(item));

			if (parameters is null)
				parameters = new List<ComponentParameter>();

			parameters.Add(item);
		}

		/// <summary>
		/// Adds an enumerable of parameters to the collection.
		/// </summary>
		/// <param name="parameters">Parameters to add.</param>
		public void Add(IEnumerable<ComponentParameter> parameters)
		{
			if (parameters is null)
				throw new ArgumentNullException(nameof(parameters));

			foreach (var cp in parameters)
			{
				Add(cp);
			}
		}

		/// <summary>
		/// Checks if the <paramref name="item"/> is in the collection.
		/// </summary>
		/// <param name="item">Parameter to check with.</param>
		/// <returns>True if <paramref name="item"/> is in the collection, false otherwise.</returns>
		public bool Contains(ComponentParameter item) => parameters?.Contains(item) ?? false;

		/// <inheritdoc/>
		public void Clear() => parameters?.Clear();

		/// <inheritdoc/>
		public void CopyTo(ComponentParameter[] array, int arrayIndex) => parameters?.CopyTo(array, arrayIndex);

		/// <inheritdoc/>
		public bool Remove(ComponentParameter item) => parameters?.Remove(item) ?? false;

		/// <summary>
		/// Creates a <see cref="RenderFragment"/> that will render a
		/// component of type <typeparamref name="TComponent"/> with
		/// the parameters in the collection passed to it.
		/// </summary>
		/// <typeparam name="TComponent">Type of component to render.</typeparam>
		[SuppressMessage("Design", "MA0051:Method is too long", Justification = "TODO: Refactor")]
		[SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:Parameter should not span multiple lines", Justification = "TODO: Refactor")]
		public RenderFragment ToRenderFragment<TComponent>()
		    where TComponent : IComponent
		{
			var cascadingValues = GetCascadingValues();

			return cascadingValues.Count > 0
				? AddCascadingValue
				: AddComponent;

			void AddCascadingValue(RenderTreeBuilder builder)
			{
				var cv = cascadingValues.Dequeue();

				builder.OpenComponent(0, cv.Type);

				if (cv.Parameter.Name is not null)
					builder.AddAttribute(1, nameof(CascadingValue<object>.Name), cv.Parameter.Name);

				builder.AddAttribute(2, nameof(CascadingValue<object>.Value), cv.Parameter.Value);
				builder.AddAttribute(3, nameof(CascadingValue<object>.IsFixed), value: true);

				if (cascadingValues.Count > 0)
					builder.AddAttribute(4, nameof(CascadingValue<object>.ChildContent), (RenderFragment)AddCascadingValue);
				else
					builder.AddAttribute(4, nameof(CascadingValue<object>.ChildContent), (RenderFragment)AddComponent);

				builder.CloseComponent();
			}

			void AddComponent(RenderTreeBuilder builder)
			{
				builder.OpenComponent<TComponent>(0);
				AddAttributes(builder);
				builder.CloseComponent();
			}

			void AddAttributes(RenderTreeBuilder builder)
			{
				if (parameters is null) return;

				var attrCount = 100;

				foreach (var pgroup in parameters.Where(x => !x.IsCascadingValue).GroupBy(x => x.Name, StringComparer.Ordinal))
				{
					var group = pgroup.ToArray();
					var groupObject = group.FirstOrDefault(x => !(x.Value is null)).Value;

					if (group.Length == 1)
					{
						var p = group[0];
						builder.AddAttribute(
							attrCount++,
							p.Name!, // BANG: ComponentParameter does not allow a regular param to be created without a name
							p.Value);

						continue;
					}

					if (groupObject is RenderFragment)
					{
						builder.AddAttribute(
							attrCount++,
							group[0].Name!, // BANG: ComponentParameter does not allow a regular param to be created without a name
							(RenderFragment)(ccBuilder =>
							{
								for (var i = 0; i < group.Length; i++)
								{
									if (group[i].Value is RenderFragment rf)
										ccBuilder.AddContent(i, rf);
								}
							}));

						continue;
					}

					var groupType = groupObject?.GetType();

					if (groupType != null && groupType.IsGenericType && groupType.GetGenericTypeDefinition() == typeof(RenderFragment<>))
					{
						builder.AddAttribute(
							attrCount++,
							group[0].Name!, // BANG: ComponentParameter does not allow a regular param to be created without a name
							WrapTemplates(groupType, group));

						continue;
					}

					throw new ArgumentException($"The parameter with the name '{pgroup.Key}' was added more than once. This parameter can only be added one time.");
				}
			}

			Queue<(ComponentParameter Parameter, Type Type)> GetCascadingValues()
			{
				var cascadingValues = parameters?.Where(x => x.IsCascadingValue)
					.Select(x => (Parameter: x, Type: GetCascadingValueType(x)))
					.ToArray() ?? Array.Empty<(ComponentParameter Parameter, Type Type)>();

				// Detect duplicated unnamed values
				for (var i = 0; i < cascadingValues.Length; i++)
				{
					for (var j = i + 1; j < cascadingValues.Length; j++)
					{
						if (cascadingValues[i].Type == cascadingValues[j].Type)
						{
							var iName = cascadingValues[i].Parameter.Name;
							if (iName is null)
							{
								var cascadingValueType = cascadingValues[i].Type.GetGenericArguments()[0];
								throw new ArgumentException($"Two or more unnamed cascading values with the type '{cascadingValueType.Name}' was added. " +
															$"Only add one unnamed cascading value of the same type.");
							}

							if (iName.Equals(cascadingValues[j].Parameter.Name, StringComparison.Ordinal))
							{
								throw new ArgumentException($"Two or more named cascading values with the name '{iName}' and the same type was added. " +
															$"Only add one named cascading value with the same name and type.");
							}
						}
					}
				}

				return new Queue<(ComponentParameter Parameter, Type Type)>(cascadingValues);
			}
		}

		/// <inheritdoc/>
		public IEnumerator<ComponentParameter> GetEnumerator()
		{
			if (parameters is not null)
			{
				for (var i = 0; i < parameters.Count; i++)
				{
					yield return parameters[i];
				}
			}
		}

		/// <inheritdoc/>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

		private static object WrapTemplates(Type templateParamterType, ComponentParameter[] templateParameters)
		{
			// gets the generic argument to RenderFragment<>, e.g. string with RenderFragment<string>
			var templateType = templateParamterType.GetGenericArguments()[0];

			// this creates an invokable version of CreateTemplateWrapper with the
			// generic type set to tmeplateType, e.g. CreateTemplateWrapper<string>
			var templateWrapper = CreateTemplateWrapperMethod.MakeGenericMethod(templateType);

			// BANG: since CreateTemplateWrapper<T> will never return null BANG (!) is safe here
			return templateWrapper.Invoke(null, new object[] { templateParameters })!;
		}

		private static RenderFragment<T> CreateTemplateWrapper<T>(ComponentParameter[] subTemplateParams)
		{
			return input => builder =>
			{
				foreach (var tp in subTemplateParams)
				{
					if (tp.Value is RenderFragment<T> rf)
						builder.AddContent(0, rf(input));
					else
						throw new ArgumentException($"The parameter with name {tp.Name} was different types of templates.", tp.Name);
				}
			};
		}

		private static Type GetCascadingValueType(ComponentParameter parameter)
		{
			if (parameter.Value is null)
				throw new InvalidOperationException("Cannot get the type of a null object");
			var cascadingValueType = parameter.Value.GetType();
			return CascadingValueType.MakeGenericType(cascadingValueType);
		}

		private static MethodInfo GetCreateTemplateWrapperMethod()
		{
			var result = typeof(ComponentParameterCollection).GetMethod(nameof(CreateTemplateWrapper), BindingFlags.NonPublic | BindingFlags.Static);
			return result ?? throw new InvalidOperationException($"Could not find the {nameof(CreateTemplateWrapper)} method.");
		}
	}
}
