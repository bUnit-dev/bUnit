# GitHub Copilot Instructions for bUnit

This document outlines the coding standards, guidelines, and best practices for contributing to the bUnit project.

## Project Overview

bUnit is a testing library for Blazor Components that runs on top of existing unit testing frameworks (xUnit, NUnit, MSTest, TUnit). The goal is to write comprehensive, stable unit tests that run in milliseconds.

## Coding Standards

### Language and Framework
- **C# Language Version**: Use `preview` features as defined in `Directory.Build.props`
- **Nullable Reference Types**: Always enabled - use nullable annotations appropriately
- **Implicit Usings**: Enabled - common namespaces are automatically imported

### Code Style (Based on .editorconfig)

#### Indentation and Formatting
- Use **tabs** for indentation (not spaces)
- Tab width: 4 spaces
- Charset: UTF-8
- Always trim trailing whitespace (except in .md files)
- Insert final newline in all files

#### C# Style Conventions
- Use `var` for built-in types and when type is apparent
- Prefer expression-bodied members for single-line methods, properties, and accessors
- Use pattern matching over `as` with null check and `is` with cast check
- Prefer object and collection initializers
- Use language keywords instead of framework type names (e.g., `int` not `Int32`)
- Always use braces for code blocks (even single-line if statements)
- Prefer `null` propagation and coalesce expressions
- Use file-scoped namespaces (C# 10+)
- Place using directives outside namespace

#### Naming Conventions
- **PascalCase**: Classes, interfaces (with `I` prefix), methods, properties, events, enums, delegates, namespaces
- **camelCase**: Private fields, local variables, parameters
- **Static readonly fields**: PascalCase
- **Constants**: PascalCase
- **Generic type parameters**: PascalCase with `T` prefix
- **No public/protected instance fields** - use properties instead

#### Modifiers
- Always specify accessibility modifiers explicitly
- Order: `public`, `private`, `protected`, `internal`, `static`, `extern`, `new`, `virtual`, `abstract`, `sealed`, `override`, `readonly`, `unsafe`, `volatile`, `async`

## Build and Test Requirements

### Building the Project
- **Always build in RELEASE mode** for validation: `dotnet build -c Release`
- All warnings are treated as errors in Release configuration
- Projects must be signed with strong name (`key.snk`)
- Enable all analyzers: `AnalysisMode` is set to `AllEnabledByDefault`

### Testing Requirements
- **All tests must pass** before submitting changes
- Run tests in Release mode: `dotnet test -c Release --no-restore`
- Tests must be provided for every bug fix and feature
- Test projects can use relaxed analyzer rules (see `tests/**/.editorconfig`)
- Tests should be specific to the changes being made
- Support for multiple test frameworks: xUnit, NUnit, MSTest, TUnit

### Verification Steps
Before submitting any changes, ensure:
1. `dotnet restore` completes successfully
2. `dotnet build -c Release` completes without warnings or errors
3. `dotnet test -c Release` completes with all tests passing
4. Code adheres to .editorconfig rules (enforced during build)

## Code Quality and Analysis

### Analyzers
- **Microsoft Code Analysis**: Enabled with all rules
- **StyleCop**: Enforced via .editorconfig
- **Meziantou Analyzer**: Specific rules enabled
- **SonarAnalyzer**: Enabled with specific suppressions
- Code style violations are enforced in build (`EnforceCodeStyleInBuild: true`)

### Code Analysis Rules
- Default severity for .NET Code Style: **error**
- `TreatWarningsAsErrors`: **true** in Release mode
- Unused parameters must be removed (`IDE0060`)
- Unnecessary suppressions must be removed (`IDE0079`)

## Development Practices

### Version Control
- Follow **trunk-based development** - base changes on `main` branch
- Use **Conventional Commits** style for commit messages
  - `feat:` for new features
  - `fix:` for bug fixes
  - `docs:` for documentation changes
  - `test:` for test additions/changes
  - `refactor:` for code refactoring
  - `chore:` for maintenance tasks

### Pull Request Guidelines
- Ensure repository can build successfully
- All tests must pass
- Follow existing coding conventions (aligned with ASP.NET Core team guidelines)
- PRs should be focused and specific to the issue being addressed
- Add/update tests to cover your changes

## Project Structure

### Main Projects
- `bunit` - Main package with all functionality
- `bunit.core` - Core testing functionality
- `bunit.web` - Web-specific components testing
- `bunit.web.query` - Testing-library.com query API implementation
- `bunit.generators` - Source generators
- `bunit.template` - Project templates (xUnit, NUnit, MSTest)

### Test Projects
- `bunit.tests` - Main test suite
- `bunit.web.query.tests` - Query API tests
- `bunit.generators.tests` - Source generator tests
- `bunit.testassets` - Shared test assets

## Additional Guidelines

### Documentation
- Update relevant documentation when making changes
- Keep XML documentation comments current
- Documentation must be clear and concise
- Code examples should be tested and working

### Dependencies
- Central Package Management is enabled (`Directory.Packages.props`)
- Follow semantic versioning for package references
- Minimize external dependencies

### Implicit Usings
The following namespaces are automatically imported (except in template and generators projects):
- `Microsoft.AspNetCore.Components`
- `Microsoft.AspNetCore.Components.RenderTree`
- `Microsoft.AspNetCore.Components.Rendering`
- `Microsoft.Extensions.DependencyInjection`
- `System.Runtime.Serialization`
- `System.Diagnostics.CodeAnalysis`

### Performance Considerations
- Tests should run in milliseconds (not seconds)
- Avoid blocking calls in async methods
- Use `ConfigureAwait(false)` where appropriate (except in test code)

### Code of Conduct
- Follow the [.NET Foundation Code of Conduct](https://dotnetfoundation.org/code-of-conduct)
- Be respectful and collaborative in all interactions

## Quick Checklist for Copilot

When making changes, always:
- ✅ Use tabs for indentation
- ✅ Enable nullable reference types
- ✅ Use file-scoped namespaces
- ✅ Build in Release mode (`-c Release`)
- ✅ Run all tests in Release mode
- ✅ Ensure no warnings (warnings = errors in Release)
- ✅ Follow naming conventions (PascalCase for public, camelCase for private)
- ✅ Add XML documentation for public APIs
- ✅ Write tests for all changes
- ✅ Use conventional commit message format
- ✅ Verify all analyzer rules pass
- ✅ Check that code compiles against all target frameworks (.NET 8, 9, 10)
