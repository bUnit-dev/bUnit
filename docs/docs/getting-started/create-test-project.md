---
uid: create-test-project
title: Creating a new bUnit Test Project
---

# Creating a new bUnit Test Project

Before you can write any tests, you need a place to put them - a test project. bUnit is not a unit test runner, so you need a general purpose test framework, like xUnit, NUnit, or MSTest, in addition to bUnit, to run your tests, and write your assertions. 

If you prefer xUnit, you can use the bUnit project template approached described in the [Create a test project with bUnit template](#create-a-test-project-with-bunit-template) section further down the page. If you want to use another general purpose testing framework, read the following section.

## Create a Test Project Manually

To create a project for testing you Blazor components that uses either of three general purpose test frameworks, you need to go through these steps:

1. Create a new xUnit/NUnit/MSTest testing project
2. Add bUnit to the test project
3. Configure project settings
4. Add the test project to your solution

These steps look like this from the `dotnet` CLI:

**1. Create a new test project**

Use the following command (click on the tab that for the test framework you would like to use):

# [xUnit](#tab/xunit)

```bash
dotnet new xunit -o <NAME OF TEST PROJECT>
```

# [NUnit](#tab/nunit)

```bash
dotnet new nunit -o <NAME OF TEST PROJECT>
```

# [MSTest](#tab/mstest)

```bash
dotnet new mstest -o <NAME OF TEST PROJECT>
```

***

where `-o <NAME OF PROJECT>` is used to name the test project.

**2. Add bUnit to the test project**

To add bUnit to your test project, first change to the newly created test projects folder, and then use the following command:

# [xUnit](#tab/xunit)

```bash
cd <NAME OF PROJECT>
dotnet add package bunit.web --version #{VERSION}#
dotnet add package bunit.xunit --version #{VERSION}#
```

# [NUnit](#tab/nunit)

```bash
cd <NAME OF PROJECT>
dotnet add package bunit.web --version #{VERSION}#
```

# [MSTest](#tab/mstest)

```bash
cd <NAME OF PROJECT>
dotnet add package bunit.web --version #{VERSION}#
```

***

**3. Configure project settings**

Then you need to change a few project settings, in particular we need to change the project's SDK to `Microsoft.NET.Sdk.Razor`, remember to set `RazorLangVersion` to `3.0`,  and set the `<TargetFramework>` to `netcoreapp3.1`, since bUnit builds on `.netstandard 2.1`.

To do so, change the first part of the test project's `.csproj` file to look like this.:

```xml
<Project Sdk="Microsoft.NET.Sdk.Razor">

  <PropertyGroup>
    <TargetFramework>netcoreapp3.1</TargetFramework>
    <RazorLangVersion>3.0</RazorLangVersion>
  </PropertyGroup>

  ...

</Project>
```

**4. Add the test project to your solution**

Then you need to add your test project to your solution (`.sln`) and add a reference between your test project and component project:

```bash
dotnet sln <NAME OF PROJECT>.sln add <NAME OF TEST PROJECT>
dotnet add <NAME OF COMPONENT PROJECT>.csproj reference <NAME OF TEST PROJECT>.csproj
```

The end result should be a test project with a `.csproj` that looks like this (other packages than bUnit might have different version numbers):


# [xUnit](#tab/xunit)

```xml
<Project Sdk="Microsoft.NET.Sdk.Razor">

  <PropertyGroup>
    <TargetFramework>netcoreapp3.1</TargetFramework>
    <RazorLangVersion>3.0</RazorLangVersion>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="bunit.web" Version="#{VERSION}#" />
    <PackageReference Include="bunit.xunit" Version="#{VERSION}#" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="16.4.0" />
    <PackageReference Include="xunit" Version="2.4.1" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.4.1">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
  </ItemGroup>

</Project>
```

# [NUnit](#tab/nunit)

```xml
<Project Sdk="Microsoft.NET.Sdk.Razor">

  <PropertyGroup>
    <TargetFramework>netcoreapp3.1</TargetFramework>
    <IsPackable>false</IsPackable>
  </PropertyGroup>

  <ItemGroup>    
    <PackageReference Include="bunit.web" Version="#{VERSION}#" />
    <PackageReference Include="nunit" Version="3.12.0" />
    <PackageReference Include="NUnit3TestAdapter" Version="3.16.1">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="16.6.1" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="<PATH TO COMPONENT LIB>.csproj" />
  </ItemGroup>

</Project>
```

# [MSTest](#tab/mstest)

```xml
<Project Sdk="Microsoft.NET.Sdk.Razor">

  <PropertyGroup>
    <TargetFramework>netcoreapp3.1</TargetFramework>
    <IsPackable>false</IsPackable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="bunit.web" Version="#{VERSION}#" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="16.5.0" />
    <PackageReference Include="MSTest.TestAdapter" Version="2.1.0" />
    <PackageReference Include="MSTest.TestFramework" Version="2.1.0" />
    <PackageReference Include="coverlet.collector" Version="1.2.0" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="<PATH TO COMPONENT LIB>.csproj" />
  </ItemGroup>

</Project>
```

***

## Create a Test Project with bUnit Template

If you want to skip a few steps in the guide above, you can use the [bUnit test project template](https://www.nuget.org/packages/bunit.template/). The bUNit project template is only available for using with xUnit as the general purpose testing framework, but that will change in the future.

The steps are as follows:

1. Install the template (only needed the first time)
2. Create a new test project
3. Add the test project to your solution


These steps look like this from the `dotnet` CLI:

**1. Install the template**

Install the template from NuGet using this command:

```bash
dotnet new --install bunit.template::#{VERSION}#
```

**2. Create a new test project**

Use the following command to create a bUnit with xUnit test project:

```bash
dotnet new bunit -o <NAME OF TEST PROJECT>
```

where `-o <NAME OF PROJECT>` is used to name the test project.

**3. Add the test project to your solution**

Then you need to add your test project to your solution (`.sln`) and add a reference between your test project and component project:

```bash
dotnet sln <NAME OF PROJECT>.sln add <NAME OF TEST PROJECT>
dotnet add <NAME OF COMPONENT PROJECT>.csproj reference <NAME OF TEST PROJECT>.csproj
```

## Further Reading

Now you are ready to write some times. To learn how, continue reading the <xref:writing-csharp-tests> and <xref:writing-razor-tests> pages.

For addition tips and tricks that will make writing tests easier, see the <xref:misc-test-tips> page.
