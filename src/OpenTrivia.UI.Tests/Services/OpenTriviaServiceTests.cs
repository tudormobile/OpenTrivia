using Tudormobile.OpenTrivia;
using Tudormobile.OpenTrivia.UI.Services;

namespace OpenTrivia.UI.Tests.Services;

[TestClass]
public class OpenTriviaServiceTests
{
    private MockOpenTriviaClient _mockClient = null!;
    private OpenTriviaService _service = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _mockClient = new MockOpenTriviaClient();
        var logger = NullLogger<OpenTriviaService>.Instance;
        _service = new OpenTriviaService(_mockClient, logger);
    }

    #region GetCategoriesAsync Tests

    [TestMethod]
    public async Task GetCategoriesAsync_WithSuccessfulResponse_ReturnsSuccess()
    {
        // Arrange
        var categories = new List<TriviaCategory>
        {
            new() { Id = 1, Name = "Category 1" },
            new() { Id = 2, Name = "Category 2" }
        };
        _mockClient.SetupCategoriesResponse(new ApiResponse<List<TriviaCategory>>(categories, null, ApiResponseCode.Success, 200));

        // Act
        var result = await _service.GetCategoriesAsync(TestContext.CancellationToken);

        // Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
        Assert.HasCount(2, result.Data);
        Assert.AreEqual("Category 1", result.Data[0].Name);
        Assert.AreEqual("Category 2", result.Data[1].Name);
    }

    [TestMethod]
    public async Task GetCategoriesAsync_WithFailedResponse_ReturnsFailure()
    {
        // Arrange
        var apiError = new ApiException("API Error");
        _mockClient.SetupCategoriesResponse(new ApiResponse<List<TriviaCategory>>(null, apiError, ApiResponseCode.NoResults, 200));

        // Act
        var result = await _service.GetCategoriesAsync(TestContext.CancellationToken);

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.IsTrue(result.IsFailure);
        Assert.IsNull(result.Data);
        Assert.AreEqual("API Error", result.ErrorMessage);
    }

    [TestMethod]
    public async Task GetCategoriesAsync_WithNullData_ReturnsFailure()
    {
        // Arrange
        _mockClient.SetupCategoriesResponse(new ApiResponse<List<TriviaCategory>>(null, null, ApiResponseCode.Success, 200));

        // Act
        var result = await _service.GetCategoriesAsync(TestContext.CancellationToken);

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.ErrorMessage);
        Assert.AreEqual("Failed to fetch categories", result.ErrorMessage);
    }

    [TestMethod]
    public async Task GetCategoriesAsync_WithException_ReturnsFailure()
    {
        // Arrange
        var exception = new InvalidOperationException("Network error");
        _mockClient.SetupThrow(exception);

        // Act
        var result = await _service.GetCategoriesAsync(TestContext.CancellationToken);

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("Network error", result.ErrorMessage);
        Assert.AreEqual(exception, result.Exception);
    }

    [TestMethod]
    public async Task GetCategoriesAsync_PassesCancellationToken()
    {
        // Arrange
        var categories = new List<TriviaCategory>
        {
            new() { Id = 1, Name = "Category 1" }
        };
        _mockClient.SetupCategoriesResponse(new ApiResponse<List<TriviaCategory>>(categories, null, ApiResponseCode.Success, 200));
        using var cts = new CancellationTokenSource();

        // Act
        var result = await _service.GetCategoriesAsync(cts.Token);

        // Assert - Verify it completes successfully with the token
        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
    }

    #endregion

    #region GetQuestionsAsync Tests

    [TestMethod]
    public async Task GetQuestionsAsync_WithSuccessfulResponse_ReturnsSuccess()
    {
        // Arrange
        var questions = new List<TriviaQuestion>
        {
            new()
            {
                Category = new TriviaCategory { Id = 1, Name = "Science" },
                Type = TriviaQuestionType.MultipleChoice,
                Difficulty = TriviaQuestionDifficulty.Easy,
                Question = "Question 1?",
                CorrectAnswer = "Answer 1",
                IncorrectAnswers = ["Wrong 1", "Wrong 2"]
            },
            new()
            {
                Category = new TriviaCategory { Id = 1, Name = "Science" },
                Type = TriviaQuestionType.MultipleChoice,
                Difficulty = TriviaQuestionDifficulty.Medium,
                Question = "Question 2?",
                CorrectAnswer = "Answer 2",
                IncorrectAnswers = ["Wrong 1", "Wrong 2"]
            }
        };
        _mockClient.SetupQuestionsResponse(new ApiResponse<List<TriviaQuestion>>(questions, null, ApiResponseCode.Success, 200));

        var categories = new[] { new TriviaCategory { Id = 1, Name = "Science" } };

        // Act
        var result = await _service.GetQuestionsAsync(10, categories, TestContext.CancellationToken);

        // Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
        Assert.HasCount(2, result.Data);
        Assert.AreEqual("Question 1?", result.Data[0].Question);
        Assert.AreEqual("Question 2?", result.Data[1].Question);
        Assert.AreEqual(1, _mockClient.GetCategoriesCallCount); // Only the preload call
        Assert.AreEqual(1, _mockClient.GetQuestionsCallCount);
    }

    [TestMethod]
    public async Task GetQuestionsAsync_CallsInitializationOnlyOnce()
    {
        // Arrange
        var questions = new List<TriviaQuestion>
        {
            new()
            {
                Category = new TriviaCategory { Id = 1, Name = "Science" },
                Type = TriviaQuestionType.MultipleChoice,
                Difficulty = TriviaQuestionDifficulty.Easy,
                Question = "Question 1?",
                CorrectAnswer = "Answer 1",
                IncorrectAnswers = ["Wrong 1", "Wrong 2"]
            }
        };
        _mockClient.SetupQuestionsResponse(new ApiResponse<List<TriviaQuestion>>(questions, null, ApiResponseCode.Success, 200));

        var categories = new[] { new TriviaCategory { Id = 1, Name = "Science" } };

        // Act - Call multiple times
        await _service.GetQuestionsAsync(10, categories, TestContext.CancellationToken);
        await _service.GetQuestionsAsync(10, categories, TestContext.CancellationToken);
        await _service.GetQuestionsAsync(10, categories, TestContext.CancellationToken);

        // Assert - Should initialize categories only once
        Assert.AreEqual(1, _mockClient.GetCategoriesCallCount);
        Assert.AreEqual(3, _mockClient.GetQuestionsCallCount);
    }

    [TestMethod]
    public async Task GetQuestionsAsync_AfterGetCategories_DoesNotCallCategoriesAgain()
    {
        // Arrange
        var questions = new List<TriviaQuestion>
        {
            new()
            {
                Category = new TriviaCategory { Id = 1, Name = "Science" },
                Type = TriviaQuestionType.MultipleChoice,
                Difficulty = TriviaQuestionDifficulty.Easy,
                Question = "Question 1?",
                CorrectAnswer = "Answer 1",
                IncorrectAnswers = ["Wrong 1", "Wrong 2"]
            }
        };
        _mockClient.SetupQuestionsResponse(new ApiResponse<List<TriviaQuestion>>(questions, null, ApiResponseCode.Success, 200));

        var categories = new[] { new TriviaCategory { Id = 1, Name = "Science" } };

        // Act - Call GetCategories first, then GetQuestions
        await _service.GetCategoriesAsync(TestContext.CancellationToken);
        await _service.GetQuestionsAsync(10, categories, TestContext.CancellationToken);

        // Assert - Should call categories only once (from GetCategoriesAsync)
        Assert.AreEqual(1, _mockClient.GetCategoriesCallCount);
        Assert.AreEqual(1, _mockClient.GetQuestionsCallCount);
    }

    [TestMethod]
    public async Task GetQuestionsAsync_WithFailedResponse_ReturnsFailure()
    {
        // Arrange
        var apiError = new ApiException("No questions available");
        _mockClient.SetupQuestionsResponse(new ApiResponse<List<TriviaQuestion>>(null, apiError, ApiResponseCode.NoResults, 200));

        var categories = new[] { new TriviaCategory { Id = 1, Name = "Science" } };

        // Act
        var result = await _service.GetQuestionsAsync(10, categories, TestContext.CancellationToken);

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("No questions available", result.ErrorMessage);
    }

    [TestMethod]
    public async Task GetQuestionsAsync_WithNullData_ReturnsFailure()
    {
        // Arrange
        _mockClient.SetupQuestionsResponse(new ApiResponse<List<TriviaQuestion>>([], null, ApiResponseCode.Success, 200));

        var categories = new[] { new TriviaCategory { Id = 1, Name = "Science" } };

        // Act
        var result = await _service.GetQuestionsAsync(10, categories, TestContext.CancellationToken);

        // Assert
        Assert.IsTrue(result.IsSuccess); // Empty list is still successful
        Assert.IsNotNull(result.Data);
        Assert.HasCount(0, result.Data);
    }

    [TestMethod]
    public async Task GetQuestionsAsync_WithException_ReturnsFailure()
    {
        // Arrange
        var exception = new HttpRequestException("Network timeout");
        _mockClient.SetupThrow(exception);

        var categories = new[] { new TriviaCategory { Id = 1, Name = "Science" } };

        // Act
        var result = await _service.GetQuestionsAsync(10, categories, TestContext.CancellationToken);

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("Network timeout", result.ErrorMessage);
        Assert.AreEqual(exception, result.Exception);
    }

    [TestMethod]
    public async Task GetQuestionsAsync_PassesCancellationToken()
    {
        // Arrange
        var questions = new List<TriviaQuestion>
        {
            new()
            {
                Category = new TriviaCategory { Id = 1, Name = "Science" },
                Type = TriviaQuestionType.MultipleChoice,
                Difficulty = TriviaQuestionDifficulty.Easy,
                Question = "Test?",
                CorrectAnswer = "Answer",
                IncorrectAnswers = ["Wrong"]
            }
        };
        _mockClient.SetupQuestionsResponse(new ApiResponse<List<TriviaQuestion>>(questions, null, ApiResponseCode.Success, 200));
        var categories = new[] { new TriviaCategory { Id = 1, Name = "Science" } };
        using var cts = new CancellationTokenSource();

        // Act
        var result = await _service.GetQuestionsAsync(10, categories, cts.Token);

        // Assert - Verify it completes successfully with the token
        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
    }

    #endregion

    #region Concurrent Access Tests

    [TestMethod]
    public async Task GetQuestionsAsync_ConcurrentCalls_InitializesOnlyOnce()
    {
        // Arrange
        var questions = new List<TriviaQuestion>
        {
            new()
            {
                Category = new TriviaCategory { Id = 1, Name = "Science" },
                Type = TriviaQuestionType.MultipleChoice,
                Difficulty = TriviaQuestionDifficulty.Easy,
                Question = "Question 1?",
                CorrectAnswer = "Answer 1",
                IncorrectAnswers = ["Wrong 1", "Wrong 2"]
            }
        };
        _mockClient.SetupQuestionsResponse(new ApiResponse<List<TriviaQuestion>>(questions, null, ApiResponseCode.Success, 200));

        var categories = new[] { new TriviaCategory { Id = 1, Name = "Science" } };

        // Act - Make concurrent calls
        var tasks = Enumerable.Range(0, 10)
            .Select(_ => _service.GetQuestionsAsync(10, categories, TestContext.CancellationToken))
            .ToArray();

        await Task.WhenAll(tasks);

        // Assert - Should initialize categories only once despite concurrent calls
        Assert.AreEqual(1, _mockClient.GetCategoriesCallCount);
        Assert.AreEqual(10, _mockClient.GetQuestionsCallCount);

        // All calls should succeed
        foreach (var task in tasks)
        {
            Assert.IsTrue(task.Result.IsSuccess);
        }
    }

    public TestContext TestContext { get; set; }

    #endregion
}
