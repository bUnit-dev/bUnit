---
uid: faking-auth
title: Faking Blazor's Authorization
---

# Faking Blazor's Authentication and Authorization

https://github.com/egil/bunit/issues/135

When testing Blazor components that require authentication and authorization, you need to set up enough of the services and behavior to work with the Blazor AuthorizeView, CascadingAuthenticationState, and AuthorizeRouteView.

You can use AuthorizeView in a component to show different content based on the user's authentication/authorization state. To set your component up for testing, you can do the following:

```c#
@using Microsoft.AspNetCore.Components.Authorization

<CascadingAuthenticationState>
	<AuthorizeView>
		<Authorized>
			Authorized!
		</Authorized>
		<NotAuthorized>
			Not authorized?
		</NotAuthorized>
	</AuthorizeView>
</CascadingAuthenticationState>
```

To easily test this type of component, bUnit has some test services that help. You will need to add these services to your test context before you render and run your tests.

The following code tests the component with an unauthenticated user. To setup an authenticated user, just call AddTestAuthorization with no parameters.

```c#
[Fact(DisplayName = "AuthorizeView with unauthenticated user")]
public void Test001()
{
	// arrange
	using var ctx = new TestContext();
	ctx.Services.AddTestAuthorization();

	// act
	var cut = ctx.RenderComponent<SimpleAuthView>();

	// assert
	cut.MarkupMatches("Not authorized?");
}
```

Now we can test with an authenticated and authorized user by calling AddTestAuthorization with a user name and authorization flag.

```c#
[Fact(DisplayName = "AuthorizeView with authenticated and authorized user")]
public void Test002()
{
	// arrange
	using var ctx = new TestContext();
	ctx.Services.AddTestAuthorization("TestUser", AuthorizationState.Authorized);

	// act
	var cut = ctx.RenderComponent<SimpleAuthView>();

	// assert
	cut.MarkupMatches("Authorized!");
}
```

Finally we can test with an authenticated and unauthorized user in the code below.

```c#
[Fact(DisplayName = "AuthorizeView with authenticated but unauthorized user")]
public void Test003()
{
	// arrange
	using var ctx = new TestContext();
	ctx.Services.AddTestAuthorization("TestUser", AuthorizationState.Unauthorized);

	// act
	var cut = ctx.RenderComponent<SimpleAuthView>();

	// assert
	cut.MarkupMatches("Not authorized?");
}
```

In addition to using these services with AuthorizeView, you can also inject the services into your component and call them in your C# code. 

```c#
@using Microsoft.AspNetCore.Components.WebAssembly.Authentication
@inject AuthenticationStateProvider AuthenticationStateProvider

<p>Test Component</p>

@if (isAuthenticated)
{
    <p>User: @userName</p>
}

@code{
    bool isAuthenticated = false;
    string userName;

    protected override async Task OnInitializedAsync()
    {
        var state = await AuthenticationStateProvider.GetAuthenticationStateAsync().ConfigureAwait(false);
        if (state != null)
        {
            this.isAuthenticated = state.User.Identity.IsAuthenticated;
            this.userName = state.User.Identity.Name;
        }
    }
}
```

Then you can write a similar test as the ones above.

```c#
[Fact(DisplayName = "Use AuthenticationStateProvider service with authenticated and authorized user")]
public void Test004()
{
	// arrange
	using var ctx = new TestContext();
	ctx.Services.AddTestAuthorization("TestUserName", AuthorizationState.Authorized);

	// act
	var cut = ctx.RenderComponent<TestComponent>();

	// assert
	Assert.Contains("<p>User: TestUserName</p>", cut.Markup, StringComparison.InvariantCulture);
}
```

That is all you need to support authorization and authentication testing in bUnit.
