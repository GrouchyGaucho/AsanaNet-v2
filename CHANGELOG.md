# Changelog

All notable changes to the AsanaNet project will be documented in this file.

## [2.1.0-alpha] - 01-14-2025

### Added
- Added comprehensive XML documentation to all models and interfaces
- Added new `AsanaTaskDependency` model for better dependency tracking
- Added `AsanaCustomFieldValue` model for custom field support
- Added generic type support for `AsanaObjectCollection<T>`
- Added comprehensive constants in `AsanaConstants` class including:
  - Additional endpoint paths
  - Common API parameters
  - Resource types
  - Default values and limits
  - Date format constants

### Enhanced
- Improved error handling with detailed error messages
- Enhanced task management functionality with:
  - Better dependency management
  - Improved attachment handling
  - Enhanced custom field support
  - Comprehensive like/heart functionality
  - Section management capabilities
- Updated all collection-returning methods to use generic `AsanaObjectCollection<T>`
- Improved type safety throughout the API
- Enhanced nullability handling in all models

### Documentation
- Added XML documentation to all public models:
  - `AsanaError`
  - `AsanaHeart`
  - `AsanaTaskMembership`
  - `AsanaSection`
  - `AsanaEnumOption`
  - `AsanaDate`
  - `AsanaTag`
  - `AsanaTaskDependency`
  - `AsanaCustomFieldValue`
  - `AsanaStory`
  - `AsanaAttachment`
- Improved method documentation in `IAsanaClient`
- Added comprehensive parameter documentation

### Technical Improvements
- Standardized error handling across all API calls
- Improved response deserialization with null checks
- Enhanced type safety with generic collections
- Added proper initialization of collection properties
- Standardized resource type handling

## [2.0.0] - 01-14-2025

### Breaking Changes
- Migrated to .NET 9.0
- Changed authentication to always use Bearer token instead of Basic authentication
- Removed old `AsanaDateTime` class in favor of standard `DateTime`
- Made `ApiKey` property init-only
- Updated `IAsanaClient` interface to reflect modern C# practices
- Moved all model classes from `Objects` folder to new `Models` folder following modern .NET conventions

### Added
- New `IAsanaClient` interface for better dependency injection support
- Added Microsoft.Extensions.DependencyInjection support
- Added proper nullable reference type support
- Added XML documentation comments
- Added modern response models with System.Text.Json serialization
- Added cancellation token support for all async operations
- Added proper exception handling and error callbacks
- Added new `Models` folder for better code organization

### Changed
- Modernized project structure to SDK-style project format
- Updated all async methods to use Task-based asynchronous pattern
- Changed error callback to use `Exception` instead of string parameters
- Made API responses more strongly typed
- Updated HTTP client usage to modern practices
- Made properties required where appropriate using C# 11 required modifier
- Improved configuration handling in sample project
- Reorganized model classes from `Objects` to `Models` folder, including:
  - `AsanaObject.cs` (base class)
  - `AsanaObjectCollection.cs`
  - `AsanaUser.cs`
  - `AsanaWorkspace.cs`
  - `AsanaTeam.cs`
  - `AsanaProject.cs`
  - `AsanaTask.cs`
  - `AsanaEvents.cs`

### Removed
- Removed T4 templates and code generation
- Removed old delegate declarations
- Removed unused namespaces and dependencies
- Removed outdated synchronous API methods
- Removed old Basic authentication option
- Removed legacy `Objects` folder in favor of modern `Models` structure

### Fixed
- Fixed authentication header format to use Bearer token
- Fixed base URL trailing slash issue
- Fixed configuration key in sample project
- Fixed nullable warnings throughout the codebase

### Dependencies
- Added Microsoft.Extensions.DependencyInjection
- Added Microsoft.Extensions.Configuration.Json
- Updated to System.Text.Json for JSON serialization

### Documentation
- Updated README.md to reflect new changes
- Added XML documentation comments to public APIs
- Updated sample project to demonstrate modern usage

## Notes
- This version represents a major update with breaking changes
- The project now follows modern .NET practices and patterns
- Improved type safety and null handling
- Better integration with dependency injection frameworks 