using System;
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
        public void Add_NullableInteger_And_Build_Should_Return_Correct_Array(int? value)
        {
            // Arrange and Act
            _sut.Add(c => c.NamedCascadingValue, value);
            var result = _sut.Build();

            // Assert
            result.Length.ShouldBe(1);
            result[0].Name.ShouldBe("NamedCascadingValue");
            result[0].Value.ShouldBe(value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("foo")]
        public void Add_String_And_Build_Should_Return_Correct_Array(string? value)
        {
            // Arrange and Act
            _sut.Add(c => c.RegularParam, value);
            var result = _sut.Build();

            // Assert
            result.Length.ShouldBe(1);
            result[0].Name.ShouldBe("RegularParam");
            result[0].Value.ShouldBe(value);
        }

        [Fact]
        public void Add_Multiple_And_Build_Should_Return_Correct_Array()
        {
            // Arrange and Act
            _sut.Add(c => c.NamedCascadingValue, 42).Add(c => c.RegularParam, "bar");
            var result = _sut.Build();

            // Assert
            result.Length.ShouldBe(2);
            result[0].Name.ShouldBe("NamedCascadingValue");
            result[0].Value.ShouldBe(42);
            result[1].Name.ShouldBe("RegularParam");
            result[1].Value.ShouldBe("bar");
        }

        [Fact]
        public void Add_GenericCallback_And_Build_Should_Return_Correct_Array()
        {
            // Arrange
            EventCallback<EventArgs> callback = EventCallback<EventArgs>.Empty;

            // Act
            _sut.Add(c => c.GenericCallback, callback);
            var result = _sut.Build();

            // Assert
            result.Length.ShouldBe(1);
            result[0].Name.ShouldBe("GenericCallback");
            result[0].Value.ShouldBe(callback);
        }

        [Fact]
        public void Add_Duplicate_Property_Should_Throw_Exception()
        {
            // Arrange
            _sut.Add(c => c.NamedCascadingValue, null);

            // Act and Assert
            Assert.Throws<ArgumentException>(() => _sut.Add(c => c.NamedCascadingValue, null));
        }
    }
}