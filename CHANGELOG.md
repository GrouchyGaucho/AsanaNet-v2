# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.1.0-alpha] - 2025-01-14

### Added
- Enhanced test coverage with comprehensive test suite
- Support for both Basic and OAuth authentication
- Improved multipart form data handling for file uploads
- Event-based error notifications
- Detailed API documentation
- Comprehensive test report
- Strong request validation with descriptive messages

### Changed
- Updated to .NET 9.0
- Improved error handling with specific exception types
- Enhanced mock infrastructure for testing
- Updated authentication flow
- Modernized JSON handling using System.Text.Json

### Fixed
- Content type handling in file uploads
- Authentication header formatting
- Error response deserialization
- Task dependency handling
- Multipart form data boundaries

### Security
- Improved API key handling
- Enhanced OAuth token validation
- Secure header management
- Better error message sanitization

## [2.0.0] - 2025-01-01

### Added
- Initial release of modernized version
- .NET 9.0 support
- Async/await implementation
- Dependency injection support
- Modern C# features
- Basic test suite

### Changed
- Complete rewrite from original 2012 version
- Updated to modern .NET practices
- Enhanced error handling
- Improved type safety

### Removed
- Legacy synchronous methods
- Obsolete authentication methods
- Deprecated API endpoints

## [1.0.0] - 2012

### Added
- Initial release by Antony Woods
- Basic Asana API support
- Simple authentication
- Core task operations 