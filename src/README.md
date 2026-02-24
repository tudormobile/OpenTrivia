# OpenTrivia Library Source Code
The OpenTrivia library can be built, tested, documented, and packaged for distribution using the batch file in the root of the repository.
```
build.cmd
```
## Tooling
Tool dependencies are installed/updated as follows:
- docfx - documentation generation tool
- dotnet-coverage - code coverage tool
- dotnet-reportgenerator-globaltool - report generation
## Build and test
```
dotnet build
dotnet test
```
Locally, you can use the *build.cmd* script to build everything from the command line.
## Package and deploy
Building a release configuration will generate nuget package(s) in the respective output folders.
## Projects
- src\OpenTrivia
    - Library
- src\OpenTrivia.UI
    - Windows Desktop Library (WPF)
- src\OpenTrivia.Service
    - Web Services Library (AspNet)
- src\OpenTrivia.Tests
    - Unit tests
- src\OpenTrivia.UI.Tests
    - Unit tests for desktop library
- src\OpenTrivia.IntegrationTests
    - Integration tests (these hit the actual OpenTriviaDB web services)
- docs\docfx.json
    - Documentation
- samples\
    - **SimpleConsoleApp** - Simple console application
    - **ExtendedConsoleApp** - Console application using extensibility model
    - **WpfSampleApp** - Sample Windows Desktop (WPF) application showcasing the UI library
