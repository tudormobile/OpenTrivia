namespace Tudormobile.OpenTrivia;

/// <summary>
/// Represents the response from an Open Trivia API call.
/// </summary>
/// <typeparam name="T">The type of data returned by the API call.</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// Gets the data returned by the API call, or null if the request failed.
    /// </summary>
    public T? Data { get; }

    /// <summary>
    /// Gets the error that occurred during the API call, or null if the request was successful.
    /// </summary>
    public ApiException? Error { get; }

    /// <summary>
    /// Gets the error message from the <see cref="Error"/>, or null if no error occurred.
    /// </summary>
    public string? ErrorMessage => Error?.Message;

    /// <summary>
    /// Gets the response code returned by the API.
    /// </summary>
    public ApiResponseCode ResponseCode { get; internal set; }

    /// <summary>
    /// Gets the HTTP status code of the response.
    /// </summary>
    public int StatusCode { get; init; }

    /// <summary>
    /// Gets a value indicating whether the API call was successful (no error occurred).
    /// </summary>
    public bool IsSuccess => Error == null && ResponseCode == ApiResponseCode.Success;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiResponse{T}"/> class.
    /// </summary>
    /// <param name="data">Data returned by the API call</param>
    /// <param name="error">Error that occurred during the API call</param>
    /// <param name="responseCode">Response code returned by the API.</param>
    /// <param name="statusCode">HTTP status code of the response.</param>
    public ApiResponse(T? data = default, ApiException? error = null, ApiResponseCode responseCode = ApiResponseCode.Unknown, int statusCode = 0)
    {
        Data = data;
        Error = error;
        ResponseCode = responseCode;
        StatusCode = statusCode;
    }
}
