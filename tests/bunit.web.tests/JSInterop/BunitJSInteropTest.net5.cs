#if NET5_0
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bunit;
using Microsoft.JSInterop;
using Shouldly;
using Xunit;

namespace Bunit.JSInterop
{
	public partial class BunitJSInteropTest
	{
		[Fact(DisplayName = "Mock throws exception when in strict mode and IJSUnmarshalledRuntime invocation has not been setup with one argument")]
		public void Test047()
		{
			var sut = CreateSut(JSRuntimeMode.Strict);
			var identifier = "func";
			var args = new[] { "bar" };

			var exception = Should.Throw<JSRuntimeUnhandledInvocationException>(() => ((IJSUnmarshalledRuntime)sut.JSRuntime).InvokeUnmarshalled<string, object>(identifier, "bar"));
			exception.Invocation.Identifier.ShouldBe(identifier);
			exception.Invocation.Arguments.ShouldBe(args);
		}

		[Fact(DisplayName = "Mock throws exception when in strict mode and IJSUnmarshalledRuntime invocation has not been setup with two arguments")]
		public void Test048()
		{
			var sut = CreateSut(JSRuntimeMode.Strict);
			var identifier = "func";
			var args = new[] { "bar", "baz" };

			var exception = Should.Throw<JSRuntimeUnhandledInvocationException>(() => ((IJSUnmarshalledRuntime)sut.JSRuntime).InvokeUnmarshalled<string, string, object>(identifier, "bar", "baz"));
			exception.Invocation.Identifier.ShouldBe(identifier);
			exception.Invocation.Arguments.ShouldBe(args);
		}

		[Fact(DisplayName = "Mock throws exception when in strict mode and IJSUnmarshalledRuntime invocation has not been setup with three arguments")]
		public void Test049()
		{
			var sut = CreateSut(JSRuntimeMode.Strict);
			var identifier = "func";
			var args = new[] { "bar", "baz", "bau" };

			var exception = Should.Throw<JSRuntimeUnhandledInvocationException>(() => ((IJSUnmarshalledRuntime)sut.JSRuntime).InvokeUnmarshalled<string, string, string, object>(identifier, "bar", "baz", "bau"));
			exception.Invocation.Identifier.ShouldBe(identifier);
			exception.Invocation.Arguments.ShouldBe(args);
		}

		[Fact(DisplayName = "Mock throws exception when in strict mode and IJSUnmarshalledRuntime invocation has not been setup with zero arguments")]
		public void Test050()
		{
			var sut = CreateSut(JSRuntimeMode.Strict);
			var identifier = "func";

			var exception = Should.Throw<JSRuntimeUnhandledInvocationException>(() => ((IJSUnmarshalledRuntime)sut.JSRuntime).InvokeUnmarshalled<object>(identifier));
			exception.Invocation.Identifier.ShouldBe(identifier);
			exception.Invocation.Arguments.ShouldBeEmpty();
		}

		[Fact(DisplayName = "After IJSUnmarshalledRuntime invocation a invocation should be visible from the Invocations list when parsed one arguments.")]
		public void Test051()
		{
			var identifier = "fooFunc";
			var args = new[] { "bar"};
			var sut = CreateSut(JSRuntimeMode.Strict);

			var planned = sut.Setup<Guid>("fooFunc", args);
			planned.SetResult(Guid.NewGuid());

			var _ = ((IJSUnmarshalledRuntime)sut.JSRuntime).InvokeUnmarshalled<string, Guid>(identifier, "bar");

			var invocation = sut.Invocations[identifier].Single();
			invocation.Identifier.ShouldBe(identifier);
			invocation.Arguments.ShouldBe(args);
		}

		[Fact(DisplayName = "After IJSUnmarshalledRuntime invocation a invocation should be visible from the Invocations list when parsed two arguments.")]
		public void Test052()
		{
			var identifier = "fooFunc";
			var args = new[] { "bar", "baz" };
			var sut = CreateSut(JSRuntimeMode.Strict);

			var planned = sut.Setup<Guid>("fooFunc", args);
			planned.SetResult(Guid.NewGuid());

			var _ = ((IJSUnmarshalledRuntime)sut.JSRuntime).InvokeUnmarshalled<string, string, Guid>(identifier, "bar", "baz");

			var invocation = sut.Invocations[identifier].Single();
			invocation.Identifier.ShouldBe(identifier);
			invocation.Arguments.ShouldBe(args);
		}

