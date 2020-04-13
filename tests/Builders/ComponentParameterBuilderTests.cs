using System;
using System.Threading.Tasks;
using Bunit.SampleComponents;
using Microsoft.AspNetCore.Components;
using Shouldly;
using Xunit;

namespace Bunit
{
    public class ComponentParameterBuilderTests
    {
        private readonly ComponentParameterBuilder<AllTypesOfParams<string>> _sut;

        public ComponentParameterBuilderTests()
        {
            _sut = new ComponentParameterBuilder<AllTypesOfParams<string>>();
        }

        [Fact(DisplayName = "Add CascadingParameter (nullable integer) and Build")]
        public void Test001()
        {
            // Arrange
            const int value = 42;

            // Arrange
            _sut.Add(c => c.NamedCascadingValue, value);
            var result = _sut.Build();

            // Assert
            result.Count.ShouldBe(1);

            var parameter = result[0];
            parameter.IsCascadingValue.ShouldBeTrue();
            parameter.Name.ShouldBe(nameof(AllTypesOfParams<string>.NamedCascadingValue));
            parameter.Value.ShouldBe(value);
        }

        [Theory(DisplayName = "Add Parameter (string) and Build")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("foo")]
        public void Test002(string? value)
        {
            // Arrange and Act
            _sut.Add(c => c.RegularParam, value);
            var result = _sut.Build();

            // Assert
            result.Count.ShouldBe(1);

            var parameter = result[0];
            parameter.IsCascadingValue.ShouldBeFalse();
            parameter.Name.ShouldBe(nameof(AllTypesOfParams<string>.RegularParam));
            parameter.Value.ShouldBe(value);
        }

        [Fact(DisplayName = "Add Parameter (RenderFragment) and Build")]
        public void Test003()
        {
            // Arrange
            string value = "test";

            // Act
            _sut.Add(c => c.OtherContent, value);
            var result = _sut.Build();

            // Assert
            result.Count.ShouldBe(1);

            var parameter = result[0];
            parameter.IsCascadingValue.ShouldBeFalse();
            parameter.Name.ShouldBe(nameof(AllTypesOfParams<string>.OtherContent));
            parameter.Value.ShouldBeOfType<RenderFragment>();
        }

        [Fact(DisplayName = "Add Parameter (RenderFragment<TValue>) and Build")]
        public void Test004()
        {
            // Arrange and Act
            _sut.Add(c => c.ItemTemplate, num => $"<p>{num}</p>");
            var result = _sut.Build();

            // Assert
            result.Count.ShouldBe(1);

            var parameter = result[0];
            parameter.IsCascadingValue.ShouldBeFalse();
            parameter.Name.ShouldBe(nameof(AllTypesOfParams<string>.ItemTemplate));
            parameter.Value.ShouldBeOfType<RenderFragment<string>>();
        }

        [Fact(DisplayName = "Add Parameter (Template) and Build")]
        public void Test005()
        {
            // Arrange and Act
            _sut.Add(c => c.ItemTemplate, num => builder => builder.AddMarkupContent(0, $"<p>{num}</p>"));
            var result = _sut.Build();

            // Assert
            result.Count.ShouldBe(1);

            var parameter = result[0];
            parameter.IsCascadingValue.ShouldBeFalse();
            parameter.Name.ShouldBe(nameof(AllTypesOfParams<string>.ItemTemplate));
            parameter.Value.ShouldBeOfType<RenderFragment<string>>();
        }

        [Fact(DisplayName = "Add Parameter (NonGenericCallback) and Build")]
        public void Test006()
        {
            // Arrange
            var @event = EventCallback.Empty;
            Func<Task> callback = () => Task.FromResult(@event);

            // Act
            _sut.Add(c => c.NonGenericCallback, callback);
            var result = _sut.Build();

            // Assert
            result.Count.ShouldBe(1);

            var parameter = result[0];
            parameter.IsCascadingValue.ShouldBeFalse();
            parameter.Name.ShouldBe(nameof(AllTypesOfParams<string>.NonGenericCallback));
            parameter.Value.ShouldNotBeNull();
        }

