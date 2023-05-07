using Bunit.JSInterop.InvocationHandlers.Implementation;
using Microsoft.JSInterop.Implementation;

#pragma warning disable CS0618 // This method is obsolete.

namespace Bunit.JSInterop;

public class BunitJSObjectReferenceTest : TestContext
{
	private static readonly Type JSVoidResultType =
			typeof(Microsoft.JSInterop.Infrastructure.IJSVoidResult);

	[Theory(DisplayName = "Calling Setup<JSObjectReference> or Setup<IJSObjectReference> throws")]
	[InlineData("import", null)]
	[InlineData("import", "file.js")]
	[InlineData("customImport", null)]
	[InlineData("customImport", "file.js")]
	public void Test001(string identifier, object arg1)
	{
		Should.Throw<ArgumentException>(() => JSInterop.Setup<JSObjectReference>(identifier, arg1));
		Should.Throw<ArgumentException>(() => JSInterop.Setup<IJSObjectReference>(identifier, arg1));
	}

	[Theory(DisplayName = "Calling SetupModule(null) or SetupModule(empty string) throws")]
	[InlineData(null)]
	[InlineData("")]
	[InlineData("  ")]
	public void Test002(string url)
		=> Should.Throw<ArgumentException>(() => JSInterop.SetupModule(url));

	[Fact(DisplayName = "Calling SetupModule(jsInterop, identifier, invocationMatcher) with any null values throws")]
	public void Test003()
	{
		Should.Throw<ArgumentNullException>(() => default(BunitJSInterop)!.SetupModule("identifier", _ => true));
	}

	[Fact(DisplayName = "Calling SetupModule(uri) registers handler for module JS Interop")]
	public void Test010()
	{
		JSInterop.SetupModule("FOO.js");

		JSInterop.TryGetInvokeHandler<IJSObjectReference>("import", "FOO.js")
			.ShouldBeOfType<JSObjectReferenceInvocationHandler>();
	}

	[Fact(DisplayName = "Calling SetupModule(invocationMatcher) registers handler for module JS Interop")]
	public void Test011()
	{
		JSInterop.SetupModule(_ => true);

		JSInterop.TryGetInvokeHandler<IJSObjectReference>("import", "FOO.js")
			.ShouldBeOfType<JSObjectReferenceInvocationHandler>();
	}

	[Fact(DisplayName = "Calling the catch-all SetupModule() registers handler for module JS Interop")]
	public void Test012()
	{
		JSInterop.SetupModule();

		JSInterop.TryGetInvokeHandler<IJSObjectReference>("foo")
			.ShouldBeOfType<JSObjectReferenceInvocationHandler>();
	}

	[Fact(DisplayName = "Calling SetupModule(customImport, args) registers handler for module JS Interop")]
	public void Test013()
	{
		JSInterop.SetupModule("foo", Array.Empty<object>());

		JSInterop.TryGetInvokeHandler<IJSObjectReference>("foo")
			.ShouldBeOfType<JSObjectReferenceInvocationHandler>();
	}

	[Fact(DisplayName = "Handler for specific module name returns IJSObjectReference when receiving matching invocation")]
	public async Task Test020()
	{
		var moduleName = "FOO.js";
		JSInterop.SetupModule(moduleName);

		var module = await JSInterop.JSRuntime.InvokeAsync<IJSObjectReference>("import", moduleName);

		module.ShouldNotBeNull();
	}

	[Theory(DisplayName = "Handler for specific module name doesn't match other module names")]
	[InlineData(null)]
	[InlineData("")]
	[InlineData("BAR.js")]
	public void Test021(string requestedRoduleName)
	{
		JSInterop.SetupModule("FOO.js");

		Should.Throw<JSRuntimeUnhandledInvocationException>(() => JSInterop.JSRuntime.InvokeAsync<IJSObjectReference>("import", requestedRoduleName));
	}

	[Fact(DisplayName = "Handler for matcher returns IJSObjectReference when receiving matching invocation")]
	public async Task Test022()
	{
		var moduleName = "FOO.js";
		JSInterop.SetupModule(x => x.Arguments?[0]?.ToString() == moduleName);

		var module = await JSInterop.JSRuntime.InvokeAsync<IJSObjectReference>("import", moduleName);

		module.ShouldBeAssignableTo<IJSObjectReference>();
	}

	[Fact(DisplayName = "Handler for matcher returns IJSObjectReference when receiving matching invocation")]
	public async Task Test026()
	{
		var moduleName = "FOO.js";
		JSInterop.SetupModule("customImport", new[] { moduleName });

		var module = await JSInterop.JSRuntime.InvokeAsync<IJSObjectReference>("customImport", moduleName);

		module.ShouldBeAssignableTo<IJSObjectReference>();
	}

