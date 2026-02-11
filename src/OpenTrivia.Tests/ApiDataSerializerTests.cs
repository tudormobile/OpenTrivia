using System.Text.Json;

namespace OpenTrivia.Tests;

[TestClass]
public class ApiDataSerializerTests
{
    [TestMethod]
    public void ApiDataSerializer_GetResponseCode_ReturnsSuccess()
    {
        // Arrange
        var doc = JsonDocument.Parse("{\"response_code\":0}");
        var serializer = new ApiDataSerializer();

        // Act
        var responseCode = serializer.GetResponseCode(doc);

        // Assert
        Assert.AreEqual(ApiResponseCode.Success, responseCode);
    }

    [TestMethod]
    public void ApiDataSerializer_GetResponseCode_WithInvalidResponseCode_ReturnsUnknown()
    {
        // Arrange
        var doc = JsonDocument.Parse("{\"response_code\":999}");
        var serializer = new ApiDataSerializer();
        // Act
        var responseCode = serializer.GetResponseCode(doc);
        // Assert
        Assert.AreEqual(ApiResponseCode.Unknown, responseCode);
    }

    [TestMethod]
    public void ApiDataSerializer_GetResponseCode_WithMissingResponseCode_ReturnsUnknown()
    {
        // Arrange
        var doc = JsonDocument.Parse("{}");
        var serializer = new ApiDataSerializer();
        // Act
        var responseCode = serializer.GetResponseCode(doc);
        // Assert
        Assert.AreEqual(ApiResponseCode.Unknown, responseCode);
    }

    [TestMethod]
    public void ApiDataSerializer_GetResponseCode_WithNullResponseCode_ReturnsUnknown()
    {
        // Arrange
        var doc = JsonDocument.Parse("{\"response_code\":null}");
        var serializer = new ApiDataSerializer();
        // Act
        var responseCode = serializer.GetResponseCode(doc);
        // Assert
        Assert.AreEqual(ApiResponseCode.Unknown, responseCode);
    }

    [TestMethod]
    public void ApiDataSerializer_GetResponseCode_WithNonNumericResponseCode_ReturnsUnknown()
    {
        // Arrange
        var doc = JsonDocument.Parse("{\"response_code\":\"not_a_number\"}");
        var serializer = new ApiDataSerializer();
        // Act
        var responseCode = serializer.GetResponseCode(doc);
        // Assert
        Assert.AreEqual(ApiResponseCode.Unknown, responseCode);
    }

    [TestMethod]
    public void ApiDataSerializer_GetResponseCode_WithNegativeResponseCode_ReturnsUnknown()
    {
        // Arrange
        var doc = JsonDocument.Parse("{\"response_code\":-1}");
        var serializer = new ApiDataSerializer();
        // Act
        var responseCode = serializer.GetResponseCode(doc);
        // Assert
        Assert.AreEqual(ApiResponseCode.Unknown, responseCode);

    }

    [TestMethod]
    public void ApiDataSerializer_DeserializeTriviaCategories_ReturnsCategories()
    {
        var json = @"{
            ""trivia_categories"": [
                { ""id"": 9, ""name"": ""General Knowledge"" },
                { ""id"": 10, ""name"": ""Entertainment: Books"" }
            ]
        }";

