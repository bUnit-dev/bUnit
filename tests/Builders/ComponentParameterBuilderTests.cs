using System;
using System.Linq;
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

            var parameter = result.First();
            parameter.Name.ShouldBe("NamedCascadingValue");
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

            var parameter = result.First();
            parameter.Name.ShouldBe("RegularParam");
            parameter.Value.ShouldBe(value);
        }

        [Fact(DisplayName = "Add UnnamedParameter (integer) and Build")]
        public void Test003()
        {
            // Arrange
            const int value = 42;
            _sut.Add(value);

            // Act
            var result = _sut.Build();

            // Assert
            result.Count.ShouldBe(1);

            var parameter = result.First();
            parameter.Name.ShouldBeNull();
            parameter.Value.ShouldBe(value);
        }

        [Fact(DisplayName = "Add multiple and Build")]
        public void Test004()
        {
            // Arrange and Act
            _sut.Add(c => c.NamedCascadingValue, 42).Add(c => c.RegularParam, "bar");
            var result = _sut.Build();

            // Assert
            result.Count.ShouldBe(2);

            var first = result.First();
            first.Name.ShouldBe("NamedCascadingValue");
            first.Value.ShouldBe(42);

            var second = result.Last();
            second.Name.ShouldBe("RegularParam");
            second.Value.ShouldBe("bar");
        }

        [Fact(DisplayName = "Add Parameter (NonGenericCallback) and Build")]
        public void Test005()
        {
            // Arrange
            var @event = EventCallback.Empty;
            Func<Task> callback = () => Task.FromResult(@event);

            // Act
            _sut.Add(c => c.NonGenericCallback, callback);
            var result = _sut.Build();

            // Assert
            result.Count.ShouldBe(1);

            var parameter = result.First();
            parameter.Name.ShouldBe("NonGenericCallback");
            parameter.Value.ShouldNotBeNull();
        }

        [Fact(DisplayName = "Add Parameter (GenericCallback) and Build")]
        public void Test006()
        {
            // Arrange
            var @event = EventCallback<EventArgs>.Empty;
            Func<EventArgs, Task> callback = (args) => Task.FromResult(@event);

            // Act
            _sut.Add(c => c.GenericCallback, callback);
            var result = _sut.Build();

            // Assert
            result.Count.ShouldBe(1);

            var parameter = result.First();
            parameter.Name.ShouldBe("GenericCallback");
            parameter.Value.ShouldNotBeNull();
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

        [Fact(DisplayName = "Add with a selectorExpression which is not a property should throw Exception")]
        public void Test103()
        {
            // Act and Assert
            Assert.Throws<ArgumentException>(() => _sut.Add(c => c.DummyMethod(), 42));
        }
    }
}