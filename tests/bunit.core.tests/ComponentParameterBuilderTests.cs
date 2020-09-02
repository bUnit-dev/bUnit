using System;
using System.Threading.Tasks;
using Bunit.TestAssets.SampleComponents;
using Bunit.TestDoubles.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Shouldly;

using Xunit;

namespace Bunit
{
	public class ComponentParameterBuilderTests
	{
		[Fact(DisplayName = "Add with a parameterSelector for a CascadingParameter and a nullable integer as value and Build should return the correct ComponentParameters")]
		public void Test001()
		{
			// Arrange
			var sut = CreateSut();
			const int value = 42;

			// Arrange
			sut.Add(c => c.NamedCascadingValue, value);
			var result = sut.Build();

			// Assert
			result.Count.ShouldBe(1);

			var parameter = result[0];
			parameter.IsCascadingValue.ShouldBeTrue();
			parameter.Name.ShouldBe(nameof(AllTypesOfParams<string>.NamedCascadingValue));
			parameter.Value.ShouldBe(value);
		}

		[Theory(DisplayName = "Add with a parameterSelector for a Parameter and a string as value and Build should return the correct ComponentParameters")]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("foo")]
		public void Test002(string? value)
		{
			// Arrange
			var sut = CreateSut();

			// Act
			sut.Add(c => c.RegularParam, value);
			var result = sut.Build();

			// Assert
			result.Count.ShouldBe(1);

			var parameter = result[0];
			parameter.IsCascadingValue.ShouldBeFalse();
			parameter.Name.ShouldBe(nameof(AllTypesOfParams<string>.RegularParam));
			parameter.Value.ShouldBe(value);
		}

		[Fact(DisplayName = "Add with a parameterSelector for a RenderFragment and a markup string as value and Build should return the correct ComponentParameters")]
		public void Test003()
		{
			// Arrange
			var sut = CreateSut();
			string value = "test";

			// Act
			sut.Add(c => c.OtherContent, value);
			var result = sut.Build();

			// Assert
			result.Count.ShouldBe(1);

			var parameter = result[0];
			parameter.IsCascadingValue.ShouldBeFalse();
			parameter.Name.ShouldBe(nameof(AllTypesOfParams<string>.OtherContent));
			parameter.Value.ShouldBeOfType<RenderFragment>();
		}

		[Fact(DisplayName = "Add with a parameterSelector for a RenderFragment<TValue> and a markupFactory as value and Build should return the correct ComponentParameters")]
		public void Test004()
		{
			// Arrange
			var sut = CreateSut();

			// Act
			sut.Add(c => c.ItemTemplate, num => $"<p>{num}</p>");
			var result = sut.Build();

			// Assert
			result.Count.ShouldBe(1);

			var parameter = result[0];
			parameter.IsCascadingValue.ShouldBeFalse();
			parameter.Name.ShouldBe(nameof(AllTypesOfParams<string>.ItemTemplate));
			parameter.Value.ShouldBeOfType<RenderFragment<string>>();
		}

		[Fact(DisplayName = "Add with a parameterSelector for a template (RenderFragment<TValue>) and a template as value and Build should return the correct ComponentParameters")]
		public void Test005()
		{
			// Arrange
			var sut = CreateSut();

			// Act
			sut.Add(c => c.ItemTemplate, num => builder => builder.AddMarkupContent(0, $"<p>{num}</p>"));
			var result = sut.Build();

			// Assert
			result.Count.ShouldBe(1);

			var parameter = result[0];
			parameter.IsCascadingValue.ShouldBeFalse();
			parameter.Name.ShouldBe(nameof(AllTypesOfParams<string>.ItemTemplate));
			parameter.Value.ShouldBeOfType<RenderFragment<string>>();
		}

		[Fact(DisplayName = "Add with a parameterSelector for a NonGenericEventCallback and a async-callback as value and Build should return the correct ComponentParameters")]
		public void Test006()
		{
			// Arrange
			var sut = CreateSut();
			var @event = EventCallback.Empty;
			Func<Task> callback = () => Task.FromResult(@event);

			// Act
			sut.Add(c => c.NonGenericCallback, callback);
			var result = sut.Build();

			// Assert
			result.Count.ShouldBe(1);

			var parameter = result[0];
			parameter.IsCascadingValue.ShouldBeFalse();
			parameter.Name.ShouldBe(nameof(AllTypesOfParams<string>.NonGenericCallback));
			parameter.Value.ShouldNotBeNull();
		}

