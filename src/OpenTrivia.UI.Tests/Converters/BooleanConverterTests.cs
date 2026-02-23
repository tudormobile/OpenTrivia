using System.Windows;
using Tudormobile.OpenTrivia.UI.Converters;

namespace OpenTrivia.UI.Tests.Converters;

[TestClass]
public class BooleanConverterTests
{
    private BooleanConverter _converter = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _converter = new BooleanConverter();
    }

    [TestMethod]
    public void Convert_TrueToVisibility_ReturnsVisible()
    {
        var result = _converter.Convert(true, typeof(Visibility), null, null);
        Assert.AreEqual(Visibility.Visible, result);
    }

    [TestMethod]
    public void Convert_FalseToVisibility_ReturnsCollapsed()
    {
        var result = _converter.Convert(false, typeof(Visibility), null, null);
        Assert.AreEqual(Visibility.Collapsed, result);
    }

    [TestMethod]
    public void Convert_TrueToString_ReturnsTrue()
    {
        var result = _converter.Convert(true, typeof(string), null, null);
        Assert.AreEqual("True", result);
    }

    [TestMethod]
    public void Convert_FalseToString_ReturnsFalse()
    {
        var result = _converter.Convert(false, typeof(string), null, null);
        Assert.AreEqual("False", result);
    }

    [TestMethod]
    public void Convert_TrueToInt_ReturnsOne()
    {
        var result = _converter.Convert(true, typeof(int), null, null);
        Assert.AreEqual(1, result);
    }

    [TestMethod]
    public void Convert_FalseToInt_ReturnsZero()
    {
        var result = _converter.Convert(false, typeof(int), null, null);
        Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Convert_TrueToDouble_ReturnsOne()
    {
        var result = _converter.Convert(true, typeof(double), null, null);
        Assert.AreEqual(1.0, result);
    }

    [TestMethod]
    public void Convert_FalseToDouble_ReturnsZero()
    {
        var result = _converter.Convert(false, typeof(double), null, null);
        Assert.AreEqual(0.0, result);
    }

    [TestMethod]
    public void Convert_TrueToFloat_ReturnsOne()
    {
        var result = _converter.Convert(true, typeof(float), null, null);
        Assert.AreEqual(1f, result);
    }

    [TestMethod]
    public void Convert_FalseToFloat_ReturnsZero()
    {
        var result = _converter.Convert(false, typeof(float), null, null);
        Assert.AreEqual(0f, result);
    }

    [TestMethod]
    public void Convert_TrueToDecimal_ReturnsOne()
    {
        var result = _converter.Convert(true, typeof(decimal), null, null);
        Assert.AreEqual(1m, result);
    }

    [TestMethod]
    public void Convert_FalseToDecimal_ReturnsZero()
    {
        var result = _converter.Convert(false, typeof(decimal), null, null);
        Assert.AreEqual(0m, result);
    }

    [TestMethod]
    public void Convert_TrueToLong_ReturnsOne()
    {
        var result = _converter.Convert(true, typeof(long), null, null);
        Assert.AreEqual(1L, result);
    }

    [TestMethod]
    public void Convert_FalseToLong_ReturnsZero()
    {
        var result = _converter.Convert(false, typeof(long), null, null);
        Assert.AreEqual(0L, result);
    }

    [TestMethod]
    public void Convert_TrueToShort_ReturnsOne()
    {
        var result = _converter.Convert(true, typeof(short), null, null);
        Assert.AreEqual((short)1, result);
    }

    [TestMethod]
    public void Convert_FalseToShort_ReturnsZero()
    {
        var result = _converter.Convert(false, typeof(short), null, null);
        Assert.AreEqual((short)0, result);
    }

    [TestMethod]
    public void Convert_TrueToByte_ReturnsOne()
    {
        var result = _converter.Convert(true, typeof(byte), null, null);
        Assert.AreEqual((byte)1, result);
    }

    [TestMethod]
    public void Convert_FalseToByte_ReturnsZero()
    {
        var result = _converter.Convert(false, typeof(byte), null, null);
        Assert.AreEqual((byte)0, result);
    }

    [TestMethod]
    public void Convert_TrueToChar_ReturnsT()
    {
        var result = _converter.Convert(true, typeof(char), null, null);
        Assert.AreEqual('T', result);
    }

    [TestMethod]
    public void Convert_FalseToChar_ReturnsF()
    {
        var result = _converter.Convert(false, typeof(char), null, null);
        Assert.AreEqual('F', result);
    }

    [TestMethod]
    public void Convert_TrueToBool_ReturnsTrue()
    {
        var result = _converter.Convert(true, typeof(bool), null, null);
        Assert.IsTrue((bool)result!);
    }

    [TestMethod]
    public void Convert_FalseToBool_ReturnsFalse()
    {
        var result = _converter.Convert(false, typeof(bool), null, null);
        Assert.IsFalse((bool)result!);
    }

    [TestMethod]
    public void Convert_WithCustomTrueValue_ReturnsCustomValue()
    {
        _converter.TrueValue = "Yes";
        var result = _converter.Convert(true, typeof(string), null, null);
        Assert.AreEqual("Yes", result);
    }

    [TestMethod]
    public void Convert_WithCustomFalseValue_ReturnsCustomValue()
    {
        _converter.FalseValue = "No";
        var result = _converter.Convert(false, typeof(string), null, null);
        Assert.AreEqual("No", result);
    }

    [TestMethod]
    public void Convert_WithIsInverted_InvertsResult()
    {
        _converter.IsInverted = true;
        var result = _converter.Convert(true, typeof(Visibility), null, null);
        Assert.AreEqual(Visibility.Collapsed, result);
    }

    [TestMethod]
    public void Convert_NonBooleanValue_ThrowsInvalidOperationException()
    {
        Assert.ThrowsExactly<InvalidOperationException>(() =>
            _converter.Convert("not a bool", typeof(string), null, null));
    }

    [TestMethod]
    public void Convert_UnsupportedTargetType_ThrowsInvalidOperationException()
    {
        Assert.ThrowsExactly<InvalidOperationException>(() =>
            _converter.Convert(true, typeof(DateTime), null, null));
    }

    [TestMethod]
    public void ConvertBack_VisibleToBoolean_ReturnsTrue()
    {
        var result = _converter.ConvertBack(Visibility.Visible, typeof(bool), null, null);
        Assert.IsTrue((bool)result!);
    }

    [TestMethod]
    public void ConvertBack_CollapsedToBoolean_ReturnsFalse()
    {
        var result = _converter.ConvertBack(Visibility.Collapsed, typeof(bool), null, null);
        Assert.IsFalse((bool)result!);
    }

    [TestMethod]
    public void ConvertBack_HiddenToBoolean_ReturnsFalse()
    {
        var result = _converter.ConvertBack(Visibility.Hidden, typeof(bool), null, null);
        Assert.IsFalse((bool)result!);
    }

    [TestMethod]
    public void ConvertBack_TrueStringToBoolean_ReturnsTrue()
    {
        var result = _converter.ConvertBack("True", typeof(bool), null, null);
        Assert.IsTrue((bool)result!);
    }

    [TestMethod]
    public void ConvertBack_FalseStringToBoolean_ReturnsFalse()
    {
        var result = _converter.ConvertBack("False", typeof(bool), null, null);
        Assert.IsFalse((bool)result!);
    }

    [TestMethod]
    public void ConvertBack_TrueStringCaseInsensitive_ReturnsTrue()
    {
        var result = _converter.ConvertBack("true", typeof(bool), null, null);
        Assert.IsTrue((bool)result!);
    }

    [TestMethod]
    public void ConvertBack_NonZeroInt_ReturnsTrue()
    {
        var result = _converter.ConvertBack(1, typeof(bool), null, null);
        Assert.IsTrue((bool)result!);
    }

    [TestMethod]
    public void ConvertBack_ZeroInt_ReturnsFalse()
    {
        var result = _converter.ConvertBack(0, typeof(bool), null, null);
        Assert.IsFalse((bool)result!);
    }

    [TestMethod]
    public void ConvertBack_NonZeroDouble_ReturnsTrue()
    {
        var result = _converter.ConvertBack(1.5, typeof(bool), null, null);
        Assert.IsTrue((bool)result!);
    }

    [TestMethod]
    public void ConvertBack_ZeroDouble_ReturnsFalse()
    {
        var result = _converter.ConvertBack(0.0, typeof(bool), null, null);
        Assert.IsFalse((bool)result!);
    }

    [TestMethod]
    public void ConvertBack_NonZeroFloat_ReturnsTrue()
    {
        var result = _converter.ConvertBack(2.5f, typeof(bool), null, null);
        Assert.IsTrue((bool)result!);
    }

    [TestMethod]
    public void ConvertBack_ZeroFloat_ReturnsFalse()
    {
        var result = _converter.ConvertBack(0f, typeof(bool), null, null);
        Assert.IsFalse((bool)result!);
    }

    [TestMethod]
    public void ConvertBack_NonZeroDecimal_ReturnsTrue()
    {
        var result = _converter.ConvertBack(3.5m, typeof(bool), null, null);
        Assert.IsTrue((bool)result!);
    }

    [TestMethod]
    public void ConvertBack_ZeroDecimal_ReturnsFalse()
    {
        var result = _converter.ConvertBack(0m, typeof(bool), null, null);
        Assert.IsFalse((bool)result!);
    }

    [TestMethod]
    public void ConvertBack_NonZeroLong_ReturnsTrue()
    {
        var result = _converter.ConvertBack(100L, typeof(bool), null, null);
        Assert.IsTrue((bool)result!);
    }

    [TestMethod]
    public void ConvertBack_ZeroLong_ReturnsFalse()
    {
        var result = _converter.ConvertBack(0L, typeof(bool), null, null);
        Assert.IsFalse((bool)result!);
    }

    [TestMethod]
    public void ConvertBack_NonZeroShort_ReturnsTrue()
    {
        var result = _converter.ConvertBack((short)5, typeof(bool), null, null);
        Assert.IsTrue((bool)result!);
    }

    [TestMethod]
    public void ConvertBack_ZeroShort_ReturnsFalse()
    {
        var result = _converter.ConvertBack((short)0, typeof(bool), null, null);
        Assert.IsFalse((bool)result!);
    }

    [TestMethod]
    public void ConvertBack_NonZeroByte_ReturnsTrue()
    {
        var result = _converter.ConvertBack((byte)255, typeof(bool), null, null);
        Assert.IsTrue((bool)result!);
    }

    [TestMethod]
    public void ConvertBack_ZeroByte_ReturnsFalse()
    {
        var result = _converter.ConvertBack((byte)0, typeof(bool), null, null);
        Assert.IsFalse((bool)result!);
    }

    [TestMethod]
    public void ConvertBack_CharT_ReturnsTrue()
    {
        var result = _converter.ConvertBack('T', typeof(bool), null, null);
        Assert.IsTrue((bool)result!);
    }

    [TestMethod]
    public void ConvertBack_CharLowercaseT_ReturnsTrue()
    {
        var result = _converter.ConvertBack('t', typeof(bool), null, null);
        Assert.IsTrue((bool)result!);
    }

    [TestMethod]
    public void ConvertBack_CharF_ReturnsFalse()
    {
        var result = _converter.ConvertBack('F', typeof(bool), null, null);
        Assert.IsFalse((bool)result!);
    }

    [TestMethod]
    public void ConvertBack_BooleanTrue_ReturnsTrue()
    {
        var result = _converter.ConvertBack(true, typeof(bool), null, null);
        Assert.IsTrue((bool)result!);
    }

    [TestMethod]
    public void ConvertBack_BooleanFalse_ReturnsFalse()
    {
        var result = _converter.ConvertBack(false, typeof(bool), null, null);
        Assert.IsFalse((bool)result!);
    }

    [TestMethod]
    public void ConvertBack_WithCustomTrueValue_ReturnsTrue()
    {
        _converter.TrueValue = "Yes";
        var result = _converter.ConvertBack("Yes", typeof(bool), null, null);
        Assert.IsTrue((bool)result!);
    }

    [TestMethod]
    public void ConvertBack_WithCustomFalseValue_ReturnsFalse()
    {
        _converter.FalseValue = "No";
        var result = _converter.ConvertBack("No", typeof(bool), null, null);
        Assert.IsFalse((bool)result!);
    }

    [TestMethod]
    public void ConvertBack_WithIsInverted_InvertsResult()
    {
        _converter.IsInverted = true;
        var result = _converter.ConvertBack(Visibility.Visible, typeof(bool), null, null);
        Assert.IsFalse((bool)result!);
    }

    [TestMethod]
    public void ConvertBack_NonBooleanTargetType_ThrowsInvalidOperationException()
    {
        Assert.ThrowsExactly<InvalidOperationException>(() =>
            _converter.ConvertBack(Visibility.Visible, typeof(string), null, null));
    }

    [TestMethod]
    public void ConvertBack_UnsupportedValueType_ThrowsInvalidOperationException()
    {
        Assert.ThrowsExactly<InvalidOperationException>(() =>
            _converter.ConvertBack(DateTime.Now, typeof(bool), null, null));
    }

    [TestMethod]
    public void RoundTrip_TrueToVisibilityAndBack_ReturnsTrue()
    {
        var converted = _converter.Convert(true, typeof(Visibility), null, null);
        var result = _converter.ConvertBack(converted!, typeof(bool), null, null);
        Assert.IsTrue((bool)result!);
    }

    [TestMethod]
    public void RoundTrip_FalseToStringAndBack_ReturnsFalse()
    {
        var converted = _converter.Convert(false, typeof(string), null, null);
        var result = _converter.ConvertBack(converted!, typeof(bool), null, null);
        Assert.IsFalse((bool)result!);
    }

    [TestMethod]
    public void RoundTrip_WithCustomValues_WorksCorrectly()
    {
        _converter.TrueValue = "Active";
        _converter.FalseValue = "Inactive";

        var convertedTrue = _converter.Convert(true, typeof(string), null, null);
        var convertedFalse = _converter.Convert(false, typeof(string), null, null);

        Assert.AreEqual("Active", convertedTrue);
        Assert.AreEqual("Inactive", convertedFalse);

        var backTrue = _converter.ConvertBack(convertedTrue!, typeof(bool), null, null);
        var backFalse = _converter.ConvertBack(convertedFalse!, typeof(bool), null, null);

        Assert.IsTrue((bool)backTrue!);
        Assert.IsFalse((bool)backFalse!);
    }

}
