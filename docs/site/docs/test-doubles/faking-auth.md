---
uid: faking-auth
title: Faking Blazor's Authentication and Authorization
---

# Faking Blazor's Authentication and Authorization

https://github.com/egil/bunit/issues/135

When testing Blazor components that require authentication and authorization, you need to set up enough of the services and behavior to work with the Blazor AuthorizeView, CascadingAuthenticationState, and AuthorizeRouteView.

You can use AuthorizeView in a component to show different content based on the user's authentication/authorization state. To set your component up for testing, you can do the following:

[!code-html[SimpleAuthView.razor](../../../samples/components/SimpleAuthView.razor)]

To easily test this type of component, bUnit has some test services that help. You will need to add these services to your test context before you render and run your tests.

The following code tests the component with an unauthenticated user. To setup an authenticated user, just call AddTestAuthorization with no parameters.

[!code-csharp[](../../../samples/tests/xunit/SimpleAuthViewTest.cs#L9-L20)]

Now we can test with an authenticated and authorized user by calling AddTestAuthorization with a user name and authorization flag.

[!code-csharp[](../../../samples/tests/xunit/SimpleAuthViewTest.cs#L23-L35)]

Finally we can test with an authenticated and unauthorized user in the code below.

[!code-csharp[](../../../samples/tests/xunit/SimpleAuthViewTest.cs#L38-L50)]

In addition to using these services with AuthorizeView, you can also inject the services into your component and call them in your C# code. 

[!code-html[InjectAuthService.razor](../../../samples/components/InjectAuthService.razor)]

Then you can write a similar test as the ones above.

[!code-csharp[](../../../samples/tests/xunit/InjectAuthServiceTest.cs#L10-L22)]

That is all you need to support authorization and authentication testing in bUnit.
