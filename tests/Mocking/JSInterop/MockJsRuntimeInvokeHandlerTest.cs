using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Shouldly;
using Xunit;
using Xunit.Sdk;

namespace Bunit.Mocking.JSInterop
{
    public class MockJsRuntimeInvokeHandlerTest
    {
        [Fact(DisplayName = "Mock returns default value in loose mode without invocation setup")]
        public async Task Test001()
        {
            var sut = new MockJsRuntimeInvokeHandler(JsRuntimeMockMode.Loose);

            var result = await sut.ToJsRuntime().InvokeAsync<object>("ident", Array.Empty<object>());

            result.ShouldBe(default);
        }

        [Fact(DisplayName = "After invocation a invocation should be visible from the Invocations list")]
        public void Test002()
        {
            var identifier = "fooFunc";
            var args = new[] { "bar", "baz" };
            using var cts = new CancellationTokenSource();
            var sut = new MockJsRuntimeInvokeHandler(JsRuntimeMockMode.Loose);

            sut.ToJsRuntime().InvokeAsync<object>(identifier, cts.Token, args);

            var invocation = sut.Invocations[identifier].Single();
            invocation.Identifier.ShouldBe(identifier);
            invocation.Arguments.ShouldBe(args);
            invocation.CancellationToken.ShouldBe(cts.Token);
        }

        [Fact(DisplayName = "Mock throws exception when in strict mode and invocation has not been setup")]
        public async Task Test003()
        {
            var sut = new MockJsRuntimeInvokeHandler(JsRuntimeMockMode.Strict);
            var identifier = "func";
            var args = new[] { "bar", "baz" };

            var exception = await Should.ThrowAsync<UnplannedJsInvocationException>(sut.ToJsRuntime().InvokeVoidAsync(identifier, args).AsTask());
            exception.Invocation.Identifier.ShouldBe(identifier);
            exception.Invocation.Arguments.ShouldBe(args);

            exception = Should.Throw<UnplannedJsInvocationException>(() => sut.ToJsRuntime().InvokeAsync<object>(identifier, args));
            exception.Invocation.Identifier.ShouldBe(identifier);
            exception.Invocation.Arguments.ShouldBe(args);
        }

        [Fact(DisplayName = "Invocations receives before a planned invocation " +
                            "has result set receives the same result")]
        public async Task Test005()
        {
            var identifier = "func";
            var expectedResult = Guid.NewGuid();
            var sut = new MockJsRuntimeInvokeHandler(JsRuntimeMockMode.Strict);
            var plannedInvoke = sut.Setup<Guid>(identifier);

            var jsRuntime = sut.ToJsRuntime();
            var i1 = jsRuntime.InvokeAsync<Guid>(identifier);
            var i2 = jsRuntime.InvokeAsync<Guid>(identifier);

            plannedInvoke.SetResult(expectedResult);

            (await i1).ShouldBe(expectedResult);
            (await i2).ShouldBe(expectedResult);
        }

        [Fact(DisplayName = "Invocations receives after a planned invocation " +
                            "has result set does not receive the same result as " +
                            "the invocations before the result was set the first time")]
        public async Task Test006()
        {
            var identifier = "func";
            var sut = new MockJsRuntimeInvokeHandler(JsRuntimeMockMode.Strict);
            var plannedInvoke = sut.Setup<Guid>(identifier);
            var jsRuntime = sut.ToJsRuntime();

            var expectedResult1 = Guid.NewGuid();
            var i1 = jsRuntime.InvokeAsync<Guid>(identifier);
            plannedInvoke.SetResult(expectedResult1);

            var expectedResult2 = Guid.NewGuid();
            var i2 = jsRuntime.InvokeAsync<Guid>(identifier);
            plannedInvoke.SetResult(expectedResult2);

            (await i1).ShouldBe(expectedResult1);
            (await i2).ShouldBe(expectedResult2);
        }

        [Fact(DisplayName = "A planned invocation can be cancelled for any waiting received invocations.")]
        public void Test007()
        {
            var identifier = "func";
            var sut = new MockJsRuntimeInvokeHandler(JsRuntimeMockMode.Strict);
            var plannedInvoke = sut.Setup<Guid>(identifier);
            var invocation = sut.ToJsRuntime().InvokeAsync<Guid>(identifier);

            plannedInvoke.SetCanceled();

            invocation.IsCanceled.ShouldBeTrue();
        }

        [Fact(DisplayName = "A planned invocation can throw an exception for any waiting received invocations.")]
        public async Task Test008()
        {
            var identifier = "func";
            var sut = new MockJsRuntimeInvokeHandler(JsRuntimeMockMode.Strict);
            var plannedInvoke = sut.Setup<Guid>(identifier);
            var invocation = sut.ToJsRuntime().InvokeAsync<Guid>(identifier);
            var expectedException = new InvalidOperationException("TADA");

            plannedInvoke.SetException(expectedException);

            var actual = await Should.ThrowAsync<InvalidOperationException>(invocation.AsTask());
            actual.ShouldBe(expectedException);
            invocation.IsFaulted.ShouldBeTrue();
        }

