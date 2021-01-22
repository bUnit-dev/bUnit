---
uid: misc-test-tips
title: Miscellaneous Testing Tips
---

# Miscellaneous bUnit Testing Tips

Here are a few testing tips and tricks that have proven useful to us. These don’t fit naturally on other pages but are useful enough to be highlighted here.

## Projects Structure and Tips and Tricks

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

## Using the same Root Namespace and Folder Structure

A neat trick, which will limit the number of `import` statements needed in your test project, is to set the root namespace to the same as that of the production code project, _AND_ use the same folder structure as shown above. Following the example above, the `MyComponentLibTests.csproj` file should contain this:

```xml
<PropertyGroup>
  <RootNamespace>Company.MyComponentLib</RootNamespace>
</PropertyGroup>
```

This makes the tooling in Visual Studio and other IDEs automatically assign the same namespaces to new test classes and test components when they are created.

## Capturing Logs from ILogger in Test Output

TODO: Document XunitLogger and XunitLoggerFactory

## Copying/pasting HTML easier

When writing C# based tests, you may want to copy/paste HTML into C# strings from something like a Razor file, for example. This is tedious to do manually as you have to escape the quotes and other special characters, for example `<div class="alert">` needs to be written as `"<div class=\"alert\">"`. The extension, [SmartPaster2019](https://marketplace.visualstudio.com/items?itemName=martinw.SmartPaster2013), allows us to copy strings in any character that needs to be escaped will be automatically.
<!--stackedit_data:
eyJoaXN0b3J5IjpbLTI2OTg0ODY3N119
-->