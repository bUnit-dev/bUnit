#if NET5_0_OR_GREATER
using System;
using Microsoft.AspNetCore.Components;

namespace Bunit.TestDoubles
{
	internal static class ComponentDoubleFactory
	{
		private static readonly Type DummyType = typeof(Dummy<>);
		private static readonly Type StubType = typeof(Stub<>);

		/// <summary>
		/// Create an instance of the <see cref="Dummy{TComponent}"/>, where <c>TComponent</c>
		/// is the <paramref name="componentType"/>.
		/// </summary>
		/// <param name="componentType">The <c>TComponent</c> type.</param>
		/// <returns>A instance of <see cref="Dummy{TComponent}"/>, where <c>TComponent</c> is <paramref name="componentType"/>.</returns>
		public static IComponent CreateDummy(Type componentType)
		{
			var typeToCreate = DummyType.MakeGenericType(componentType);
			return (IComponent)Activator.CreateInstance(typeToCreate)!;
		}

		/// <summary>
		/// Create an instance of the <see cref="Stub{TComponent}"/>, where <c>TComponent</c>
		/// is the <paramref name="componentType"/>.
		/// </summary>
		/// <param name="componentType">The <c>TComponent</c> type.</param>
		/// <param name="stubOptions">Render options for the <see cref="Stub{TComponent}"/>.</param>
		/// <returns>A instance of <see cref="Stub{TComponent}"/>, where <c>TComponent</c> is <paramref name="componentType"/>.</returns>
		public static IComponent CreateStub(Type componentType, StubOptions stubOptions)
		{
			var typeToCreate = StubType.MakeGenericType(componentType);
			return (IComponent)Activator.CreateInstance(typeToCreate, new object[] { stubOptions })!;
		}
	}
}
#endif
