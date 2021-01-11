using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Bunit.RazorTesting
{
	public class FixtureBaseTest : TestContext
	{
		private class FixtureComponent : FixtureBase<FixtureComponent>
		{
			protected override Task RunAsync() => RunAsync(this);
		}

		[Fact(DisplayName = "Setup, SetupAsync and Test methods are called in the correct order")]
		[SuppressMessage("Minor Bug", "S4158:Empty collections should not be accessed or iterated", Justification = "False positive!")]
		public async Task Test001()
		{
			var callLog = new List<string>(3);
			var cut = RenderComponent<FixtureComponent>(builder => builder
				.Add(p => p.Setup, Setup)
				.Add(p => p.SetupAsync, SetupAsync)
				.Add(p => p.Test, Test)
				.AddChildContent("FOO"));

			await cut.Instance.RunTestAsync();

			callLog[0].ShouldBe(nameof(Setup));
			callLog[1].ShouldBe(nameof(SetupAsync));
			callLog[2].ShouldBe(nameof(Test));

			void Setup(FixtureComponent fixture) => callLog?.Add(nameof(Setup));
			Task SetupAsync(FixtureComponent fixture)
			{
				callLog?.Add(nameof(SetupAsync));
				return Task.CompletedTask;
			}

			void Test(FixtureComponent fixture) => callLog?.Add(nameof(Test));
		}

		[Fact(DisplayName = "Setup, SetupAsync and TestAsync methods are called in the correct order")]
		[SuppressMessage("Minor Bug", "S4158:Empty collections should not be accessed or iterated", Justification = "False positive!")]
		public async Task Test002()
		{
			var callLog = new List<string>(3);
			var cut = RenderComponent<FixtureComponent>(builder => builder
				.Add(p => p.Setup, Setup)
				.Add(p => p.SetupAsync, SetupAsync)
				.Add(p => p.TestAsync, TestAsync)
				.AddChildContent("FOO"));

			await cut.Instance.RunTestAsync();

			callLog[0].ShouldBe(nameof(Setup));
			callLog[1].ShouldBe(nameof(SetupAsync));
			callLog[2].ShouldBe(nameof(TestAsync));

			void Setup(FixtureComponent fixture) => callLog?.Add(nameof(Setup));
			Task SetupAsync(FixtureComponent fixture)
			{
				callLog?.Add(nameof(SetupAsync));
				return Task.CompletedTask;
			}

			Task TestAsync(FixtureComponent fixture)
			{
				callLog?.Add(nameof(TestAsync));
				return Task.CompletedTask;
			}
		}

		[Fact(DisplayName = "Run fails when no ChildContent is provided")]
		public void Test010()
		{
			var cut = RenderComponent<FixtureComponent>(builder => builder
				.Add(p => p.Test, _ => { }));

			Should.Throw<ArgumentException>(() => cut.Instance.RunTestAsync())
				.ParamName.ShouldBe(nameof(FixtureComponent.ChildContent));
		}

		[Fact(DisplayName = "Run fails when both Test or TestAsync is missing")]
		public void Test011()
		{
			var cut = RenderComponent<FixtureComponent>(builder => builder
				.AddChildContent("FOO"));

			Should.Throw<ArgumentException>(() => cut.Instance.RunTestAsync())
				.ParamName.ShouldBe(nameof(FixtureComponent.Test));
		}

		[Fact(DisplayName = "Run fails when both Test or TestAsync is missing")]
		public void Test012()
		{
			var cut = RenderComponent<FixtureComponent>(builder => builder
				.Add(p => p.Test, _ => { })
				.Add(p => p.TestAsync, _ => Task.CompletedTask)
				.AddChildContent("FOO"));

			Should.Throw<ArgumentException>(() => cut.Instance.RunTestAsync())
				.ParamName.ShouldBe(nameof(FixtureComponent.Test));
		}
	}
}
