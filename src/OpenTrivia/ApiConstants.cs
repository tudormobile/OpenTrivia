namespace Tudormobile.OpenTrivia;

/// <summary>
/// Contains constants for the Open Trivia API.
/// </summary>
public static class ApiConstants
{
    /// <summary>
    /// The base URL for the Open Trivia Database API.
    /// </summary>
    public const string BaseQuestionUrl = "https://opentdb.com/api.php";

    /// <summary>
    /// The URL of the Open Trivia Database API token endpoint.
    /// </summary>
    public const string TokenUrl = "https://opentdb.com/api_token.php";

    /// <summary>
    /// The URL for retrieving trivia categories from the Open Trivia Database API.
    /// </summary>
    public const string CategoryUrl = "https://opentdb.com/api_category.php";

    /// <summary>
    /// The API version currently in use.
    /// </summary>
    public const int ApiRevision = 0;

    /// <summary>
    /// The maximum number of questions that can be requested in a single API call.
    /// </summary>
    public const int MaxAmount = 50;

    /// <summary>
    /// Api rate limit in seconds. Each IP can only access the API once every 5 seconds.
    /// </summary>
    public const int RateLimitSeconds = 5;
}
