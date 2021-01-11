using System;
using System.Linq;
using System.Reflection;
using Moq;

namespace Bunit.TestUtililities
{
	/// <summary>
	/// Helper methods for creating mocks.
	/// </summary>
	public static class MockingHelpers
	{
		private static readonly MethodInfo MockOfInfo = typeof(Mock)
			.GetMethods()
			.First(x => string.Equals(x.Name, nameof(Mock.Of), StringComparison.Ordinal) && x.GetParameters().Length == 0);

		private static readonly Type DelegateType = typeof(MulticastDelegate);
		private static readonly Type StringType = typeof(string);

		/// <summary>
		/// Creates a mock instance of <paramref name="type"/>.
		/// </summary>
		/// <param name="type">Type to create a mock of.</param>
		/// <returns>An instance of <paramref name="type"/>.</returns>
		public static object ToMockInstance(this Type type)
		{
			if (type is null)
				throw new ArgumentNullException(nameof(type));

			if (type.IsMockable())
			{
				var result = MockOfInfo.MakeGenericMethod(type).Invoke(null, Array.Empty<object>());

				if (result is null)
					throw new NotSupportedException($"Cannot create an mock of {type.FullName}.");

				return result;
			}

			if (type.Equals(StringType))
			{
				return string.Empty;
			}

			throw new NotSupportedException($"Cannot create an mock of {type.FullName}. Type to mock must be an interface, a delegate, or a non-sealed, non-static class.");
		}

		/// <summary>
		/// Gets whether a type is mockable by <see cref="Moq"/>.
		/// </summary>
		public static bool IsMockable(this Type type)
		{
			if (type is null)
				throw new ArgumentNullException(nameof(type));

			if (type.IsSealed)
				return type.IsDelegateType();
			return true;
		}

		/// <summary>
		/// Gets whether a type is a delegate type.
		/// </summary>
		public static bool IsDelegateType(this Type type)
		{
			return Equals(type, DelegateType);
		}
	}
}
