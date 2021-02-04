using System;
using System.Collections.Generic;
using AngleSharp.Dom;
using Bunit.Asserting;
using Bunit.JSInterop;
using Bunit.JSInterop.InvocationHandlers;
using Microsoft.AspNetCore.Components;

namespace Bunit
{
	/// <summary>
	/// Assert extensions for JSRuntimeMock.
	/// </summary>
	public static class JSRuntimeAssertExtensions
	{
		/// <summary>
		/// Verifies that the <paramref name="identifier"/> was never invoked on the <paramref name="jsInterop"/>.
		/// </summary>
		/// <param name="jsInterop">The bUnit JSInterop to verify against.</param>
		/// <param name="identifier">Identifier of invocation that should not have happened.</param>
		/// <param name="userMessage">A custom user message to display if the assertion fails.</param>
		public static void VerifyNotInvoke(this BunitJSInterop jsInterop, string identifier, string? userMessage = null)
			=> VerifyNotInvoke(jsInterop?.Invocations ?? throw new ArgumentNullException(nameof(jsInterop)), identifier, userMessage);

		/// <summary>
		/// Verifies that the <paramref name="identifier"/> was never invoked on the <paramref name="handler"/>.
		/// </summary>
		/// <param name="handler">Handler to verify against.</param>
		/// <param name="identifier">Identifier of invocation that should not have happened.</param>
		/// <param name="userMessage">A custom user message to display if the assertion fails.</param>
		public static void VerifyNotInvoke<TResult>(this JSRuntimeInvocationHandlerBase<TResult> handler, string identifier, string? userMessage = null)
			=> VerifyNotInvoke(handler?.Invocations ?? throw new ArgumentNullException(nameof(handler)), identifier, userMessage);

		/// <summary>
		/// Verifies that the <paramref name="identifier"/> has been invoked one time.
		/// </summary>
		/// <param name="jsInterop">The bUnit JSInterop to verify against.</param>
		/// <param name="identifier">Identifier of invocation that should have been invoked.</param>
		/// <param name="userMessage">A custom user message to display if the assertion fails.</param>
		/// <returns>The <see cref="JSRuntimeInvocation"/>.</returns>
		public static JSRuntimeInvocation VerifyInvoke(this BunitJSInterop jsInterop, string identifier, string? userMessage = null)
			=> jsInterop.VerifyInvoke(identifier, 1, userMessage)[0];

		/// <summary>
		/// Verifies that the <paramref name="identifier"/> has been invoked <paramref name="calledTimes"/> times.
		/// </summary>
		/// <param name="jsInterop">The bUnit JSInterop to verify against.</param>
		/// <param name="identifier">Identifier of invocation that should have been invoked.</param>
		/// <param name="calledTimes">The number of times the invocation is expected to have been called.</param>
		/// <param name="userMessage">A custom user message to display if the assertion fails.</param>
		/// <returns>The <see cref="JSRuntimeInvocation"/>.</returns>
		public static IReadOnlyList<JSRuntimeInvocation> VerifyInvoke(this BunitJSInterop jsInterop, string identifier, int calledTimes, string? userMessage = null)
			=> VerifyInvoke(jsInterop?.Invocations ?? throw new ArgumentNullException(nameof(jsInterop)), identifier, calledTimes, userMessage);

		/// <summary>
		/// Verifies that the <paramref name="identifier"/> has been invoked one time.
		/// </summary>
		/// <param name="handler">Handler to verify against.</param>
		/// <param name="identifier">Identifier of invocation that should have been invoked.</param>
		/// <param name="userMessage">A custom user message to display if the assertion fails.</param>
		/// <returns>The <see cref="JSRuntimeInvocation"/>.</returns>
		public static JSRuntimeInvocation VerifyInvoke<TResult>(this JSRuntimeInvocationHandlerBase<TResult> handler, string identifier, string? userMessage = null)
			=> handler.VerifyInvoke(identifier, 1, userMessage)[0];

