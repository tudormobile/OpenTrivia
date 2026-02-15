namespace OpenTrivia.Tests;

[TestClass]
public class OpenTriviaClientTests
{
    public TestContext TestContext { get; set; } // MSTest will set this property

    [TestMethod]
    public void OpenTriviaClient_Create_WithArguments_CreatesClient()
    {
        // Arrange
        using var mockHandler = new MockHttpMessageHandler();
        using var httpClient = new HttpClient(mockHandler);
        var serializer = new ApiDataSerializer();

        // Act
        var client = IOpenTriviaClient.Create(httpClient, serializer: serializer);

        // Assert
        Assert.IsInstanceOfType<IOpenTriviaClient>(client);
    }

    [TestMethod]
    public async Task OpenTriviaClient_GetSessionTokenAsync_ReturnsToken()
    {
        // Arrange
        using var mockHandler = new MockHttpMessageHandler()
        {
            JsonResponse = @"{
                ""response_code"": 0,
                ""response_message"": ""Token Generated Successfully!"",
                ""token"": ""12345""
            }"
        };
        using var httpClient = new HttpClient(mockHandler);
        var client = new OpenTriviaClient(httpClient);

        // Act
        var response = await client.GetSessionTokenAsync(cancellationToken: TestContext.CancellationToken);

