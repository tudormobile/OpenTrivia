using Tudormobile.OpenTrivia;

namespace OpenTrivia.UI.Tests.Services;

/// <summary>
/// Mock implementation of IOpenTriviaClient for testing purposes.
/// </summary>
internal class MockOpenTriviaClient : IOpenTriviaClient
{
    private readonly List<TriviaCategory> _categories;
    private readonly List<TriviaQuestion> _questions;
    private ApiResponse<List<TriviaCategory>>? _categoriesResponse;
    private ApiResponse<List<TriviaQuestion>>? _questionsResponse;
    private bool _shouldThrow;
    private Exception? _exceptionToThrow;

    public int GetCategoriesCallCount { get; private set; }
    public int GetQuestionsCallCount { get; private set; }

    public MockOpenTriviaClient()
    {
        _categories =
        [
            new TriviaCategory { Id = 9, Name = "General Knowledge" },
            new TriviaCategory { Id = 10, Name = "Entertainment: Books" },
            new TriviaCategory { Id = 11, Name = "Entertainment: Film" }
        ];

        _questions =
        [
            new TriviaQuestion
            {
                Category = _categories[0],
                Type = TriviaQuestionType.MultipleChoice,
                Difficulty = TriviaQuestionDifficulty.Easy,
                Question = "What is the capital of France?",
                CorrectAnswer = "Paris",
                IncorrectAnswers = ["London", "Berlin", "Madrid"]
            },
            new TriviaQuestion
            {
                Category = _categories[1],
                Type = TriviaQuestionType.TrueFalse,
                Difficulty = TriviaQuestionDifficulty.Medium,
                Question = "The book '1984' was written by George Orwell.",
                CorrectAnswer = "True",
                IncorrectAnswers = ["False"]
            }
        ];
    }

    public void SetupCategoriesResponse(ApiResponse<List<TriviaCategory>> response)
    {
        _categoriesResponse = response;
    }

    public void SetupQuestionsResponse(ApiResponse<List<TriviaQuestion>> response)
    {
        _questionsResponse = response;
    }

    public void SetupThrow(Exception exception)
    {
        _shouldThrow = true;
        _exceptionToThrow = exception;
    }

    public Task<ApiResponse<List<TriviaCategory>>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        GetCategoriesCallCount++;

        cancellationToken.ThrowIfCancellationRequested();

        if (_shouldThrow && _exceptionToThrow != null)
        {
            throw _exceptionToThrow;
        }

        var response = _categoriesResponse ?? new ApiResponse<List<TriviaCategory>>(_categories, null, ApiResponseCode.Success, 200);
        return Task.FromResult(response);
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
        GetQuestionsCallCount++;

        cancellationToken.ThrowIfCancellationRequested();

        if (_shouldThrow && _exceptionToThrow != null)
        {
            throw _exceptionToThrow;
        }

        var response = _questionsResponse ?? new ApiResponse<List<TriviaQuestion>>(_questions, null, ApiResponseCode.Success, 200);
        return Task.FromResult(response);
    }

    public Task<ApiResponse<List<TriviaQuestion>>> GetQuestionsAsync(
#pragma warning disable IDE0060 // Remove unused parameter
        int amount,
        IEnumerable<TriviaCategory> categories,
        TriviaQuestionDifficulty? difficulty = null,
        TriviaQuestionType? type = null,
        ApiEncodingType? encoding = null,
        ApiSessionToken? token = null,
        CancellationToken cancellationToken = default)
#pragma warning restore IDE0060 // Remove unused parameter
    {
        GetQuestionsCallCount++;

        cancellationToken.ThrowIfCancellationRequested();

        if (_shouldThrow && _exceptionToThrow != null)
        {
            throw _exceptionToThrow;
        }

        var response = _questionsResponse ?? new ApiResponse<List<TriviaQuestion>>(_questions, null, ApiResponseCode.Success, 200);
        return Task.FromResult(response);
    }

    #region Not Implemented Methods

    public Task<ApiResponse<ApiSessionToken>> GetSessionTokenAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<ApiSessionToken>> ResetSessionTokenAsync(ApiSessionToken token, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<TriviaQuestionCount>> GetQuestionCountAsync(TriviaCategory category, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    #endregion
}
