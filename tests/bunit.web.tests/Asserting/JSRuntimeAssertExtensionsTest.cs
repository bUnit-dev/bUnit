using System;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Bunit.JSInterop;
using Bunit.JSInterop.InvocationHandlers;
using Bunit.Rendering;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Moq;
using Shouldly;
using Xunit;

namespace Bunit.Asserting
{
	public class JSRuntimeAssertExtensionsTest
	{
		private static BunitJSInterop CreateSut(JSRuntimeMode mode = JSRuntimeMode.Loose) => new BunitJSInterop { Mode = mode };

		[Fact(DisplayName = "BunitJSInterop.VerifyNotInvoke throws if handler is null")]
		public void Test001()
		{
			Should.Throw<ArgumentNullException>(() => default(BunitJSInterop)!.VerifyNotInvoke(string.Empty));
		}

		[Fact(DisplayName = "BunitJSInterop.VerifyNotInvoke throws JSInvokeCountExpectedException if identifier " +
							"has been invoked one or more times")]
		public async Task Test002()
		{
			var identifier = "test";
			var sut = CreateSut();

			await sut.JSRuntime.InvokeVoidAsync(identifier);

			Should.Throw<JSInvokeCountExpectedException>(() => sut.VerifyNotInvoke(identifier));
		}

		[Fact(DisplayName = "BunitJSInterop.VerifyNotInvoke throws JSInvokeCountExpectedException if identifier " +
							"has been invoked one or more times, with custom error message")]
		public async Task Test003()
		{
			var identifier = "test";
			var errMsg = "HELLO WORLD";
			var sut = CreateSut();

			await sut.JSRuntime.InvokeVoidAsync(identifier);

			Should.Throw<JSInvokeCountExpectedException>(() => sut.VerifyNotInvoke(identifier, errMsg))
				.Message.ShouldContain(errMsg);
		}

		[Fact(DisplayName = "BunitJSInterop.VerifyNotInvoke does not throw if identifier has not been invoked")]
		public void Test004()
		{
			var sut = CreateSut();
			sut.VerifyNotInvoke("FOOBAR");
		}

		[Fact(DisplayName = "BunitJSInterop.VerifyInvoke throws if handler is null")]
		public void Test100()
		{
			BunitJSInterop? sut = null;
			Should.Throw<ArgumentNullException>(() => (sut!).VerifyInvoke(string.Empty));
			Should.Throw<ArgumentNullException>(() => (sut!).VerifyInvoke(string.Empty, 42));
		}

		[Fact(DisplayName = "BunitJSInterop.VerifyInvoke throws invokeCount is less than 1")]
		public void Test101()
		{
			var sut = CreateSut();

			Should.Throw<ArgumentException>(() => sut.VerifyInvoke(string.Empty, 0));
		}

		[Fact(DisplayName = "BunitJSInterop.VerifyInvoke throws JSInvokeCountExpectedException when " +
							"invocation count doesn't match the expected")]
		public async Task Test103()
		{
			var sut = CreateSut();
			var identifier = "test";
			await sut.JSRuntime.InvokeVoidAsync(identifier);

			var actual = Should.Throw<JSInvokeCountExpectedException>(() => sut.VerifyInvoke(identifier, 2));
			actual.ExpectedInvocationCount.ShouldBe(2);
			actual.ActualInvocationCount.ShouldBe(1);
			actual.Identifier.ShouldBe(identifier);
		}

		[Fact(DisplayName = "BunitJSInterop.VerifyInvoke returns the invocation(s) if the expected count matched")]
		public async Task Test104()
		{
			var sut = CreateSut();
			var identifier = "test";
			await sut.JSRuntime.InvokeVoidAsync(identifier);

			var invocations = sut.VerifyInvoke(identifier, 1);
			invocations.ShouldBeSameAs(sut.Invocations[identifier]);

			var invocation = sut.VerifyInvoke(identifier);
			invocation.ShouldBe(sut.Invocations[identifier][0]);
		}

