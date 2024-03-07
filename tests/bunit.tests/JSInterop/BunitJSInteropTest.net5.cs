#if !NET9_0_OR_GREATER
#pragma warning disable CS0618 // This method is obsolete.
namespace Bunit.JSInterop;

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

	[Fact(DisplayName = "An IJSUnmarshalledRuntime invocation should return the correct result when passed one arguments.")]
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

	[Fact(DisplayName = "An IJSUnmarshalledRuntime invocation should return the correct result when passed two arguments.")]
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

	[Fact(DisplayName = "An IJSUnmarshalledRuntime invocation should return the correct result when passed three arguments.")]
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

	[Fact(DisplayName = "An IJSUnmarshalledRuntime invocation should return the correct result when passed zero arguments.")]
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

	[Theory(DisplayName = "When calling InvokeUnmarshalled(identifier), then the invocation should be visible from the Invocations list"), AutoData]
	public void Test310(string identifier)
	{
		var sut = CreateSut(JSRuntimeMode.Loose);
		var jsUnmarshalledRuntime = (IJSUnmarshalledRuntime)sut.JSRuntime;

		jsUnmarshalledRuntime.InvokeUnmarshalled<string>(identifier);

		sut.Invocations[identifier]
			.ShouldHaveSingleItem()
			.ShouldBe(new JSRuntimeInvocation(
				identifier,
				cancellationToken: null,
				Array.Empty<object>(),
				typeof(string),
				invocationMethodName: "InvokeUnmarshalled"));
	}

	[Theory(DisplayName = "When calling InvokeUnmarshalled(identifier, arg0), then the invocation should be visible from the Invocations list"), AutoData]
	public void Test306(string identifier, string arg0)
	{
		var sut = CreateSut(JSRuntimeMode.Loose);
		var jsUnmarshalledRuntime = (IJSUnmarshalledRuntime)sut.JSRuntime;

		jsUnmarshalledRuntime.InvokeUnmarshalled<string, string>(identifier, arg0);

		sut.Invocations[identifier]
			.ShouldHaveSingleItem()
			.ShouldBe(new JSRuntimeInvocation(
				identifier,
				cancellationToken: null,
				args: new[] { arg0 },
				resultType: typeof(string),
				invocationMethodName: "InvokeUnmarshalled"));
	}

	[Theory(DisplayName = "When calling InvokeUnmarshalled(identifier, arg0, arg1), then the invocation should be visible from the Invocations list"), AutoData]
	public void Test307(string identifier, string arg0, string arg1)
	{
		var sut = CreateSut(JSRuntimeMode.Loose);
		var jsUnmarshalledRuntime = (IJSUnmarshalledRuntime)sut.JSRuntime;

		jsUnmarshalledRuntime.InvokeUnmarshalled<string, string, string>(identifier, arg0, arg1);

		sut.Invocations[identifier]
			.ShouldHaveSingleItem()
			.ShouldBe(new JSRuntimeInvocation(
				identifier,
				cancellationToken: null,
				args: new[] { arg0, arg1 },
				resultType: typeof(string),
				invocationMethodName: "InvokeUnmarshalled"));
	}

	[Theory(DisplayName = "When calling InvokeUnmarshalled(identifier, arg0, arg1, arg2), then the invocation should be visible from the Invocations list"), AutoData]
	public void Test308(string identifier, string arg0, string arg1, string arg2)
	{
		var sut = CreateSut(JSRuntimeMode.Loose);
		var jsUnmarshalledRuntime = (IJSUnmarshalledRuntime)sut.JSRuntime;

		jsUnmarshalledRuntime.InvokeUnmarshalled<string, string, string, string>(
			identifier, arg0, arg1, arg2);

		sut.Invocations[identifier]
			.ShouldHaveSingleItem()
			.ShouldBe(new JSRuntimeInvocation(
				identifier,
				cancellationToken: null,
				args: new[] { arg0, arg1, arg2 },
				resultType: typeof(string),
				invocationMethodName: "InvokeUnmarshalled"));
	}
}
#endif