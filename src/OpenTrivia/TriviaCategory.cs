namespace Tudormobile.OpenTrivia;

/// <summary>
/// Represents a trivia category.
/// </summary>
public record TriviaCategory
{
    /// <summary>
    /// The unique identifier for the category.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The name of the category.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Returns the name of the trivia category.
    /// </summary>
    /// <returns>The name of the category.</returns>
    public override string ToString() => Name;
}
