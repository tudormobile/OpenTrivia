using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Tudormobile.OpenTrivia.UI.Converters;

/// <summary>
/// Provides a value converter that maps Boolean values to customizable representations for various target types,
/// supporting both forward and reverse conversion.
/// </summary>
/// <remarks>The BooleanConverter enables conversion between Boolean values and other types, such as strings,
/// integers, and visibility states, with the ability to specify custom values for <see langword="true"/> and <see
/// langword="false"/>. The conversion direction can be inverted by setting the IsInverted property, which swaps the
/// true and false values during conversion. If TrueValue or FalseValue is not set, default representations are used
/// based on the target type. This converter is commonly used in data binding scenarios to adapt Boolean values for UI
/// elements or other data types.</remarks>
public class BooleanConverter : IValueConverter
{
    private static readonly object UnsetValue = new();    // sentinel representing "not set"
    /// <summary>
    /// Gets or sets the value that represents <see langword="true"/> value when converting.
    /// </summary>
    public object? TrueValue { get; set; } = UnsetValue;

    /// <summary>
    /// Gets or sets the value that represents a logical "false" value when converting.
    /// </summary>
    public object? FalseValue { get; set; } = UnsetValue;

    /// <summary>
    /// Gets or sets a value indicating whether the operation is inverted. (True becomes False and vice versa).
    /// </summary>
    public bool IsInverted { get; set; } = false;

    /// <inheritdoc/>
    public object? Convert(object value, Type targetType, object? parameter, CultureInfo? culture)
    {
        if (value is not bool)
        {
            throw new InvalidOperationException("The value must be a Boolean.");
        }
        if (IsInverted) value = !(bool)value;
        if ((bool)value && TrueValue != UnsetValue) return TrueValue;
        if (!(bool)value && FalseValue != UnsetValue) return FalseValue;
        return targetType switch
        {
            _ when targetType == typeof(Visibility) => (bool)value ? Visibility.Visible : Visibility.Collapsed,
            _ when targetType == typeof(string) => (bool)value ? "True" : "False",
            _ when targetType == typeof(int) => (bool)value ? 1 : 0,
            _ when targetType == typeof(double) => (bool)value ? 1.0 : 0.0,
            _ when targetType == typeof(float) => (bool)value ? 1f : 0f,
            _ when targetType == typeof(decimal) => (bool)value ? 1m : 0m,
            _ when targetType == typeof(long) => (bool)value ? 1L : 0L,
            _ when targetType == typeof(short) => (bool)value ? (short)1 : (short)0,
            _ when targetType == typeof(byte) => (bool)value ? (byte)1 : (byte)0,
            _ when targetType == typeof(char) => (bool)value ? 'T' : 'F',
            _ when targetType == typeof(bool) => value,
            _ => throw new InvalidOperationException($"Cannot convert Boolean to {targetType.FullName}."),
        };
    }

    /// <inheritdoc/>
    public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo? culture)
    {
        if (targetType != typeof(bool))
        {
            throw new InvalidOperationException("The target type must be a Boolean.");
        }
        var result = ConvertBackInternal(value, targetType);
        return IsInverted ? !result : result;
    }

    private bool ConvertBackInternal(object value, Type targetType)
    {
        if (TrueValue != UnsetValue && value.Equals(TrueValue)) return true;
        if (FalseValue != UnsetValue && value.Equals(FalseValue)) return false;
        return value switch
        {
            _ when value is Visibility => (Visibility)value == Visibility.Visible,
            _ when value is string => string.Equals((string)value, "True", StringComparison.OrdinalIgnoreCase),
            _ when value is int => (int)value != 0,
            _ when value is double => (double)value != 0.0,
            _ when value is float => (float)value != 0f,
            _ when value is decimal => (decimal)value != 0m,
            _ when value is long => (long)value != 0L,
            _ when value is short => (short)value != 0,
            _ when value is byte => (byte)value != 0,
            _ when value is char => char.ToUpperInvariant((char)value) == 'T',
            _ when value is bool => (bool)value,
            _ => throw new InvalidOperationException($"Cannot convert to Boolean."),
        };
    }
}
