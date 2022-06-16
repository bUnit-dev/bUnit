# Breaking changes

This document will list all notable breaking changes, making it easier to migrate.

## `v1` to `v2`
 - `Click` and `DoubleClick` (as well as their `async` friends) now only accepts either no parameter or a parameter of type [`MouseEventArgs`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.web.mouseeventargs).