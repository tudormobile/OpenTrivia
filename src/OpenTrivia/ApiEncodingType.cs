namespace Tudormobile.OpenTrivia;

/// <summary>
/// Specifies the encoding type for API responses from the Open Trivia Database.
/// </summary>
public enum ApiEncodingType
{
    /// <summary>
    /// Default encoding (no encoding specified) for API responses.
    /// </summary>
    Default,

    /// <summary>
    /// URL encoding based on RFC 3986 standard for API responses.
    /// </summary>
    Url3986,

    /// <summary>
    /// Base64 encoding for API responses.
    /// </summary>
    Base64
}
