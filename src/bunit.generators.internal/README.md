# Tips for developing with the generator

When changing the source generator, to see the effect, clearing the build cache may be necessary:

```
dotnet build-server shutdown
```

A good way to quickly see if the generate is producing output:

```
dotnet build-server shutdown && dotnet clean && dotnet test -p:TargetFramework=net8.0
```
