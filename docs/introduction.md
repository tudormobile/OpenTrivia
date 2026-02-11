# Introduction

```
using Tudormobile.OpenTrivia;
```

## Tudormobile.OpenTrivia
This namespace contains the basic building blocks for accessing the Open Trivia Database API.

The basic model consists of a client to access the Open Trivia DB web service. Use the *OpenTriviaClient* to retrieve categories and trivia questions from the service.
```cs
var httpClient = new HttpClient();  // Http client to use
var client = new OpenTriviaClient(httpClient);

var categories = await client.GetCategoriesAsync();
var questions = await client.GetQuestionsAsync(
                    amount: 10,
                    difficulty: TriviaQuestionDifficulty.Hard,
                    type: TriviaQuestionType.MultipleChoice);
```
HttpClient management is the responsibility of the host application.

### Tudormobile.OpenTrivia.Extensions
The extensibility model provides the building blocks for an extensible implementation, including interfaces for the *IOpenTriviaClient*, Json serialization, a builder pattern, and additional methods that extend the library.   
```cs
var client = OpenTriviaClient.GetBuilder()
            .WithHttpClient(new HttpClient())
            .Build();
```

### Dependency Injection
The OpenTrivia library takes advantage of the dotnet dependency injection model, extending the IServiceCollection to provide an implementation of IOpenTriviaClient that can be added to the collection using *AddOpenTriviaClient()* extension method.
```cs
HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHttpClient();           // required for OpenTriviaClient to use
builder.Services.AddOpenTriviaClient(options => options.Encoding = ApiEncoding.Base64);
```

### Sample Code
Some simple code samples are provided in the *samples/* folder. 
- SimpleConsoleApp
    - A console application using the simple OpenTriviaClient (no extensions).
- ExtendedConsoleApp
    - A console application using the entity object model provided by the library as well as Microsoft's dependency injection, logging, and application host extensions.
