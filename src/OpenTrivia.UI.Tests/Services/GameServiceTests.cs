using Tudormobile.OpenTrivia;
using Tudormobile.OpenTrivia.UI.Services;

namespace OpenTrivia.UI.Tests.Services;

[TestClass]
public class GameServiceTests
{
    private MockOpenTriviaClient _mockClient = null!;
    private IOpenTriviaService _openTriviaService = null!;
    private GameService _gameService = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _mockClient = new MockOpenTriviaClient();
        var openTriviaLogger = NullLogger<OpenTriviaService>.Instance;
        var gameLogger = NullLogger<GameService>.Instance;
        _openTriviaService = new OpenTriviaService(_mockClient, openTriviaLogger);
        _gameService = new GameService(_openTriviaService, gameLogger);
    }

    [TestMethod]
    public async Task CreateGameAsync_WithSuccessfulResponse_ReturnsGame()
    {
        // Arrange
        var categories = new[]
        {
            new TriviaCategory { Id = 9, Name = "General Knowledge" },
            new TriviaCategory { Id = 10, Name = "Entertainment: Books" }
        };

        var questions = new List<TriviaQuestion>
        {
            new()
            {
                Category = categories[0],
                Type = TriviaQuestionType.MultipleChoice,
                Difficulty = TriviaQuestionDifficulty.Easy,
                Question = "What is 2+2?",
                CorrectAnswer = "4",
                IncorrectAnswers = ["3", "5", "6"]
            },
            new()
            {
                Category = categories[1],
                Type = TriviaQuestionType.TrueFalse,
                Difficulty = TriviaQuestionDifficulty.Medium,
                Question = "The book '1984' was written by George Orwell.",
                CorrectAnswer = "True",
                IncorrectAnswers = ["False"]
            }
        };

        _mockClient.SetupQuestionsResponse(new ApiResponse<List<TriviaQuestion>>(questions, null, ApiResponseCode.Success, 200));

        // Act
        var result = await _gameService.CreateGameAsync(2, categories, TestContext.CancellationToken);

        // Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
        Assert.IsNull(result.ErrorMessage);
        Assert.IsNull(result.Exception);
    }

    [TestMethod]
    public async Task CreateGameAsync_WithFailedResponse_ReturnsFailure()
    {
        // Arrange
        var categories = new[] { new TriviaCategory { Id = 9, Name = "General Knowledge" } };
        var apiError = new ApiException("No results");
        _mockClient.SetupQuestionsResponse(new ApiResponse<List<TriviaQuestion>>(null, apiError, ApiResponseCode.NoResults, 200));

        // Act
        var result = await _gameService.CreateGameAsync(10, categories, TestContext.CancellationToken);

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.IsTrue(result.IsFailure);
        Assert.IsNull(result.Data);
        Assert.IsNotNull(result.ErrorMessage);
        Assert.Contains("Failed to create game", result.ErrorMessage);
        Assert.Contains("No results", result.ErrorMessage);
    }

    [TestMethod]
    public async Task CreateGameAsync_WithException_ReturnsFailure()
    {
        // Arrange
        var categories = new[] { new TriviaCategory { Id = 9, Name = "General Knowledge" } };
        var exception = new HttpRequestException("Network error");
        _mockClient.SetupThrow(exception);

        // Act
        var result = await _gameService.CreateGameAsync(10, categories, TestContext.CancellationToken);

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.IsNull(result.Data);
        Assert.IsNotNull(result.ErrorMessage);
        Assert.Contains("Failed to create game", result.ErrorMessage);
        Assert.AreEqual(exception, result.Exception);
    }

    [TestMethod]
    public async Task CreateGameAsync_PassesCancellationToken()
    {
        // Arrange
        var categories = new[] { new TriviaCategory { Id = 9, Name = "General Knowledge" } };
        var questions = new List<TriviaQuestion>
        {
            new()
            {
                Category = categories[0],
                Type = TriviaQuestionType.TrueFalse,
                Difficulty = TriviaQuestionDifficulty.Easy,
                Question = "Test?",
                CorrectAnswer = "Yes",
                IncorrectAnswers = ["No"]
            }
        };
        _mockClient.SetupQuestionsResponse(new ApiResponse<List<TriviaQuestion>>(questions, null, ApiResponseCode.Success, 200));
        using var cts = new CancellationTokenSource();

        // Act
        var result = await _gameService.CreateGameAsync(1, categories, cts.Token);

        // Assert - Verify it completes successfully with the token
        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
    }

    [TestMethod]
    public async Task CreateGameAsync_WithDefaultCancellationToken_Works()
    {
        // Arrange
        var categories = new[] { new TriviaCategory { Id = 9, Name = "General Knowledge" } };
        var questions = new List<TriviaQuestion>
        {
            new()
            {
                Category = categories[0],
                Type = TriviaQuestionType.TrueFalse,
                Difficulty = TriviaQuestionDifficulty.Easy,
                Question = "Test?",
                CorrectAnswer = "Yes",
                IncorrectAnswers = ["No"]
            }
        };
        _mockClient.SetupQuestionsResponse(new ApiResponse<List<TriviaQuestion>>(questions, null, ApiResponseCode.Success, 200));

        // Act - Call without explicitly providing CancellationToken (using default)
        var result = await _gameService.CreateGameAsync(1, categories, TestContext.CancellationToken);

        // Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
    }

    [TestMethod]
    public async Task CreateGameAsync_WithMultipleCategories_CreatesGame()
    {
        // Arrange
        var categories = new[]
        {
            new TriviaCategory { Id = 9, Name = "General Knowledge" },
            new TriviaCategory { Id = 10, Name = "Entertainment: Books" },
            new TriviaCategory { Id = 11, Name = "Entertainment: Film" }
        };

        var questions = new List<TriviaQuestion>
        {
            new()
            {
                Category = categories[0],
                Type = TriviaQuestionType.MultipleChoice,
                Difficulty = TriviaQuestionDifficulty.Easy,
                Question = "Question 1?",
                CorrectAnswer = "Answer 1",
                IncorrectAnswers = ["Wrong 1"]
            },
            new()
            {
                Category = categories[1],
                Type = TriviaQuestionType.TrueFalse,
                Difficulty = TriviaQuestionDifficulty.Medium,
                Question = "Question 2?",
                CorrectAnswer = "Answer 2",
                IncorrectAnswers = ["Wrong 2"]
            },
            new()
            {
                Category = categories[2],
                Type = TriviaQuestionType.MultipleChoice,
                Difficulty = TriviaQuestionDifficulty.Hard,
                Question = "Question 3?",
                CorrectAnswer = "Answer 3",
                IncorrectAnswers = ["Wrong 3"]
            }
        };

        _mockClient.SetupQuestionsResponse(new ApiResponse<List<TriviaQuestion>>(questions, null, ApiResponseCode.Success, 200));

        // Act
        var result = await _gameService.CreateGameAsync(3, categories, TestContext.CancellationToken);

        // Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
    }

    [TestMethod]
    public async Task CreateGameAsync_PreservesExceptionInformation()
    {
        // Arrange
        var categories = new[] { new TriviaCategory { Id = 9, Name = "General Knowledge" } };
        var exception = new InvalidOperationException("Specific error");
        _mockClient.SetupThrow(exception);

        // Act
        var result = await _gameService.CreateGameAsync(10, categories, TestContext.CancellationToken);

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(exception, result.Exception);
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Exception);
        Assert.AreEqual("Specific error", result.Exception.Message);
    }

    public TestContext TestContext { get; set; } // MSTest will set this property
}
