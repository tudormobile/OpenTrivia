using Microsoft.Extensions.Logging;

namespace Tudormobile.OpenTrivia.UI.Services;

/// <summary>
/// Implements game management services for creating and managing trivia games.
/// </summary>
internal class GameService : IGameService
{
    private readonly IOpenTriviaService _openTriviaService;
    private readonly ILogger<IGameService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameService"/> class.
    /// </summary>
    /// <param name="openTriviaService">The Open Trivia service used to fetch questions.</param>
    /// <param name="logger">The logger for diagnostic information.</param>
    public GameService(IOpenTriviaService openTriviaService, ILogger<IGameService> logger)
    {
        _openTriviaService = openTriviaService;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<ServiceResult<TriviaGame>> CreateGameAsync(int amount, IEnumerable<TriviaCategory> categories, CancellationToken cancellationToken = default)
    {
        var categoryList = categories.ToList(); // materialize the categories to avoid multiple enumerations
        _logger.LogInformation("Creating game with {Amount} questions from {CategoryCount} categories", amount, categoryList.Count);

        var questions = await _openTriviaService.GetQuestionsAsync(amount, categoryList, cancellationToken);

        if (questions.IsSuccess)
        {
            _logger.LogInformation("Successfully created game with {QuestionCount} questions", questions.Data!.Count);
            return ServiceResult.Success(new TriviaGame(questions.Data!, categoryList));
        }

        _logger.LogWarning("Failed to create game: {Error}", questions.ErrorMessage);
        return ServiceResult.Failure<TriviaGame>($"Failed to create game: {questions.ErrorMessage}", questions.Exception);
    }
}
