using AutoFixture.AutoNSubstitute;
using AutoFixture.Kernel;
using NSubstitute.Core;

namespace Bunit;

public sealed class AutoDataAttribute : AutoFixture.Xunit2.AutoDataAttribute
{
	public AutoDataAttribute()
		: base(FixtureFactory)
	{ }

	private static IFixture FixtureFactory()
	{
		var result = new Fixture();
		result.Customize<string>(x => x.FromSeed(seed => $"{seed}_{Guid.NewGuid()}"));
		result.Customize(new AutoNSubstituteCustomization());
		return result;
	}
}

public sealed class InlineAutoDataAttribute : AutoFixture.Xunit2.CompositeDataAttribute
{
	public InlineAutoDataAttribute([NotNull] params object[] data)
		: base(new InlineDataAttribute(data), new AutoDataAttribute())
	{
	}
}
