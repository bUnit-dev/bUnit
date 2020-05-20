# Creating a new bUnit with NUnit test project

To create a project for testing you Blazor components that uses NUnit as the general purpose testing framework, you need to go through these steps:

1. Create a new NUnit testing project
2. Add bUnit to it
3. Set the right project type
4. Add the test project to your solution and add a reference to your component library

These steps look like this from the dotnet CLI:

1\. Create a new test project, use the following command:

```bash
dotnet new nunit -o <NAME OF TEST PROJECT>
```

where `-o <NAME OF PROJECT>` is used to name the test project.

2\. Then you add bUnit to the test project you just created. We just add bUnit's web project, as that references `bunit.core`:

```bash
cd <NAME OF PROJECT>
dotnet add package bunit.web --version 1.0.0-beta-7#{VERSION}#
```

3\. Finally you need to update the project type in the test projects `.csproj` file from `<Project Sdk="Microsoft.NET.Sdk">` to `<Project Sdk="Microsoft.NET.Sdk.Razor">`.

4\. Add the test project to your solution and add a reference between your test project and component project:

```bash
dotnet sln <NAME OF PROJECT>.sln add <NAME OF TEST PROJECT>
dotnet add <NAME OF COMPONENT PROJECT>.csproj reference <NAME OF TEST PROJECT>.csproj
```

The end result should be a test project with a `.csproj` that looks like this (other packages than bUnit might have different version numbers):

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

## Further reading

- [Miscellaneous bUnit testing tips](/docs/misc-test-tips.html)