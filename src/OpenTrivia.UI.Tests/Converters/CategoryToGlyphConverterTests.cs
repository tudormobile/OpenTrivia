using Tudormobile.OpenTrivia;
using Tudormobile.OpenTrivia.UI.Converters;
using Tudormobile.OpenTrivia.UI.ViewModels;

namespace OpenTrivia.UI.Tests.Converters;

[TestClass]
public class CategoryToGlyphConverterTests
{
    private CategoryToGlyphConverter _converter = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _converter = new CategoryToGlyphConverter();
    }

    [TestMethod]
    [DataRow(9, "\uE7BE")]   // General Knowledge
    [DataRow(10, "\uE734")]  // Books
    [DataRow(11, "\uE714")]  // Film
    [DataRow(12, "\uE8D6")]  // Music
    [DataRow(13, "\uE734")]  // Musicals & Theatres
    [DataRow(14, "\uE7F4")]  // Television
    [DataRow(15, "\uE7FC")]  // Video Games
    [DataRow(16, "\uEA86")]  // Board Games
    [DataRow(17, "\uF196")]  // Science & Nature
    [DataRow(18, "\uE977")]  // Computers
    [DataRow(19, "\uE8EF")]  // Mathematics
    [DataRow(20, "\uE9CE")]  // Mythology
    [DataRow(21, "\uE9CE")]  // Sports
    [DataRow(22, "\uE909")]  // Geography
    [DataRow(23, "\uE82D")]  // History
    [DataRow(24, "\uE8DF")]  // Politics
    [DataRow(25, "\uE790")]  // Art
    [DataRow(26, "\uE716")]  // Celebrities
    [DataRow(27, "\uF1E8")]  // Animals
    [DataRow(28, "\uE804")]  // Vehicles
    [DataRow(29, "\uE8BA")]  // Comics
    [DataRow(30, "\uE90F")]  // Gadgets
    [DataRow(31, "\uE87C")]  // Anime & Manga
    [DataRow(32, "\uE771")]  // Cartoon & Animations
    public void Convert_ValidCategoryId_ReturnsCorrectGlyph(int categoryId, string expectedGlyph)
    {
        var result = _converter.Convert(categoryId, typeof(string), null, CultureInfo.CurrentCulture);

        Assert.AreEqual(expectedGlyph, result);
    }

    [TestMethod]
    [DataRow(-1)]
    [DataRow(0)]
    [DataRow(8)]
    [DataRow(33)]
    [DataRow(999)]
    public void Convert_InvalidCategoryId_ReturnsDefaultGlyph(int categoryId)
    {
        var result = _converter.Convert(categoryId, typeof(string), null, CultureInfo.InvariantCulture);

        Assert.AreEqual("\uE9Ce", result);
    }

    [TestMethod]
    [DataRow(11, "\uE714")]  // Film
    [DataRow(12, "\uE8D6")]  // Music
    [DataRow(999, "\uE9Ce")] // Unknown
    public void Convert_SelectableCategory_ReturnsCorrectGlyph(int categoryId, string expectedGlyph)
    {
        var triviaCategory = new TriviaCategory { Id = categoryId, Name = "Test Category" };
        var selectableCategory = new SelectableCategory(triviaCategory);

        var result = _converter.Convert(selectableCategory, typeof(string), null, CultureInfo.InvariantCulture);

        Assert.AreEqual(expectedGlyph, result);
    }

    [TestMethod]
    [DataRow(18, "\uE977")]  // Computers
    [DataRow(21, "\uE9CE")]  // Sports
    [DataRow(500, "\uE9Ce")] // Unknown
    public void Convert_TriviaCategory_ReturnsCorrectGlyph(int categoryId, string expectedGlyph)
    {
        var category = new TriviaCategory { Id = categoryId, Name = "Test Category" };

        var result = _converter.Convert(category, typeof(string), null, CultureInfo.InvariantCulture);

        Assert.AreEqual(expectedGlyph, result);
    }

    [TestMethod]
    public void Convert_NullValue_ReturnsDefaultGlyph()
    {
        var result = _converter.Convert(null!, typeof(string), null, CultureInfo.InvariantCulture);

        Assert.AreEqual("\uE9Ce", result);
    }

    [TestMethod]
    [DataRow("not a valid type")]
    [DataRow(12.5)]
    [DataRow(true)]
    public void Convert_InvalidInputType_ReturnsDefaultGlyph(object invalidValue)
    {
        var result = _converter.Convert(invalidValue, typeof(string), null, CultureInfo.InvariantCulture);

        Assert.AreEqual("\uE9Ce", result);
    }

    [TestMethod]
    [DataRow(typeof(object))]
    [DataRow(typeof(int))]
    [DataRow(typeof(bool))]
    public void Convert_NonStringTargetType_ReturnsDefaultGlyph(Type targetType)
    {
        var result = _converter.Convert(11, targetType, null, CultureInfo.InvariantCulture);

        Assert.AreEqual("\uE9CE", result);
    }

    [TestMethod]
    public void Convert_WithParameter_IgnoresParameter()
    {
        var result = _converter.Convert(10, typeof(string), "ignored", CultureInfo.InvariantCulture);

        Assert.AreEqual("\uE734", result);
    }

    [TestMethod]
    public void Convert_WithDifferentCulture_IgnoresCulture()
    {
        var result = _converter.Convert(22, typeof(string), null, new CultureInfo("fr-FR"));

        Assert.AreEqual("\uE909", result);
    }

    [TestMethod]
    public void Convert_WithNullCulture_WorksCorrectly()
    {
        var result = _converter.Convert(17, typeof(string), null, CultureInfo.CurrentCulture);

        Assert.AreEqual("\uF196", result);
    }

    [TestMethod]
    public void ConvertBack_AnyValue_ThrowsNotImplementedException()
    {
        Assert.ThrowsExactly<NotImplementedException>(() =>
            _converter.ConvertBack("\uE714", typeof(int), null, CultureInfo.CurrentCulture));
    }

    [TestMethod]
    public void ConvertBack_NullValue_ThrowsNotImplementedException()
    {
        Assert.ThrowsExactly<NotImplementedException>(() =>
            _converter.ConvertBack(null!, typeof(int), null, CultureInfo.InvariantCulture));
    }
}