        var doc = JsonDocument.Parse(json);
        var serializer = new ApiDataSerializer();
        // Act
        var categories = serializer.DeserializeTriviaCategories(doc);
        // Assert
        Assert.HasCount(2, categories);
        Assert.AreEqual(9, categories[0].Id);
        Assert.AreEqual("General Knowledge", categories[0].Name);
        Assert.AreEqual(10, categories[1].Id);
        Assert.AreEqual("Entertainment: Books", categories[1].Name);
    }

    [TestMethod]
    public void ApiDataSerializer_DeserializeTriviaCategories_WithEmptyCategories_ReturnsEmptyList()
    {
        var json = @"{ ""trivia_categories"": [] }";
        var doc = JsonDocument.Parse(json);
        var serializer = new ApiDataSerializer();
        // Act
        var categories = serializer.DeserializeTriviaCategories(doc);
        // Assert
        Assert.IsNotNull(categories);
        Assert.IsFalse(categories.Any());
    }

    [TestMethod]
    public void ApiDataSerializer_DeserializeTriviaCategories_WithMissingCategories_ReturnsEmptyList()
    {
        var json = @"{}";
        var doc = JsonDocument.Parse(json);
        var serializer = new ApiDataSerializer();
        // Act
        var categories = serializer.DeserializeTriviaCategories(doc);
        // Assert
        Assert.IsNotNull(categories);
        Assert.IsFalse(categories.Any());
    }

    [TestMethod]
    public void ApiDataSerializer_DeserializeTriviaQuestions_ReturnsQuestions()
    {
        // Arrange
        var json = @"
{
  ""response_code"": 0,
  ""results"": [
    {
      ""type"": ""multiple"",
      ""difficulty"": ""easy"",
      ""category"": ""Entertainment: Video Games"",
      ""question"": ""Which franchise does the creature &quot;Slowpoke&quot; originate from?"",
      ""correct_answer"": ""Pokemon"",
      ""incorrect_answers"": [
        ""Dragon Ball"",
        ""Sonic The Hedgehog"",
        ""Yugioh""
      ]
    },
    {
      ""type"": ""multiple"",
      ""difficulty"": ""medium"",
      ""category"": ""Entertainment: Video Games"",
      ""question"": ""What is the name of the island introduced in the ARMA III: APEX expansion pack?"",
      ""correct_answer"": ""Tanoa"",
      ""incorrect_answers"": [
        ""Altis"",
        ""Stratis"",
        ""Malden""
      ]
    },
    {
      ""type"": ""boolean"",
      ""difficulty"": ""hard"",
      ""category"": ""Entertainment: Video Games"",
      ""question"": ""What is the name of the island introduced in the ARMA III: APEX expansion pack?"",
      ""correct_answer"": ""True"",
      ""incorrect_answers"": [""False""]
    }
  ]
}";
        var doc = JsonDocument.Parse(json);
        var serializer = new ApiDataSerializer();
        // Act
        var questions = serializer.DeserializeTriviaQuestions(doc);
        // Assert
        Assert.HasCount(3, questions);
        Assert.AreEqual("Which franchise does the creature &quot;Slowpoke&quot; originate from?", questions[0].Question);
        Assert.AreEqual("Pokemon", questions[0].CorrectAnswer);
        Assert.AreEqual("What is the name of the island introduced in the ARMA III: APEX expansion pack?", questions[1].Question);
        Assert.AreEqual("Tanoa", questions[1].CorrectAnswer);

        Assert.AreEqual(TriviaQuestionDifficulty.Easy, questions[0].Difficulty);
        Assert.AreEqual(TriviaQuestionDifficulty.Medium, questions[1].Difficulty);
        Assert.AreEqual(TriviaQuestionDifficulty.Hard, questions[2].Difficulty);

        Assert.AreEqual(TriviaQuestionType.MultipleChoice, questions[0].Type);
        Assert.AreEqual(TriviaQuestionType.MultipleChoice, questions[1].Type);
        Assert.AreEqual(TriviaQuestionType.TrueFalse, questions[2].Type);

    }

    [TestMethod]
    public void ApiDataSerializer_DeserializeTriviaQuestions_WithEmptyResults_ReturnsEmptyList()
    {
        var json = @"{ ""response_code"": 0, ""results"": [] }";
        var doc = JsonDocument.Parse(json);
        var serializer = new ApiDataSerializer();
        // Act
        var questions = serializer.DeserializeTriviaQuestions(doc);
        // Assert
        Assert.IsNotNull(questions);
        Assert.IsFalse(questions.Any());
    }

    [TestMethod]
    public void ApiDataSerializer_DeserializeTriviaQuestions_WithMissingResults_ReturnsEmptyList()
    {
        var json = @"{ ""response_code"": 0 }";
        var doc = JsonDocument.Parse(json);
        var serializer = new ApiDataSerializer();
        // Act
        var questions = serializer.DeserializeTriviaQuestions(doc);
        // Assert
        Assert.IsNotNull(questions);
        Assert.IsFalse(questions.Any());
    }

    [TestMethod]
    public void ApiDataSerializer_DeserializeTriviaQuestions_WithInvalidDifficulty_Throws()
    {
        var json = @"{ ""response_code"": 0, ""results"": [ {""type"": ""boolean"", ""difficulty"": ""invalid"", ""question"": ""What is the name of the island introduced in the ARMA III: APEX expansion pack?"",
      ""correct_answer"": ""Tanoa"",
      ""incorrect_answers"": [
        ""Altis"",
        ""Stratis"",
        ""Malden""
      ]
 } ] }";
        var doc = JsonDocument.Parse(json);
        var serializer = new ApiDataSerializer();
        // Act & Assert
        Assert.ThrowsExactly<InvalidOperationException>(() => _ = serializer.DeserializeTriviaQuestions(doc));
    }

    [TestMethod]
    public void ApiDataSerializer_DeserializeTriviaQuestions_WithInvalidType_Throws()
    {
        var json = @"{ ""response_code"": 0, ""results"": [ {""type"": ""invalid"", ""difficulty"": ""easy"", ""question"": ""What is the name of the island introduced in the ARMA III: APEX expansion pack?"",
      ""correct_answer"": ""Tanoa"",
      ""incorrect_answers"": [
        ""Altis"",
        ""Stratis"",
        ""Malden""
      ]
 } ] }";
        var doc = JsonDocument.Parse(json);
        var serializer = new ApiDataSerializer();
        // Act & Assert
        Assert.ThrowsExactly<InvalidOperationException>(() => _ = serializer.DeserializeTriviaQuestions(doc));
    }


}