		[Fact(DisplayName = "Add with a parameterSelector for a NonGenericEventCallback and a callback as value and Build should return the correct ComponentParameters")]
		public void Test007()
		{
			// Arrange
			var sut = CreateSut();

			// Act
			sut.Add(c => c.NonGenericCallback, () => throw new Exception("NonGenericCallback"));
			var result = sut.Build();

			// Assert
			result.Count.ShouldBe(1);

			var parameter = result[0];
			parameter.IsCascadingValue.ShouldBeFalse();
			parameter.Name.ShouldBe(nameof(AllTypesOfParams<string>.NonGenericCallback));
			parameter.Value.ShouldNotBeNull();
		}

		[Fact(DisplayName = "Add with a parameterSelector for a GenericEventCallback and a async-callback as value and Build should return the correct ComponentParameters")]
		public void Test008()
		{
			// Arrange
			var sut = CreateSut();
			var @event = EventCallback<EventArgs>.Empty;
			Func<EventArgs, Task> callback = (args) => Task.FromResult(@event);

			// Act
			sut.Add(c => c.GenericCallback, callback);
			var result = sut.Build();

			// Assert
			result.Count.ShouldBe(1);

			var parameter = result[0];
			parameter.IsCascadingValue.ShouldBeFalse();
			parameter.Name.ShouldBe(nameof(AllTypesOfParams<string>.GenericCallback));
			parameter.Value.ShouldNotBeNull();
		}

		[Fact(DisplayName = "Add with a parameterSelector for a GenericEventCallback and a callback as value and Build should return the correct ComponentParameters")]
		public void Test009()
		{
			// Arrange
			var sut = CreateSut();

			// Act
			sut.Add(c => c.GenericCallback, args => throw new Exception("GenericCallback"));
			var result = sut.Build();

			// Assert
			result.Count.ShouldBe(1);

			var parameter = result[0];
			parameter.IsCascadingValue.ShouldBeFalse();
			parameter.Name.ShouldBe(nameof(AllTypesOfParams<string>.GenericCallback));
			parameter.Value.ShouldNotBeNull();
		}

		[Fact(DisplayName = "Add with multiple mixed parameterSelectors and valid values and Build should return the correct ComponentParameters")]
		public void Test010()
		{
			// Arrange
			var sut = CreateSut();

			// Act
			sut.Add(c => c.NamedCascadingValue, 42).Add(c => c.RegularParam, "bar");
			var result = sut.Build();

			// Assert
			result.Count.ShouldBe(2);

			var first = result[0];
			first.IsCascadingValue.ShouldBeTrue();
			first.Name.ShouldBe(nameof(AllTypesOfParams<string>.NamedCascadingValue));
			first.Value.ShouldBe(42);

			var second = result[1];
			second.IsCascadingValue.ShouldBeFalse();
			second.Name.ShouldBe(nameof(AllTypesOfParams<string>.RegularParam));
			second.Value.ShouldBe("bar");
		}

		[Fact(DisplayName = "Add with a parameterSelectors for multiple RenderFragments and childBuilders as values and Build should return the correct ComponentParameters")]
		public void Test011()
		{
			// Arrange
			var sut = CreateSut<TwoComponentWrapper>();

			// Act
			sut.Add<Simple1>(wrapper => wrapper.First, childBuilder =>
				{
					childBuilder
						.Add(c => c.Header, "H1")
						.Add(c => c.AttrValue, "A1");
				})
				.Add<AllTypesOfParams<int>>(wrapper => wrapper.Second, childBuilder =>
				{
					childBuilder
						.Add(c => c.RegularParam, "test");
				});
			var result = sut.Build();

			// Assert
			result.Count.ShouldBe(2);

			var first = result[0];
			first.IsCascadingValue.ShouldBeFalse();
			first.Name.ShouldBe(nameof(TwoComponentWrapper.First));
			first.Value.ShouldBeOfType<RenderFragment>();

			var second = result[1];
			second.IsCascadingValue.ShouldBeFalse();
			second.Name.ShouldBe(nameof(TwoComponentWrapper.Second));
			second.Value.ShouldBeOfType<RenderFragment>();
		}