		/// <summary>
		/// Verifies that the <paramref name="identifier"/> has been invoked <paramref name="calledTimes"/> times.
		/// </summary>
		/// <param name="handler">Handler to verify against.</param>
		/// <param name="identifier">Identifier of invocation that should have been invoked.</param>
		/// <param name="calledTimes">The number of times the invocation is expected to have been called.</param>
		/// <param name="userMessage">A custom user message to display if the assertion fails.</param>
		/// <returns>The <see cref="JSRuntimeInvocation"/>.</returns>
		public static IReadOnlyList<JSRuntimeInvocation> VerifyInvoke<TResult>(this JSRuntimeInvocationHandlerBase<TResult> handler, string identifier, int calledTimes, string? userMessage = null)
			=> VerifyInvoke(handler?.Invocations ?? throw new ArgumentNullException(nameof(handler)), identifier, calledTimes, userMessage);

		/// <summary>
		/// Verifies that an argument <paramref name="actualArgument"/>
		/// passed to an JSRuntime invocation is an <see cref="ElementReference"/>
		/// to the <paramref name="expectedTargetElement"/>.
		/// </summary>
		/// <param name="actualArgument">object to verify.</param>
		/// <param name="expectedTargetElement">expected targeted element.</param>
		public static void ShouldBeElementReferenceTo(this object? actualArgument, IElement expectedTargetElement)
		{
			if (actualArgument is null)
				throw new ArgumentNullException(nameof(actualArgument));
			if (expectedTargetElement is null)
				throw new ArgumentNullException(nameof(expectedTargetElement));
			if (actualArgument is not ElementReference elmRef)
				throw new ActualExpectedAssertException(actualArgument.GetType().Name, nameof(ElementReference), "Actual argument type", "Expected argument type", $"The argument was not an {nameof(ElementReference)}");

			var elmRefAttrName = Htmlizer.ElementReferenceAttrName;
			var expectedId = expectedTargetElement.GetAttribute(elmRefAttrName);
			if (string.IsNullOrEmpty(expectedId) || !elmRef.Id.Equals(expectedId, StringComparison.Ordinal))
			{
				throw new ActualExpectedAssertException(
					expectedTargetElement.ToMarkupElementOnly(),
					$"{elmRefAttrName}=\"{elmRef.Id}\"",
					"Actual element reference",
					"Expected referenced element",
					"Element does not have a the expected element reference.");
			}
		}

		private static IReadOnlyList<JSRuntimeInvocation> VerifyInvoke(JSRuntimeInvocationDictionary allInvocations, string identifier, int calledTimes, string? userMessage = null)
		{
			if (string.IsNullOrWhiteSpace(identifier))
				throw new ArgumentException($"'{nameof(identifier)}' cannot be null or whitespace.", nameof(identifier));

			if (calledTimes < 1)
				throw new ArgumentException($"Use {nameof(VerifyNotInvoke)} to verify an identifier has not been invoked.", nameof(calledTimes));

			var invocations = allInvocations[identifier];

			if (invocations.Count == 0)
			{
				throw new JSInvokeCountExpectedException(identifier, calledTimes, 0, nameof(VerifyInvoke), userMessage);
			}

			if (invocations.Count != calledTimes)
			{
				throw new JSInvokeCountExpectedException(identifier, calledTimes, allInvocations.Count, nameof(VerifyInvoke), userMessage);
			}

			return invocations;
		}

		private static void VerifyNotInvoke(JSRuntimeInvocationDictionary allInvocations, string identifier, string? userMessage = null)
		{
			if (string.IsNullOrWhiteSpace(identifier))
				throw new ArgumentException($"'{nameof(identifier)}' cannot be null or whitespace.", nameof(identifier));

			var invocationCount = allInvocations[identifier].Count;

			if (invocationCount > 0)
			{
				throw new JSInvokeCountExpectedException(identifier, 0, invocationCount, nameof(VerifyNotInvoke), userMessage);
			}
		}
	}
}
