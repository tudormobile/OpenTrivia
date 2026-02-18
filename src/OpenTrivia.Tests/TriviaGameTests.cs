namespace OpenTrivia.Tests;

[TestClass]
public class TriviaGameTests
{
    [TestMethod]
    public void TriviaGame_ShouldInitializeWithEmptyCollections()
    {
        // Arrange
        var questions = Array.Empty<TriviaQuestion>();
        var categories = Array.Empty<TriviaCategory>();

        // Act
        var game = new TriviaGame(questions, categories);

        // Assert
        Assert.IsNotNull(game.Questions);
        Assert.IsNotNull(game.Categories);
        Assert.AreEqual(0, game.Questions.Count);
        Assert.AreEqual(0, game.Categories.Count);
    }

    [TestMethod]
    public void TriviaGame_ShouldThrowArgumentNullException_WhenQuestionsIsNull()
    {
        // Arrange
        IEnumerable<TriviaQuestion>? questions = null;
        var categories = Array.Empty<TriviaCategory>();

        // Act & Assert
        Assert.ThrowsExactly<ArgumentNullException>(() => new TriviaGame(questions!, categories));
    }

    [TestMethod]
    public void TriviaGame_ShouldThrowArgumentNullException_WhenCategoriesIsNull()
    {
        // Arrange
        var questions = Array.Empty<TriviaQuestion>();
        IEnumerable<TriviaCategory>? categories = null;

        // Act & Assert
        Assert.ThrowsExactly<ArgumentNullException>(() => new TriviaGame(questions, categories!));
    }

    [TestMethod]
    public void TriviaGame_ShouldInitializeWithProvidedQuestions()
    {
        // Arrange
        var category = new TriviaCategory { Id = 9, Name = "General Knowledge" };
        var questions = new List<TriviaQuestion>
        {
            new() 
            { 
                Category = category, 
                Type = TriviaQuestionType.MultipleChoice,
                Difficulty = TriviaQuestionDifficulty.Easy,
                Question = "Question 1", 
                CorrectAnswer = "Answer 1", 
                IncorrectAnswers = ["Wrong 1", "Wrong 2", "Wrong 3"] 
            },
            new() 
            { 
                Category = category, 
                Type = TriviaQuestionType.TrueFalse,
                Difficulty = TriviaQuestionDifficulty.Medium,
                Question = "Question 2", 
                CorrectAnswer = "Answer 2", 
                IncorrectAnswers = ["Wrong 1"] 
            }
        };
        var categories = new List<TriviaCategory> { category };

        // Act
        var game = new TriviaGame(questions, categories);

        // Assert
        Assert.IsNotNull(game.Questions);
        Assert.AreEqual(2, game.Questions.Count);
        Assert.AreEqual("Question 1", game.Questions[0].Question);
        Assert.AreEqual("Question 2", game.Questions[1].Question);
        Assert.AreEqual(TriviaQuestionDifficulty.Easy, game.Questions[0].Difficulty);
        Assert.AreEqual(TriviaQuestionDifficulty.Medium, game.Questions[1].Difficulty);
    }

    [TestMethod]
    public void TriviaGame_ShouldInitializeWithProvidedCategories()
    {
        // Arrange
        var categories = new List<TriviaCategory>
        {
            new() { Id = 9, Name = "General Knowledge" },
            new() { Id = 10, Name = "Books" }
        };
        var questions = Array.Empty<TriviaQuestion>();

        // Act
        var game = new TriviaGame(questions, categories);

        // Assert
        Assert.IsNotNull(game.Categories);
        Assert.AreEqual(2, game.Categories.Count);
        Assert.AreEqual("General Knowledge", game.Categories[0].Name);
        Assert.AreEqual("Books", game.Categories[1].Name);
        Assert.AreEqual(9, game.Categories[0].Id);
        Assert.AreEqual(10, game.Categories[1].Id);
    }

