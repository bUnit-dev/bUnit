# Creating a new test project

To create a project for testing you Blazor components, first install the [bUnit Project Template](https://www.nuget.org/packages/bunit.template/) from NuGet, using this command:

```
dotnet new --install bunit.template::#{VERSION}#
```

Then to create a new project, use the following command:

```
dotnet new bunit -o <NAME OF PROJECT>
```

where `-o <NAME OF PROJECT>` is used to name the test project.

## Creating a new Blazor test project manually

If you do not want to use the Blazor test project template, you can create an empty class library and the modify the `.csproj` to match the following:

```xml
<Project Sdk="Microsoft.NET.Sdk.Razor">

  <PropertyGroup>
    <TargetFramework>netcoreapp3.1</TargetFramework>
    <RazorLangVersion>3.0</RazorLangVersion>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.Components" Version="3.1.0" />
    <PackageReference Include="Microsoft.AspNetCore.Components.Web" Version="3.1.0" />
    <PackageReference Include="bunit" Version="#{VERSION}#" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="16.4.0" />
    <PackageReference Include="xunit" Version="2.4.1" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.4.1">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
  </ItemGroup>

</Project>
```

## Projects structure and tips and tricks

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

### Use same root namespace and folder structure in both test- and production project

A neat trick, which will limit the `import` statements needed in your test project, is to set the root namespace to the same as that of the production code project, _AND_ use the same folder structure as shown above. Following the example above, the `MyComponentLibTests.csproj` file should contain:

```xml
<PropertyGroup>
  <RootNamespace>Company.MyComponentLib</RootNamespace>
</PropertyGroup>
```

### Make copy/past of HTML easier

When writing C# based tests, you sometime want to copy/paste some HTML into C# strings from e.g. a Razor file. This is tedious to do manually as you have to escape the quotes and other special characters. The extension, [SmartPaster2019](https://marketplace.visualstudio.com/items?itemName=martinw.SmartPaster2013), allows us to copy strings where any character that needs to be escaped will be automatically.
