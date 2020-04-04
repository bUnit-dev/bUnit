using System;
using System.Linq;
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

        [Theory]
        [InlineData(null)]
        [InlineData(42)]
        public void Add_NullableInteger_And_Build_Should_Return_Correct_ReadonlyCollection(int? value)
        {
            // Arrange and Act
            _sut.Add(c => c.NamedCascadingValue, value);
            var result = _sut.Build();

            // Assert
            result.Count.ShouldBe(1);

            var parameter = result.First();
            parameter.Name.ShouldBe("NamedCascadingValue");
            parameter.Value.ShouldBe(value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("foo")]
        public void Add_String_And_Build_Should_Return_Correct_ReadonlyCollection(string? value)
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

        [Fact]
        public void AddCascading_Integer_Without_Name_Return_Correct_ReadonlyCollection()
        {
            // Arrange
            const int value = 42;
            _sut.AddCascading(value);

            // Act
            var result = _sut.Build();

            // Assert
            result.Count.ShouldBe(1);

            var parameter = result.First();
            parameter.Name.ShouldBeNull();
            parameter.Value.ShouldBe(value);
        }

        [Fact]
        public void Add_Multiple_And_Build_Should_Return_Correct_ReadonlyCollection()
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

        [Fact]
        public void Add_GenericCallback_And_Build_Should_Return_Correct_ReadonlyCollection()
        {
            // Arrange
            EventCallback<EventArgs> callback = EventCallback<EventArgs>.Empty;

            // Act
            _sut.Add(c => c.GenericCallback, callback);
            var result = _sut.Build();

            // Assert
            result.Count.ShouldBe(1);

            var parameter = result.First();
            parameter.Name.ShouldBe("GenericCallback");
            parameter.Value.ShouldBe(callback);
        }

        [Fact]
        public void Add_Duplicate_Property_Should_Throw_Exception()
        {
            // Arrange
            _sut.Add(c => c.NamedCascadingValue, null);

            // Act and Assert
            Assert.Throws<ArgumentException>(() => _sut.Add(c => c.NamedCascadingValue, null));
        }

        [Fact]
        public void AddCascading_With_NullValue_Should_Throw_Exception()
        {
            // Act and Assert
            Assert.Throws<ArgumentNullException>(() => _sut.AddCascading(c => c.NamedCascadingValue, null));
        }
    }
}