using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Egil.RazorComponents.Testing.Mocking.JSInterop
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
        public void Test003()
        {
            var sut = new MockJsRuntimeInvokeHandler(JsRuntimeMockMode.Strict);

            Should.Throw<UnplannedJsInvocationException>(() => sut.ToJsRuntime().InvokeAsync<object>("ident", new[] { "bar", "baz" }));
        }

        [Fact(DisplayName = "Mock returns task from planned invocation when one is present")]
        public async Task Test004()
        {
            var expectedResult = "HELLO WORLD";
            var ident = "fooFunc";
            var sut = new MockJsRuntimeInvokeHandler(JsRuntimeMockMode.Strict);
            sut.Setup<string>(ident).SetResult(expectedResult);

            var result = await sut.ToJsRuntime().InvokeAsync<string>(ident, Array.Empty<string>());

            result.ShouldBe(expectedResult);
        }
    }
}
