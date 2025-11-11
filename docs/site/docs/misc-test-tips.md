---
uid: misc-test-tips
title: Miscellaneous testing tips
---

# Miscellaneous bUnit testing tips

Here are a few testing tips and tricks that have proven useful to us. These don’t fit naturally on other pages but are useful enough to be highlighted here.

## Projects structure and tips and tricks

The recommended solution/project structure for a test and production code project set-up is as follows:

```
src
| MyComponentLib.csproj (namespace e.g. "Company.MyComponentLib")
| _Imports.razor
| Component1.razor
| SubFolder
  | SubComponent1.razor

test
| MyComponentLibTests.csproj (with project reference to MyComponentLib.csproj)
| _Imports.razor
| Component1Test.cs
| SubFolder
  | SubComponent1Test.cs
```

## Using the same root namespace and folder structure

A neat trick, which will limit the number of `import` statements needed in your test project, is to set the root namespace to the same as that of the production code project, _AND_ use the same folder structure as shown above. Following the example above, the `MyComponentLibTests.csproj` file should contain this:

```xml
<PropertyGroup>
  <RootNamespace>Company.MyComponentLib</RootNamespace>
</PropertyGroup>
```

This makes the tooling in Visual Studio and other IDEs automatically assign the same namespaces to new test classes and test components when they are created.

## Capturing logs from ILogger in test output

It can sometimes be helpful to capture log messages sent to `ILogger` types in the components under test and/or the bUnit and Blazor internals. 

With xUnit, this can be done as follows:

1. Add the following packages to your test project: [Serilog](https://www.nuget.org/packages/Serilog), [Serilog.Extensions.Logging](https://www.nuget.org/packages/Serilog.Extensions.Logging), [Serilog.Expressions](https://www.nuget.org/packages/Serilog.Expressions), and [Serilog.Sinks.XUnit](https://www.nuget.org/packages/Serilog.Sinks.XUnit).
2. Add the following class/extension method to your test project (which replicates the signature of the removed `AddXunitLogger` method):  
  
  ```csharp
  using Microsoft.Extensions.Logging;
  using Serilog;
  using Serilog.Events;
  using Serilog.Templates;
  using Xunit.Abstractions;
  
  namespace Xunit;
  
  public static class ServiceCollectionLoggingExtensions
  {
    public static IServiceCollection AddXunitLogger(this IServiceCollection services, ITestOutputHelper outputHelper)
    {
      var serilogLogger = new LoggerConfiguration()
        .MinimumLevel.Verbose()
        .WriteTo.TestOutput(
          testOutputHelper: outputHelper,
          formatter: new ExpressionTemplate("[{UtcDateTime(@t):mm:ss.ffffff} | {@l:u3} | {Substring(SourceContext, LastIndexOf(SourceContext, '.') + 1)} | {Coalesce(EventId.Name, '<none>')}] {@m}\n{@x}"),
          restrictedToMinimumLevel: LogEventLevel.Verbose)
        .CreateLogger();
      
      services.AddSingleton<ILoggerFactory>(_ => new LoggerFactory().AddSerilog(serilogLogger, dispose: true));
      services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
      return services;
    }
  }
  ```

3. In each test class whose tests should capture log messages, add a constructor that takes the `ITestOutputHelper` as input, and pass that to the `AddXunitLogger` extension method created in the previous step, e.g.:  
  
  ```csharp
  using System;
  using Microsoft.Extensions.DependencyInjection;
  using Bunit;
  using Xunit;
  using Xunit.Abstractions;

  namespace MyTests;
  
  public class MyComponenTest : BunitContext
  {
    public MyComponenTest(ITestOutputHelper outputHelper)
    {
      Services.AddXunitLogger(outputHelper);
    }

    [Fact]
    public void Test() ...
  }  
  ```

## Easier HTML copying/pasting

When writing tests in `.cs` files, you may want to copy/paste HTML into C# strings from something like a Razor file, for example. This is tedious to do manually as you have to escape quotes and other special characters, for example `<div class="alert">` needs to be written as `"<div class=\"alert\">"`. The extension [SmartPaster](https://marketplace.visualstudio.com/items?itemName=martinw.SmartPaster) automatically escapes any characters that need to be escaped when it is used to copy strings.
