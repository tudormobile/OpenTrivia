using System.Text.Json;

namespace Tudormobile.OpenTrivia;

/// <summary>
/// Defines methods for deserializing API responses from the Open Trivia Database.
/// </summary>
public interface IApiDataSerializer
{
    /// <summary>
    /// Extracts the response code from an API response JSON document.
    /// </summary>
    /// <param name="document">The JSON document containing the API response.</param>
    /// <returns>The <see cref="ApiResponseCode"/> indicating the result of the API request.</returns>
    ApiResponseCode GetResponseCode(JsonDocument document);

    /// <summary>
    /// Deserializes a collection of trivia questions from a JSON document.
    /// </summary>
    /// <param name="document">The JSON document containing the trivia questions data.</param>
    /// <returns>A list of <see cref="TriviaQuestion"/> objects parsed from the response.</returns>
    List<TriviaQuestion> DeserializeTriviaQuestions(JsonDocument document);

    /// <summary>
    /// Deserializes a collection of trivia categories from a JSON document.
    /// </summary>
    /// <param name="document">The JSON document containing the trivia categories data.</param>
    /// <returns>A list of <see cref="TriviaCategory"/> objects parsed from the response.</returns>
    List<TriviaCategory> DeserializeTriviaCategories(JsonDocument document);

    /// <summary>
    /// Deserializes a session token from a JSON document.
    /// </summary>
    /// <param name="document">The JSON document containing the session token data.</param>
    /// <returns>An <see cref="ApiSessionToken"/> object parsed from the response.</returns>
    ApiSessionToken DeserializeSessionToken(JsonDocument document);
}
