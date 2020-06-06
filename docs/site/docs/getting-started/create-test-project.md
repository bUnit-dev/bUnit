---
uid: create-test-project
title: Creating a new bUnit Test Project
---

# Creating a new bUnit Test Project

To write tests, you need a place to put them - a test project. bUnit is not a unit test runner, so a general-purpose test framework, like xUnit, NUnit, or MSTest, is needed, in addition to bUnit, to write and run tests. 

To use bUnit with xUnit, the easiest approached is using the bUnit project template described in the [Create a test project with bUnit template](#create-a-test-project-with-bunit-template) section further down the page. To create a test project manually in a general-purpose testing frameworks agnostic way, read the following section.

## Create a Test Project Manually

This section will take you through the steps listed below to manually create a project for testing Blazor components, that uses either of three general purpose test frameworks. Read the rest of the section to get the all the details. Skip any steps you have already previously completed, e.g. if you already have a test project.

1. Create a new xUnit/NUnit/MSTest testing project
2. Add bUnit to the test project
3. Configure project settings
4. Add the test project to your existing solution

These steps look like this from the `dotnet` CLI:

**1. Create a new test project**

Use the following command (_click on the tab that for the test framework of choice_):

# [xUnit](#tab/xunit)

```dotnetcli
dotnet new xunit -o <NAME OF TEST PROJECT>
```

# [NUnit](#tab/nunit)

```dotnetcli
dotnet new nunit -o <NAME OF TEST PROJECT>
```

# [MSTest](#tab/mstest)

```dotnetcli
dotnet new mstest -o <NAME OF TEST PROJECT>
```

***

where `-o <NAME OF PROJECT>` is used to name the test project.

**2. Add bUnit to the test project**

To add bUnit to the test project, change to the newly created test projects folder and then use the following command:

# [xUnit](#tab/xunit)

```dotnetcli
cd <NAME OF PROJECT>
dotnet add package bunit.web --version #{NBGV_NuGetPackageVersion}#
dotnet add package bunit.xunit --version #{NBGV_NuGetPackageVersion}#
```

# [NUnit](#tab/nunit)

```dotnetcli
cd <NAME OF PROJECT>
dotnet add package bunit.web --version #{NBGV_NuGetPackageVersion}#
```

# [MSTest](#tab/mstest)

```dotnetcli
cd <NAME OF PROJECT>
dotnet add package bunit.web --version #{NBGV_NuGetPackageVersion}#
```

***

**3. Configure project settings**

The test projects setting needs to be set accordingly: 

- the project's SDK to `Microsoft.NET.Sdk.Razor`
- set `RazorLangVersion` to `3.0`
- set the `<TargetFramework>` to `netcoreapp3.1` (bUnit builds on `.netstandard 2.1`)

To do so, change the first part of the test projects `.csproj` file to look like this.:

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

If using Visual Studio, add the test project to your solution (`.sln`), and add a reference between the test project and project containing the components that should be tested:

```dotnetcli
dotnet sln <NAME OF PROJECT>.sln add <NAME OF TEST PROJECT>
dotnet add <NAME OF COMPONENT PROJECT>.csproj reference <NAME OF TEST PROJECT>.csproj
```

The result should be a test project with a `.csproj` that looks like this (other packages than bUnit might have different version numbers):

# [xUnit](#tab/xunit)

```xml
<Project Sdk="Microsoft.NET.Sdk.Razor">

  <PropertyGroup>
    <TargetFramework>netcoreapp3.1</TargetFramework>
    <RazorLangVersion>3.0</RazorLangVersion>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="bunit.web" Version="#{NBGV_NuGetPackageVersion}#" />
    <PackageReference Include="bunit.xunit" Version="#{NBGV_NuGetPackageVersion}#" />
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
    <PackageReference Include="bunit.web" Version="#{NBGV_NuGetPackageVersion}#" />
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
    <PackageReference Include="bunit.web" Version="#{NBGV_NuGetPackageVersion}#" />
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

To skip a few steps in the guide above, use the [bUnit test project template](https://www.nuget.org/packages/bunit.template/). The bUnit project template is only available for using with xUnit as the general-purpose testing framework, but that will change in the future.

The steps are as follows:

1. Install the template (only needed the first time)
2. Create a new test project
3. Add the test project to your solution

These steps look like this from the `dotnet` CLI:

**1. Install the template**

Install the template from NuGet using this command:

```dotnetcli
dotnet new --install bunit.template::#{NBGV_NuGetPackageVersion}#
```

**2. Create a new test project**

Use the following command to create a bUnit with xUnit test project:

```dotnetcli
dotnet new bunit -o <NAME OF TEST PROJECT>
```

where `-o <NAME OF PROJECT>` is used to name the test project.

**3. Add the test project to your solution**

If using Visual Studio, add the test project to your solution (`.sln`), and add a reference between the test project and project containing the components that should be tested:

```dotnetcli
dotnet sln <NAME OF PROJECT>.sln add <NAME OF TEST PROJECT>
dotnet add <NAME OF COMPONENT PROJECT>.csproj reference <NAME OF TEST PROJECT>.csproj
```

## Further Reading

To start creating tests, continue reading the <xref:writing-csharp-tests> and <xref:writing-razor-tests> pages.

For addition tips and tricks that will make writing tests easier, see the <xref:misc-test-tips> page.