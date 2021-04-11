---
uid: create-test-project
title: Creating a new bUnit test project
---

# Creating a new bUnit test project

To write tests, you need a place to put them - a test project. bUnit is not a unit test runner, so a general-purpose test framework like xUnit, NUnit, or MSTest is needed in addition to bUnit in order to write and run tests. 

To use bUnit with xUnit, the easiest approach is to use the bUnit project template described in the [Create a test project with bUnit template](#creating-a-test-project-with-bunit-template) section further down the page. To create a test project manually and in a general-purpose testing frameworks agnostic way, read the following section.

## Creating a test project manually

This section will take you through the steps required to create a project for testing Blazor components using bUnit. Any of the three general-purpose test frameworks shown in step 1 below can be used. Briefly, here is what we will do:

1. Create a new xUnit/NUnit/MSTest testing project
2. Add bUnit to the test project
3. Configure project settings
4. Add the test project to your existing solution

Let's look at these in more detail. These steps look like this from the 'dotnet' CLI:

**1. Create a new test project**

Use the following command (_click on the tab for the test framework of choice_):

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

The `-o` option in the `dotnet new` command above is used to specify the name of the test project.

**2. Add bUnit to the test project**

To add bUnit to the test project, change to the newly created test projects folder and use the following command:

```dotnetcli
cd <NAME OF PROJECT>
dotnet add package bunit --version #{NBGV_NuGetPackageVersion}#
```

**3. Configure project settings**

The test projects setting needs to be set to the following: 

- the project's SDK needs to be set to `Microsoft.NET.Sdk.Razor`
- set the `<TargetFramework>` to `net5.0`

> [!NOTE]
> bUnit works with `netcoreapp3.1` and `netstandard2.1` test projects as well.

To do so, change the first part of the test projects `.csproj` file to look like this.:

```xml
<Project Sdk="Microsoft.NET.Sdk.Razor">

  <PropertyGroup>
    <TargetFramework>net5.0</TargetFramework>
  </PropertyGroup>  ...

</Project>
```

**4. Add the test project to your solution**

If using Visual Studio, add the test project to your solution (`.sln`), and add a reference between the test project and the project containing the components that are to be tested:

```dotnetcli
dotnet sln <NAME OF PROJECT>.sln add <NAME OF TEST PROJECT>
dotnet add <NAME OF TEST PROJECT>.csproj reference <NAME OF COMPONENT PROJECT>.csproj
```

The result should be a test project with a `.csproj` that looks like this (non bUnit packages may have different version numbers):

# [xUnit](#tab/xunit)

```xml
<Project Sdk="Microsoft.NET.Sdk.Razor">

  <PropertyGroup>
    <TargetFramework>net5.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="bunit" Version="1.0.16" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="16.7.1" />
    <PackageReference Include="xunit" Version="2.4.1" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.4.3">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="<PATH TO COMPONENT LIB>.csproj" />
  </ItemGroup>
  
</Project>
```

# [NUnit](#tab/nunit)

```xml
<Project Sdk="Microsoft.NET.Sdk.Razor">

  <PropertyGroup>
    <TargetFramework>net5.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>    
    <PackageReference Include="bunit" Version="1.0.16" />
    <PackageReference Include="nunit" Version="3.12.0" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="16.7.1" />
    <PackageReference Include="NUnit3TestAdapter" Version="3.16.1">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
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
    <TargetFramework>net5.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="bunit" Version="1.0.16" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="16.7.1" />
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

## Creating a test project with bUnit template

To skip a few steps in the guide above, use the [bUnit test project template](https://www.nuget.org/packages/bunit.template/). The bUnit project template currently only works with the xUnit general-purpose testing framework, but other frameworks will be supported in the future.

The steps  for creating a test project with the bUnit template are as follows:

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

The `-o` option in the `dotnet new` command above is used to specify the name of the test project.

**3. Add the test project to your solution**

If using Visual Studio, add the test project to your solution (`.sln`), and add a reference between the test project and the project containing the components that should be tested:

```dotnetcli
dotnet sln <NAME OF PROJECT>.sln add <NAME OF TEST PROJECT>
dotnet add <NAME OF TEST PROJECT>.csproj reference <NAME OF COMPONENT PROJECT>.csproj
```

This will allow the test project to see and test the components in the component project.

## Further reading

To start creating tests, continue reading the <xref:writing-tests> page.
