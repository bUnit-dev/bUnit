---
uid: mocking-httpclient
title: Mocking HttpClient
---

# Mocking `HttpClient`

Mocking the `HttpClient` service in .NET Core is a bit more cumbersome than interface-based services like `IJSRuntime`. 
There is currently no built-in mock for `HttpClient` in bUnit, but with the use of 
[RichardSzalay.MockHttp](https://www.nuget.org/packages/RichardSzalay.MockHttp/) we can easily add one that works
with bUnit.

To use RichardSzalay.MockHttp, add the following package reference to your test project's .csproj file:

```xml
<PackageReference Include="RichardSzalay.MockHttp" Version="6.0.0" />
```

To make it easier to work with [RichardSzalay.MockHttp](https://www.nuget.org/packages/RichardSzalay.MockHttp/), add 
the following extension class to your test project. It makes it easier to add the `HttpClient` mock to 
bUnit's test context's `Services` collection, and configure responses to requests:

[!code-csharp[MockHttpClientBunitHelpers.cs](../../../samples/tests/xunit/MockHttpClientBunitHelpers.cs?start=3&end=46)]

With the helper methods in place, you can do the following in your tests:

```csharp
using var ctx = new TestContext();
var mock = ctx.Services.AddMockHttpClient();
mock.When("/getData").RespondJson(new List<Data>{ ... });
```

This registers the mock `HttpClient` in bUnit's test context's `Services` collection, and then tells the mock that when a request is received for `/getData`, it should respond with the `new List<Data>{ ... }`, serialized as JSON.

> [!TIP]
> You can add additional `RespondXXX` methods to the `MockHttpClientBunitHelpers` class to fit your testing needs.
<!--stackedit_data:
eyJoaXN0b3J5IjpbNDkzNTI2MzY3XX0=
-->