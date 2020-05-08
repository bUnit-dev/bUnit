using System;
using System.Collections.Generic;

using Bunit.Rendering;

using Shouldly;

using Xunit;

namespace Bunit
{
	public class ComponentParameterTest
	{
		public static IEnumerable<object[]> GetEqualsTestData()
		{
			var name = "foo";
			var value = "bar";
			var p1 = ComponentParameter.CreateParameter(name, value);
			var p2 = ComponentParameter.CreateParameter(name, value);
			var p3 = ComponentParameter.CreateCascadingValue(name, value);
			var p4 = ComponentParameter.CreateParameter(string.Empty, value);
			var p5 = ComponentParameter.CreateParameter(name, string.Empty);

			yield return new object[] { p1, p1, true };
			yield return new object[] { p1, p2, true };
			yield return new object[] { p3, p3, true };
			yield return new object[] { p1, p3, false };
			yield return new object[] { p1, p4, false };
			yield return new object[] { p1, p5, false };
		}

		[Fact(DisplayName = "Creating a cascading value throws")]
		public void Test001()
		{
			Should.Throw<ArgumentNullException>(() => ComponentParameter.CreateCascadingValue(null, null!));
			Should.Throw<ArgumentNullException>(() => { ComponentParameter p = (null, null, true); });
		}

		[Fact(DisplayName = "Creating a regular parameter without a name throws")]
		public void Test002()
		{
			Should.Throw<ArgumentNullException>(() => ComponentParameter.CreateParameter(null!, null));
			Should.Throw<ArgumentNullException>(() => { ComponentParameter p = (null, null, false); });
		}

		[Theory(DisplayName = "Equals compares correctly")]
		[MemberData(nameof(GetEqualsTestData))]
		public void Test003(ComponentParameter left, ComponentParameter right, bool expectedResult)
		{
			left.Equals(right).ShouldBe(expectedResult);
			right.Equals(left).ShouldBe(expectedResult);
			(left == right).ShouldBe(expectedResult);
			(left != right).ShouldNotBe(expectedResult);
			left.Equals((object)right).ShouldBe(expectedResult);
			right.Equals((object)left).ShouldBe(expectedResult);
		}

		[Fact(DisplayName = "Equals operator works as expected with non compatible types")]
		public void Test004()
		{
			ComponentParameter.CreateParameter(string.Empty, string.Empty)
				.Equals(new object())
				.ShouldBeFalse();
		}

		[Theory(DisplayName = "GetHashCode returns same result for equal ComponentParameter")]
		[MemberData(nameof(GetEqualsTestData))]
		public void Test005(ComponentParameter left, ComponentParameter right, bool expectedResult)
		{
			left.GetHashCode().Equals(right.GetHashCode()).ShouldBe(expectedResult);
		}
	}
}