	[Theory(DisplayName = "Catch-all handler returns IJSObjectReference for all non-empty module names")]
	[InlineData("import", "FOO.js")]
	[InlineData("customImport", null)]
	[InlineData("customImport", "BAR.js")]
	public async Task Test023(string identifier, object? arg1)
	{
		JSInterop.SetupModule();

		var module = await JSInterop.JSRuntime.InvokeAsync<IJSObjectReference>(identifier, new[] { arg1 });

		module.ShouldBeAssignableTo<IJSObjectReference>();
	}

	[Theory(DisplayName = "JSInterop in loose mode returns IJSObjectReference for all non-empty module names without explicit SetupModule call")]
	[InlineData("import", "FOO.js")]
	[InlineData("customImport", null)]
	[InlineData("customImport", "BAR.js")]
	public async Task Test025(string identifier, object? arg1)
	{
		JSInterop.Mode = JSRuntimeMode.Loose;

		var module = await JSInterop.JSRuntime.InvokeAsync<IJSObjectReference>(identifier, new[] { arg1 });

		module.ShouldBeAssignableTo<IJSObjectReference>();
	}

	[Fact(DisplayName = "Module JSInterop inherits the root JSInterop's Mode")]
	public void Test030()
	{
		JSInterop.Mode = JSRuntimeMode.Loose;
		JSInterop.SetupModule().Mode.ShouldBe(JSInterop.Mode);

		JSInterop.Mode = JSRuntimeMode.Strict;
		JSInterop.SetupModule("foo.js").Mode.ShouldBe(JSInterop.Mode);
	}

	[Fact(DisplayName = "Changing mode in root JSInterop changes it in module JSInterop when it's not been set explicitly there")]
	public void Test031()
	{
		var moduleJSInterop = JSInterop.SetupModule();

		JSInterop.Mode = JSRuntimeMode.Loose;
		moduleJSInterop.Mode.ShouldBe(JSInterop.Mode);

		JSInterop.Mode = JSRuntimeMode.Strict;
		moduleJSInterop.Mode.ShouldBe(JSInterop.Mode);
	}

	[Fact(DisplayName = "Changing mode on module JSInterop breaks inherited mode from root JSInterop")]
	public void Test032()
	{
		var moduleJSInterop = JSInterop.SetupModule();
		moduleJSInterop.Mode = JSRuntimeMode.Strict;

		JSInterop.Mode = JSRuntimeMode.Loose;

		moduleJSInterop.Mode.ShouldBe(JSRuntimeMode.Strict);
	}

	[Fact(DisplayName = "InvokeAsync<TValue> on module in loose mode returns default TValue when no matching module JSInterops are registered")]
	public async Task Test040()
	{
		JSInterop.Mode = JSRuntimeMode.Loose;
		var module = await JSInterop.JSRuntime.InvokeAsync<IJSObjectReference>("import", "FOO.js");

		var actual = await module.InvokeAsync<string>("helloWorld");

		actual.ShouldBe(default(string));
	}

	[Fact(DisplayName = "InvokeAsync<IJSObjectReference> calls is registered in the root JSInterop Invocations list only")]
	public async Task Test050()
	{
		var moduleJSInterop = JSInterop.SetupModule("FOO.js");

		await JSInterop.JSRuntime.InvokeAsync<IJSObjectReference>("import", "FOO.js");

		JSInterop.Invocations
			.ShouldHaveSingleItem()
			.ShouldSatisfyAllConditions(
				x => x.Identifier.ShouldBe("import"),
				x => x.Arguments.ShouldHaveSingleItem().ShouldBe("FOO.js"));

		moduleJSInterop.Invocations.ShouldBeEmpty();
	}

	[Fact(DisplayName = "Module.Invocation is registered in both module and root JSInterop")]
	public async Task Test055()
	{
		JSInterop.Mode = JSRuntimeMode.Loose;
		var moduleJSInterop = JSInterop.SetupModule("FOO.js");
		var module = await JSInterop.JSRuntime.InvokeAsync<IJSObjectReference>("import", "FOO.js");

		await module.InvokeAsync<string>("helloWorld");

		moduleJSInterop.Invocations
			.ShouldHaveSingleItem()
			.Identifier.ShouldBe("helloWorld");

		JSInterop.Invocations
			.Last()
			.Identifier.ShouldBe("helloWorld");
	}

	[Fact(DisplayName = "TryGetModuleJSInterop returns registered module handler when called with parameters that the handler matches with")]
	public void Test060()
	{
		var expected = JSInterop.SetupModule("FOO.js");

		var actual = JSInterop.TryGetModuleJSInterop("import", "FOO.js");

		actual.ShouldBe(expected);
	}

