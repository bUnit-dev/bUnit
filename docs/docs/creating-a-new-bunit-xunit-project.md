# Creating a new bUnit with xUnit test project

To create a project for testing you Blazor components, first install the [bUnit Project Template](https://www.nuget.org/packages/bunit.template/) from NuGet, using this command:

```bash
dotnet new --install bunit.template::#{VERSION}#
```

Then to create a new test project, use the following command:

```bash
dotnet new bunit -o <NAME OF TEST PROJECT>
```

where `-o <NAME OF PROJECT>` is used to name the test project.

**Optional CLI steps**

Link the test project to your solution

```bash
dotnet sln <NAME OF PROJECT>.sln add <NAME OF TEST PROJECT>
```

Add a reference your components to be tested in your test project.

```bash
dotnet add <NAME OF COMPONENT PROJECT>.csproj reference <NAME OF TEST PROJECT>.csproj
```

## Creating a new Blazor test project manually

If you do not want to use the Blazor test project template, you can create an empty class library and the modify the `.csproj` to match the following:

```xml
<Project Sdk="Microsoft.NET.Sdk.Razor">

  <PropertyGroup>
    <TargetFramework>netcoreapp3.1</TargetFramework>
    <RazorLangVersion>3.0</RazorLangVersion>
  </PropertyGroup>

  <ItemGroup>
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

## Further reading

- [Miscellaneous bUnit testing tips](/docs/misc-test-tips.html)