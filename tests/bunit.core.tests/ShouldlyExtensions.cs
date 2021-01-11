using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Shouldly;

namespace Bunit
{
	public static class ShouldlyExtensions
	{
		public static void ShouldSatisfyAllConditions<T>(this T actual, params Action<T>[] conditions)
		{
			var conds = conditions.Select(x => (Action)(() => x.Invoke(actual))).ToArray();
			ShouldSatisfyAllConditionsTestExtensions.ShouldSatisfyAllConditions(actual, conds);
		}

		public static void ShouldBeParameter<T>(this ComponentParameter parameter, string? name, [AllowNull] T value, bool isCascadingValue)
		{
			parameter.ShouldSatisfyAllConditions(
				x => x.Name.ShouldBe(name),
				x => x.Value.ShouldBe(value),
				x => x.IsCascadingValue.ShouldBe(isCascadingValue));
		}

		public static T ShouldBeParameter<T>(this ComponentParameter parameter, string? name, bool isCascadingValue)
		{
			parameter.ShouldSatisfyAllConditions(
				x => x.Name.ShouldBe(name),
				x => x.Value.ShouldBeOfType<T>(),
				x => x.Value.ShouldNotBeNull(),
				x => x.IsCascadingValue.ShouldBe(isCascadingValue));
			return (T)parameter.Value!;
		}
	}
}
