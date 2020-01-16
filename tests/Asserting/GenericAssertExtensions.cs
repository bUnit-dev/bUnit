using System;
using System.Collections.Generic;
using Shouldly;
using Xunit;
using Xunit.Sdk;

namespace Egil.RazorComponents.Testing.Asserting
{
    public class GenericAssertExtensions
    {
        [Fact(DisplayName = "ShouldNotBeNull throws exception when input is class and null")]
        public void Test001()
        {
            Exception? exception = null;
            object? input = null;
            try
            {
                input.ShouldNotBeNull();
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            var actual = exception.ShouldBeOfType<XunitException>();
            actual.Message.ShouldContain("ShouldNotBeNull");
        }

        [Fact(DisplayName = "ShouldNotBeNull throws exception when input is struct and null")]
        public void Test002()
        {
            Exception? exception = null;
            int? input = null;
            try
            {
                input.ShouldNotBeNull();
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            var actual = exception.ShouldBeOfType<XunitException>();
            actual.Message.ShouldContain("ShouldNotBeNull");
        }

        [Fact(DisplayName = "ShouldNotBeNull returns input is class and it is not null")]
        public void Test003()
        {
            object? input = new object();
            var output = input.ShouldNotBeNull();
            output.ShouldBe(input);
        }

        [Fact(DisplayName = "ShouldNotBeNull returns input is struct and it is not null")]
        public void Test004()
        {
            int? input = 42;
            var output = input.ShouldNotBeNull();
            output.ShouldBe(input);
        }

        [Fact(DisplayName = "ShouldBeOfType throws when actual is a different type")]
        public void Test005()
        {
            Exception? exception = null;
            try
            {
                "foo".ShouldBeOfType<int>();
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.IsType<IsTypeException>(exception);
        }

        [Fact(DisplayName = "ShouldBeOfType returns input when type is as expected")]
        public void Test006()
        {
            object? input = "foo";
            var actual = input.ShouldBeOfType<string>();
            actual.ShouldBe(input);
        }
    }
}
