using System.Globalization;
using System.Windows.Data;
using Tudormobile.OpenTrivia.UI.ViewModels;

namespace Tudormobile.OpenTrivia.UI.Converters;

/// <summary>
/// Converts trivia category identifiers to Segoe MDL2 Assets glyph characters for visual representation.
/// </summary>
/// <remarks>
/// This converter supports conversion from <see cref="int"/>, <see cref="SelectableCategory"/>, 
/// and <see cref="TriviaCategory"/> types to string glyphs representing category icons.
/// Each category ID maps to a specific Unicode character from the Segoe MDL2 Assets font.
/// </remarks>
public class CategoryToGlyphConverter : IValueConverter
{
    /// <summary>
    /// Converts a trivia category value to its corresponding Segoe MDL2 Assets glyph character.
    /// </summary>
    /// <param name="value">The category value to convert. Can be an <see cref="int"/> category ID, 
    /// a <see cref="SelectableCategory"/>, or a <see cref="TriviaCategory"/>.</param>
    /// <param name="targetType">The type of the binding target property. When <see cref="string"/>, 
    /// returns the appropriate glyph character; otherwise returns a default glyph.</param>
    /// <param name="parameter">Optional converter parameter (not used).</param>
    /// <param name="culture">The culture to use in the converter (not used).</param>
    /// <returns>A string containing a Unicode glyph character representing the category, 
    /// or a question mark glyph (❓) if the category is not recognized.</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var id = value switch
        {
            int intValue => intValue,
            SelectableCategory category => category.Id,
            TriviaCategory triviaCategory => triviaCategory.Id,
            _ => -1
        };

        return targetType == typeof(string)
            ? id switch
            {
                9 => "\uE7BE", // General Knowledge
                10 => "\uE734", // Books
                11 => "\uE714", // Film
                12 => "\uE8D6", // Music
                13 => "\uE734", // Musicals & Theatres
                14 => "\uE7F4", // Television
                15 => "\uE7EC", // Video Games
                16 => "\uEA86", // Board Games
                17 => "\uF196", // Science & Nature
                18 => "\uE977", // Computers
                19 => "\uE8EF", // Mathematics
                20 => "\uE9C8", // Mythology
                21 => "\uE9C8", // Sports
                22 => "\uE909", // Geography
                23 => "\uE820", // History
                24 => "\uE804", // Politics
                25 => "\uE790", // Art
                26 => "\uE716", // Celebrities
                27 => "\uF1E8", // Animals
                28 => "\uE804", // Vehicles
                29 => "\uE8BA", // Comics
                30 => "\uE90F", // Gadgets
                31 => "\uE87C", // Anime & Manga
                32 => "\uE771", // Cartoon & Animations
                _ => "\u2753"
            }
            : "\ue9c8";
    }

    /// <summary>
    /// Converts a glyph character back to a category value.
    /// </summary>
    /// <param name="value">The glyph character to convert back.</param>
    /// <param name="targetType">The type to convert to.</param>
    /// <param name="parameter">Optional converter parameter (not used).</param>
    /// <param name="culture">The culture to use in the converter (not used).</param>
    /// <returns>Not supported - this method always throws <see cref="NotImplementedException"/>.</returns>
    /// <exception cref="NotImplementedException">This converter does not support two-way binding.</exception>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
