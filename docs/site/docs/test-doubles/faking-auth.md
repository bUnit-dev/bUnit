---
uid: faking-auth
title: Faking Authentication and Authorization
---

# Faking Authentication and Authorization

bUnit comes with test specific implementation of Blazor's authentication and authorization types that makes it easy to test components that use Blazor's `<AuthorizeView>`, `<CascadingAuthenticationState>`, and `<AuthorizeRouteView>` components, and the `AuthenticationStateProvider` type.

The test implementation of Blazor's authentication and authorization can be put into the following states:

- **Authenticating**
- **Unauthenticated** and **unauthorized**
- **Authenticated** and **unauthorized**
- **Authenticated** and **authorized** 
- **Authenticated** and **authorized** with one or more **roles** and/or **policies**

bUnit's authentication and authorization implementation is easily available by calling [AddTestAuthorization()](xref:Bunit.FakeAuthorizationExtensions.AddTestAuthorization(Bunit.TestServiceProvider)) on a test context's `Services` collection. It returns an instance of the <xref:Bunit.TestDoubles.Authorization.TestAuthorizationContext> type that allows you to control the authentication and authorization state for a test.

The following sections will show how to set each of these states in a test.

## Setting Authenticating State

TODO

## Setting Authenticated and Authorized States

The examples in the following sections will use `<UserInfo>` component listed below. It uses an injected `AuthenticationStateProvider`, the `<CascadingAuthenticationState>` and `<AuthorizeView>` components to show the user name when a user is authenticated, and it shows the authorization state, when the authenticated user is authorized.

[!code-html[UserInfo.razor](../../../samples/components/UserInfo.razor)]

### Unauthenticated and unauthorized state

To set the state to unauthenticated and unauthorized, do the following:

[!code-csharp[UserInfoTest.cs](../../../samples/tests/xunit/UserInfoTest.cs?start=12&end=19&highlight=2)]

The highlighted line shows how `AddTestAuthorization()` is used to add the test specific implementation of Blazor's authentication and authorization types to the `Services` collection, which makes authentication state available to other services and components used throughout the test that requires it.

After calling `AddTestAuthorization()`, the default authentication state is unauthenticated and unauthorized.

### Authenticated and unauthorized state

To set the state to authenticated and unauthorized, do the following:

[!code-csharp[UserInfoTest.cs](../../../samples/tests/xunit/UserInfoTest.cs?start=26&end=35&highlight=2,3)]

After calling `AddTestAuthorization()`, the returned <xref:Bunit.TestDoubles.Authorization.TestAuthorizationContext> is used to set the authenticated and authorization state through the <xref:Bunit.TestDoubles.Authorization.TestAuthorizationContext.SetAuthorized(System.String,Bunit.TestDoubles.Authorization.AuthorizationState)> method.

### Authenticated and Authorized state

To set the state to authenticated and authorized, do the following:

[!code-csharp[UserInfoTest.cs](../../../samples/tests/xunit/UserInfoTest.cs?start=42&end=51&highlight=2,3)]

After calling `AddTestAuthorization()`, the returned <xref:Bunit.TestDoubles.Authorization.TestAuthorizationContext> is used to set the authenticated and authorization state through the <xref:Bunit.TestDoubles.Authorization.TestAuthorizationContext.SetAuthorized(System.String,Bunit.TestDoubles.Authorization.AuthorizationState)> method. 

Note, the second parameter, `AuthorizationState`, is optional, and defaults to `AuthorizationState.Authorized`, if not specified.

## Setting Authorization Details

The following section will show how to specify **roles** and/or **policies** in a test.

The examples will use `<UserRights>` component listed below. It the `<AuthorizeView>` components to include different content based on the **roles** and/or **policies** specified in each test.

[!code-html[UserRights.razor](../../../samples/components/UserRights.razor)]

### Roles

To specify one or more roles for the authenticated and authorized user, do the following:

[!code-csharp[UserRightsTest.cs](../../../samples/tests/xunit/UserRightsTest.cs?start=28&end=40&highlight=4)]

The highlighted line shows how the <xref:Bunit.TestDoubles.Authorization.TestAuthorizationContext.SetRoles(System.String[])> method is used to specify one role. To specify multiple roles, do the following:

[!code-csharp[](../../../samples/tests/xunit/UserRightsTest.cs?start=50&end=50)]

### Policies

To specify one or more policies for the authenticated and authorized user, do the following:

[!code-csharp[UserRightsTest.cs](../../../samples/tests/xunit/UserRightsTest.cs?start=67&end=79&highlight=4)]

The highlighted line shows how the <xref:Bunit.TestDoubles.Authorization.TestAuthorizationContext.SetPolicies(System.String[])> method is used to specify one policy. To specify multiple policies, do the following:

[!code-csharp[](../../../samples/tests/xunit/UserRightsTest.cs?start=89&end=89)]

### Example passing both roles and a policy

The following example specifies two roles and one policy for the authenticated and authorized user:

[!code-csharp[UserRightsTest.cs](../../../samples/tests/xunit/UserRightsTest.cs?start=105&end=120&highlight=4-6)]