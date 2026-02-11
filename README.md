# OpenTrivia
Open Trivia Database API library

[![Build and Deploy](https://github.com/tudormobile/OpenTrivia/actions/workflows/dotnet.yml/badge.svg)](https://github.com/tudormobile/OpenTrivia/actions/workflows/dotnet.yml)  [![Publish Docs](https://github.com/tudormobile/OpenTrivia/actions/workflows/docs.yml/badge.svg)](https://github.com/tudormobile/OpenTrivia/actions/workflows/docs.yml)

Copyright (C) 2026 Bill Tudor  

A c# library to access the Open Trivia Database (OpenTriviaDB) Api.

### Quick Start

#### Low-level (Basic) API:
```cs
using Tudormobile.OpenTrivia;

var httpClient = new HttpClient();
var client = new OpenTriviaClient(httpClient);

var query = await client.GetQuestionsAsync(amount: 10);

Console.WriteLine($"Found {query.Data.Count} questions.")
```
> [!TIP]
> See the sample '*SimpleConsoleApp*'.

#### Using the extensions:
```cs
using Tudormobile.OpenTrivia;
using Tudormobile.OpenTrivia.Extensions;

```
#### Using dependency injection:
```cs
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tudormobile.OpenTrivia;
using Tudormobile.OpenTrivia.Extensions;

```
> [!TIP]
> See the sample '*ExtendedConsoleApp*'.

[NuGET Package README](docs/README.md) | [Source Code README](src/README.md) | [API Documentation](https://tudormobile.github.io/OpenTrivia/)