		[Fact(DisplayName = "AddChildContent with a childBuilders and Build should return the correct ComponentParameters")]
		public void Test012()
		{
			// Arrange
			var sut = CreateSut<Wrapper>();

			// Act
			sut.AddChildContent<Simple1>(childBuilder =>
			{
				childBuilder
					.Add(c => c.Header, "H1")
					.Add(c => c.AttrValue, "A1");
			});
			var result = sut.Build();

			// Assert
			result.Count.ShouldBe(1);

			var first = result[0];
			first.IsCascadingValue.ShouldBeFalse();
			first.Name.ShouldBe(nameof(Wrapper.ChildContent));
			first.Value.ShouldBeOfType<RenderFragment>();
		}

		[Fact(DisplayName = "AddChildContent with a string markup and Build should return the correct ComponentParameters")]
		public void Test013()
		{
			// Arrange
			var sut = CreateSut<Wrapper>();

			// Act
			sut.AddChildContent("x");
			var result = sut.Build();

			// Assert
			result.Count.ShouldBe(1);

			var first = result[0];
			first.IsCascadingValue.ShouldBeFalse();
			first.Name.ShouldBe(nameof(Wrapper.ChildContent));
			first.Value.ShouldBeOfType<RenderFragment>();
		}

		[Fact(DisplayName = "Add unnamed CascadingParameter with a value and Build should return the correct ComponentParameters")]
		public void Test014()
		{
			// Arrange
			var sut = CreateSut();
			const int value = 42;

			// Act
			sut.Add(value);
			var result = sut.Build();

			// Assert
			result.Count.ShouldBe(1);

			var parameter = result[0];
			parameter.IsCascadingValue.ShouldBeTrue();
			parameter.Name.ShouldBeNull();
			parameter.Value.ShouldBe(value);
		}

		[Fact(DisplayName = "AddUnmatched with a key and value and Build should return the correct ComponentParameters")]
		public void Test015()
		{
			// Arrange
			var sut = CreateSut();
			const string key = "some-unmatched-attribute";
			const int value = 42;

			// Arrange
			sut.AddUnmatched(key, value);
			var result = sut.Build();

			// Assert
			result.Count.ShouldBe(1);

			var parameter = result[0];
			parameter.IsCascadingValue.ShouldBeFalse();
			parameter.Name.ShouldBe(key);
			parameter.Value.ShouldBe(value);
		}

		[Fact(DisplayName = "Add duplicate name should throw Exception")]
		public void Test100()
		{
			// Arrange
			var sut = CreateSut();
			sut.Add(c => c.NamedCascadingValue, 42);

			// Act and Assert
			Assert.Throws<ArgumentException>(() => sut.Add(c => c.NamedCascadingValue, 43));
		}

		[Fact(DisplayName = "Add CascadingParameter (with null value) should throw Exception")]
		public void Test101()
		{
			// Arrange
			var sut = CreateSut();

			// Act and Assert
			Assert.Throws<ArgumentNullException>(() => sut.Add(c => c.NamedCascadingValue, null));
		}

		[Fact(DisplayName = "Add with a property which does not have the [Parameter] or [CascadingParameter] attribute defined should throw Exception")]
		public void Test102()
		{
			// Arrange
			var sut = CreateSut();

			// Act and Assert
			Assert.Throws<ArgumentException>(() => sut.Add(c => c.NoParameterProperty, 42));
		}

		[Fact(DisplayName = "AddChildContent without a ChildContent property defined should throw Exception")]
		public void Test103()
		{
			// Arrange
			var sut = CreateSut<Simple1>();

			// Act and Assert
			Assert.Throws<ArgumentException>(() => sut.AddChildContent("html"));
		}

		[Fact(DisplayName = "Add with a selectorExpression which is not a property should throw Exception")]
		public void Test104()
		{
			// Arrange
			var sut = CreateSut();

			// Act and Assert
			Assert.Throws<ArgumentException>(() => sut.Add(c => c.DummyMethod(), 42));
		}




		private static ComponentParameterBuilder<AllTypesOfParams<string>> CreateSut()
			=> CreateSut<AllTypesOfParams<string>>();

