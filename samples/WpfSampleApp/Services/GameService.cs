using Tudormobile.OpenTrivia;

namespace WpfSampleApp.Services;

internal class GameService : IGameService
{
    private readonly IOpenTriviaService _openTriviaService;
    internal GameService(IOpenTriviaService openTriviaService) { _openTriviaService = openTriviaService; }

    public async Task<TriviaGame> CreateGameAsync(IEnumerable<TriviaCategory> categories)
    {
        var questions = await _openTriviaService.GetQuestionsAsync(10, categories);

        return new TriviaGame(questions, categories);
    }
}

public interface IGameService
{
    public static IGameService Create(IOpenTriviaService openTriviaService)
        => new GameService(openTriviaService);
    public Task<TriviaGame> CreateGameAsync(IEnumerable<TriviaCategory> categories);
}