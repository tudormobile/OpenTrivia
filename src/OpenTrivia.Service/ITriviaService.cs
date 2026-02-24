namespace Tudormobile.OpenTrivia.Service
{
    public interface ITriviaService
    {
        Task<IResult> CreateGameAsync(HttpContext context, CancellationToken cancellationToken = default);
        Task<IResult> GetCategoriesAsync(HttpContext context, CancellationToken cancellationToken);
        Task<IResult> GetGameAsync(HttpContext context, string id, CancellationToken cancellationToken = default);
        Task<IResult> GetQuestionsAsync(HttpContext context, int amount, string? category = null, string? difficulty = null, string? type = null, string? encode = null, CancellationToken cancellationToken = default);
        Task<IResult> GetStatusAsync(HttpContext context, CancellationToken cancellationToken);
    }
}