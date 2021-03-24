using System;
using System.Collections.Generic;
using System.Threading;
using AutoFixture.Xunit2;
using Shouldly;
using Xunit;

namespace Bunit.JSInterop
{
	public class JSRuntimeInvocationTest
	{
		public static IEnumerable<object[]> GetEqualsTestData()
		{
			var token = new CancellationToken(true);
			var args = new object[] { 1, "baz" };

			var i1 = new JSRuntimeInvocation("foo", token, args, typeof(string), "InvokeAsync");
			var i2 = new JSRuntimeInvocation("foo", token, args, typeof(string), "InvokeAsync");
			var i3 = new JSRuntimeInvocation("bar", token, args, typeof(int), "Invoke");
			var i4 = new JSRuntimeInvocation("foo", CancellationToken.None, args, typeof(int), "Invoke");
			var i5 = new JSRuntimeInvocation("foo", token, Array.Empty<object>(), typeof(int), "Invoke");
			var i6 = new JSRuntimeInvocation("foo", token, new object[] { 2, "woop" }, typeof(int), "Invoke");
			var i7 = new JSRuntimeInvocation("foo", token, args, typeof(int), "InvokeAsync");
			var i8 = new JSRuntimeInvocation("foo", token, args, typeof(string), "Invoke");
			var i9 = new JSRuntimeInvocation("foo", null, args, typeof(string), "InvokeAsync");

			yield return new object[] { i1, i1, true };
			yield return new object[] { i1, i2, true };
			yield return new object[] { i1, i3, false };
			yield return new object[] { i1, i4, false };
			yield return new object[] { i1, i5, false };
			yield return new object[] { i1, i6, false };
			yield return new object[] { i1, i7, false };
			yield return new object[] { i1, i8, false };
			yield return new object[] { i1, i9, false };
		}

		[Theory(DisplayName = "Equals operator works as expected")]
		[MemberData(nameof(GetEqualsTestData))]
		public void Test002(JSRuntimeInvocation left, JSRuntimeInvocation right, bool expectedResult)
		{
			left.Equals(right).ShouldBe(expectedResult);
			right.Equals(left).ShouldBe(expectedResult);
			(left == right).ShouldBe(expectedResult);
			(left != right).ShouldNotBe(expectedResult);
			left.Equals((object)right).ShouldBe(expectedResult);
			right.Equals((object)left).ShouldBe(expectedResult);
		}

		[Fact(DisplayName = "Equals operator works as expected with non compatible types")]
		public void Test003()
		{
			default(JSRuntimeInvocation).Equals(new object()).ShouldBeFalse();
		}

		[Theory(DisplayName = "GetHashCode returns same result for equal JSRuntimeInvocations")]
		[MemberData(nameof(GetEqualsTestData))]
		public void Test004(JSRuntimeInvocation left, JSRuntimeInvocation right, bool expectedResult)
		{
			left.GetHashCode().Equals(right.GetHashCode()).ShouldBe(expectedResult);
		}

		[Theory(DisplayName = "When result type is object, then IsVoidResultInvocation is true"), AutoData]
		public void Test005(string identifier)
		{
			var sut = new JSRuntimeInvocation(
				identifier,
				CancellationToken.None,
				Array.Empty<object?>(),
				typeof(object),
				string.Empty);

			sut.IsVoidResultInvocation.ShouldBeTrue();
		}

		[Theory(DisplayName = "When result type is not object, then IsVoidResultInvocation is false"), AutoData]
		public void Test006(string identifier)
		{
			var sut = new JSRuntimeInvocation(
				identifier,
				CancellationToken.None,
				Array.Empty<object?>(),
				typeof(string),
				string.Empty);

			sut.IsVoidResultInvocation.ShouldBeFalse();
		}
	}
}
