#if NET5_0
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Bunit.JSInterop.InvocationHandlers.Implementation;
using Microsoft.JSInterop;
using Microsoft.JSInterop.Implementation;
using Shouldly;
using Xunit;

namespace Bunit.JSInterop
{
	public class BunitJSObjectReferenceTest : TestContext
	{
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
			Should.Throw<ArgumentException>(() => JSInterop.SetupModule(string.Empty, _ => true));
			Should.Throw<ArgumentNullException>(() => JSInterop.SetupModule("import", default(InvocationMatcher)!));
		}

		[Fact(DisplayName = "Calling SetupModule(uri) registers handler for module JS Interop")]
		public void Test010()
		{
			JSInterop.SetupModule("FOO.js");

			JSInterop.TryGetInvokeHandler<IJSObjectReference>("import", "FOO.js")
				.ShouldNotBeNull()
				.ShouldSatisfyAllConditions(
					x => x.Identifier.ShouldBe("import"),
					x => x.ShouldBeOfType<JSObjectReferenceInvocationHandler>());
		}

		[Fact(DisplayName = "Calling SetupModule(invocationMatcher) registers handler for module JS Interop")]
		public void Test011()
		{
			JSInterop.SetupModule(invocation => true);

			JSInterop.TryGetInvokeHandler<IJSObjectReference>("import", "FOO.js")
				.ShouldNotBeNull()
				.ShouldSatisfyAllConditions(
					x => x.Identifier.ShouldBe("import"),
					x => x.ShouldBeOfType<JSObjectReferenceInvocationHandler>());
		}

		[Fact(DisplayName = "Calling the catch-all SetupModule() registers handler for module JS Interop")]
		public void Test012()
		{
			JSInterop.SetupModule();

			JSInterop.TryGetInvokeHandler<IJSObjectReference>("foo")
				.ShouldNotBeNull()
				.ShouldSatisfyAllConditions(
					x => x.Identifier.ShouldBe(JSObjectReferenceInvocationHandler.CatchAllIdentifier),
					x => x.ShouldBeOfType<JSObjectReferenceInvocationHandler>());
		}

		[Fact(DisplayName = "Calling SetupModule(customImport, args) registers handler for module JS Interop")]
		public void Test013()
		{
			JSInterop.SetupModule("foo", Array.Empty<object>());

			JSInterop.TryGetInvokeHandler<IJSObjectReference>("foo")
				.ShouldNotBeNull()
				.ShouldSatisfyAllConditions(
					x => x.Identifier.ShouldBe("foo"),
					x => x.ShouldBeOfType<JSObjectReferenceInvocationHandler>());
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
		public async Task Test070()
		{
			JSInterop.Mode = JSRuntimeMode.Loose;

			var jsRuntime = await JSInterop.JSRuntime.InvokeAsync<IJSObjectReference>("FOO.js");

			jsRuntime.ShouldBeAssignableTo<IJSInProcessObjectReference>();
		}

		[Fact(DisplayName = "IJSObjectReference can be cast to IJSUnmarshalledObjectReference")]
		public async Task Test071()
		{
			JSInterop.Mode = JSRuntimeMode.Loose;

			var jsRuntime = await JSInterop.JSRuntime.InvokeAsync<IJSObjectReference>("FOO.js");

			jsRuntime.ShouldBeAssignableTo<IJSUnmarshalledObjectReference>();
		}

		[Fact(DisplayName = "IJSInProcessObjectReference-invocations is handled by handlers from BunitJSInterop")]
		public async Task Test080()
		{
			JSInterop.Mode = JSRuntimeMode.Loose;
			var jsInProcess = (IJSInProcessObjectReference)(await JSInterop.JSRuntime.InvokeAsync<IJSObjectReference>("FOO.js"));

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
			var jsUnmarshalled = (IJSUnmarshalledObjectReference)(await JSInterop.JSRuntime.InvokeAsync<IJSObjectReference>("FOO.js"));

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
	}
}
#endif
