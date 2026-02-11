namespace Tudormobile.OpenTrivia;

/// <summary>
/// Represents a session token for the API.
/// </summary>
public class ApiSessionToken
{
    /// <summary>
    /// The session token used for API requests.
    /// </summary>
    public string Token { get; init; }

    /// <summary>
    /// Creates a new instance of the <see cref="ApiSessionToken"/> class.
    /// </summary>
    /// <param name="token"></param>
    public ApiSessionToken(string token = "")
    {
        ArgumentNullException.ThrowIfNull(token, nameof(token));
        Token = token;
    }

    /// <inheritdoc/>
    public override string ToString() => Token;
}
