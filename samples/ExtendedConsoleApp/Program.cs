using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tudormobile.OpenTrivia;
using Tudormobile.OpenTrivia.Extensions;

namespace ExtendedConsoleApp;

internal class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        // Setup Host with Open Trivia DB Client
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        builder.Services
            .AddOpenTriviaClient(options => { options.UseAutoDecoding().WithEncoding(ApiEncodingType.Base64); })
            .AddLogging(builder => builder.AddConsole());

        using IHost host = builder.Build();
        var logger = host.Services.GetRequiredService<ILogger<Program>>();

        // Grab a client and get some trivia questions
        var client = host.Services.GetRequiredService<IOpenTriviaClient>();

        var categories = await client.GetCategoriesAsync();
        var category = categories.Data?.FirstOrDefault();
        var questions = await client.GetQuestionsAsync(5, category, TriviaQuestionDifficulty.Easy, TriviaQuestionType.TrueFalse);

        logger.LogInformation("GetQuestionsAsync IsSuccess: '{success}'", questions.IsSuccess);
        logger.LogInformation("GetQuestionsAsync ErrorMessage: '{message}'", questions.ErrorMessage);
        logger.LogInformation("GetQuestionsAsync Data: '{count} questions.'", questions.Data?.Count ?? 0);

        // print some result details
        if (questions.Data is not null)
        {
            foreach (var (index, question) in questions.Data.Index())
            {
                logger.LogInformation("Question {index}: {question}", index + 1, question.Question);
            }
        }
    }
}
