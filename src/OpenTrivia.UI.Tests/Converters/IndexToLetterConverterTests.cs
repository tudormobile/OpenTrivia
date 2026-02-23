using Tudormobile.OpenTrivia.UI.Converters;

namespace OpenTrivia.UI.Tests.Converters;

[TestClass]
public class IndexToLetterConverterTests
{
    private IndexToLetterConverter _converter = null!;

    [TestInitialize]
    public void Setup()
    {
        _converter = new IndexToLetterConverter();
    }

    [TestMethod]
    public void Convert_ZeroIndex_ReturnsA()
    {
        // Arrange
        const int index = 0;

        // Act
        var result = _converter.Convert(index, typeof(string), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.AreEqual("A", result);
    }

    [TestMethod]
    public void Convert_OneIndex_ReturnsB()
    {
        // Arrange
        const int index = 1;

        // Act
        var result = _converter.Convert(index, typeof(string), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.AreEqual("B", result);
    }

    [TestMethod]
    public void Convert_TwentyFiveIndex_ReturnsZ()
    {
        // Arrange
        const int index = 25;

        // Act
        var result = _converter.Convert(index, typeof(string), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.AreEqual("Z", result);
    }

    [TestMethod]
    public void Convert_TwentySixIndex_ReturnsEmptyString()
    {
        // Arrange
        const int index = 26;

        // Act
        var result = _converter.Convert(index, typeof(string), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.AreEqual(string.Empty, result);
    }

    [TestMethod]
    public void Convert_NegativeIndex_ReturnsEmptyString()
    {
        // Arrange
        const int index = -1;

        // Act
        var result = _converter.Convert(index, typeof(string), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.AreEqual(string.Empty, result);
    }

    [TestMethod]
    public void Convert_NullValue_ReturnsEmptyString()
    {
        // Arrange
        object? value = null;

        // Act
        var result = _converter.Convert(value!, typeof(string), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.AreEqual(string.Empty, result);
    }

    [TestMethod]
    public void Convert_NonIntegerValue_ReturnsEmptyString()
    {
        // Arrange
        const string value = "not an integer";

        // Act
        var result = _converter.Convert(value, typeof(string), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.AreEqual(string.Empty, result);
    }

    [TestMethod]
    public void Convert_DoubleValue_ReturnsEmptyString()
    {
        // Arrange
        const double value = 1.5;

        // Act
        var result = _converter.Convert(value, typeof(string), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.AreEqual(string.Empty, result);
    }

    [TestMethod]
    [DataRow(0, "A")]
    [DataRow(1, "B")]
    [DataRow(2, "C")]
    [DataRow(3, "D")]
    [DataRow(4, "E")]
    [DataRow(5, "F")]
    [DataRow(10, "K")]
    [DataRow(15, "P")]
    [DataRow(20, "U")]
    [DataRow(24, "Y")]
    [DataRow(25, "Z")]
    public void Convert_ValidIndices_ReturnsCorrectLetters(int index, string expectedLetter)
    {
        // Act
        var result = _converter.Convert(index, typeof(string), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.AreEqual(expectedLetter, result);
    }

    [TestMethod]
    [DataRow(-10)]
    [DataRow(-1)]
    [DataRow(26)]
    [DataRow(27)]
    [DataRow(100)]
    public void Convert_InvalidIndices_ReturnsEmptyString(int index)
    {
        // Act
        var result = _converter.Convert(index, typeof(string), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.AreEqual(string.Empty, result);
    }

    [TestMethod]
    public void ConvertBack_ThrowsNotImplementedException()
    {
        // Arrange
        const string value = "A";

        // Act & Assert
        Assert.ThrowsExactly<NotImplementedException>(() =>
            _converter.ConvertBack(value, typeof(int), null, CultureInfo.InvariantCulture));
    }

    [TestMethod]
    public void Convert_AllValidIndices_ProduceUniqueLetters()
    {
        // Arrange
        var results = new HashSet<string>();

        // Act
        for (int i = 0; i < 26; i++)
        {
            var result = _converter.Convert(i, typeof(string), null, CultureInfo.InvariantCulture) as string;
            results.Add(result!);
        }

        // Assert
        Assert.HasCount(26, results, "All indices should produce unique letters");
    }

    [TestMethod]
    public void Convert_SequentialIndices_ProduceSequentialLetters()
    {
        // Arrange & Act
        var result0 = (string)_converter.Convert(0, typeof(string), null, CultureInfo.InvariantCulture);
        var result1 = (string)_converter.Convert(1, typeof(string), null, CultureInfo.InvariantCulture);
        var result2 = (string)_converter.Convert(2, typeof(string), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.AreEqual('A', result0[0]);
        Assert.AreEqual('B', result1[0]);
        Assert.AreEqual('C', result2[0]);
        Assert.AreEqual(result0[0] + 1, result1[0], "B should be one character after A");
        Assert.AreEqual(result1[0] + 1, result2[0], "C should be one character after B");
    }

    [TestMethod]
    public void Convert_DifferentCultureInfo_ProducesConsistentResults()
    {
        // Arrange
        const int index = 5;
        var cultures = new[]
        {
            CultureInfo.InvariantCulture,
            CultureInfo.GetCultureInfo("en-US"),
            CultureInfo.GetCultureInfo("fr-FR"),
            CultureInfo.GetCultureInfo("de-DE")
        };

        // Act & Assert
        foreach (var culture in cultures)
        {
            var result = _converter.Convert(index, typeof(string), null, culture);
            Assert.AreEqual("F", result, $"Culture {culture.Name} should produce 'F'");
        }
    }
}
