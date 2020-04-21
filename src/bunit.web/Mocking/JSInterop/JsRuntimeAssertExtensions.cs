using AngleSharp.Dom;
using Bunit.Asserting;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace Bunit.Mocking.JSInterop
{
	/// <summary>
	/// Assert extensions for JsRuntimeMock
	/// </summary>
	public static class JsRuntimeAssertExtensions
	{
		/// <summary>
		/// Verifies that the <paramref name="identifier"/> was never invoked on the <paramref name="handler"/>.
		/// </summary>
		/// <param name="handler">Handler to verify against.</param>
		/// <param name="identifier">Identifier of invocation that should not have happened.</param>
		/// <param name="userMessage">A custom user message to display if the assertion fails.</param>
		public static void VerifyNotInvoke(this MockJsRuntimeInvokeHandler handler, string identifier, string? userMessage = null)
		{
			if (handler is null)
				throw new ArgumentNullException(nameof(handler));
			if (handler.Invocations.TryGetValue(identifier, out var invocations) && invocations.Count > 0)
			{
				throw new JsInvokeCountExpectedException(identifier, 0, invocations.Count, nameof(VerifyNotInvoke), userMessage);
			}
		}

		/// <summary>
		/// Verifies that the <paramref name="identifier"/> has been invoked one time.
		/// </summary>
		/// <param name="handler">Handler to verify against.</param>
		/// <param name="identifier">Identifier of invocation that should have been invoked.</param>
		/// <param name="userMessage">A custom user message to display if the assertion fails.</param>
		/// <returns>The <see cref="JsRuntimeInvocation"/>.</returns>
		public static JsRuntimeInvocation VerifyInvoke(this MockJsRuntimeInvokeHandler handler, string identifier, string? userMessage = null)
			=> VerifyInvoke(handler, identifier, 1, userMessage)[0];

		/// <summary>
		/// Verifies that the <paramref name="identifier"/> has been invoked <paramref name="calledTimes"/> times.
		/// </summary>
		/// <param name="handler">Handler to verify against.</param>
		/// <param name="identifier">Identifier of invocation that should have been invoked.</param>
		/// <param name="calledTimes">The number of times the invocation is expected to have been called.</param>
		/// <param name="userMessage">A custom user message to display if the assertion fails.</param>
		/// <returns>The <see cref="JsRuntimeInvocation"/>.</returns>
		public static IReadOnlyList<JsRuntimeInvocation> VerifyInvoke(this MockJsRuntimeInvokeHandler handler, string identifier, int calledTimes, string? userMessage = null)
		{
			if (handler is null)
				throw new ArgumentNullException(nameof(handler));

			if (calledTimes < 1)
				throw new ArgumentException($"Use {nameof(VerifyNotInvoke)} to verify an identifier has not been invoked.", nameof(calledTimes));

			if (!handler.Invocations.TryGetValue(identifier, out var invocations))
			{
				throw new JsInvokeCountExpectedException(identifier, calledTimes, 0, nameof(VerifyInvoke), userMessage);
			}

			if (invocations.Count != calledTimes)
			{
				throw new JsInvokeCountExpectedException(identifier, calledTimes, invocations.Count, nameof(VerifyInvoke), userMessage);
			}

			return invocations;
		}

		/// <summary>
		/// Verifies that an argument <paramref name="actualArgument"/>
		/// passed to an JsRuntime invocation is an <see cref="ElementReference"/>
		/// to the <paramref name="expectedTargetElement"/>.
		/// </summary>
		/// <param name="actualArgument">object to verify.</param>
		/// <param name="expectedTargetElement">expected targeted element.</param>
		public static void ShouldBeElementReferenceTo(this object actualArgument, IElement expectedTargetElement)
		{
			if (actualArgument is null)
				throw new ArgumentNullException(nameof(actualArgument));
			if (expectedTargetElement is null)
				throw new ArgumentNullException(nameof(expectedTargetElement));

			if (!(actualArgument is ElementReference elmRef))
				throw new ActualExpectedAssertException(actualArgument.GetType().Name, nameof(ElementReference), "Actual argument type", "Expected argument type", $"The argument was not an {nameof(ElementReference)}");

			var elmRefAttrName = Htmlizer.ELEMENT_REFERENCE_ATTR_NAME;
			var expectedId = expectedTargetElement.GetAttribute(elmRefAttrName);
			if (string.IsNullOrEmpty(expectedId) || !elmRef.Id.Equals(expectedId, StringComparison.Ordinal))
			{
				throw new ActualExpectedAssertException(expectedTargetElement.ToMarkupElementOnly(),
														$"{elmRefAttrName}=\"{elmRef.Id}\"",
														"Actual element reference",
														"Expected referenced element",
														"Element does not have a the expected element reference.");
			}
		}
	}
}
