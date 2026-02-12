namespace Tudormobile.OpenTrivia.Extensions;

/// <summary>
/// Provides extension methods for working with instances of <see cref="IOpenTriviaClient"/>.
/// </summary>
/// <remarks>
/// This class contains static methods that extend the functionality of <see cref="IOpenTriviaClient"/>
/// implementations, enabling fluent configuration and builder patterns. These extensions are intended to simplify
/// client setup and integration scenarios.
/// <para>
/// The code uses the newer C# extension method syntax with the 'extension' keyword for better readability and conciseness.
/// </para>
/// </remarks>
public static class OpenTriviaClientExtensions
{
    // Extension block - receiver type only. These methods appear as static methods on OpenTriviaClient.
    extension(IOpenTriviaClient)
    {
        /// <summary>
        /// Creates a new builder for configuring and constructing an instance of an Open Trivia client.
        /// </summary>
        /// <returns>An <see cref="IOpenTriviaClientBuilder"/> that can be used to configure and build an <see
        /// cref="IOpenTriviaClient"/> instance.</returns>
        public static IOpenTriviaClientBuilder GetBuilder() => new OpenTriviaClientBuilder();
    }

    // Extension block - receiver type and instance parameter. These methods appear as instance methods on IOpenTriviaClient.
    extension(IOpenTriviaClient client)
    {
        /// <summary>
        /// Retrieves trivia questions from the API with optional filtering across multiple categories.
        /// </summary>
        /// <param name="amount">The number of questions to retrieve (1-50) for EACH provided category.</param>
        /// <param name="categories">The categories to filter by, or null for any category.</param>
        /// <param name="difficulty">The difficulty level to filter by, or null for any difficulty.</param>
        /// <param name="type">The question type to filter by, or null for any type.</param>
        /// <param name="encoding">The encoding type for the response, or null for default encoding.</param>
        /// <param name="token">The session token to use, or null for no session.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>An <see cref="ApiResponse{T}"/> containing the list of trivia questions.</returns>
        /// <remarks>
        /// This API call can take a long time to complete due to the rate limit of 1 request every 5 seconds. At
        /// a minimum, this method will take 5 * (n-1) seconds to complete, where n is the number of categories specified. 
        /// </remarks>
        public async Task<ApiResponse<List<TriviaQuestion>>> GetQuestionsAsync(int amount, IEnumerable<TriviaCategory> categories, TriviaQuestionDifficulty? difficulty = null, TriviaQuestionType? type = null, ApiEncodingType? encoding = null, ApiSessionToken? token = null, CancellationToken cancellationToken = default)
        {
            List<TriviaQuestion> allQuestions = [];
            ApiResponse<List<TriviaQuestion>> questions = new(error: new ApiException("No categories provided"), data: null)
            {
                ResponseCode = ApiResponseCode.InvalidParameter,
                StatusCode = 400
            };

            ArgumentNullException.ThrowIfNull(categories, nameof(categories));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount, nameof(amount));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(amount, ApiConstants.MaxAmount, nameof(amount));

            // If multiple categories are provided, we need to make separate requests for each category and combine the results
            // Furthermore, the API rate limits to 1 request per second, so we need to manage that as well if multiple categories are provided
            bool isFirstRequest = true;
            if (categories.Any())
            {
                foreach (var category in categories)
                {
                    if (!isFirstRequest)
                    {
                        await Task.Delay(ApiConstants.RateLimitSeconds * 1000, cancellationToken);
                    }
                    isFirstRequest = false;
                    questions = await client.GetQuestionsAsync(amount, category, difficulty, type, encoding, token, cancellationToken);
                    if (questions.Data is not null)
                    {
                        allQuestions.AddRange(questions.Data);
                    }
                }
            }

            return new ApiResponse<List<TriviaQuestion>>(error: questions.Error, data: allQuestions)
            {
                ResponseCode = questions.ResponseCode,
                StatusCode = questions.StatusCode
            };

        }
    }
}