#if NET5_0_OR_GREATER
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Bunit.TestDoubles
{
	/// <summary>
	/// Represents a test double stub of a component of type <typeparamref name="TComponent"/>.
	/// </summary>
	/// <typeparam name="TComponent">The stub type.</typeparam>
	public sealed class Stub<TComponent> : IComponent
		where TComponent : IComponent
	{
		private RenderHandle renderHandle;

		/// <summary>
		/// Gets the parameters that was passed to the <typeparamref name="TComponent"/>
		/// that this stub replaced in the component tree.
		/// </summary>
		[Parameter(CaptureUnmatchedValues = true)]
		public IReadOnlyDictionary<string, object> Parameters { get; private set; } = ImmutableDictionary<string, object>.Empty;

		/// <summary>
		/// Gets the render options for this <see cref="Stub{TComponent}"/>.
		/// </summary>
		public StubOptions Options { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Stub{TComponent}"/> class.
		/// </summary>
		public Stub() : this(StubOptions.Default)
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="Stub{TComponent}"/> class.
		/// </summary>
		/// <param name="options">Render options for this <see cref="Stub{TComponent}"/>.</param>
		public Stub(StubOptions options)
		{
			Options = options;
		}

		/// <inheritdoc/>
		void IComponent.Attach(RenderHandle renderHandle) => this.renderHandle = renderHandle;

		/// <inheritdoc/>
		Task IComponent.SetParametersAsync(ParameterView parameters)
		{
			Parameters = parameters.ToDictionary();
			renderHandle.Render(RenderSubbedComponent);
			return Task.CompletedTask;
		}

		private void RenderSubbedComponent(RenderTreeBuilder builder)
		{
			var stubbedType = typeof(TComponent);
			var name = GetComponentName(stubbedType);

			builder.OpenElement(0, name);

			if (Options.AddDiffIgnore)
				builder.AddAttribute(1, "diff:ignore");
			if (Options.AddParameters)
				RenderParameters(builder, stubbedType);

			builder.CloseElement();
		}

		private void RenderParameters(RenderTreeBuilder builder, Type stubbedType)
		{
			builder.AddMultipleAttributes(2, Parameters);

			if (stubbedType.IsGenericType)
			{
				var genericTypeValue = stubbedType.GetGenericArguments();
				var genericArgs = stubbedType.GetGenericTypeDefinition().GetGenericArguments();
				for (int i = 0; i < genericArgs.Length; i++)
				{
					builder.AddAttribute(3, genericArgs[i].Name, genericTypeValue[i].Name);
				}
			}
		}

		private static string GetComponentName(Type stubbedType)
		{
			var name = stubbedType.Name;
			if (stubbedType.IsGenericType)
			{
				name = name[..name.IndexOf('`', StringComparison.OrdinalIgnoreCase)];
			}

			return name;
		}
	}
}
#endif
