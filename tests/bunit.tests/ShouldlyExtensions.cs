namespace Bunit;

internal static class ShouldlyExtensions
{
	public static void ShouldSatisfyAllConditions<TCondition>(this TCondition actual, params Action<TCondition>[] conditions)
	{
		var conds = conditions.Select(x => (Action)(() => x.Invoke(actual))).ToArray();
		ShouldSatisfyAllConditionsTestExtensions.ShouldSatisfyAllConditions(actual, conds);
	}

	public static void ShouldBeParameter<TValue>(this ComponentParameter parameter, string? name, [AllowNull] TValue value, bool isCascadingValue)
	{
		parameter.ShouldSatisfyAllConditions(
			x => x.Name.ShouldBe(name),
			x => x.Value.ShouldBe(value),
			x => x.IsCascadingValue.ShouldBe(isCascadingValue));
	}

	public static TValue ShouldBeParameter<TValue>(this ComponentParameter parameter, string? name, bool isCascadingValue)
	{
		parameter.ShouldSatisfyAllConditions(
			x => x.Name.ShouldBe(name),
			x => x.Value.ShouldBeOfType<TValue>(),
			x => x.Value.ShouldNotBeNull(),
			x => x.IsCascadingValue.ShouldBe(isCascadingValue));
		return (TValue)parameter.Value!;
	}
}
