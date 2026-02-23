namespace Tudormobile.OpenTrivia.UI.Services;

/// <summary>
/// Represents the result of a service operation that can either succeed with data or fail with an error.
/// </summary>
/// <typeparam name="T">The type of data returned on success.</typeparam>
public class ServiceResult<T>
{
    /// <summary>
    /// Gets a value indicating whether the operation was successful.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets a value indicating whether the operation failed.
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Gets the data returned by the operation, or null if the operation failed.
    /// </summary>
    public T? Data { get; }

    /// <summary>
    /// Gets the error message if the operation failed, or null if it succeeded.
    /// </summary>
    public string? ErrorMessage { get; }

    /// <summary>
    /// Gets the exception that caused the failure, if any.
    /// </summary>
    public Exception? Exception { get; }

    private ServiceResult(bool isSuccess, T? data, string? errorMessage, Exception? exception = null)
    {
        IsSuccess = isSuccess;
        Data = data;
        ErrorMessage = errorMessage;
        Exception = exception;
    }

    /// <summary>
    /// Creates a successful result with the specified data.
    /// </summary>
    /// <param name="data">The data to include in the result.</param>
    /// <returns>A successful service result containing the data.</returns>
    public static ServiceResult<T> Success(T data)
        => new ServiceResult<T>(true, data, null);

    /// <summary>
    /// Creates a failed result with the specified error message and optional exception.
    /// </summary>
    /// <param name="errorMessage">The error message describing the failure.</param>
    /// <param name="exception">The exception that caused the failure, if any.</param>
    /// <returns>A failed service result containing the error information.</returns>
    public static ServiceResult<T> Failure(string errorMessage, Exception? exception = null)
        => new ServiceResult<T>(false, default, errorMessage, exception);

    /// <summary>
    /// Executes one of two functions based on whether the operation succeeded or failed.
    /// </summary>
    /// <typeparam name="TResult">The type of the result to return.</typeparam>
    /// <param name="onSuccess">The function to execute if the operation succeeded.</param>
    /// <param name="onFailure">The function to execute if the operation failed.</param>
    /// <returns>The result of executing the appropriate function.</returns>
    public TResult Match<TResult>(
        Func<T, TResult> onSuccess,
        Func<string, TResult> onFailure)
    {
        return IsSuccess && Data is not null
            ? onSuccess(Data)
            : onFailure(ErrorMessage ?? "Unknown error");
    }

    /// <summary>
    /// Asynchronously executes one of two functions based on whether the operation succeeded or failed.
    /// </summary>
    /// <typeparam name="TResult">The type of the result to return.</typeparam>
    /// <param name="onSuccess">The asynchronous function to execute if the operation succeeded.</param>
    /// <param name="onFailure">The asynchronous function to execute if the operation failed.</param>
    /// <returns>A task representing the asynchronous operation, containing the result of executing the appropriate function.</returns>
    public async Task<TResult> MatchAsync<TResult>(
        Func<T, Task<TResult>> onSuccess,
        Func<string, Task<TResult>> onFailure)
    {
        return IsSuccess && Data is not null
            ? await onSuccess(Data)
            : await onFailure(ErrorMessage ?? "Unknown error");
    }
}

/// <summary>
/// Provides non-generic helper methods for creating ServiceResult instances.
/// </summary>
public static class ServiceResult
{
    /// <summary>
    /// Creates a successful result with the specified data.
    /// </summary>
    /// <typeparam name="T">The type of data in the result.</typeparam>
    /// <param name="data">The data to include in the result.</param>
    /// <returns>A successful service result containing the data.</returns>
    public static ServiceResult<T> Success<T>(T data)
        => ServiceResult<T>.Success(data);

    /// <summary>
    /// Creates a failed result with the specified error message.
    /// </summary>
    /// <typeparam name="T">The type of data the result would have contained on success.</typeparam>
    /// <param name="errorMessage">The error message describing the failure.</param>
    /// <returns>A failed service result containing the error message.</returns>
    public static ServiceResult<T> Failure<T>(string errorMessage)
        => ServiceResult<T>.Failure(errorMessage);

    /// <summary>
    /// Creates a failed result from an exception.
    /// </summary>
    /// <typeparam name="T">The type of data the result would have contained on success.</typeparam>
    /// <param name="exception">The exception that caused the failure.</param>
    /// <returns>A failed service result containing the exception message and exception.</returns>
    public static ServiceResult<T> Failure<T>(Exception exception)
        => ServiceResult<T>.Failure(exception.Message, exception);

    /// <summary>
    /// Creates a failed result with the specified error message and exception.
    /// </summary>
    /// <typeparam name="T">The type of data the result would have contained on success.</typeparam>
    /// <param name="errorMessage">The error message describing the failure.</param>
    /// <param name="exception">The exception that caused the failure, if any.</param>
    /// <returns>A failed service result containing the error information.</returns>
    public static ServiceResult<T> Failure<T>(string errorMessage, Exception? exception)
        => ServiceResult<T>.Failure(errorMessage, exception);
}