        // Assert
        Assert.IsTrue(response.IsSuccess);
        Assert.IsNotNull(response.Data);
        Assert.AreEqual(ApiResponseCode.Success, response.ResponseCode);
        Assert.IsFalse(string.IsNullOrWhiteSpace(response.Data.Token));
        Assert.AreEqual("12345", response.Data.Token);
    }

    [TestMethod]
    public async Task OpenTriviaClient_GetSessionTokenAsync_WithNullTokenResponse_ReturnsEmptyToken()
    {
        // Arrange
        using var mockHandler = new MockHttpMessageHandler()
        {
            JsonResponse = @"{
                ""response_code"": 0,
                ""response_message"": ""Token Generated Successfully!"",
                ""token"": null
            }"
        };
        using var httpClient = new HttpClient(mockHandler);
        var client = new OpenTriviaClient(httpClient);

        // Act
        var response = await client.GetSessionTokenAsync(cancellationToken: TestContext.CancellationToken);

        // Assert
        Assert.IsTrue(response.IsSuccess);
        Assert.IsNotNull(response.Data);
        Assert.AreEqual(ApiResponseCode.Success, response.ResponseCode);
        Assert.AreEqual(String.Empty, response.Data.Token);
    }

    [TestMethod]
    public async Task OpenTriviaClient_ResetSessionTokenAsync_ReturnsSuccess()
    {
        // Arrange
        var existingToken = new ApiSessionToken("12345");
        using var mockHandler = new MockHttpMessageHandler()
        {
            JsonResponse = @"{
                ""response_code"": 0,
                ""response_message"": ""Token Reset Successfully!"",
                ""token"": ""12345""
            }"
        };
        using var httpClient = new HttpClient(mockHandler);
        var client = new OpenTriviaClient(httpClient);

        // Act
        var response = await client.ResetSessionTokenAsync(existingToken, cancellationToken: TestContext.CancellationToken);

        // Assert
        Assert.IsTrue(response.IsSuccess);
        Assert.IsNotNull(response.Data);
        Assert.AreEqual(ApiResponseCode.Success, response.ResponseCode);
        Assert.IsFalse(string.IsNullOrWhiteSpace(response.Data.Token));
        Assert.AreEqual(existingToken.Token, response.Data.Token);
    }

    [TestMethod]
    public async Task OpenTriviaClient_ResetSessionTokenAsync_WithMissingToken_ReturnsTokenNotFound()
    {
        // Arrange
        var existingToken = new ApiSessionToken("12345");
        using var mockHandler = new MockHttpMessageHandler()
        {
            JsonResponse = @"{""response_code"":3,""token"":""12345""}"
        };
        using var httpClient = new HttpClient(mockHandler);
        var client = new OpenTriviaClient(httpClient);

        // Act
        var response = await client.ResetSessionTokenAsync(existingToken, cancellationToken: TestContext.CancellationToken);

        // Assert
        Assert.IsFalse(response.IsSuccess);
        Assert.AreEqual(ApiResponseCode.TokenNotFound, response.ResponseCode);
        Assert.IsNotNull(response.Data);
        Assert.AreEqual(existingToken.Token, response.Data.Token);
    }

    [DataRow(0, DisplayName = "Zero amount")]
    [DataRow(-1, DisplayName = "Negative amount")]
    [DataRow(51, DisplayName = "Amount exceeds maximum (51)")]
    [TestMethod]
    public async Task OpenTriviaClient_GetQuestionsAsync_WithInvalidAmount_Throws(int invalidAmount)
    {
        // Arrange
        using var mockHandler = new MockHttpMessageHandler()
        {
            JsonResponse = @"{
                ""response_code"": 0,
                ""results"": []
            }"
        };
        using var httpClient = new HttpClient(mockHandler);
        var client = new OpenTriviaClient(httpClient);
        // Act & Assert
        await Assert.ThrowsExactlyAsync<ArgumentOutOfRangeException>(async () =>
        {
            var response = await client.GetQuestionsAsync(invalidAmount, cancellationToken: TestContext.CancellationToken);
        });

    }

    [TestMethod]
    public async Task OpenTriviaClient_GetQuestionsAsync_NoOptions()
    {
        // Arrange
        using var mockHandler = new MockHttpMessageHandler()
        {
            JsonResponse = @"{
                ""response_code"": 0,
                ""results"": []
            }"
        };
        using var httpClient = new HttpClient(mockHandler);
        var client = new OpenTriviaClient(httpClient);
        // Act
        var response = await client.GetQuestionsAsync(1, cancellationToken: TestContext.CancellationToken);
        // Assert
        Assert.Contains("amount=1", mockHandler.ProvidedRequestUri!.Query);
    }

    [TestMethod]
    public async Task OpenTriviaClient_GetQuestionsAsync_WithCategory()
    {
        // Arrange
        using var mockHandler = new MockHttpMessageHandler()
        {
            JsonResponse = @"{
                ""response_code"": 0,
                ""results"": []
            }"
        };
        using var httpClient = new HttpClient(mockHandler);
        var client = new OpenTriviaClient(httpClient);
        var category = new TriviaCategory() { Name = "Test Category", Id = 99 };
        // Act
        var response = await client.GetQuestionsAsync(10, category, cancellationToken: TestContext.CancellationToken);
        // Assert
        Assert.Contains("&category=99", mockHandler.ProvidedRequestUri!.Query);
    }

    [TestMethod]
    public async Task OpenTriviaClient_GetQuestionsAsync_WithDifficulty()
    {
        // Arrange
        using var mockHandler = new MockHttpMessageHandler()
        {
            JsonResponse = @"{
                ""response_code"": 0,
                ""results"": []
            }"
        };
        using var httpClient = new HttpClient(mockHandler);
        var client = new OpenTriviaClient(httpClient);
        // Act
        var response = await client.GetQuestionsAsync(10, difficulty: TriviaQuestionDifficulty.Hard, cancellationToken: TestContext.CancellationToken);
        // Assert
        Assert.Contains("&difficulty=hard", mockHandler.ProvidedRequestUri!.Query);
    }

    [TestMethod]
    public async Task OpenTriviaClient_GetQuestionsAsync_WithType()
    {
        // Arrange
        using var mockHandler = new MockHttpMessageHandler()
        {
            JsonResponse = @"{
                ""response_code"": 0,
                ""results"": []
            }"
        };
        using var httpClient = new HttpClient(mockHandler);
        var client = new OpenTriviaClient(httpClient);
        // Act
        var response = await client.GetQuestionsAsync(10, type: TriviaQuestionType.TrueFalse, cancellationToken: TestContext.CancellationToken);
        // Assert
        Assert.Contains("&type=boolean", mockHandler.ProvidedRequestUri!.Query);
    }

    [TestMethod]
    public async Task OpenTriviaClient_GetQuestionsAsync_WithTypeMultipleChoice()
    {
        // Arrange
        using var mockHandler = new MockHttpMessageHandler()
        {
            JsonResponse = @"{
                ""response_code"": 0,
                ""results"": []
            }"
        };
        using var httpClient = new HttpClient(mockHandler);
        var client = new OpenTriviaClient(httpClient);
        // Act
        var response = await client.GetQuestionsAsync(10, type: TriviaQuestionType.MultipleChoice, cancellationToken: TestContext.CancellationToken);
        // Assert
        Assert.Contains("&type=multiple", mockHandler.ProvidedRequestUri!.Query);
    }

    [TestMethod]
    public async Task OpenTriviaClient_GetQuestionsAsync_WithEncoding()
    {
        // Arrange
        using var mockHandler = new MockHttpMessageHandler()
        {
            JsonResponse = @"{
                ""response_code"": 0,
                ""results"": []
            }"
        };
        using var httpClient = new HttpClient(mockHandler);
        var client = new OpenTriviaClient(httpClient);

        // Act
        var response = await client.GetQuestionsAsync(10, encoding: ApiEncodingType.Base64, cancellationToken: TestContext.CancellationToken);
        // Assert
        Assert.Contains("&encode=base64", mockHandler.ProvidedRequestUri!.Query);
    }

    [TestMethod]
    public async Task OpenTriviaClient_GetQuestionsAsync_WithToken()
    {
        // Arrange
        using var mockHandler = new MockHttpMessageHandler()
        {
            JsonResponse = @"{
                ""response_code"": 0,
                ""results"": []
            }"
        };
        using var httpClient = new HttpClient(mockHandler);
        var client = new OpenTriviaClient(httpClient);
        var token = new ApiSessionToken("123");
        // Act
        var response = await client.GetQuestionsAsync(10, token: token, cancellationToken: TestContext.CancellationToken);
        // Assert
        Assert.Contains("&token=123", mockHandler.ProvidedRequestUri!.Query);
    }

    [TestMethod]
    public async Task OpenTriviaClient_GetCategoriesAsync_ReturnsCategories()
    {
        // Arrange
        var json = @"{
  ""trivia_categories"": [
    {
      ""id"": 9,
      ""name"": ""General Knowledge""
    },
    {
      ""id"": 10,
      ""name"": ""Entertainment: Books""
    },
    {
      ""id"": 11,
      ""name"": ""Entertainment: Film""
    },
    {
      ""id"": 12,
      ""name"": ""Entertainment: Music""
    },
    {
      ""id"": 13,
      ""name"": ""Entertainment: Musicals & Theatres""
    }
  ]
}";
        using var mockHandler = new MockHttpMessageHandler() { JsonResponse = json };
        using var httpClient = new HttpClient(mockHandler);
        var client = new OpenTriviaClient(httpClient);
        // Act
        var response = await client.GetCategoriesAsync(TestContext.CancellationToken);
        // Assert
        Assert.IsTrue(response.IsSuccess);
        Assert.IsNotNull(response.Data);
        Assert.HasCount(5, response.Data);
    }

    [TestMethod]
    public async Task OpenTriviaClient_GetCategoriesAsync_WithNoResults_ReturnsEmptyCategories()
    {
        // Arrange
        var json = @"{}";
        using var mockHandler = new MockHttpMessageHandler() { JsonResponse = json };
        using var httpClient = new HttpClient(mockHandler);
        var client = new OpenTriviaClient(httpClient);
        // Act
        var response = await client.GetCategoriesAsync(TestContext.CancellationToken);
        // Assert
        Assert.IsFalse(response.IsSuccess);
        Assert.AreEqual(ApiResponseCode.NoResults, response.ResponseCode);
        Assert.IsNotNull(response.Data);
        Assert.HasCount(0, response.Data);
    }

    [TestMethod]
    public async Task OpenTriviaClient_GetQuestionsWithDefaultEncoding_ReturnsEncodedResponse()
    {
        // Arrange
        var json = @"{
  ""response_code"": 0,
  ""results"": [
    {
      ""type"": ""boolean"",
      ""difficulty"": ""medium"",
      ""category"": ""Entertainment: Video Games"",
      ""question"": ""The first Maxis game to feature the fictional language &quot;Simlish&quot; was The Sims (2000)."",
      ""correct_answer"": ""False"",
      ""incorrect_answers"": [
        ""True""
      ]
    }
  ]
}";
        using var mockHandler = new MockHttpMessageHandler() { JsonResponse = json };
        using var httpClient = new HttpClient(mockHandler);
        var client = new OpenTriviaClient(httpClient, encodingType: null);

        // Act
        var response = await client.GetQuestionsAsync(1, cancellationToken: TestContext.CancellationToken);

        // Assert
        Assert.IsTrue(response.IsSuccess);
        Assert.IsNotNull(response.Data);
        Assert.HasCount(1, response.Data);
        Assert.Contains("&quot;", response.Data[0].Question);
    }

    [TestMethod]
    public async Task OpenTriviaClient_GetQuestionsWithDefaultEncodingAndAutoDecode_ReturnsDecodedResponse()
    {
        // Arrange
        var json = @"{
  ""response_code"": 0,
  ""results"": [
    {
      ""type"": ""boolean"",
      ""difficulty"": ""medium"",
      ""category"": ""Entertainment: Video Games"",
      ""question"": ""The first Maxis game to feature the fictional language &quot;Simlish&quot; was The Sims (2000)."",
      ""correct_answer"": ""False"",
      ""incorrect_answers"": [
        ""True""
      ]
    }
  ]
}";
        using var mockHandler = new MockHttpMessageHandler() { JsonResponse = json };
        using var httpClient = new HttpClient(mockHandler);
        var client = new OpenTriviaClient(httpClient, autoDecode: true, encodingType: null);

        // Act
        var response = await client.GetQuestionsAsync(1, cancellationToken: TestContext.CancellationToken);

        // Assert
        Assert.IsTrue(response.IsSuccess);
        Assert.IsNotNull(response.Data);
        Assert.HasCount(1, response.Data);
        Assert.Contains(" \"Simlish\" ", response.Data[0].Question, "Failed to decode the question");
    }

    [TestMethod]
    public async Task OpenTriviaClient_GetQuestionsWithBase64Encoding_ReturnsBase64Response()
    {
        // Arrange
        var json = @"{
  ""response_code"": 0,
  ""results"": [
    {
      ""type"": ""bXVsdGlwbGU="",
      ""difficulty"": ""ZWFzeQ=="",
      ""category"": ""U2NpZW5jZSAmIE5hdHVyZQ=="",
      ""question"": ""V2hpY2ggb2YgdGhlIGZvbGxvd2luZyBibG9vZCB2ZXNzZWxzIGNhcnJpZXMgZGVveHlnZW5hdGVkIGJsb29kPw=="",
      ""correct_answer"": ""UHVsbW9uYXJ5IEFydGVyeQ=="",
      ""incorrect_answers"": [
        ""UHVsbW9uYXJ5IFZlaW4="",
        ""QW9ydGE="",
        ""Q29yb25hcnkgQXJ0ZXJ5""
      ]
    }
  ]
}";
        using var mockHandler = new MockHttpMessageHandler() { JsonResponse = json };
        using var httpClient = new HttpClient(mockHandler);
        var client = new OpenTriviaClient(httpClient, encodingType: ApiEncodingType.Base64);

        // Act
        var response = await client.GetQuestionsAsync(1, cancellationToken: TestContext.CancellationToken);

        // Assert
        Assert.IsTrue(response.IsSuccess);
        Assert.IsNotNull(response.Data);
        Assert.HasCount(1, response.Data);
        Assert.AreEqual("V2hpY2ggb2YgdGhlIGZvbGxvd2luZyBibG9vZCB2ZXNzZWxzIGNhcnJpZXMgZGVveHlnZW5hdGVkIGJsb29kPw==", response.Data[0].Question, "Failed to return the base64 encoded question");
        Assert.AreEqual("UHVsbW9uYXJ5IEFydGVyeQ==", response.Data[0].CorrectAnswer, "Failed to return the base64 encoded correct answer");
        Assert.AreEqual("UHVsbW9uYXJ5IFZlaW4=", response.Data[0].IncorrectAnswers[0], "Failed to return the base64 encoded incorrect answer 1");
        Assert.AreEqual("QW9ydGE=", response.Data[0].IncorrectAnswers[1], "Failed to return the base64 encoded incorrect answer 2");
        Assert.AreEqual("Q29yb25hcnkgQXJ0ZXJ5", response.Data[0].IncorrectAnswers[2], "Failed to return the base64 encoded incorrect answer 3");

    }


    [TestMethod]
    public async Task OpenTriviaClient_GetQuestionsWithBase64EncodingAndAutoDecode_ReturnsDecodedResponse()
    {
        // Arrange
        var json = @"{
  ""response_code"": 0,
  ""results"": [
    {
      ""type"": ""bXVsdGlwbGU="",
      ""difficulty"": ""ZWFzeQ=="",
      ""category"": ""U2NpZW5jZSAmIE5hdHVyZQ=="",
      ""question"": ""V2hpY2ggb2YgdGhlIGZvbGxvd2luZyBibG9vZCB2ZXNzZWxzIGNhcnJpZXMgZGVveHlnZW5hdGVkIGJsb29kPw=="",
      ""correct_answer"": ""UHVsbW9uYXJ5IEFydGVyeQ=="",
      ""incorrect_answers"": [
        ""UHVsbW9uYXJ5IFZlaW4="",
        ""QW9ydGE="",
        ""Q29yb25hcnkgQXJ0ZXJ5""
      ]
    }
  ]
}";
        using var mockHandler = new MockHttpMessageHandler() { JsonResponse = json };
        using var httpClient = new HttpClient(mockHandler);
        var client = new OpenTriviaClient(httpClient, autoDecode: true, encodingType: ApiEncodingType.Base64);

        // Act
        var response = await client.GetQuestionsAsync(1, cancellationToken: TestContext.CancellationToken);

        // Assert
        Assert.IsTrue(response.IsSuccess);
        Assert.IsNotNull(response.Data);
        Assert.HasCount(1, response.Data);
        Assert.AreEqual("Which of the following blood vessels carries deoxygenated blood?", response.Data[0].Question, "Failed to return the decoded question");
        Assert.AreEqual("Pulmonary Artery", response.Data[0].CorrectAnswer, "Failed to return the decoded correct answer");
        Assert.AreEqual("Pulmonary Vein", response.Data[0].IncorrectAnswers[0], "Failed to return the decoded incorrect answer 1");
        Assert.AreEqual("Aorta", response.Data[0].IncorrectAnswers[1], "Failed to return the decoded incorrect answer 2");
        Assert.AreEqual("Coronary Artery", response.Data[0].IncorrectAnswers[2], "Failed to return the decoded incorrect answer 3");

        Assert.AreEqual("Science & Nature", response.Data[0].Category.Name, "Failed to return the decoded category");
        Assert.AreEqual(TriviaQuestionDifficulty.Easy, response.Data[0].Difficulty, "Failed to return the decoded difficulty");
        Assert.AreEqual(TriviaQuestionType.MultipleChoice, response.Data[0].Type, "Failed to return the decoded question type");
    }

    [TestMethod]
    public async Task OpenTriviaClient_GetQuestionsWithClientDefaultEncodingAndMethodBase64EncodingAndAutoDecode_ReturnsDecodedResponse()
    {
        // Arrange
        var json = @"{
  ""response_code"": 0,
  ""results"": [
    {
      ""type"": ""bXVsdGlwbGU="",
      ""difficulty"": ""ZWFzeQ=="",
      ""category"": ""U2NpZW5jZSAmIE5hdHVyZQ=="",
      ""question"": ""V2hpY2ggb2YgdGhlIGZvbGxvd2luZyBibG9vZCB2ZXNzZWxzIGNhcnJpZXMgZGVveHlnZW5hdGVkIGJsb29kPw=="",
      ""correct_answer"": ""UHVsbW9uYXJ5IEFydGVyeQ=="",
      ""incorrect_answers"": [
        ""UHVsbW9uYXJ5IFZlaW4="",
        ""QW9ydGE="",
        ""Q29yb25hcnkgQXJ0ZXJ5""
      ]
    }
  ]
}";
        using var mockHandler = new MockHttpMessageHandler() { JsonResponse = json };
        using var httpClient = new HttpClient(mockHandler);
        var client = new OpenTriviaClient(httpClient, autoDecode: true, encodingType: ApiEncodingType.Default); // use different value from the method call below.

        // Act
        var response = await client.GetQuestionsAsync(1, encoding: ApiEncodingType.Base64, cancellationToken: TestContext.CancellationToken);

        // Assert
        Assert.IsTrue(response.IsSuccess);
        Assert.IsNotNull(response.Data);
        Assert.HasCount(1, response.Data);
        Assert.AreEqual("Which of the following blood vessels carries deoxygenated blood?", response.Data[0].Question, "Failed to return the decoded question");
        Assert.AreEqual("Pulmonary Artery", response.Data[0].CorrectAnswer, "Failed to return the decoded correct answer");
        Assert.AreEqual("Pulmonary Vein", response.Data[0].IncorrectAnswers[0], "Failed to return the decoded incorrect answer 1");
        Assert.AreEqual("Aorta", response.Data[0].IncorrectAnswers[1], "Failed to return the decoded incorrect answer 2");
        Assert.AreEqual("Coronary Artery", response.Data[0].IncorrectAnswers[2], "Failed to return the decoded incorrect answer 3");

    }

    [TestMethod]
    public async Task OpenTriviaClient_GetQuestionsWithUrl3986EncodingAndAutoDecode_ReturnsDecodedResponse()
    {
        // Arrange
        var json = @"{
  ""response_code"": 0,
  ""results"": [
    {
      ""type"": ""boolean"",
      ""difficulty"": ""medium"",
      ""category"": ""Entertainment%3A%20Music"",
      ""question"": ""The%20music%20group%20Daft%20Punk%20got%20their%20name%20from%20a%20negative%20review%20they%20received."",
      ""correct_answer"": ""True"",
      ""incorrect_answers"": [
        ""False""
      ]
    }
  ]
}";
        using var mockHandler = new MockHttpMessageHandler() { JsonResponse = json };
        using var httpClient = new HttpClient(mockHandler);
        var client = new OpenTriviaClient(httpClient, autoDecode: true, encodingType: ApiEncodingType.Url3986);

        // Act
        var response = await client.GetQuestionsAsync(1,
            type: TriviaQuestionType.TrueFalse,
            difficulty: TriviaQuestionDifficulty.Medium,
            cancellationToken: TestContext.CancellationToken);

        // Assert
        Assert.IsTrue(response.IsSuccess);
        Assert.IsNotNull(response.Data);
        Assert.HasCount(1, response.Data);

        Assert.AreEqual("Entertainment: Music", response.Data[0].Category.Name, "Failed to return the decoded category");
        Assert.AreEqual(TriviaQuestionDifficulty.Medium, response.Data[0].Difficulty, "Failed to return the decoded difficulty");
        Assert.AreEqual(TriviaQuestionType.TrueFalse, response.Data[0].Type, "Failed to return the decoded question type");
    }

    [TestMethod]
    public async Task OpenTriviaClient_GetQuestionsWithUrl3986EncodingAndHardDifficultyAndAutoDecode_ReturnsDecodedResponse()
    {
        // Arrange
        var json = @"{
  ""response_code"": 0,
  ""results"": [
    {
      ""type"": ""boolean"",
      ""difficulty"": ""hard"",
      ""category"": ""Entertainment%3A%20Music"",
      ""question"": ""The%20music%20group%20Daft%20Punk%20got%20their%20name%20from%20a%20negative%20review%20they%20received."",
      ""correct_answer"": ""True"",
      ""incorrect_answers"": [
        ""False""
      ]
    }
  ]
}";
        using var mockHandler = new MockHttpMessageHandler() { JsonResponse = json };
        using var httpClient = new HttpClient(mockHandler);
        var client = new OpenTriviaClient(httpClient, autoDecode: true);

        // Act
        var response = await client.GetQuestionsAsync(1,
            type: TriviaQuestionType.TrueFalse,
            difficulty: TriviaQuestionDifficulty.Hard,
            encoding: ApiEncodingType.Url3986,
            cancellationToken: TestContext.CancellationToken);

        // Assert
        Assert.IsTrue(response.IsSuccess);
        Assert.IsNotNull(response.Data);
        Assert.HasCount(1, response.Data);
        Assert.AreEqual("Entertainment: Music", response.Data[0].Category.Name, "Failed to return the decoded category");
        Assert.AreEqual(TriviaQuestionDifficulty.Hard, response.Data[0].Difficulty, "Failed to return the decoded difficulty");
        Assert.AreEqual(TriviaQuestionType.TrueFalse, response.Data[0].Type, "Failed to return the decoded question type");
    }

    [TestMethod]
    public async Task OpenTriviaClient_GetQuestionsWithBase64EncodingAndMediumAndAutoDecode_ReturnsDecodedResponse()
    {
        // Arrange
        var json = @"{
  ""response_code"": 0,
  ""results"": [
    {
      ""type"": ""Ym9vbGVhbg=="",
      ""difficulty"": ""bWVkaXVt"",
      ""category"": ""TXl0aG9sb2d5"",
      ""question"": ""QWNjb3JkaW5nIHRvIE5vcnNlIG15dGhvbG9neSwgTG9raSBpcyBhIG1vdGhlci4="",
      ""correct_answer"": ""VHJ1ZQ=="",
      ""incorrect_answers"": [
        ""RmFsc2U=""
      ]
    }
  ]
}";
        using var mockHandler = new MockHttpMessageHandler() { JsonResponse = json };
        using var httpClient = new HttpClient(mockHandler);
        var client = new OpenTriviaClient(httpClient, autoDecode: true, encodingType: ApiEncodingType.Base64);

        // Act
        var response = await client.GetQuestionsAsync(1,
            type: TriviaQuestionType.TrueFalse,
            difficulty: TriviaQuestionDifficulty.Medium,
            cancellationToken: TestContext.CancellationToken);

        // Assert
        Assert.IsTrue(response.IsSuccess);
        Assert.IsNotNull(response.Data);
        Assert.HasCount(1, response.Data);

        Assert.AreEqual("Mythology", response.Data[0].Category.Name, "Failed to return the decoded category");
        Assert.AreEqual(TriviaQuestionDifficulty.Medium, response.Data[0].Difficulty, "Failed to return the decoded difficulty");
        Assert.AreEqual(TriviaQuestionType.TrueFalse, response.Data[0].Type, "Failed to return the decoded question type");
    }

    [TestMethod]
    public async Task OpenTriviaClient_GetQuestionsWithBase64EncodingAndHardDifficultyAndAutoDecode_ReturnsDecodedResponse()
    {
        // Arrange
        var json = @"{
  ""response_code"": 0,
  ""results"": [
    {
      ""type"": ""Ym9vbGVhbg=="",
      ""difficulty"": ""aGFyZA=="",
      ""category"": ""RW50ZXJ0YWlubWVudDogQ2FydG9vbiAmIEFuaW1hdGlvbnM="",
      ""question"": ""U25hZ2dsZXB1c3Mgd2FzIHBhcnQgb2YgdGhlIFlvZ2kgWWFob29pZXMgaW4gdGhlIDE5Nzcgc2hvdyBTY29vYnkncyBBbGwtU3RhciBMYWZmLWEtTHltcGljcy4="",
      ""correct_answer"": ""RmFsc2U="",
      ""incorrect_answers"": [
        ""VHJ1ZQ==""
      ]
    }
  ]
}";
        using var mockHandler = new MockHttpMessageHandler() { JsonResponse = json };
        using var httpClient = new HttpClient(mockHandler);
        var client = new OpenTriviaClient(httpClient, autoDecode: true);

        // Act
        var response = await client.GetQuestionsAsync(1,
            type: TriviaQuestionType.TrueFalse,
            difficulty: TriviaQuestionDifficulty.Hard,
            encoding: ApiEncodingType.Base64,
            cancellationToken: TestContext.CancellationToken);

        // Assert
        Assert.IsTrue(response.IsSuccess);
        Assert.IsNotNull(response.Data);
        Assert.HasCount(1, response.Data);
        Assert.AreEqual("Entertainment: Cartoon & Animations", response.Data[0].Category.Name, "Failed to return the decoded category");
        Assert.AreEqual(TriviaQuestionDifficulty.Hard, response.Data[0].Difficulty, "Failed to return the decoded difficulty");
        Assert.AreEqual(TriviaQuestionType.TrueFalse, response.Data[0].Type, "Failed to return the decoded question type");
    }

    [TestMethod]
    public async Task OpenTriviaClient_GetQuestionsWithBase64EncodingAndMedium_ReturnsDecodedResponse()
    {
        // Arrange
        var json = @"{
  ""response_code"": 0,
  ""results"": [
    {
      ""type"": ""Ym9vbGVhbg=="",
      ""difficulty"": ""bWVkaXVt"",
      ""category"": ""TXl0aG9sb2d5"",
      ""question"": ""QWNjb3JkaW5nIHRvIE5vcnNlIG15dGhvbG9neSwgTG9raSBpcyBhIG1vdGhlci4="",
      ""correct_answer"": ""VHJ1ZQ=="",
      ""incorrect_answers"": [
        ""RmFsc2U=""
      ]
    }
  ]
}";
        using var mockHandler = new MockHttpMessageHandler() { JsonResponse = json };
        using var httpClient = new HttpClient(mockHandler);
        var client = new OpenTriviaClient(httpClient, autoDecode: false, encodingType: ApiEncodingType.Base64);

        // Act
        var response = await client.GetQuestionsAsync(1,
            type: TriviaQuestionType.TrueFalse,
            difficulty: TriviaQuestionDifficulty.Medium,
            cancellationToken: TestContext.CancellationToken);

        // Assert
        Assert.IsTrue(response.IsSuccess);
        Assert.IsNotNull(response.Data);
        Assert.HasCount(1, response.Data);

        Assert.AreEqual("TXl0aG9sb2d5", response.Data[0].Category.Name, "Failed to return the decoded category");
        Assert.AreEqual(TriviaQuestionDifficulty.Medium, response.Data[0].Difficulty, "Failed to return the decoded difficulty");
        Assert.AreEqual(TriviaQuestionType.TrueFalse, response.Data[0].Type, "Failed to return the decoded question type");
    }

    [TestMethod]
    public async Task OpenTriviaClient_GetQuestionsWithBase64EncodingAndHardDifficulty_ReturnsDecodedResponse()
    {
        // Arrange
        var json = @"{
  ""response_code"": 0,
  ""results"": [
    {
      ""type"": ""Ym9vbGVhbg=="",
      ""difficulty"": ""aGFyZA=="",
      ""category"": ""RW50ZXJ0YWlubWVudDogQ2FydG9vbiAmIEFuaW1hdGlvbnM="",
      ""question"": ""U25hZ2dsZXB1c3Mgd2FzIHBhcnQgb2YgdGhlIFlvZ2kgWWFob29pZXMgaW4gdGhlIDE5Nzcgc2hvdyBTY29vYnkncyBBbGwtU3RhciBMYWZmLWEtTHltcGljcy4="",
      ""correct_answer"": ""RmFsc2U="",
      ""incorrect_answers"": [
        ""VHJ1ZQ==""
      ]
    }
  ]
}";
        using var mockHandler = new MockHttpMessageHandler() { JsonResponse = json };
        using var httpClient = new HttpClient(mockHandler);
        var client = new OpenTriviaClient(httpClient, autoDecode: false);

        // Act
        var response = await client.GetQuestionsAsync(1,
            type: TriviaQuestionType.TrueFalse,
            difficulty: TriviaQuestionDifficulty.Hard,
            encoding: ApiEncodingType.Base64,
            cancellationToken: TestContext.CancellationToken);

        // Assert
        Assert.IsTrue(response.IsSuccess);
        Assert.IsNotNull(response.Data);
        Assert.HasCount(1, response.Data);
        Assert.AreEqual("RW50ZXJ0YWlubWVudDogQ2FydG9vbiAmIEFuaW1hdGlvbnM=", response.Data[0].Category.Name, "Failed to return the decoded category");
        Assert.AreEqual(TriviaQuestionDifficulty.Hard, response.Data[0].Difficulty, "Failed to return the decoded difficulty");
        Assert.AreEqual(TriviaQuestionType.TrueFalse, response.Data[0].Type, "Failed to return the decoded question type");
    }

    [TestMethod]
    public async Task OpenTriviaClient_GetQuestionCountAsync_ReturnsApiResult()
    {
        // Arrange
        var json = @"{
  ""category_id"": 14,
  ""category_question_count"": {
    ""total_question_count"": 189,
    ""total_easy_question_count"": 73,
    ""total_medium_question_count"": 86,
    ""total_hard_question_count"": 30
  }
}";
        using var mockHandler = new MockHttpMessageHandler() { JsonResponse = json };
        using var httpClient = new HttpClient(mockHandler);
        var client = new OpenTriviaClient(httpClient);
        var category = new TriviaCategory()
        {
            Id = 14,
            Name = "Entertainment: Television"
        };

        // Act
        var response = await client.GetQuestionCountAsync(category, cancellationToken: TestContext.CancellationToken);

        // Assert
        Assert.IsTrue(response.IsSuccess);
        Assert.IsNotNull(response.Data);
        Assert.AreEqual(189, response.Data.TotalQuestionCount);
        Assert.AreEqual(73, response.Data.EasyQuestionCount);
        Assert.AreEqual(86, response.Data.MediumQuestionCount);
        Assert.AreEqual(30, response.Data.HardQuestionCount);
        Assert.Contains("category=14", mockHandler.ProvidedRequestUri!.Query);

    }

    [TestMethod]
    public async Task OpenTriviaClient_GetQuestionCountAsync_WithInvalidJson_ReturnsApiResult()
    {
        // Arrange
        var json = @"{
  ""category_id"": 14,
  ""category_question_count"": {
    ""total_medium_question_count"": 86,
    ""total_hard_question_count"": 30
  }
}";
        using var mockHandler = new MockHttpMessageHandler() { JsonResponse = json };
        using var httpClient = new HttpClient(mockHandler);
        var client = new OpenTriviaClient(httpClient);
        var category = new TriviaCategory()
        {
            Id = 14,
            Name = "Entertainment: Television"
        };

        // Act
        var response = await client.GetQuestionCountAsync(category, cancellationToken: TestContext.CancellationToken);

        // Assert
        Assert.IsFalse(response.IsSuccess);
        Assert.IsNull(response.Data);
    }

}
