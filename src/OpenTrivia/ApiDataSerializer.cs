using System.Text.Json;

namespace Tudormobile.OpenTrivia;

internal class ApiDataSerializer : IApiDataSerializer
{
    public ApiSessionToken DeserializeSessionToken(JsonDocument document)
    {
        var token = document.RootElement.TryGetProperty("token", out var tokenProperty)
                    && tokenProperty.ValueKind == JsonValueKind.String
            ? tokenProperty.GetString()!
            : string.Empty;
        return new ApiSessionToken(token);
    }

    public List<TriviaCategory> DeserializeTriviaCategories(JsonDocument document)
    {
        var categories = new List<TriviaCategory>();
        if (document.RootElement.TryGetProperty("trivia_categories", out var categoriesArray) &&
            categoriesArray.ValueKind == JsonValueKind.Array)
        {
            foreach (var categoryElement in categoriesArray.EnumerateArray())
            {
                var category = new TriviaCategory
                {
                    Id = categoryElement.GetProperty("id").GetInt32(),
                    Name = categoryElement.GetProperty("name").GetString()!
                };
                categories.Add(category);
            }
        }
        return categories;
    }

    public List<TriviaQuestion> DeserializeTriviaQuestions(JsonDocument document)
    {
        var categoryId = 1;
        List<TriviaCategory> categories = [];
        var questions = new List<TriviaQuestion>();
        if (document.RootElement.TryGetProperty("results", out var questionsArray) &&
            questionsArray.ValueKind == JsonValueKind.Array)
        {
            foreach (var questionElement in questionsArray.EnumerateArray())
            {
                var categoryName = questionElement.TryGetProperty("category", out var categoryElement) && categoryElement.ValueKind == JsonValueKind.String
                    ? categoryElement.GetString()!
                    : string.Empty;
                var category = categories.FirstOrDefault(c => c.Name == categoryName);
                if (category == null)
                {
                    category = new TriviaCategory
                    {
                        Id = categoryId++,
                        Name = categoryName
                    };
                    categories.Add(category);
                }
                var question = new TriviaQuestion()
                {
                    Type = questionElement.GetProperty("type").GetString() switch
                    {
                        "multiple" => TriviaQuestionType.MultipleChoice,
                        "boolean" => TriviaQuestionType.TrueFalse,
                        _ => throw new InvalidOperationException($"Unknown question type: {questionElement.GetProperty("type").GetString()}")
                    },
                    Difficulty = questionElement.GetProperty("difficulty").GetString() switch
                    {
                        "easy" => TriviaQuestionDifficulty.Easy,
                        "medium" => TriviaQuestionDifficulty.Medium,
                        "hard" => TriviaQuestionDifficulty.Hard,
                        _ => throw new InvalidOperationException($"Unknown question difficulty: {questionElement.GetProperty("difficulty").GetString()}")
                    },
                    Category = category,
                    Question = questionElement.GetProperty("question").GetString()!,
                    CorrectAnswer = questionElement.GetProperty("correct_answer").GetString()!,
                    IncorrectAnswers = [.. questionElement.GetProperty("incorrect_answers").EnumerateArray().Select(a => a.GetString()!)]
                };
                questions.Add(question);
            }
        }
        return questions;
    }

    public ApiResponseCode GetResponseCode(JsonDocument document)
    {
        var responseCode = document.RootElement.TryGetProperty("response_code", out var responseCodeProperty)
                    && responseCodeProperty.ValueKind == JsonValueKind.Number
                    && responseCodeProperty.TryGetInt32(out var responseCodeValue)
                    && Enum.IsDefined((ApiResponseCode)responseCodeValue)
                    ? (ApiResponseCode)responseCodeValue
                    : ApiResponseCode.Unknown;
        return responseCode;
    }
}