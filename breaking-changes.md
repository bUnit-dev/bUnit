# Breaking changes

This document will list all notable breaking changes, making it easier to migrate.

## `v1` to `v2`
 - `Click`, `DoubleClick`, `MouseOver`, `MouseOut`, `MouseMove`, `MouseDown`, `MouseUp`; `Wheel`, `MouseWheel` and `ContextMenu` now only accepts either no parameter or a parameter of type [`MouseEventArgs`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.web.mouseeventargs).