		private static ComponentParameterBuilder<TComponent> CreateSut<TComponent>() where TComponent : IComponent
			=> new ComponentParameterBuilder<TComponent>();
	}

	public class ComponentParameterBuilderTest : TestContext
	{
		string GetMarkupFromRenderFragment(RenderFragment renderFragment)
		{
			return ((IRenderedFragment)Renderer.RenderFragment(renderFragment)).Markup;
		}

		[Fact(DisplayName = "All types of parameters are correctly assigned to component on render")]
		public void Test005()
		{
			Services.AddMockJSRuntime();

			var cut = RenderComponent<AllTypesOfParams<string>>(parameters => parameters
				.AddUnmatched("some-unmatched-attribute", "unmatched value")
				.Add(p => p.RegularParam, "some value")
				//.Add(42)
				.Add(p => p.UnnamedCascadingValue, 42)
				.Add(p => p.NamedCascadingValue, 1337)
				.Add(p => p.NonGenericCallback, () => throw new Exception("NonGenericCallback"))
				.Add(p => p.GenericCallback, args => throw new Exception("GenericCallback"))
				.AddChildContent(nameof(AllTypesOfParams<string>.ChildContent))
				.Add(p => p.OtherContent, nameof(AllTypesOfParams<string>.OtherContent))
				.Add(p => p.ItemTemplate, (item) => (builder) => throw new Exception("ItemTemplate"))
			);

			// assert that all parameters have been set correctly
			var instance = cut.Instance;
			instance.Attributes["some-unmatched-attribute"].ShouldBe("unmatched value");
			instance.RegularParam.ShouldBe("some value");
			instance.UnnamedCascadingValue.ShouldBe(42);
			instance.NamedCascadingValue.ShouldBe(1337);
			Should.Throw<Exception>(async () => await instance.NonGenericCallback.InvokeAsync(EventArgs.Empty)).Message.ShouldBe("NonGenericCallback");
			Should.Throw<Exception>(async () => await instance.GenericCallback.InvokeAsync(EventArgs.Empty)).Message.ShouldBe("GenericCallback");

			GetMarkupFromRenderFragment(instance.ChildContent!).ShouldBe(nameof(AllTypesOfParams<string>.ChildContent));
			GetMarkupFromRenderFragment(instance.OtherContent!).ShouldBe(nameof(AllTypesOfParams<string>.OtherContent));
			Should.Throw<Exception>(() => instance.ItemTemplate!("")(new RenderTreeBuilder())).Message.ShouldBe("ItemTemplate");
		}

		[Fact]
		public void MyTestMethod()
		{
			Services.AddMockJSRuntime();

			var cut = RenderComponent<AllTypesOfParams<string>>(parameterBuilder => parameterBuilder
				.AddUnmatched("some-unmatched-attribute", "unmatched value")
				.Add(p => p.RegularParam, "some value")
				.Add(p => p.UnnamedCascadingValue, 42)
				.Add(p => p.NamedCascadingValue, 1337)
				.Add(p => p.NonGenericCallback, () => throw new Exception("NonGenericCallback"))
				.Add(p => p.GenericCallback, (EventArgs args) => throw new Exception("GenericCallback"))
				.Add(p => p.ChildContent, nameof(AllTypesOfParams<string>.ChildContent))
				.Add(p => p.OtherContent, nameof(AllTypesOfParams<string>.OtherContent))
				.Add(p => p.ItemTemplate, (item) => (builder) => throw new Exception("ItemTemplate"))
			);

			// assert that all parameters have been set correctly
			var instance = cut.Instance;
			instance.Attributes["some-unmatched-attribute"].ShouldBe("unmatched value");
			instance.RegularParam.ShouldBe("some value");
			instance.UnnamedCascadingValue.ShouldBe(42); // Currently fails.
			instance.NamedCascadingValue.ShouldBe(1337);
			Should.Throw<Exception>(async () => await instance.NonGenericCallback.InvokeAsync(EventArgs.Empty)).Message.ShouldBe("NonGenericCallback");
			Should.Throw<Exception>(async () => await instance.GenericCallback.InvokeAsync(EventArgs.Empty)).Message.ShouldBe("GenericCallback");
		}
	}
}
