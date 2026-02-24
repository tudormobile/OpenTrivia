# Tudormobile OpenTrivia Service Library
**Tudormobile.OpenTrivia.Service** provides a library to build web services that access the Open Trivia Database API.  

[Source Code](https://github.com/tudormobile/OpenTrivia) | [Documentation](https://tudormobile.github.io/OpenTrivia/) | [API documentation](https://tudormobile.github.io/OpenTrivia/api/Tudormobile.html)
## Getting Started
### Install the package
```bash
dotnet add package Tudormobile.OpenTrivia.Service
```
### Prerequisites
Microsoft.AspNetCore.App is required to use the Tudormobile.OpenTrivia.Service library.
```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
```
Requires .NET 10.0 or later.

### Dependencies
Tudormobile.OpenTrivia (will be automatically installed as a package dependency)

**Tudormobile.OpenTrivia.Service** is released as open source under the MIT license. Bug reports and contributions can be made at [the GitHub repository](https://github.com/tudormobile/OpenTrivia).

## Endpoints
The following endpoints are available in the Tudormobile.OpenTrivia.Service library:
- **/trivia/api/v1**: Root endpoint that provides information about the service.
- **/trivia/api/v1/categories**: Retrieves a list of trivia categories.
- **/trivia/api/v1/questions**: Endpoint for retrieving trivia questions based on specified criteria such as category, difficulty, and type.
- **/trivia/api/v1/game**: Endpoint for managing trivia games, including creating a new game, joining an existing game, and retrieving game details.

The questions and categories endpoints will proxy requests to the Open Trivia Database API and are provided only as a convenience. Feel free to use the Open Trivia Database API directly if you prefer. The game endpoint is specific to this service and will manage game state and player interactions within the service. The game endpoint utilizes category IDs from the Open Trivia Database API, so you can use the categories endpoint to retrieve valid category IDs for use in the game endpoint.

See the [API documentation](https://tudormobile.github.io/OpenTrivia) for a complete description of the available endpoints as well as detailed information on request and response formats for each endpoint.

## Api Response Format
All API responses from the Tudormobile.OpenTrivia.Service library are returned in JSON format. The structure of the response will vary depending on the endpoint being accessed, but generally follows a consistent format that includes success/failure status and relevant data about the request.

For example, the response from the categories endpoint looks like this:
```json
{
  "success": true,
  "data": {
    "trivia_categories": [
    {"id": 9, "name": "General Knowledge"},
    {"id": 10, "name": "Entertainment: Books"},
    ...
    ],
  "error": null
}
```
The response from the questions endpoint looks like this:
```json
{
  "success": true,
  "data": {
    "response_code": 0,
    "results": [
      {
        "category": "General Knowledge",
        "type": "multiple",
        "difficulty": "easy",
        "question": "What is the capital of France?",
        "correct_answer": "Paris",
        "incorrect_answers": ["London", "Berlin", "Madrid"]
      },
      ...
    ]
  },
  "error": null
}
```
The response from the game endpoint will vary based on the specific action being performed (e.g. creating a game, joining a game, retrieving game details), but will generally include information about the game state, players, and questions.
```json
{
  "success": true,
  "data": {
    "game_id": "12345",
    "categories": [9, 10],
    "difficulty": ["easy", "medium", "hard"],
    "types": ["multiple"],
    "groups" : [
      {
        "group_id": "group1", 
        "name": "Group A",
        "group_state": "approved",
        "players": [
          {"player_id": "player1", "name": "Alice", "score": 10},
          {"player_id": "player2", "name": "Bob", "score": 5}
        ]
      },
      {
        "group_id": "group2", 
        "name": "Group B",
        "group_state": "approved",
        "players": [
          {"player_id": "player1", "name": "Alice", "score": 10},
          {"player_id": "player2", "name": "Bob", "score": 5}
        ]
      }
    ],
      {
        "category": "General Knowledge",
        "type": "multiple",
        "difficulty": "easy",
        "question": "What is the capital of France?",
        "correct_answer": "Paris",
        "incorrect_answers": ["London", "Berlin", "Madrid"]
      },
      ...
    ],
    "game_state": { ... }
  },
  "error": null
}
```
The error object (shown as null in the above examples) is normally ommitted when the request is successful, but will contain information about any errors that occurred during the request if the success field is false. The structure of the error object is as follows:
```json
{
  "code": "ERROR_CODE",
  "message": "Detailed error message describing the issue."
}
```
Error codes are specific to the endpoint and action being performed, but may include codes such as "INVALID_CATEGORY", "GAME_NOT_FOUND", "PLAYER_ALREADY_JOINED", etc. The error message will provide more detailed information about the nature of the error and how to resolve it. The API documentation provides a complete list of possible error codes and their meanings for each endpoint.
## License
**Tudormobile.OpenTrivia.Service** is released under the MIT License.