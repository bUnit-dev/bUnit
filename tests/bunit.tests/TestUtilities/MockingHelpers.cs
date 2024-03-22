using Bunit.Rendering;

namespace Bunit.TestUtilities;

/// <summary>
/// Helper methods for creating mocks.
/// </summary>
public static class MockingHelpers
{
	private static readonly Type DelegateType = typeof(MulticastDelegate);
	private static readonly Type StringType = typeof(string);

	/// <summary>
	/// Creates a mock instance of <paramref name="type"/>.
	/// </summary>
	/// <param name="type">Type to create a mock of.</param>
	/// <returns>An instance of <paramref name="type"/>.</returns>
	public static object ToMockInstance(this Type type)
	{
		ArgumentNullException.ThrowIfNull(type);

		if (type.Name == typeof(IRenderedComponent<>).Name)
		{
			var result = Substitute.For<IRenderedComponent<IComponent>>();
			return result ?? throw new NotSupportedException($"Cannot create an mock of {type.FullName}.");
		}

		if (type.IsMockable())
		{
			return Substitute.For([type], []);
		}

		if (type == StringType)
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
		ArgumentNullException.ThrowIfNull(type);

		return !type.IsSealed || type.IsDelegateType();
	}

	/// <summary>
	/// Gets whether a type is a delegate type.
	/// </summary>
	public static bool IsDelegateType(this Type type) => type == DelegateType;
	
	private sealed class RenderedComponentFake : RenderedFragment
	{
		public RenderedComponentFake() : base(0, Fake())
		{
		}

		private static IServiceProvider Fake()
		{
			using var instance = new BunitHtmlParser();
			var fake = Substitute.For<IServiceProvider>();
			fake.GetService(typeof(BunitHtmlParser)).Returns(instance);
			return fake;
		}
	}
}
