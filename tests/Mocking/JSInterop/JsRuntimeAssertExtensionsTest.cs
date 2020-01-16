using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Egil.RazorComponents.Testing.Diffing;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Moq;
using Shouldly;
using Xunit;
using Xunit.Sdk;

namespace Egil.RazorComponents.Testing.Mocking.JSInterop
{
    public class JsRuntimeAssertExtensionsTest
    {
        [Fact(DisplayName = "VerifyNotInvoke throws if handler is null")]
        public void Test001()
        {
            MockJsRuntimeInvokeHandler? handler = null;
            Should.Throw<ArgumentNullException>(() => JsRuntimeAssertExtensions.VerifyNotInvoke(handler!, ""));
        }

        [Fact(DisplayName = "VerifyNotInvoke throws JsInvokeCountExpectedException if identifier " +
                            "has been invoked one or more times")]
        public async Task Test002()
        {
            var identifier = "test";
            var handler = new MockJsRuntimeInvokeHandler();
            await handler.ToJsRuntime().InvokeVoidAsync(identifier);

            Should.Throw<JsInvokeCountExpectedException>(() => handler.VerifyNotInvoke(identifier));
        }

        [Fact(DisplayName = "VerifyNotInvoke throws JsInvokeCountExpectedException if identifier " +
                    "has been invoked one or more times, with custom error message")]
        public async Task Test003()
        {
            var identifier = "test";
            var errMsg = "HELLO WORLD";
            var handler = new MockJsRuntimeInvokeHandler();
            await handler.ToJsRuntime().InvokeVoidAsync(identifier);

            Should.Throw<JsInvokeCountExpectedException>(() => handler.VerifyNotInvoke(identifier, errMsg))
                .UserMessage.ShouldEndWith(errMsg);
        }

        [Fact(DisplayName = "VerifyNotInvoke does not throw if identifier has not been invoked")]
        public void Test004()
        {
            var handler = new MockJsRuntimeInvokeHandler();

            handler.VerifyNotInvoke("FOOBAR");
        }

        [Fact(DisplayName = "VerifyInvoke throws if handler is null")]
        public void Test100()
        {
            MockJsRuntimeInvokeHandler? handler = null;
            Should.Throw<ArgumentNullException>(() => JsRuntimeAssertExtensions.VerifyInvoke(handler!, ""));
            Should.Throw<ArgumentNullException>(() => JsRuntimeAssertExtensions.VerifyInvoke(handler!, "", 42));
        }

        [Fact(DisplayName = "VerifyInvoke throws invokeCount is less than 1")]
        public void Test101()
        {
            var handler = new MockJsRuntimeInvokeHandler();

            Should.Throw<ArgumentException>(() => handler.VerifyInvoke("", 0));
        }

        [Fact(DisplayName = "VerifyInvoke throws JsInvokeCountExpectedException when " +
                            "invocation count doesn't match the expected")]
        public async Task Test103()
        {
            var identifier = "test";
            var handler = new MockJsRuntimeInvokeHandler();
            await handler.ToJsRuntime().InvokeVoidAsync(identifier);

            var actual = Should.Throw<JsInvokeCountExpectedException>(() => handler.VerifyInvoke(identifier, 2));
            actual.ExpectedInvocationCount.ShouldBe(2);
            actual.ActualInvocationCount.ShouldBe(1);
            actual.Identifier.ShouldBe(identifier);
        }

        [Fact(DisplayName = "VerifyInvoke returns the invocation(s) if the expected count matched")]
        public async Task Test104()
        {
            var identifier = "test";
            var handler = new MockJsRuntimeInvokeHandler();
            await handler.ToJsRuntime().InvokeVoidAsync(identifier);

            var invocations = handler.VerifyInvoke(identifier, 1);
            invocations.ShouldBeSameAs(handler.Invocations[identifier]);

            var invocation = handler.VerifyInvoke(identifier);
            invocation.ShouldBe(handler.Invocations[identifier][0]);
        }

        [Fact(DisplayName = "ShouldBeElementReferenceTo throws if actualArgument or targeted element is null")]
        public void Test200()
        {
            Should.Throw<ArgumentNullException>(() => JsRuntimeAssertExtensions.ShouldBeElementReferenceTo(null!, null!))
                .ParamName.ShouldBe("actualArgument");
            Should.Throw<ArgumentNullException>(() => JsRuntimeAssertExtensions.ShouldBeElementReferenceTo(string.Empty, null!))
                .ParamName.ShouldBe("expectedTargetElement");
        }

        [Fact(DisplayName = "ShouldBeElementReferenceTo throws if actualArgument is not a ElementReference")]
        public void Test201()
        {
            var obj = new object();
            Should.Throw<IsTypeException>(() => obj.ShouldBeElementReferenceTo(Mock.Of<IElement>()));
        }

        [Fact(DisplayName = "ShouldBeElementReferenceTo throws if element reference does not point to the provided element")]
        public void Test202()
        {
            using var htmlParser = new TestHtmlParser();
            var elmRef = new ElementReference(Guid.NewGuid().ToString());
            var elm = htmlParser.Parse($"<p {Htmlizer.ELEMENT_REFERENCE_ATTR_NAME}=\"ASDF\" />").First() as IElement;

            Should.Throw<AssertActualExpectedException>(() => elmRef.ShouldBeElementReferenceTo(elm));

            var elmWithoutRefAttr = htmlParser.Parse($"<p />").First() as IElement;

            Should.Throw<AssertActualExpectedException>(() => elmRef.ShouldBeElementReferenceTo(elmWithoutRefAttr));
        }
    }
}