	[Fact(DisplayName = "TryGetModuleJSInterop returns null when called with parameters that the handler does not matches with")]
	public void Test061()
	{
		JSInterop.SetupModule("FOO.js");

		var actual = JSInterop.TryGetModuleJSInterop("import", "BAR.js");

		actual.ShouldBeNull();
	}

	[Fact(DisplayName = "IJSObjectReference can be cast to IJSInProcessObjectReference")]
	public void Test070()
	{
		JSInterop.Mode = JSRuntimeMode.Loose;

		var jsRuntime = GetBunitJSObjectReference();

		jsRuntime.ShouldBeAssignableTo<IJSInProcessObjectReference>();
	}

	[Fact(DisplayName = "IJSObjectReference can be cast to IJSUnmarshalledObjectReference")]
	public void Test071()
	{
		JSInterop.Mode = JSRuntimeMode.Loose;

		var jsRuntime = GetBunitJSObjectReference();

		jsRuntime.ShouldBeAssignableTo<IJSUnmarshalledObjectReference>();
	}

	[Fact(DisplayName = "IJSInProcessObjectReference-invocations is handled by handlers from BunitJSInterop")]
	public async Task Test080()
	{
		JSInterop.Mode = JSRuntimeMode.Loose;
		var jsInProcess = (IJSInProcessObjectReference)GetBunitJSObjectReference();

		await jsInProcess.InvokeAsync<string>("bar1");
		await jsInProcess.InvokeAsync<string>("bar2", "baz");
		await jsInProcess.InvokeVoidAsync("bar3");
		await jsInProcess.InvokeVoidAsync("bar4", "baz");
		jsInProcess.Invoke<string>("bar5");
		jsInProcess.Invoke<string>("bar6", "baz");

		JSInterop.VerifyInvoke("bar1");
		JSInterop.VerifyInvoke("bar2").Arguments.ShouldBe(new[] { "baz" });
		JSInterop.VerifyInvoke("bar3");
		JSInterop.VerifyInvoke("bar4").Arguments.ShouldBe(new[] { "baz" });
		JSInterop.VerifyInvoke("bar5");
		JSInterop.VerifyInvoke("bar6").Arguments.ShouldBe(new[] { "baz" });
	}

	[Fact(DisplayName = "IJSUnmarshalledObjectReference-invocations is handled by handlers from BunitJSInterop")]
	public async Task Test081()
	{
		JSInterop.Mode = JSRuntimeMode.Loose;
		var jsUnmarshalled = (IJSUnmarshalledObjectReference)GetBunitJSObjectReference();

		await jsUnmarshalled.InvokeAsync<string>("bar1");
		await jsUnmarshalled.InvokeAsync<string>("bar2", "baz");
		await jsUnmarshalled.InvokeVoidAsync("bar3");
		await jsUnmarshalled.InvokeVoidAsync("bar4", "baz");
		jsUnmarshalled.Invoke<string>("bar5");
		jsUnmarshalled.Invoke<string>("bar6", "baz");
		jsUnmarshalled.InvokeUnmarshalled<string>("bar7");
		jsUnmarshalled.InvokeUnmarshalled<string, string>("bar8", "baz");
		jsUnmarshalled.InvokeUnmarshalled<string, string, string>("bar9", "baz", "boo");
		jsUnmarshalled.InvokeUnmarshalled<string, string, string, string>("bar10", "baz", "boo", "bah");

		JSInterop.VerifyInvoke("bar1");
		JSInterop.VerifyInvoke("bar2").Arguments.ShouldBe(new[] { "baz" });
		JSInterop.VerifyInvoke("bar3");
		JSInterop.VerifyInvoke("bar4").Arguments.ShouldBe(new[] { "baz" });
		JSInterop.VerifyInvoke("bar5");
		JSInterop.VerifyInvoke("bar6").Arguments.ShouldBe(new[] { "baz" });
		JSInterop.VerifyInvoke("bar7");
		JSInterop.VerifyInvoke("bar8").Arguments.ShouldBe(new[] { "baz" });
		JSInterop.VerifyInvoke("bar9").Arguments.ShouldBe(new[] { "baz", "boo" });
		JSInterop.VerifyInvoke("bar10").Arguments.ShouldBe(new[] { "baz", "boo", "bah" });
	}

