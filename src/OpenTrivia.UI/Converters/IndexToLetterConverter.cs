using System.Globalization;
using System.Windows.Data;

namespace Tudormobile.OpenTrivia.UI.Converters;

/// <summary>
/// Converts a zero-based numeric index to its corresponding uppercase letter representation.
/// </summary>
/// <remarks>
/// This converter is useful for displaying answer choices in multiple-choice questions where answers
/// are traditionally labeled A, B, C, D, etc. The converter supports indices 0-25, mapping them to
/// letters A-Z. Values outside this range return an empty string.
/// <para>
/// <strong>Examples:</strong>
/// <list type="bullet">
/// <item>0 → "A"</item>
/// <item>1 → "B"</item>
/// <item>2 → "C"</item>
/// <item>25 → "Z"</item>
/// <item>26 or higher → ""</item>
/// <item>-1 or lower → ""</item>
/// </list>
/// </para>
/// </remarks>
public class IndexToLetterConverter : IValueConverter
{
    /// <summary>
    /// Converts a zero-based integer index to its corresponding uppercase letter.
    /// </summary>
    /// <param name="value">The zero-based index to convert. Must be an integer between 0 and 25.</param>
    /// <param name="targetType">The type of the binding target property. This parameter is not used.</param>
    /// <param name="parameter">An optional parameter. This parameter is not used.</param>
    /// <param name="culture">The culture to use in the converter. This parameter is not used.</param>
    /// <returns>
    /// A string containing a single uppercase letter (A-Z) if the index is valid (0-25);
    /// otherwise, returns an empty string.
    /// </returns>
    /// <example>
    /// <code>
    /// var converter = new IndexToLetterConverter();
    /// var result = converter.Convert(0, typeof(string), null, CultureInfo.InvariantCulture);
    /// // result = "A"
    /// </code>
    /// </example>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // Check if the value is an integer within the valid range (0-25 for A-Z)
        if (value is int index && index >= 0 && index < 26)
        {
            // Convert the index to a letter by adding it to 'A' (ASCII 65)
            // 0 + 'A' = 'A', 1 + 'A' = 'B', etc.
            return ((char)('A' + index)).ToString();
        }

        // Return empty string for invalid input
        return string.Empty;
    }

    /// <summary>
    /// Converts a letter back to its zero-based index. This operation is not supported.
    /// </summary>
    /// <param name="value">The value to convert back.</param>
    /// <param name="targetType">The type to convert to.</param>
    /// <param name="parameter">An optional parameter.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>This method always throws NotImplementedException.</returns>
    /// <exception cref="NotImplementedException">
    /// This converter is designed for one-way binding only. Reverse conversion is not supported.
    /// </exception>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException("IndexToLetterConverter supports one-way binding only.");
    }
}