		[Fact(DisplayName = "ShouldBeElementReferenceTo throws if actualArgument or targeted element is null")]
		public void Test200()
		{
			Should.Throw<ArgumentNullException>(() => JSRuntimeAssertExtensions.ShouldBeElementReferenceTo(null!, null!))
				.ParamName.ShouldBe("actualArgument");
			Should.Throw<ArgumentNullException>(() => string.Empty.ShouldBeElementReferenceTo(null!))
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
			using var htmlParser = new BunitHtmlParser();
			var elmRef = new ElementReference(Guid.NewGuid().ToString());
			var elm = (IElement)htmlParser.Parse($"<p {Htmlizer.ElementReferenceAttrName}=\"ASDF\" />").First();

			Should.Throw<ActualExpectedAssertException>(() => elmRef.ShouldBeElementReferenceTo(elm));

			var elmWithoutRefAttr = (IElement)htmlParser.Parse($"<p />").First();

			Should.Throw<ActualExpectedAssertException>(() => elmRef.ShouldBeElementReferenceTo(elmWithoutRefAttr));
		}

		[Fact(DisplayName = "JSRuntimeInvocationHandler.VerifyNotInvoke throws if handler is null")]
		public void Test301()
		{
			Should.Throw<ArgumentNullException>(() => default(JSRuntimeInvocationHandler)!.VerifyNotInvoke(string.Empty));
		}

		[Fact(DisplayName = "JSRuntimeInvocationHandler.VerifyNotInvoke throws JSInvokeCountExpectedException if identifier " +
					"has been invoked one or more times")]
		public async Task Test302()
		{
			var identifier = "test";
			var sut = CreateSut();
			var handler = sut.SetupVoid().SetVoidResult();

			await sut.JSRuntime.InvokeVoidAsync(identifier);

			Should.Throw<JSInvokeCountExpectedException>(() => handler.VerifyNotInvoke(identifier));
		}

		[Fact(DisplayName = "JSRuntimeInvocationHandler.VerifyNotInvoke throws JSInvokeCountExpectedException if identifier " +
					"has been invoked one or more times, with custom error message")]
		public async Task Test303()
		{
			var identifier = "test";
			var errMsg = "HELLO WORLD";
			var sut = CreateSut();
			var handler = sut.SetupVoid().SetVoidResult();

			await sut.JSRuntime.InvokeVoidAsync(identifier);

			Should.Throw<JSInvokeCountExpectedException>(() => handler.VerifyNotInvoke(identifier, errMsg))
				.Message.ShouldContain(errMsg);
		}

		[Fact(DisplayName = "JSRuntimeInvocationHandler.VerifyNotInvoke does not throw if identifier has not been invoked")]
		public void Test304()
		{
			var sut = CreateSut();
			var handler = sut.SetupVoid().SetVoidResult();

			handler.VerifyNotInvoke("FOOBAR");
		}

		[Fact(DisplayName = "JSRuntimeInvocationHandler.VerifyInvoke throws if handler is null")]
		public void Test305()
		{
			Should.Throw<ArgumentNullException>(() => default(JSRuntimeInvocationHandler)!.VerifyInvoke(string.Empty));
			Should.Throw<ArgumentNullException>(() => default(JSRuntimeInvocationHandler)!.VerifyInvoke(string.Empty, 42));
		}

		[Fact(DisplayName = "JSRuntimeInvocationHandler.VerifyInvoke throws invokeCount is less than 1")]
		public void Test306()
		{
			var sut = CreateSut();
			var handler = sut.SetupVoid().SetVoidResult();

			Should.Throw<ArgumentException>(() => handler.VerifyInvoke(string.Empty, 0));
		}

		[Fact(DisplayName = "JSRuntimeInvocationHandler.VerifyInvoke throws JSInvokeCountExpectedException when " +
							"invocation count doesn't match the expected")]
		public async Task Test307()
		{
			var sut = CreateSut();
			var identifier = "test";
			var handler = sut.SetupVoid().SetVoidResult();

			await sut.JSRuntime.InvokeVoidAsync(identifier);

			var actual = Should.Throw<JSInvokeCountExpectedException>(() => handler.VerifyInvoke(identifier, 2));
			actual.ExpectedInvocationCount.ShouldBe(2);
			actual.ActualInvocationCount.ShouldBe(1);
			actual.Identifier.ShouldBe(identifier);
		}

		[Fact(DisplayName = "JSRuntimeInvocationHandler.VerifyInvoke returns the invocation(s) if the expected count matched")]
		public async Task Test308()
		{
			var sut = CreateSut();
			var identifier = "test";
			var handler = sut.SetupVoid().SetVoidResult();

			await sut.JSRuntime.InvokeVoidAsync(identifier);

			var invocations = handler.VerifyInvoke(identifier, 1);
			invocations.ShouldBeSameAs(handler.Invocations[identifier]);

			var invocation = handler.VerifyInvoke(identifier);
			invocation.ShouldBe(handler.Invocations[identifier][0]);
		}
	}
}
