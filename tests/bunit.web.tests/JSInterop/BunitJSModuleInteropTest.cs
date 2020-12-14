#if NET5_0
using System;
using System.Linq;
using System.Threading.Tasks;
using Bunit.JSInterop.InvocationHandlers;
using Bunit.JSInterop.InvocationHandlers.Implementation;
using Microsoft.JSInterop;
using Microsoft.JSInterop.Implementation;
using Shouldly;
using Xunit;

namespace Bunit.JSInterop
{
    public class BunitJSModuleInteropTest : TestContext
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

		[Fact(DisplayName = "Calling SetupModule(uri) registers handler for module JS Interop")]
		public void Test010()
		{
			JSInterop.SetupModule("FOO.js");

			JSInterop.TryGetInvokeHandler<IJSObjectReference>("import", "FOO.js")
				.ShouldNotBeNull()
				.ShouldSatisfyAllConditions(
					x => x.Identifier.ShouldBe("import"),
					x => x.ShouldBeOfType<JSObjectReferenceInvocationHandler>()
				);
		}

		[Fact(DisplayName = "Calling SetupModule(invocationMatcher) registers handler for module JS Interop")]
		public void Test011()
		{
			JSInterop.SetupModule(invocation => true);

			JSInterop.TryGetInvokeHandler<IJSObjectReference>("import", "FOO.js")
				.ShouldNotBeNull()
				.ShouldSatisfyAllConditions(
					x => x.Identifier.ShouldBe("import"),
					x => x.ShouldBeOfType<JSObjectReferenceInvocationHandler>()
				);
		}

		[Fact(DisplayName = "Calling the catch-all SetupModule() registers handler for module JS Interop")]
		public void Test012()
		{
			JSInterop.SetupModule();

			JSInterop.TryGetInvokeHandler<IJSObjectReference>("foo")
				.ShouldNotBeNull()
				.ShouldSatisfyAllConditions(
					x => x.Identifier.ShouldBe(JSObjectReferenceInvocationHandler.CatchAllIdentifier),
					x => x.ShouldBeOfType<JSObjectReferenceInvocationHandler>()
				);
		}

		[Fact(DisplayName = "Calling SetupModule(customImport, args) registers handler for module JS Interop")]
		public void Test013()
		{
			JSInterop.SetupModule("foo", Array.Empty<object>());

			JSInterop.TryGetInvokeHandler<IJSObjectReference>("foo")
				.ShouldNotBeNull()
				.ShouldSatisfyAllConditions(
					x => x.Identifier.ShouldBe("foo"),
					x => x.ShouldBeOfType<JSObjectReferenceInvocationHandler>()
				);
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

			Should.Throw<JSRuntimeUnhandledInvocationException>(
				() => JSInterop.JSRuntime.InvokeAsync<IJSObjectReference>("import", requestedRoduleName)
			);
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
					x => x.Arguments.ShouldHaveSingleItem().ShouldBe("FOO.js")
				);

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

		// moduleJSInterop has setup methods IJSObjectReference calls that mirros BuntJSInterop
	}
}
#endif
