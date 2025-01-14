# Contributing to AsanaNet

Thank you for your interest in contributing to AsanaNet! This document provides guidelines and instructions for contributing to the project.

## Table of Contents
- [Code of Conduct](#code-of-conduct)
- [Getting Started](#getting-started)
- [Development Process](#development-process)
- [Pull Request Process](#pull-request-process)
- [Coding Standards](#coding-standards)
- [Testing Guidelines](#testing-guidelines)
- [Documentation](#documentation)

## Code of Conduct

This project adheres to a Code of Conduct that all contributors are expected to follow. Please read [CODE_OF_CONDUCT.md](./CODE_OF_CONDUCT.md) before contributing.

## Getting Started

1. Fork the repository
2. Clone your fork:
   ```bash
   git clone https://github.com/your-username/AsanaNet.git
   ```
3. Add the upstream remote:
   ```bash
   git remote add upstream https://github.com/original-owner/AsanaNet.git
   ```
4. Create a feature branch:
   ```bash
   git checkout -b feature/your-feature-name
   ```

## Development Process

1. **Environment Setup**
   - Install .NET 9.0 SDK
   - Install Visual Studio 2022 or VS Code
   - Install required extensions

2. **Building the Project**
   ```bash
   dotnet build
   ```

3. **Running Tests**
   ```bash
   dotnet test
   ```

4. **Code Style**
   - Use the provided `.editorconfig`
   - Run code cleanup before committing:
     ```bash
     dotnet format
     ```

## Pull Request Process

1. **Before Creating a PR**
   - Ensure all tests pass
   - Update documentation if needed
   - Add tests for new features
   - Follow coding standards
   - Update the changelog

2. **Creating a PR**
   - Create a descriptive title
   - Fill out the PR template
   - Reference any related issues
   - Provide a clear description

3. **PR Review Process**
   - Address reviewer comments
   - Keep the PR focused and small
   - Maintain a clean commit history
   - Update based on feedback

## Coding Standards

### C# Conventions
- Use PascalCase for class names and public members
- Use camelCase for private fields
- Prefix private fields with underscore
- Use async/await consistently
- Follow Microsoft's C# coding conventions

### Example
```csharp
public class ExampleClass
{
    private readonly string _privateField;

    public ExampleClass(string value)
    {
        _privateField = value ?? throw new ArgumentNullException(nameof(value));
    }

    public async Task<string> GetValueAsync()
    {
        await Task.Delay(100);
        return _privateField;
    }
}
```

### Documentation
- Use XML comments for public APIs
- Keep comments clear and concise
- Document exceptions and parameters
- Include code examples where helpful

## Testing Guidelines

### Unit Tests
- Use xUnit for testing
- Follow Arrange-Act-Assert pattern
- Mock external dependencies
- Test edge cases and error conditions

### Example Test
```csharp
public class ExampleTests
{
    [Fact]
    public async Task Method_Scenario_ExpectedBehavior()
    {
        // Arrange
        var sut = new SystemUnderTest();

        // Act
        var result = await sut.MethodAsync();

        // Assert
        Assert.NotNull(result);
    }
}
```

### Integration Tests
- Test real API interactions
- Use appropriate test fixtures
- Clean up test data
- Handle authentication properly

## Documentation

### API Documentation
- Document all public APIs
- Include usage examples
- Explain parameters and return values
- Document exceptions

### README Updates
- Keep installation instructions current
- Update feature list
- Maintain correct version numbers
- Document breaking changes

### Changelog
- Follow semantic versioning
- Document all significant changes
- Include upgrade instructions
- Credit contributors

## Release Process

1. **Version Update**
   - Update version numbers
   - Update changelog
   - Update documentation

2. **Testing**
   - Run all tests
   - Perform manual testing
   - Check documentation

3. **Release**
   - Create release branch
   - Create release tag
   - Update NuGet package
   - Publish release notes

## Questions and Support

- Create an issue for bugs
- Use discussions for questions
- Join our community chat
- Check existing issues first

## License

By contributing to AsanaNet, you agree that your contributions will be licensed under the MIT License. 