    [TestMethod]
    public void TriviaGame_ShouldInitializeWithQuestionsAndCategories()
    {
        // Arrange
        var categories = new List<TriviaCategory>
        {
            new() { Id = 9, Name = "General Knowledge" },
            new() { Id = 10, Name = "Books" }
        };
        var questions = new List<TriviaQuestion>
        {
            new() 
            { 
                Category = categories[0], 
                Type = TriviaQuestionType.MultipleChoice,
                Difficulty = TriviaQuestionDifficulty.Easy,
                Question = "Question 1", 
                CorrectAnswer = "Answer 1", 
                IncorrectAnswers = ["Wrong 1", "Wrong 2", "Wrong 3"] 
            },
            new() 
            { 
                Category = categories[1], 
                Type = TriviaQuestionType.TrueFalse,
                Difficulty = TriviaQuestionDifficulty.Hard,
                Question = "Question 2", 
                CorrectAnswer = "Answer 2", 
                IncorrectAnswers = ["Wrong 1"] 
            }
        };

        // Act
        var game = new TriviaGame(questions, categories);

        // Assert
        Assert.IsNotNull(game.Questions);
        Assert.IsNotNull(game.Categories);
        Assert.AreEqual(2, game.Questions.Count);
        Assert.AreEqual(2, game.Categories.Count);
        Assert.AreEqual("Question 1", game.Questions[0].Question);
        Assert.AreEqual("General Knowledge", game.Categories[0].Name);
    }

    [TestMethod]
    public void TriviaGame_ShouldCreateIndependentCopyOfQuestions()
    {
        // Arrange
        var category = new TriviaCategory { Id = 9, Name = "General Knowledge" };
        var questions = new List<TriviaQuestion>
        {
            new() 
            { 
                Category = category, 
                Type = TriviaQuestionType.MultipleChoice,
                Difficulty = TriviaQuestionDifficulty.Easy,
                Question = "Question 1", 
                CorrectAnswer = "Answer 1", 
                IncorrectAnswers = ["Wrong 1", "Wrong 2", "Wrong 3"] 
            }
        };
        var categories = new List<TriviaCategory> { category };

        // Act
        var game = new TriviaGame(questions, categories);
        questions.Add(new() 
        { 
            Category = category, 
            Type = TriviaQuestionType.TrueFalse,
            Difficulty = TriviaQuestionDifficulty.Medium,
            Question = "Question 2", 
            CorrectAnswer = "Answer 2", 
            IncorrectAnswers = ["Wrong 1"] 
        });

        // Assert
        Assert.AreEqual(1, game.Questions.Count);
        Assert.AreEqual(2, questions.Count);
    }

    [TestMethod]
    public void TriviaGame_ShouldCreateIndependentCopyOfCategories()
    {
        // Arrange
        var categories = new List<TriviaCategory>
        {
            new() { Id = 9, Name = "General Knowledge" }
        };
        var questions = Array.Empty<TriviaQuestion>();

        // Act
        var game = new TriviaGame(questions, categories);
        categories.Add(new() { Id = 10, Name = "Books" });

        // Assert
        Assert.AreEqual(1, game.Categories.Count);
        Assert.AreEqual(2, categories.Count);
    }

    [TestMethod]
    public void TriviaGame_Questions_ShouldBeReadOnly()
    {
        // Arrange
        var category = new TriviaCategory { Id = 9, Name = "General Knowledge" };
        var questions = new List<TriviaQuestion>
        {
            new() 
            { 
                Category = category, 
                Type = TriviaQuestionType.MultipleChoice,
                Difficulty = TriviaQuestionDifficulty.Easy,
                Question = "Question 1", 
                CorrectAnswer = "Answer 1", 
                IncorrectAnswers = ["Wrong 1", "Wrong 2", "Wrong 3"] 
            }
        };
        var categories = new List<TriviaCategory> { category };

        // Act
        var game = new TriviaGame(questions, categories);

        // Assert
        Assert.IsInstanceOfType<IReadOnlyList<TriviaQuestion>>(game.Questions);
        Assert.AreEqual(1, game.Questions.Count);
    }

