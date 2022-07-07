#if NET6_0_OR_GREATER
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Bunit.TestAssets.SampleComponents;
using Microsoft.AspNetCore.Components;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Bunit.TestDoubles;

public class FakePersistentComponentStateTest : TestContext
{
	public FakePersistentComponentStateTest(ITestOutputHelper outputHelper)
	{
		Services.AddXunitLogger(outputHelper);
	}

	[Fact(DisplayName = "AddFakePersistentComponentState is registers PersistentComponentState in services")]
	public void Test001()
	{
		_ = this.AddFakePersistentComponentState();

		var actual = Services.GetService<PersistentComponentState>();

		actual.ShouldNotBeNull();
	}

	[Fact(DisplayName = "AddFakePersistentComponentState enables PersistentComponentState injection into components")]
	public void Test002()
	{
		this.AddFakePersistentComponentState();

		var cut = RenderComponent<PersistentComponentStateSample>();

		cut.Instance.State.ShouldNotBeNull();
	}

	[Theory(DisplayName = "Persist stores state in store for components to consume")]
	[AutoData]
	public void Test011(string key, string data)
	{
		var fakeState = this.AddFakePersistentComponentState();

		fakeState.Persist(key, data);

		var store = Services.GetService<PersistentComponentState>();
		store.TryTakeFromJson<string>(key, out var actual).ShouldBeTrue();
		actual.ShouldBe(data);
	}

	[Fact(DisplayName = "TryTake returns true if key contains data saved in store")]
	public void Test012()
	{
		var fakeState = this.AddFakePersistentComponentState();
		var cut = RenderComponent<PersistentComponentStateSample>();

		fakeState.TriggerOnPersisting();

		fakeState.TryTake<WeatherForecast[]>(PersistentComponentStateSample.PersistenceKey, out var actual).ShouldBeTrue();
		actual.ShouldBeEquivalentTo(cut.Instance.Forecasts);
	}

	[Theory(DisplayName = "TryTake returns false if key is not in store")]
	[AutoData]
	public void Test013(string key)
	{
		var fakeState = this.AddFakePersistentComponentState();

		fakeState.TryTake<string>(key, out var actual).ShouldBeFalse();
	}

	[Fact(DisplayName = "TriggerOnPersisting triggers OnPersisting callbacks added to store")]
	public void Test014()
	{
		var onPersistingCalledTimes = 0;
		var fakeState = this.AddFakePersistentComponentState();
		var store = Services.GetService<PersistentComponentState>();
		store.RegisterOnPersisting(() =>
		{
			onPersistingCalledTimes++;
			return Task.CompletedTask;
		});

		fakeState.TriggerOnPersisting();

		onPersistingCalledTimes.ShouldBe(1);
	}
}
#endif
