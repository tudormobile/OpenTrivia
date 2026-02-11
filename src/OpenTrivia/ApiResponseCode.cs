namespace Tudormobile.OpenTrivia;

/// <summary>
/// Contains the response codes returned by the API.
/// </summary>
public enum ApiResponseCode
{
    /// <summary>
    /// Indicates that the value is unknown or not specified.
    /// </summary>
    /// <remarks>Use this value when the actual value cannot be determined or is not applicable. This is
    /// typically used as a default or sentinel value to represent an undefined state.</remarks>
    Unknown = -1,

    /// <summary>
    /// Returned results successfully.
    /// </summary>
    Success = 0,

    /// <summary>
    /// Could not return results. The API doesn't have enough questions for your query.
    /// </summary>
    NoResults = 1,

    /// <summary>
    /// Contains an invalid parameter. Arguements passed in aren't valid.
    /// </summary>
    InvalidParameter = 2,

    /// <summary>
    /// Token does not exist.
    /// </summary>
    TokenNotFound = 3,

    /// <summary>
    /// Token has returned all possible questions for the specified query. Resetting the Token is necessary.
    /// </summary>
    TokenEmpty = 4,

    /// <summary>
    /// Too many requests have occurred. Each IP can only access the API once every 5 seconds.
    /// </summary>
    RateLimit = 5
}