    [TestMethod]
    public void TriviaGame_Categories_ShouldBeReadOnly()
    {
        // Arrange
        var categories = new List<TriviaCategory>
        {
            new() { Id = 9, Name = "General Knowledge" }
        };
        var questions = Array.Empty<TriviaQuestion>();

        // Act
        var game = new TriviaGame(questions, categories);

        // Assert
        Assert.IsInstanceOfType<IReadOnlyList<TriviaCategory>>(game.Categories);
        Assert.AreEqual(1, game.Categories.Count);
    }

    [TestMethod]
    public void TriviaGame_ShouldHandleMixedQuestionTypes()
    {
        // Arrange
        var category = new TriviaCategory { Id = 9, Name = "General Knowledge" };
        var questions = new List<TriviaQuestion>
        {
            new() 
            { 
                Category = category, 
                Type = TriviaQuestionType.MultipleChoice,
                Difficulty = TriviaQuestionDifficulty.Easy,
                Question = "Multiple Choice Question", 
                CorrectAnswer = "Correct", 
                IncorrectAnswers = ["Wrong 1", "Wrong 2", "Wrong 3"] 
            },
            new() 
            { 
                Category = category, 
                Type = TriviaQuestionType.TrueFalse,
                Difficulty = TriviaQuestionDifficulty.Easy,
                Question = "True/False Question", 
                CorrectAnswer = "True", 
                IncorrectAnswers = ["False"] 
            }
        };
        var categories = new List<TriviaCategory> { category };

        // Act
        var game = new TriviaGame(questions, categories);

        // Assert
        Assert.AreEqual(2, game.Questions.Count);
        Assert.AreEqual(TriviaQuestionType.MultipleChoice, game.Questions[0].Type);
        Assert.AreEqual(TriviaQuestionType.TrueFalse, game.Questions[1].Type);
    }

    [TestMethod]
    public void TriviaGame_ShouldHandleMixedDifficulties()
    {
        // Arrange
        var category = new TriviaCategory { Id = 9, Name = "General Knowledge" };
        var questions = new List<TriviaQuestion>
        {
            new() 
            { 
                Category = category, 
                Type = TriviaQuestionType.MultipleChoice,
                Difficulty = TriviaQuestionDifficulty.Easy,
                Question = "Easy Question", 
                CorrectAnswer = "Answer", 
                IncorrectAnswers = ["Wrong 1", "Wrong 2", "Wrong 3"] 
            },
            new() 
            { 
                Category = category, 
                Type = TriviaQuestionType.MultipleChoice,
                Difficulty = TriviaQuestionDifficulty.Medium,
                Question = "Medium Question", 
                CorrectAnswer = "Answer", 
                IncorrectAnswers = ["Wrong 1", "Wrong 2", "Wrong 3"] 
            },
            new() 
            { 
                Category = category, 
                Type = TriviaQuestionType.MultipleChoice,
                Difficulty = TriviaQuestionDifficulty.Hard,
                Question = "Hard Question", 
                CorrectAnswer = "Answer", 
                IncorrectAnswers = ["Wrong 1", "Wrong 2", "Wrong 3"] 
            }
        };
        var categories = new List<TriviaCategory> { category };

        // Act
        var game = new TriviaGame(questions, categories);

        // Assert
        Assert.AreEqual(3, game.Questions.Count);
        Assert.AreEqual(TriviaQuestionDifficulty.Easy, game.Questions[0].Difficulty);
        Assert.AreEqual(TriviaQuestionDifficulty.Medium, game.Questions[1].Difficulty);
        Assert.AreEqual(TriviaQuestionDifficulty.Hard, game.Questions[2].Difficulty);
    }
}