		[Fact(DisplayName = "After IJSUnmarshalledRuntime invocation a invocation should be visible from the Invocations list when parsed three arguments.")]
		public void Test053()
		{
			var identifier = "fooFunc";
			var args = new[] { "bar", "baz", "boa" };
			var sut = CreateSut(JSRuntimeMode.Strict);

			var planned = sut.Setup<Guid>("fooFunc", args);
			planned.SetResult(Guid.NewGuid());

			var _ = ((IJSUnmarshalledRuntime)sut.JSRuntime).InvokeUnmarshalled<string, string, string, Guid>(identifier, "bar", "baz", "boa");

			var invocation = sut.Invocations[identifier].Single();
			invocation.Identifier.ShouldBe(identifier);
			invocation.Arguments.ShouldBe(args);
		}

		[Fact(DisplayName = "After IJSUnmarshalledRuntime invocation a invocation should be visible from the Invocations list when parsed zero arguments.")]
		public void Test054()
		{
			var identifier = "fooFunc";
			var args = new[] { "bar", "baz", "boa" };
			var sut = CreateSut(JSRuntimeMode.Strict);

			var planned = sut.Setup<Guid>("fooFunc", args);
			planned.SetResult(Guid.NewGuid());

			var _ = ((IJSUnmarshalledRuntime)sut.JSRuntime).InvokeUnmarshalled<string, string, string, Guid>(identifier, "bar", "baz", "boa");

			var invocation = sut.Invocations[identifier].Single();
			invocation.Identifier.ShouldBe(identifier);
			invocation.Arguments.ShouldBe(args);
		}

		[Fact(DisplayName = "An IJSUnmarshalledRuntime invocation should return the correct result when parsed one arguments.")]
		public void Test055()
		{
			var identifier = "fooFunc";
			var args = new[] { "bar" };
			var sut = CreateSut(JSRuntimeMode.Strict);

			var expectedResult = Guid.NewGuid();
			var planned = sut.Setup<Guid>("fooFunc", args);
			planned.SetResult(expectedResult);

			var actual = ((IJSUnmarshalledRuntime)sut.JSRuntime).InvokeUnmarshalled<string, Guid>(identifier, "bar");
			actual.ShouldBe(expectedResult);
		}

		[Fact(DisplayName = "An IJSUnmarshalledRuntime invocation should return the correct result when parsed two arguments.")]
		public void Test056()
		{
			var identifier = "fooFunc";
			var args = new[] { "bar", "baz" };
			var sut = CreateSut(JSRuntimeMode.Strict);

			var expectedResult = Guid.NewGuid();
			var planned = sut.Setup<Guid>("fooFunc", args);
			planned.SetResult(expectedResult);

			var actual = ((IJSUnmarshalledRuntime)sut.JSRuntime).InvokeUnmarshalled<string, string, Guid>(identifier, "bar", "baz");
			actual.ShouldBe(expectedResult);
		}

		[Fact(DisplayName = "An IJSUnmarshalledRuntime invocation should return the correct result when parsed three arguments.")]
		public void Test057()
		{
			var identifier = "fooFunc";
			var args = new[] { "bar", "baz", "bao" };
			var sut = CreateSut(JSRuntimeMode.Strict);

			var expectedResult = Guid.NewGuid();
			var planned = sut.Setup<Guid>("fooFunc", args);
			planned.SetResult(expectedResult);

			var actual = ((IJSUnmarshalledRuntime)sut.JSRuntime).InvokeUnmarshalled<string, string, string, Guid>(identifier, "bar", "baz", "bao");
			actual.ShouldBe(expectedResult);
		}

		[Fact(DisplayName = "An IJSUnmarshalledRuntime invocation should return the correct result when parsed zero arguments.")]
		public void Test058()
		{
			var identifier = "fooFunc";
			var sut = CreateSut(JSRuntimeMode.Strict);

			var expectedResult = Guid.NewGuid();
			var planned = sut.Setup<Guid>("fooFunc");
			planned.SetResult(expectedResult);

			var actual = ((IJSUnmarshalledRuntime)sut.JSRuntime).InvokeUnmarshalled<Guid>(identifier);
			actual.ShouldBe(expectedResult);
		}
	}
}
#endif
