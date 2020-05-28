---
uid: misc-test-tips
title: Miscellaneous Testing Tips
---

# Miscellaneous bUnit Testing Tips

Here is a few testing tips and tricks that have proven useful to us.

## Projects Structure and Tips and Tricks

The recommended solution/project structure for a test and production code project set-up is:

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

## Use same Root Namespace and Folder Structure

A neat trick, which will limit the `import` statements needed in your test project, is to set the root namespace to the same as that of the production code project, _AND_ use the same folder structure as shown above. Following the example above, the `MyComponentLibTests.csproj` file should contain:

```xml
<PropertyGroup>
  <RootNamespace>Company.MyComponentLib</RootNamespace>
</PropertyGroup>
```

## Capture Logs from ILogger in Test Output

TODO: Document XunitLogger and XunitLoggerFactory

## Make copy/paste of HTML easier

When writing C# based tests, you sometime want to copy/paste some HTML into C# strings from e.g. a Razor file. This is tedious to do manually as you have to escape the quotes and other special characters. The extension, [SmartPaster2019](https://marketplace.visualstudio.com/items?itemName=martinw.SmartPaster2013), allows us to copy strings where any character that needs to be escaped will be automatically.
