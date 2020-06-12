using System;
using System.Linq;
using System.Threading.Tasks;

using AngleSharp.Dom;

using Bunit.Asserting;
using Bunit.Diffing;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using Moq;

using Shouldly;

using Xunit;

namespace Bunit.Mocking.JSInterop
{
	public class JSRuntimeAssertExtensionsTest
	{
		[Fact(DisplayName = "VerifyNotInvoke throws if handler is null")]
		public void Test001()
		{
			MockJSRuntimeInvokeHandler? handler = null;
			Should.Throw<ArgumentNullException>(() => JSRuntimeAssertExtensions.VerifyNotInvoke(handler!, ""));
		}

		[Fact(DisplayName = "VerifyNotInvoke throws JSInvokeCountExpectedException if identifier " +
							"has been invoked one or more times")]
		public async Task Test002()
		{
			var identifier = "test";
			var handler = new MockJSRuntimeInvokeHandler();
			await handler.ToJSRuntime().InvokeVoidAsync(identifier);

			Should.Throw<JSInvokeCountExpectedException>(() => handler.VerifyNotInvoke(identifier));
		}

		[Fact(DisplayName = "VerifyNotInvoke throws JSInvokeCountExpectedException if identifier " +
					"has been invoked one or more times, with custom error message")]
		public async Task Test003()
		{
			var identifier = "test";
			var errMsg = "HELLO WORLD";
			var handler = new MockJSRuntimeInvokeHandler();
			await handler.ToJSRuntime().InvokeVoidAsync(identifier);

			Should.Throw<JSInvokeCountExpectedException>(() => handler.VerifyNotInvoke(identifier, errMsg))
				.Message.ShouldContain(errMsg);
		}

		[Fact(DisplayName = "VerifyNotInvoke does not throw if identifier has not been invoked")]
		public void Test004()
		{
			var handler = new MockJSRuntimeInvokeHandler();

			handler.VerifyNotInvoke("FOOBAR");
		}

		[Fact(DisplayName = "VerifyInvoke throws if handler is null")]
		public void Test100()
		{
			MockJSRuntimeInvokeHandler? handler = null;
			Should.Throw<ArgumentNullException>(() => JSRuntimeAssertExtensions.VerifyInvoke(handler!, ""));
			Should.Throw<ArgumentNullException>(() => JSRuntimeAssertExtensions.VerifyInvoke(handler!, "", 42));
		}

		[Fact(DisplayName = "VerifyInvoke throws invokeCount is less than 1")]
		public void Test101()
		{
			var handler = new MockJSRuntimeInvokeHandler();

			Should.Throw<ArgumentException>(() => handler.VerifyInvoke("", 0));
		}

		[Fact(DisplayName = "VerifyInvoke throws JSInvokeCountExpectedException when " +
							"invocation count doesn't match the expected")]
		public async Task Test103()
		{
			var identifier = "test";
			var handler = new MockJSRuntimeInvokeHandler();
			await handler.ToJSRuntime().InvokeVoidAsync(identifier);

			var actual = Should.Throw<JSInvokeCountExpectedException>(() => handler.VerifyInvoke(identifier, 2));
			actual.ExpectedInvocationCount.ShouldBe(2);
			actual.ActualInvocationCount.ShouldBe(1);
			actual.Identifier.ShouldBe(identifier);
		}

		[Fact(DisplayName = "VerifyInvoke returns the invocation(s) if the expected count matched")]
		public async Task Test104()
		{
			var identifier = "test";
			var handler = new MockJSRuntimeInvokeHandler();
			await handler.ToJSRuntime().InvokeVoidAsync(identifier);

			var invocations = handler.VerifyInvoke(identifier, 1);
			invocations.ShouldBeSameAs(handler.Invocations[identifier]);

			var invocation = handler.VerifyInvoke(identifier);
			invocation.ShouldBe(handler.Invocations[identifier][0]);
		}

		[Fact(DisplayName = "ShouldBeElementReferenceTo throws if actualArgument or targeted element is null")]
		public void Test200()
		{
			Should.Throw<ArgumentNullException>(() => JSRuntimeAssertExtensions.ShouldBeElementReferenceTo(null!, null!))
				.ParamName.ShouldBe("actualArgument");
			Should.Throw<ArgumentNullException>(() => JSRuntimeAssertExtensions.ShouldBeElementReferenceTo(string.Empty, null!))
				.ParamName.ShouldBe("expectedTargetElement");
		}

		[Fact(DisplayName = "ShouldBeElementReferenceTo throws if actualArgument is not a ElementReference")]
		public void Test201()
		{
			var obj = new object();
			Should.Throw<ActualExpectedAssertException>(() => obj.ShouldBeElementReferenceTo(Mock.Of<IElement>()));
		}

		[Fact(DisplayName = "ShouldBeElementReferenceTo throws if element reference does not point to the provided element")]
		public void Test202()
		{
			using var htmlParser = new HtmlParser();
			var elmRef = new ElementReference(Guid.NewGuid().ToString());
			var elm = (IElement)htmlParser.Parse($"<p {Htmlizer.ELEMENT_REFERENCE_ATTR_NAME}=\"ASDF\" />").First();

			Should.Throw<ActualExpectedAssertException>(() => elmRef.ShouldBeElementReferenceTo(elm));

			var elmWithoutRefAttr = (IElement)htmlParser.Parse($"<p />").First();

			Should.Throw<ActualExpectedAssertException>(() => elmRef.ShouldBeElementReferenceTo(elmWithoutRefAttr));
		}
	}
}
