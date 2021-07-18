#if NET5_0_OR_GREATER
using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Bunit.TestDoubles
{
	/// <summary>
	/// Represents a test double stub of a component of type <typeparamref name="TComponent"/>.
	/// </summary>
	/// <typeparam name="TComponent">The stub type.</typeparam>
	public sealed class Stub<TComponent> : ComponentDoubleBase<TComponent>
		where TComponent : IComponent
	{
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
		public override string ToString() => $"Stub<{DoubledType.Name}>";

		/// <inheritdoc/>
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			Debug.Assert(builder is not null);

			if (!Options.RenderPlaceholder)
				return;

			builder.OpenElement(0, GetComponentName(DoubledType));

			if (Options.RenderParameters)
				RenderParameters(builder, DoubledType);

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
