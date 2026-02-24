using System.Diagnostics.CodeAnalysis;

namespace OpenTrivia.Service.Tests;

/// <summary>
/// Stub implementation of <see cref="IOpenTriviaClient"/> for testing service endpoints
/// without making real HTTP calls to the Open Trivia Database API.
/// </summary>
[ExcludeFromCodeCoverage]
internal class StubOpenTriviaClient : IOpenTriviaClient
{
    private readonly List<TriviaCategory> _categories =
    [
        new() { Id = 9, Name = "General Knowledge" },
        new() { Id = 10, Name = "Entertainment: Books" },
    ];

    // Configurable responses — set these to override the default success behavior
    internal ApiResponse<List<TriviaCategory>>? CategoriesResponse { get; set; }
    internal ApiResponse<List<TriviaQuestion>>? QuestionsResponse { get; set; }

    // Captured arguments from the last GetQuestionsAsync call
    internal int? LastQuestionsAmount { get; private set; }
    internal TriviaCategory? LastQuestionsCategory { get; private set; }
    internal TriviaQuestionDifficulty? LastQuestionsDifficulty { get; private set; }
    internal TriviaQuestionType? LastQuestionsType { get; private set; }
    internal ApiEncodingType? LastQuestionsEncoding { get; private set; }

    public Task<ApiResponse<List<TriviaCategory>>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(CategoriesResponse
            ?? new ApiResponse<List<TriviaCategory>>(_categories, responseCode: ApiResponseCode.Success, statusCode: 200));
    }

    public Task<ApiResponse<List<TriviaQuestion>>> GetQuestionsAsync(
        int amount,
        TriviaCategory? category = null,
        TriviaQuestionDifficulty? difficulty = null,
        TriviaQuestionType? type = null,
        ApiEncodingType? encoding = null,
        ApiSessionToken? token = null,
        CancellationToken cancellationToken = default)
    {
        LastQuestionsAmount = amount;
        LastQuestionsCategory = category;
        LastQuestionsDifficulty = difficulty;
        LastQuestionsType = type;
        LastQuestionsEncoding = encoding;

        if (QuestionsResponse != null)
        {
            return Task.FromResult(QuestionsResponse);
        }

        var questions = new List<TriviaQuestion>
        {
            new()
            {
                Question = "What is the capital of France?",
                CorrectAnswer = "Paris",
                IncorrectAnswers = ["London", "Berlin", "Madrid"],
                Category = _categories[0],
                Difficulty = TriviaQuestionDifficulty.Easy,
                Type = TriviaQuestionType.MultipleChoice,
            }
        };
        return Task.FromResult(new ApiResponse<List<TriviaQuestion>>(questions, responseCode: ApiResponseCode.Success, statusCode: 200));
    }

    public Task<ApiResponse<ApiSessionToken>> GetSessionTokenAsync(CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public Task<ApiResponse<ApiSessionToken>> ResetSessionTokenAsync(ApiSessionToken token, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public Task<ApiResponse<TriviaQuestionCount>> GetQuestionCountAsync(TriviaCategory category, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}
