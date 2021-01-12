using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace Bunit.Rendering
{
	/// <summary>
	/// Represents a root render tree, wherein components under tests will be rendered.
	/// Components added to the render tree must have either a <c>ChildContent</c> or
	/// <c>Body</c> parameter.
	/// </summary>
	public sealed class RootRenderTree : IReadOnlyCollection<RootRenderTreeRegistration>
	{
		private readonly List<RootRenderTreeRegistration> registrations = new();

		/// <summary>
		/// Adds a component to the render tree. This method can
		/// be called multiple times, with each invocation adding a component
		/// to the render tree. The <typeparamref name="TComponent"/> must have a <c>ChildContent</c>
		/// or <c>Body</c> parameter.
		/// </summary>
		/// <typeparam name="TComponent">The type of the component to add to the render tree.</typeparam>
		/// <param name="parameterBuilder">An optional parameter builder, used to pass parameters to <typeparamref name="TComponent"/>.</param>
		public void Add<TComponent>(Action<ComponentParameterCollectionBuilder<TComponent>>? parameterBuilder = null)
		    where TComponent : IComponent
		{
			var registration = new RootRenderTreeRegistration(typeof(TComponent), CreateRenderFragmentBuilder(parameterBuilder));
			registrations.Add(registration);
		}

		/// <summary>
		/// Try to add a component to the render tree if it has not already been added. This method can
		/// be called multiple times, with each invocation adding a component
		/// to the render tree. The <typeparamref name="TComponent"/> must have a <c>ChildContent</c>
		/// or <c>Body</c> parameter.
		/// </summary>
		/// <remarks>
		/// This method will only add the component to the render tree if it has not already been added.
		/// Use <see cref="Add{TComponent}(Action{ComponentParameterCollectionBuilder{TComponent}}?)"/> to
		/// add the same component multiple times.
		/// </remarks>
		/// <typeparam name="TComponent">The type of the component to add to the render tree.</typeparam>
		/// <param name="parameterBuilder">An optional parameter builder, used to pass parameters to <typeparamref name="TComponent"/>.</param>
		/// <returns>True if component was added, false if it was previously added and not added again.</returns>
		public bool TryAdd<TComponent>(Action<ComponentParameterCollectionBuilder<TComponent>>? parameterBuilder = null)
		    where TComponent : IComponent
		{
			var componentType = typeof(TComponent);
			if (registrations.Any(x => x.ComponentType == componentType))
				return false;

			var registration = new RootRenderTreeRegistration(componentType, CreateRenderFragmentBuilder(parameterBuilder));
			registrations.Add(registration);

			return true;
		}

		/// <inheritdoc/>
		public int Count => registrations.Count;

		/// <inheritdoc/>
		public IEnumerator<RootRenderTreeRegistration> GetEnumerator() => registrations.GetEnumerator();

		/// <inheritdoc/>
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		/// <summary>
		/// Creates a new <see cref="RenderFragment"/> that wraps <paramref name="target"/>
		/// inside the components registered in this <see cref="RootRenderTree"/>.
		/// </summary>
		/// <param name="target"><see cref="RenderFragment"/> to render inside the render tree.</param>
		/// <returns>A <see cref="RenderFragment"/> that renders the <paramref name="target"/> inside this <see cref="RootRenderTree"/> render tree.</returns>
		public RenderFragment Wrap(RenderFragment target)
		{
			// Wrap from the last added to the first added, as we start with the
			// target and goes from inside to out.
			var result = target;
			for (int i = registrations.Count - 1; i >= 0; i--)
			{
				result = registrations[i].RenderFragmentBuilder(result);
			}

			return result;
		}

		/// <summary>
		/// Gets the number of registered components of type <typeparamref name="TComponent"/>
		/// in the render tree.
		/// </summary>
		/// <typeparam name="TComponent">Component type to count.</typeparam>
		/// <returns>Number of components of type <typeparamref name="TComponent"/> in render tree.</returns>
		public int GetCountOf<TComponent>()
		    where TComponent : IComponent
		{
			var result = 0;
			var countType = typeof(TComponent);

			for (int i = 0; i < registrations.Count; i++)
			{
				if (countType == registrations[i].ComponentType)
					result++;
			}

			return result;
		}

		private static RenderFragment<RenderFragment> CreateRenderFragmentBuilder<TComponent>(Action<ComponentParameterCollectionBuilder<TComponent>>? parameterBuilder)
		    where TComponent : IComponent
		{
			return rc =>
			{
				var builder = new ComponentParameterCollectionBuilder<TComponent>();
				parameterBuilder?.Invoke(builder);

				var added = builder.TryAdd("ChildContent", rc) || builder.TryAdd("Body", rc);
				if (!added)
					throw new ArgumentException($"The {typeof(TComponent)} does not have a ChildContent or Body parameter. Only components with one of these parameters can be added to the root render tree.");

				return builder.Build().ToRenderFragment<TComponent>();
			};
		}
	}
}
