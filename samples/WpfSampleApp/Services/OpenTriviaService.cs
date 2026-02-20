using System.Net.Http;
using Tudormobile.OpenTrivia;
using Tudormobile.OpenTrivia.Extensions;

namespace WpfSampleApp.Services;

internal class OpenTriviaService : IOpenTriviaService
{
    internal OpenTriviaService() { }
    public async Task<List<TriviaCategory>> GetCategoriesAsync()
    {
        using var httpClient = new HttpClient();
        var client = IOpenTriviaClient.Create(httpClient);
        var categories = await client.GetCategoriesAsync();
        if (categories.IsSuccess && categories.Data is not null)
        {
            return categories.Data;
        }
        return [];
    }

    public async Task<List<TriviaQuestion>> GetQuestionsAsync(int amount, IEnumerable<TriviaCategory> categories)
    {
        using var httpClient = new HttpClient();
        var client = IOpenTriviaClient.Create(httpClient, autoDecode: true);
        _ = await client.GetCategoriesAsync(); // Preload categories to ensure they are cached and available for question retrieval
        var questions = await client.GetQuestionsAsync(amount, categories);
        if (questions.IsSuccess && questions.Data is not null)
        {
            return questions.Data;
        }
        return [];
    }
}

public interface IOpenTriviaService
{
    public static IOpenTriviaService Create() => new OpenTriviaService();

    Task<List<TriviaCategory>> GetCategoriesAsync();

    Task<List<TriviaQuestion>> GetQuestionsAsync(int amount, IEnumerable<TriviaCategory> categories);
}
