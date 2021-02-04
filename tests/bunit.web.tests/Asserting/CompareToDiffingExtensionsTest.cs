using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bunit.TestAssets.SampleComponents;
using Bunit.TestUtililities;
using Shouldly;
using Xunit;

namespace Bunit
{
	public class CompareToDiffingExtensionsTest : TestContext
	{
		/// <summary>
		/// Returns an array of arrays containing:
		/// (MethodInfo methodInfo, string argName, object[] methodArgs).
		/// </summary>
		public static IEnumerable<object[]> GetCompareToMethods()
		{
			var methods = typeof(CompareToExtensions)
				.GetMethods()
				.Where(x => x.Name.Equals(nameof(CompareToExtensions.CompareTo), StringComparison.Ordinal))
				.ToList();

			foreach (var method in methods)
			{
				var p1Info = method.GetParameters()[0];
				var p2Info = method.GetParameters()[1];
				object p1 = p1Info.ParameterType.ToMockInstance();
				object p2 = p2Info.ParameterType.ToMockInstance();

				yield return new object[] { method, p1Info.Name!, new object[] { null!, p2! } };
				yield return new object[] { method, p2Info.Name!, new object[] { p1!, null! } };
			}
		}

		[Theory(DisplayName = "CompareTo null values throws")]
		[MemberData(nameof(GetCompareToMethods))]
		public void Test001(MethodInfo methodInfo, string argName, object[] args)
		{
			Should.Throw<TargetInvocationException>(() => methodInfo.Invoke(null, args))
				.InnerException
				.ShouldBeOfType<ArgumentNullException>()
				.ParamName.ShouldBe(argName);
		}

		[Fact(DisplayName = "CompareTo with rendered fragment and string")]
		public void Test002()
		{
			var rf1 = RenderComponent<Simple1>((nameof(Simple1.Header), "FOO"));
			var rf2 = RenderComponent<Simple1>((nameof(Simple1.Header), "BAR"));

			rf1.CompareTo(rf2.Markup).Count.ShouldBe(1);
		}

		[Fact(DisplayName = "CompareTo with rendered fragment and rendered fragment")]
		public void Test003()
		{
			var rf1 = RenderComponent<Simple1>((nameof(Simple1.Header), "FOO"));
			var rf2 = RenderComponent<Simple1>((nameof(Simple1.Header), "BAR"));

			rf1.CompareTo(rf2).Count.ShouldBe(1);
		}

		[Fact(DisplayName = "CompareTo with INode and INodeList")]
		public void Test004()
		{
			var rf1 = RenderComponent<Simple1>((nameof(Simple1.Header), "FOO"));
			var rf2 = RenderComponent<Simple1>((nameof(Simple1.Header), "BAR"));

			var elm = rf1.Find("h1");
			elm.CompareTo(rf2.Nodes).Count.ShouldBe(1);
		}

		[Fact(DisplayName = "CompareTo with INodeList and INode")]
		public void Test005()
		{
			var rf1 = RenderComponent<Simple1>((nameof(Simple1.Header), "FOO"));
			var rf2 = RenderComponent<Simple1>((nameof(Simple1.Header), "BAR"));

			var elm = rf1.Find("h1");
			rf2.Nodes.CompareTo(elm).Count.ShouldBe(1);
		}
	}
}