        [Fact(DisplayName = "Add Parameter (GenericCallback) and Build")]
        public void Test007()
        {
            // Arrange
            var @event = EventCallback<EventArgs>.Empty;
            Func<EventArgs, Task> callback = (args) => Task.FromResult(@event);

            // Act
            _sut.Add(c => c.GenericCallback, callback);
            var result = _sut.Build();

            // Assert
            result.Count.ShouldBe(1);

            var parameter = result[0];
            parameter.IsCascadingValue.ShouldBeFalse();
            parameter.Name.ShouldBe(nameof(AllTypesOfParams<string>.GenericCallback));
            parameter.Value.ShouldNotBeNull();
        }

        [Fact(DisplayName = "Add multiple and Build")]
        public void Test008()
        {
            // Arrange and Act
            _sut.Add(c => c.NamedCascadingValue, 42).Add(c => c.RegularParam, "bar");
            var result = _sut.Build();

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

        [Fact(DisplayName = "Add multiple RenderFragments using ChildBuilders and Build")]
        public void Test009()
        {
            // Arrange
            var sut = new ComponentParameterBuilder<TwoComponentWrapper>()
                .Add<Simple1>(wrapper => wrapper.First, childBuilder =>
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

            // Act
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

        [Fact(DisplayName = "Add ChildContent with Builder and Build")]
        public void Test010()
        {
            // Arrange
            var sut = new ComponentParameterBuilder<Wrapper>()
                .AddChildContent<Simple1>(childBuilder =>
                {
                    childBuilder
                        .Add(c => c.Header, "H1")
                        .Add(c => c.AttrValue, "A1");
                });

            // Act
            var result = sut.Build();

            // Assert
            result.Count.ShouldBe(1);

            var first = result[0];
            first.IsCascadingValue.ShouldBeFalse();
            first.Name.ShouldBe(nameof(Wrapper.ChildContent));
            first.Value.ShouldBeOfType<RenderFragment>();
        }

        [Fact(DisplayName = "Add ChildContent with markup and Build")]
        public void Test011()
        {
            // Arrange
            var sut = new ComponentParameterBuilder<Wrapper>()
                .AddChildContent("x");

            // Act
            var result = sut.Build();

            // Assert
            result.Count.ShouldBe(1);

            var first = result[0];
            first.IsCascadingValue.ShouldBeFalse();
            first.Name.ShouldBe(nameof(Wrapper.ChildContent));
            first.Value.ShouldBeOfType<RenderFragment>();
        }

        [Fact(DisplayName = "Add unnamed CascadingParameter and Build")]
        public void Test012()
        {
            // Arrange
            const int value = 42;

            // Arrange
            _sut.Add(value);
            var result = _sut.Build();

            // Assert
            result.Count.ShouldBe(1);

            var parameter = result[0];
            parameter.IsCascadingValue.ShouldBeTrue();
            parameter.Name.ShouldBeNull();
            parameter.Value.ShouldBe(value);
        }

        [Fact(DisplayName = "Add duplicate name should throw Exception")]
        public void Test100()
        {
            // Arrange
            _sut.Add(c => c.NamedCascadingValue, 42);

            // Act and Assert
            Assert.Throws<ArgumentException>(() => _sut.Add(c => c.NamedCascadingValue, 43));
        }

        [Fact(DisplayName = "Add CascadingParameter (with null value) should throw Exception")]
        public void Test101()
        {
            // Act and Assert
            Assert.Throws<ArgumentNullException>(() => _sut.Add(c => c.NamedCascadingValue, null));
        }

        [Fact(DisplayName = "Add with a property which does not have the [Parameter] or [CascadingParameter] attribute defined should throw Exception")]
        public void Test102()
        {
            // Act and Assert
            Assert.Throws<ArgumentException>(() => _sut.Add(c => c.NoParameterProperty, 42));
        }

        [Fact(DisplayName = "AddChildContent without a ChildContent property defined should throw Exception")]
        public void Test103()
        {
            // Arrange
            var sut = new ComponentParameterBuilder<Simple1>();

            // Act and Assert
            Assert.Throws<ArgumentException>(() => sut.AddChildContent("html"));
        }

        [Fact(DisplayName = "Add with a selectorExpression which is not a property should throw Exception")]
        public void Test104()
        {
            // Act and Assert
            Assert.Throws<ArgumentException>(() => _sut.Add(c => c.DummyMethod(), 42));
        }
    }
}