        [Fact(DisplayName = "Invocations returns all from a planned invocation")]
        public void Test009()
        {
            var identifier = "func";
            var sut = new MockJsRuntimeInvokeHandler(JsRuntimeMockMode.Strict);
            var plannedInvoke = sut.Setup<Guid>(identifier, x => true);
            var i1 = sut.ToJsRuntime().InvokeAsync<Guid>(identifier, "first");
            var i2 = sut.ToJsRuntime().InvokeAsync<Guid>(identifier, "second");

            var invocations = plannedInvoke.Invocations;

            invocations.Count.ShouldBe(2);
            invocations[0].Arguments[0].ShouldBe("first");
            invocations[1].Arguments[0].ShouldBe("second");
        }

        [Fact(DisplayName = "Arguments used in Setup are matched with invocations")]
        public void Test010()
        {
            var sut = new MockJsRuntimeInvokeHandler(JsRuntimeMockMode.Strict);
            var planned = sut.Setup<object>("foo", "bar", 42);

            sut.ToJsRuntime().InvokeAsync<object>("foo", "bar", 42);

            Should.Throw<UnplannedJsInvocationException>(
                () => sut.ToJsRuntime().InvokeAsync<object>("foo", "bar", 41)
            );

            planned.Invocations.Count.ShouldBe(1);
            var invocation = planned.Invocations[0];
            invocation.Identifier.ShouldBe("foo");
            invocation.Arguments[0].ShouldBe("bar");
            invocation.Arguments[1].ShouldBe(42);
        }

        [Fact(DisplayName = "Argument matcher used in Setup are matched with invocations")]
        public void Test011()
        {
            var sut = new MockJsRuntimeInvokeHandler(JsRuntimeMockMode.Strict);
            var planned = sut.Setup<object>("foo", args => args.Count == 1);

            sut.ToJsRuntime().InvokeAsync<object>("foo", 42);

            Should.Throw<UnplannedJsInvocationException>(
                () => sut.ToJsRuntime().InvokeAsync<object>("foo", "bar", 42)
            );

            planned.Invocations.Count.ShouldBe(1);
            var invocation = planned.Invocations[0];
            invocation.Identifier.ShouldBe("foo");
            invocation.Arguments.Count.ShouldBe(1);
            invocation.Arguments[0].ShouldBe(42);
        }

        [Fact(DisplayName = "SetupVoid returns a planned invocation that does not take a result object")]
        public async Task Test012()
        {
            var identifier = "func";
            var sut = new MockJsRuntimeInvokeHandler(JsRuntimeMockMode.Strict);
            var plannedInvoke = sut.SetupVoid(identifier);

            var invocation = sut.ToJsRuntime().InvokeVoidAsync(identifier);
            plannedInvoke.SetVoidResult();

            await invocation;

            invocation.IsCompletedSuccessfully.ShouldBeTrue();
        }

        [Fact(DisplayName = "Arguments used in SetupVoid are matched with invocations")]
        public async Task Test013()
        {
            var sut = new MockJsRuntimeInvokeHandler(JsRuntimeMockMode.Strict);
            var planned = sut.SetupVoid("foo", "bar", 42);

            var i1 = sut.ToJsRuntime().InvokeVoidAsync("foo", "bar", 42);

            await Should.ThrowAsync<UnplannedJsInvocationException>(
                sut.ToJsRuntime().InvokeVoidAsync("foo", "bar", 41).AsTask()
            );

            planned.Invocations.Count.ShouldBe(1);
            var invocation = planned.Invocations[0];
            invocation.Identifier.ShouldBe("foo");
            invocation.Arguments[0].ShouldBe("bar");
            invocation.Arguments[1].ShouldBe(42);
        }

        [Fact(DisplayName = "Argument matcher used in SetupVoid are matched with invocations")]
        public async Task Test014()
        {
            var sut = new MockJsRuntimeInvokeHandler(JsRuntimeMockMode.Strict);
            var planned = sut.SetupVoid("foo", args => args.Count == 2);

            var i1 = sut.ToJsRuntime().InvokeVoidAsync("foo", "bar", 42);

            await Should.ThrowAsync<UnplannedJsInvocationException>(
                sut.ToJsRuntime().InvokeVoidAsync("foo", 42).AsTask()
            );

            await Should.ThrowAsync<UnplannedJsInvocationException>(
                sut.ToJsRuntime().InvokeVoidAsync("foo").AsTask()
             );

            planned.Invocations.Count.ShouldBe(1);
            var invocation = planned.Invocations[0];
            invocation.Identifier.ShouldBe("foo");
            invocation.Arguments.Count.ShouldBe(2);
            invocation.Arguments[0].ShouldBe("bar");
            invocation.Arguments[1].ShouldBe(42);
        }
    }
}
