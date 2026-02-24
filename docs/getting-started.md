# Getting Started

### Install the package
```bash
dotnet add package Tudormobile.OpenTrivia
```
### Dependencies
Microsoft.Extensions.Logging

### Including library provided services
```cs
using Tudormobile.OpenTrivia;
```

### Including Extensions
```cs
using Tudormobile.OpenTrivia;
using Tudormobile.OpenTrivia.Extensions;
```
Extensions provide additional methods including an entity-object model to further abstract and extend the API by providing useful entities for most API calls.

### Install the package (Windows Desktop)
```bash
dotnet add package Tudormobile.OpenTrivia.UI
```
#### Dependencies
Tudormobile.OpenTrivia
CommunityToolkit.MVVM

### Including library provided services
```cs
using Tudormobile.OpenTrivia.UI;
using Tudormobile.OpenTrivia.UI.Converters;
using Tudormobile.OpenTrivia.UI.Services;
using Tudormobile.OpenTrivia.UI.Views;
using Tudormobile.OpenTrivia.UI.ViewModels;
```
The UI library provides resources, converters, services, views, and view models for building desktop (WPF) UI for Open Trivia games and game elements.

### Install the package (AspNet Web Application)
```bash
dotnet add package Tudormobile.OpenTrivia.Service
```
#### Dependencies
Tudormobile.OpenTrivia

### Including library provided services
```cs
using Tudormobile.OpenTrivia.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTriviaService(
    options => options.WithRateLimitManagement(true));
// ...

var app = builder.Build();
// ...

// Map in the trivia service endpoints
app.UseTriviaService(prefix: String.Empty);

app.Run();
```
You can configure the endpoints to be exposed from the root of your web application or under a provided prefix. 

