---
uid: bunit-auth
title: Adding authentication and authorization
---

# Adding authentication and authorization

bUnit comes with test-specific implementations of Blazor's authentication and authorization types, making it easy to test components that use Blazor's `<AuthorizeView>`, `<CascadingAuthenticationState>` and `<AuthorizeRouteView>` components, as well as the `AuthenticationStateProvider` type.

The test implementation of Blazor's authentication and authorization can be put into the following states:

- **Authenticating**
- **Unauthenticated** and **unauthorized**
- **Authenticated** and **unauthorized**
- **Authenticated** and **authorized**
- **Authenticated** and **authorized** with one or more **roles**, **claims**, and/or **policies**

bUnit's authentication and authorization implementation is easily available by calling [`AddAuthorization()`](xref:Bunit.TestContext.AddAuthorization(Bunit.TestContext)) on a test context. This adds the necessary services to the `Services` collection and the `CascadingAuthenticationState` component to the [root render tree](xref:root-render-tree). The method returns an instance of the <xref:Bunit.TestDoubles.BunitAuthorizationContext> type that allows you to control the authentication and authorization state for a test.

> [!NOTE]
> If your test class inherits directly from bUnit's <xref:Bunit.TestContext> then you need to call the [`AddAuthorization()`](xref:Bunit.TestContext.AddAuthorization(Bunit.TestContext)) method on `this`, since `AddAuthorization()` is an extension method, otherwise it wont be available. E.g.: `AddAuthorization()`.

The following sections show how to set each of these states in a test.

## Setting authenticating, authenticated and authorized states

The examples in the following sections will use the `<UserInfo>` component listed below. This uses an injected `AuthenticationStateProvider` service  and `<CascadingAuthenticationState>` and `<AuthorizeView>` components to show the user name when a user is authenticated. It also shows the authorization state when the authenticated user is authorized.

[!code-razor[UserInfo.razor](../../../samples/components/UserInfo.razor)]

The following subsections demonstrate how to set the `<UserInfo>` into all three authentication and authorization states.

### Unauthenticated and unauthorized state

To set the state to unauthenticated and unauthorized, do the following:

[!code-csharp[UserInfoTest.cs](../../../samples/tests/xunit/UserInfoTest.cs?start=11&end=19&highlight=2)]

The highlighted line shows how `AddAuthorization()` is used to add the test-specific implementation of Blazor's authentication and authorization types to the `Services` collection, which makes the authentication state available to other services as well as components used throughout the test that require it.

After calling `AddAuthorization()`, the default authentication state is unauthenticated and unauthorized.

### Authenticating and authorizing state

To set the state to authenticating and authorizing, do the following:

[!code-csharp[UserInfoTest.cs](../../../samples/tests/xunit/UserInfoTest.cs?start=25&end=34&highlight=3)]

After calling `AddAuthorization()`, the returned <xref:Bunit.TestDoubles.BunitAuthorizationContext> is used to set the authenticating and authorizing state through the <xref:Bunit.TestDoubles.BunitAuthorizationContext.SetAuthorizing> method.

### Authenticated and unauthorized state

To set the state to authenticated and unauthorized, do the following:

[!code-csharp[UserInfoTest.cs](../../../samples/tests/xunit/UserInfoTest.cs?start=40&end=49&highlight=3)]

After calling `AddAuthorization()`, the returned <xref:Bunit.TestDoubles.BunitAuthorizationContext> is used to set the authenticated and unauthorized state through the <xref:Bunit.TestDoubles.BunitAuthorizationContext.SetAuthorized(System.String,Bunit.TestDoubles.AuthorizationState)> method.

### Authenticated and authorized state

To set the state to authenticated and authorized, do the following:

[!code-csharp[UserInfoTest.cs](../../../samples/tests/xunit/UserInfoTest.cs?start=55&end=64&highlight=3)]

After calling `AddAuthorization()`, the returned <xref:Bunit.TestDoubles.BunitAuthorizationContext> is used to set the authenticated and authorized state through the <xref:Bunit.TestDoubles.BunitAuthorizationContext.SetAuthorized(System.String,Bunit.TestDoubles.AuthorizationState)> method.

Note that the second parameter, `AuthorizationState`, is optional, and defaults to `AuthorizationState.Authorized` if not specified.

## Setting authorization details

The following section will show how to specify **roles** and/or **policies** in a test.

The examples will use the `<UserRights>` component listed below. It uses the `<AuthorizeView>` component to include different content based on the **roles**, **claims**, or **policies** specified in each test.

[!code-razor[UserRights.razor](../../../samples/components/UserRights.razor)]

### Roles

To specify one or more roles for the authenticated and authorized user, do the following:

[!code-csharp[UserRightsTest.cs](../../../samples/tests/xunit/UserRightsTest.cs?start=28&end=40&highlight=4)]

The highlighted line shows how the <xref:Bunit.TestDoubles.BunitAuthorizationContext.SetRoles(System.String[])> method is used to specify a single role. To specify multiple roles, do the following:

[!code-csharp[UserRightsTest.cs](../../../samples/tests/xunit/UserRightsTest.cs?start=46&end=59&highlight=4)]

### Policies

To specify one or more policies for the authenticated and authorized user, do the following:

[!code-csharp[UserRightsTest.cs](../../../samples/tests/xunit/UserRightsTest.cs?start=65&end=78&highlight=5)]

The highlighted line shows how the <xref:Bunit.TestDoubles.BunitAuthorizationContext.SetPolicies(System.String[])> method is used to specify one policy. To specify multiple policies, do the following:

[!code-csharp[](../../../samples/tests/xunit/UserRightsTest.cs?start=91&end=91)]

### Claims

To specify one or more claims for the authenticated and authorized user, do the following:

[!code-csharp[UserRightsTest.cs](../../../samples/tests/xunit/UserRightsTest.cs?start=101&end=117&highlight=4-7)]

The highlighted line shows how the <xref:Bunit.TestDoubles.BunitAuthorizationContext.SetClaims(System.Security.Claims.Claim[])> method is used to pass two instances of the `Claim` type.

### Example of passing both roles, claims, and policies

Let’s try to combine all the possibilities shown in the previous examples into one. The following example specifies two roles, one claim, and one policy for the authenticated and authorized user:

[!code-csharp[UserRightsTest.cs](../../../samples/tests/xunit/UserRightsTest.cs?start=123&end=140&highlight=3-7)]

With this example done, all auth-related test scenarios should be covered. If you find that one is missing, please let us know in the [bUnit discussion forum](https://github.com/egil/bUnit/discussions).

### Authentication types

To specify a authentication type for the authenticated and authorized user, do the following:

[!code-csharp[UserRightsTest.cs](../../../samples/tests/xunit/UserRightsTest.cs?start=146&end=158&highlight=4)]

The highlighted line shows how the <xref:Bunit.TestDoubles.BunitAuthorizationContext.SetAuthenticationType(System.String)> method is used to change the `Identity.AuthenticationType` of the user.
