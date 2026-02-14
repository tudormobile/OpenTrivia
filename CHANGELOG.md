# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Planned
- Additional sample applications (e.g., WPF, ASP.NET Core Web API, Blazor WebAssembly)
- Support for additional API endpoints (e.g., question count, global question count)
- Enhanced documentation with more discussion, examples, and best practices

## [1.0.0]
### Added
- Integration test suite
- Additional configuration options for `OpenTriviaClient`
- Automatic decoding of questions and answers based on encoding type
- Automatic rate limiting retries

## [0.9.0] - 2026-02-12

### Added
- Initial public release of OpenTrivia library
- `IOpenTriviaClient` interface for interacting with Open Trivia Database API
- `OpenTriviaClient` implementation with comprehensive error handling
- Session token management (`GetSessionTokenAsync`, `ResetSessionTokenAsync`)
- Question retrieval with filtering by category, difficulty, type, and encoding
- Multiple category support with automatic rate limiting (5 seconds between requests)
- Category listing functionality (`GetCategoriesAsync`)
- Builder pattern support via `IOpenTriviaClientBuilder`
- Dependency injection support with `IHttpClientFactory`
- Comprehensive logging support using `ILogger` with Debug, Info, Warning, and Error levels
- Input validation for all public methods using modern C# argument validation
- Custom `ApiException` for API-specific errors
- `ApiResponse<T>` wrapper with success/failure semantics
- `ApiResponseCode` enumeration for all API response codes
- Support for multiple encoding types (Default, Base64, URL RFC 3986)
- Automatic JSON deserialization of API responses
- Proper cancellation token support throughout the API
- Comprehensive XML documentation on all public APIs
- MIT License

### Developer Experience
- MSTest-based unit test suite with up to 100% coverage of critical paths
- Mock infrastructure for testing without API calls
- GitHub Actions CI/CD pipeline for automated builds
- Automated versioning using GitVersion (GitHubFlow)
- Automated documentation generation using DocFX
- Code coverage reporting with dotnet-coverage and ReportGenerator
- Automated NuGet package generation and publishing
- GitHub releases with automatic release notes
- Sample console applications demonstrating library usage

### Technical Details
- Targets .NET 10.0
- Uses C# 14 language features including extension syntax
- Uses async/await patterns
- Dependency on Microsoft.Extensions.Logging and Microsoft.Extensions.Http
- No other external dependencies

## [0.1.0] - 2025-01-xx (Internal)

### Added
- Initial project structure
- Basic API client implementation
- Core domain models

---

## Links

- [NuGet Package](https://www.nuget.org/packages/Tudormobile.OpenTrivia/)
- [GitHub Repository](https://github.com/tudormobile/OpenTrivia)
- [Documentation](https://tudormobile.github.io/OpenTrivia/)
- [API Documentation](https://tudormobile.github.io/OpenTrivia/api/Tudormobile.html)
- [Open Trivia Database API](https://opentdb.com/)

## How to Update This Changelog

### For Maintainers

When making changes, add entries under `## [Unreleased]` in the appropriate category:

- **Added** for new features
- **Changed** for changes in existing functionality
- **Deprecated** for soon-to-be removed features
- **Removed** for now removed features
- **Fixed** for any bug fixes
- **Security** in case of vulnerabilities

When releasing a new version:
1. Change `[Unreleased]` to the version number and date: `[X.Y.Z] - YYYY-MM-DD`
2. Add a new `[Unreleased]` section at the top
3. Commit with message: `chore: release vX.Y.Z`
