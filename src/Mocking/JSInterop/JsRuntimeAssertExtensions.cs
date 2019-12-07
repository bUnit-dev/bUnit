using AngleSharp.Dom;
using Egil.RazorComponents.Testing.Extensions;
using Egil.RazorComponents.Testing.Mocking.JsInterop;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Sdk;

namespace Egil.RazorComponents.Testing.Mocking.JSInterop
{
    public static class JsRuntimeAssertExtensions
    {
        public static void VerifyNotInvoke(this MockJsRuntimeInvokeHandler handler, string identifier, string? userMessage = null)
        {
            if (handler is null) throw new ArgumentNullException(nameof(handler));
            if (handler.Invocations.TryGetValue(identifier, out var invocations) && invocations.Count > 0)
            {
                JsInvokeCountExpectedException.ThrowJsInvokeCountExpectedException(identifier, 0, invocations.Count, nameof(VerifyNotInvoke), userMessage);
            }
        }

        public static JsRuntimeInvocation VerifyInvoke(this MockJsRuntimeInvokeHandler handler, string identifier) => VerifyInvoke(handler, identifier, 1)[0];

        public static IReadOnlyList<JsRuntimeInvocation> VerifyInvoke(this MockJsRuntimeInvokeHandler handler, string identifier, int calledTimes = 1, string? userMessage = null)
        {
            if (handler is null) throw new ArgumentNullException(nameof(handler));
            if (calledTimes < 1)
                throw new ArgumentException($"Use {nameof(VerifyNotInvoke)} to verify an identifier has not been invoked.", nameof(calledTimes));

            var invocations = handler.Invocations[identifier];

            if (invocations.Count != calledTimes)
            {
                JsInvokeCountExpectedException.ThrowJsInvokeCountExpectedException(identifier, calledTimes, invocations.Count, nameof(VerifyInvoke), userMessage);
            }

            return invocations;
        }

        public static void ShouldBeElementReferenceTo(this object actualArgument, IElement expectedTargetElement)
        {
            if (actualArgument is null) throw new ArgumentNullException(nameof(actualArgument));
            if (expectedTargetElement is null) throw new ArgumentNullException(nameof(expectedTargetElement));

            if (!(actualArgument is ElementReference elmRef))
            {
                throw new IsTypeException(typeof(ElementReference).FullName, actualArgument.GetType().FullName);
            }

            var elmRefAttrName = Htmlizer.ToBlazorAttribute("elementreference");
            var expectedId = expectedTargetElement.GetAttribute(elmRefAttrName);
            if (string.IsNullOrEmpty(expectedId) || !elmRef.Id.Equals(expectedId, StringComparison.Ordinal))
            {
                throw new AssertActualExpectedException($"{elmRefAttrName}=\"{elmRef.Id}\"",
                                                        expectedTargetElement.ToMarkupElementOnly(),
                                                        "Element does not have a the expected element reference.",
                                                        "Actual element reference",
                                                        "Expected referenced element");
            }
        }
    }
}