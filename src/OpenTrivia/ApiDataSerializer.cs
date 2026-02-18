using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using System.Web;

namespace Tudormobile.OpenTrivia;

internal class ApiDataSerializer : IApiDataSerializer
{
    // Caches category instances to ensure object reuse across API calls.
    // Categories are stable and long-lived in the Open Trivia Database API.
    private readonly ConcurrentDictionary<string, TriviaCategory> _categoryNames = [];

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
                _categoryNames[category.Name] = category;
            }
        }
        return categories;
    }

    public List<TriviaQuestion> DeserializeTriviaQuestions(JsonDocument document, ApiEncodingType? decodingType = null)
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
                    ? DecodeJsonElementString(categoryElement, decodingType)
                    : string.Empty;
                var category = categories.FirstOrDefault(c => c.Name == categoryName);
                if (category == null)
                {
                    category = _categoryNames.TryGetValue(categoryName, out var existingCategory)
                        ? existingCategory
                        : new TriviaCategory
                        {
                            Id = categoryId++,
                            Name = categoryName
                        };
                    categories.Add(category);
                }
                var question = new TriviaQuestion()
                {
                    Type = DecodeJsonElementString(questionElement.GetProperty("type"), decodingType) switch
                    {
                        "multiple" => TriviaQuestionType.MultipleChoice,
                        "boolean" => TriviaQuestionType.TrueFalse,
                        "bXVsdGlwbGU=" => TriviaQuestionType.MultipleChoice, // Base64 encoding for "multiple"
                        "Ym9vbGVhbg==" => TriviaQuestionType.TrueFalse, // Base64 encoding for "boolean"
                        _ => throw new InvalidOperationException($"Unknown question type: {questionElement.GetProperty("type").GetString()}")
                    },
                    Difficulty = DecodeJsonElementString(questionElement.GetProperty("difficulty"), decodingType) switch
                    {
                        "easy" => TriviaQuestionDifficulty.Easy,
                        "medium" => TriviaQuestionDifficulty.Medium,
                        "hard" => TriviaQuestionDifficulty.Hard,
                        "ZWFzeQ==" => TriviaQuestionDifficulty.Easy, // Base64 encoding for "easy"
                        "bWVkaXVt" => TriviaQuestionDifficulty.Medium, // Base64 encoding for "medium"
                        "aGFyZA==" => TriviaQuestionDifficulty.Hard, // Base64 encoding for "hard"
                        _ => throw new InvalidOperationException($"Unknown question difficulty: {questionElement.GetProperty("difficulty").GetString()}")
                    },
                    Category = category,
                    Question = DecodeJsonElementString(questionElement.GetProperty("question"), decodingType),
                    CorrectAnswer = DecodeJsonElementString(questionElement.GetProperty("correct_answer"), decodingType),
                    IncorrectAnswers = [.. questionElement.GetProperty("incorrect_answers").EnumerateArray().Select(a => DecodeJsonElementString(a, decodingType))]
                };
                questions.Add(question);
            }
        }
        return questions;
    }

    public TriviaQuestionCount DeserializeTriviaQuestionCount(JsonDocument document)
    {
        try
        {
            var questionCountObject = document.RootElement.GetProperty("category_question_count");
            int total = questionCountObject.GetProperty("total_question_count").GetInt32();
            int easy = questionCountObject.GetProperty("total_easy_question_count").GetInt32();
            int medium = questionCountObject.GetProperty("total_medium_question_count").GetInt32();
            int hard = questionCountObject.GetProperty("total_hard_question_count").GetInt32();
            return new TriviaQuestionCount()
            {
                TotalQuestionCount = total,
                EasyQuestionCount = easy,
                MediumQuestionCount = medium,
                HardQuestionCount = hard
            };
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to deserialize trivia question count. The JSON structure may have changed.", ex);
        }
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

    private static string DecodeJsonElementString(JsonElement jsonElement, ApiEncodingType? encodingType)
    {
        var value = jsonElement.GetString()!;
        if (encodingType.HasValue)
        {
            value = encodingType.Value switch
            {
                ApiEncodingType.Url3986 => Uri.UnescapeDataString(value),
                ApiEncodingType.Base64 => Encoding.UTF8.GetString(Convert.FromBase64String(value)),
                _ => HttpUtility.HtmlDecode(value)
            };
        }
        return value;
    }

}