	[Theory(DisplayName = "When calling InvokeUnmarshalled(identifier), then the invocation should be visible from the Invocations list"), AutoData]
	public void Test310(string identifier)
	{
		JSInterop.Mode = JSRuntimeMode.Loose;
		var jsUnmarshalledRuntime = (IJSUnmarshalledObjectReference)GetBunitJSObjectReference();

		jsUnmarshalledRuntime.InvokeUnmarshalled<string>(identifier);

		JSInterop.Invocations[identifier]
			.ShouldHaveSingleItem()
			.ShouldBe(new JSRuntimeInvocation(
				identifier,
				cancellationToken: null,
				args: Array.Empty<object>(),
				resultType: typeof(string),
				invocationMethodName: "InvokeUnmarshalled"));
	}

	[Theory(DisplayName = "When calling InvokeUnmarshalled(identifier, arg0), then the invocation should be visible from the Invocations list"), AutoData]
	public void Test306(string identifier, string arg0)
	{
		JSInterop.Mode = JSRuntimeMode.Loose;
		var jsUnmarshalledRuntime = (IJSUnmarshalledObjectReference)GetBunitJSObjectReference();

		jsUnmarshalledRuntime.InvokeUnmarshalled<string, string>(identifier, arg0);

		JSInterop.Invocations[identifier]
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
		JSInterop.Mode = JSRuntimeMode.Loose;
		var jsUnmarshalledRuntime = (IJSUnmarshalledObjectReference)GetBunitJSObjectReference();

		jsUnmarshalledRuntime.InvokeUnmarshalled<string, string, string>(identifier, arg0, arg1);

		JSInterop.Invocations[identifier]
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
		JSInterop.Mode = JSRuntimeMode.Loose;
		var jsUnmarshalledRuntime = (IJSUnmarshalledObjectReference)GetBunitJSObjectReference();

		jsUnmarshalledRuntime.InvokeUnmarshalled<string, string, string, string>(
			identifier, arg0, arg1, arg2);

		JSInterop.Invocations[identifier]
			.ShouldHaveSingleItem()
			.ShouldBe(new JSRuntimeInvocation(
				identifier,
				cancellationToken: null,
				args: new[] { arg0, arg1, arg2 },
				resultType: typeof(string),
				invocationMethodName: "InvokeUnmarshalled"));
	}

	[Theory(DisplayName = "When calling InvokeVoidAsync, then the invocation should be visible from the Invocations list"), AutoData]
	public void Test302(string identifier, string[] args, CancellationToken cancellationToken)
	{
		JSInterop.Mode = JSRuntimeMode.Loose;
		var sut = GetBunitJSObjectReference();

		sut.InvokeVoidAsync(identifier, cancellationToken, args);

		JSInterop.Invocations[identifier]
			.ShouldHaveSingleItem()
			.ShouldBe(new JSRuntimeInvocation(
				identifier,
				cancellationToken,
				args,
				JSVoidResultType,
				"InvokeVoidAsync"));
	}

	[Theory(DisplayName = "When calling InvokeAsync, then the invocation should be visible from the Invocations list"), AutoData]
	public void Test303(string identifier, string[] args, CancellationToken cancellationToken)
	{
		JSInterop.Mode = JSRuntimeMode.Loose;
		var sut = GetBunitJSObjectReference();

		sut.InvokeAsync<string>(identifier, cancellationToken, args);

		JSInterop.Invocations[identifier]
			.ShouldHaveSingleItem()
			.ShouldBe(new JSRuntimeInvocation(
				identifier,
				cancellationToken,
				args,
				typeof(string),
				"InvokeAsync"));
	}

	[Theory(DisplayName = "When calling InvokeVoid, then the invocation should be visible from the Invocations list"), AutoData]
	public void Test304(string identifier, string[] args)
	{
		JSInterop.Mode = JSRuntimeMode.Loose;
		var sut = GetBunitJSObjectReference();

		sut.InvokeVoid(identifier, args);

		JSInterop.Invocations[identifier]
			.ShouldHaveSingleItem()
			.ShouldBe(new JSRuntimeInvocation(
				identifier,
				cancellationToken: null,
				args: args,
				resultType: JSVoidResultType,
				invocationMethodName: "InvokeVoid"));
	}

	[Theory(DisplayName = "When calling Invoke, then the invocation should be visible from the Invocations list"), AutoData]
	public void Test305(string identifier, string[] args)
	{
		JSInterop.Mode = JSRuntimeMode.Loose;
		var sut = GetBunitJSObjectReference();

		sut.Invoke<int>(identifier, args);

		JSInterop.Invocations[identifier]
			.ShouldHaveSingleItem()
			.ShouldBe(new JSRuntimeInvocation(
				identifier,
				cancellationToken: null,
				args: args,
				resultType: typeof(int),
				invocationMethodName: "Invoke"));
	}

	private BunitJSObjectReference GetBunitJSObjectReference()
		=> (BunitJSObjectReference)JSInterop.JSRuntime.InvokeAsync<IJSObjectReference>("FOO.js").Result;
}
