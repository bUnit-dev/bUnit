using System;
using System.Collections.Generic;
using AngleSharp.Diffing.Core;
using Bunit.Asserting;
using Moq;
using Shouldly;
using Xunit;

namespace Bunit
{
	public class DiffAssertExtensionsTest
	{
		[Fact(DisplayName = "ShouldHaveSingleChange throws when input is null")]
		public void Test001()
		{
			IReadOnlyList<IDiff>? diffs = null;
			Exception? exception = null;

			try
			{
				DiffAssertExtensions.ShouldHaveSingleChange(diffs!);
			}
			catch (Exception ex)
			{
				exception = ex;
			}

			exception.ShouldBeOfType<ArgumentNullException>();
		}

		[Theory(DisplayName = "ShouldHaveSingleChange throws when input length not exactly 1")]
		[MemberData(nameof(GetDiffLists))]
		public void Test002(IReadOnlyList<IDiff> diffs)
		{
			Exception? exception = null;

			try
			{
				diffs.ShouldHaveSingleChange();
			}
			catch (Exception ex)
			{
				exception = ex;
			}

			exception.ShouldBeOfType<ActualExpectedAssertException>();
		}

		[Fact(DisplayName = "ShouldHaveSingleChange returns the single diff in input when there is only one")]
		public void Test003()
		{
			var input = new IDiff[] { Mock.Of<IDiff>() };

			var output = input.ShouldHaveSingleChange();

			output.ShouldBe(input[0]);
		}

		internal static IEnumerable<object[]> GetDiffLists()
		{
			yield return new object[] { Array.Empty<IDiff>() };
			yield return new object[]
			{
				new IDiff[]
				{
					Mock.Of<IDiff>(),
					Mock.Of<IDiff>(),
				},
			};
		}
	}
}
