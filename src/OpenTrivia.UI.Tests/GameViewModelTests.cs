using Tudormobile.OpenTrivia;
using Tudormobile.OpenTrivia.UI.ViewModels;

namespace OpenTrivia.UI.Tests;

[TestClass]
public class GameViewModelTests
{
    [TestMethod]
    public void GameViewModel_Constructor_InitializesCorrectly()
    {
        // Arrange
        var triviaQuestions = new[]
        {
            new TriviaQuestion
            {
                Category = new TriviaCategory { Id = 1, Name = "Science" },
                Type = TriviaQuestionType.MultipleChoice,
                Difficulty = TriviaQuestionDifficulty.Easy,
                Question = "What is H2O?",
                CorrectAnswer = "Water",
                IncorrectAnswers = ["Air", "Fire", "Earth"]
            },
            new TriviaQuestion
            {
                Category = new TriviaCategory { Id = 2, Name = "History" },
                Type = TriviaQuestionType.TrueFalse,
                Difficulty = TriviaQuestionDifficulty.Medium,
                Question = "World War II ended in 1945.",
                CorrectAnswer = "True",
                IncorrectAnswers = ["False"]
            },
            new TriviaQuestion
            {
                Category = new TriviaCategory { Id = 1, Name = "Science" },
                Type = TriviaQuestionType.MultipleChoice,
                Difficulty = TriviaQuestionDifficulty.Hard,
                Question = "What is the chemical symbol for gold?",
                CorrectAnswer = "Au",
                IncorrectAnswers = ["Ag", "Fe", "Cu"]
            }
        };
        var questionCollection = new QuestionCollection(triviaQuestions);

        // Act
        var viewModel = new GameViewModel(questionCollection);

        // Assert
        Assert.IsNotNull(viewModel);

        // Verify Questions collection
        Assert.IsNotNull(viewModel.Questions);
        Assert.HasCount(3, viewModel.Questions);
        Assert.AreEqual(1, viewModel.Questions[0].QuestionNumber);
        Assert.AreEqual(2, viewModel.Questions[1].QuestionNumber);
        Assert.AreEqual(3, viewModel.Questions[2].QuestionNumber);

        // Verify Categories collection (should have 2 distinct categories)
        Assert.IsNotNull(viewModel.Categories);
        Assert.HasCount(2, viewModel.Categories);

        // Verify categories are ordered by name (History, Science)
        Assert.AreEqual("History", viewModel.Categories[0].Name);
        Assert.AreEqual("Science", viewModel.Categories[1].Name);

        // Verify initial state
        Assert.IsNull(viewModel.SelectedQuestion);
        Assert.AreEqual(0, viewModel.Score);
    }

    [TestMethod]
    public void GameViewModel_Constructor_InitializesWithTriviaGameCorrectly()
    {
        // Arrange
        var triviaQuestions = new[]
        {
            new TriviaQuestion
            {
                Category = new TriviaCategory { Id = 1, Name = "Science" },
                Type = TriviaQuestionType.MultipleChoice,
                Difficulty = TriviaQuestionDifficulty.Easy,
                Question = "What is H2O?",
                CorrectAnswer = "Water",
                IncorrectAnswers = ["Air", "Fire", "Earth"]
            },
            new TriviaQuestion
            {
                Category = new TriviaCategory { Id = 2, Name = "History" },
                Type = TriviaQuestionType.TrueFalse,
                Difficulty = TriviaQuestionDifficulty.Medium,
                Question = "World War II ended in 1945.",
                CorrectAnswer = "True",
                IncorrectAnswers = ["False"]
            },
            new TriviaQuestion
            {
                Category = new TriviaCategory { Id = 1, Name = "Science" },
                Type = TriviaQuestionType.MultipleChoice,
                Difficulty = TriviaQuestionDifficulty.Hard,
                Question = "What is the chemical symbol for gold?",
                CorrectAnswer = "Au",
                IncorrectAnswers = ["Ag", "Fe", "Cu"]
            }
        };
        var game = new TriviaGame(triviaQuestions, triviaQuestions.Select(q => q.Category).Distinct());

        // Act
        var viewModel = new GameViewModel(game);

        // Assert
        Assert.IsNotNull(viewModel);
        // Verify Questions collection
        Assert.IsNotNull(viewModel.Questions);
        Assert.HasCount(3, viewModel.Questions);

    }


    [TestMethod]
    public void GameViewModel_Constructor_NullGame_ThrowsArgumentNullException()
    {
        // Arrange, Act & Assert
        Assert.ThrowsExactly<ArgumentNullException>(() => new GameViewModel(game: null!));
    }

    [TestMethod]
    public void GameViewModel_Constructor_NullQuestions_ThrowsArgumentNullException()
    {
        // Arrange, Act & Assert
        Assert.ThrowsExactly<ArgumentNullException>(() => new GameViewModel(questions: null!));
    }

    [TestMethod]
    public void GameViewModel_AnswerQuestion_UpdatesScore()
    {
        // Arrange
        var triviaQuestions = new[]
        {
            new TriviaQuestion
            {
                Category = new TriviaCategory { Id = 1, Name = "Science" },
                Type = TriviaQuestionType.MultipleChoice,
                Difficulty = TriviaQuestionDifficulty.Easy,
                Question = "What is H2O?",
                CorrectAnswer = "Water",
                IncorrectAnswers = ["Air", "Fire", "Earth"]
            },
            new TriviaQuestion
            {
                Category = new TriviaCategory { Id = 2, Name = "History" },
                Type = TriviaQuestionType.TrueFalse,
                Difficulty = TriviaQuestionDifficulty.Medium,
                Question = "World War II ended in 1945.",
                CorrectAnswer = "True",
                IncorrectAnswers = ["False"]
            },
            new TriviaQuestion
            {
                Category = new TriviaCategory { Id = 1, Name = "Science" },
                Type = TriviaQuestionType.MultipleChoice,
                Difficulty = TriviaQuestionDifficulty.Hard,
                Question = "What is the chemical symbol for gold?",
                CorrectAnswer = "Au",
                IncorrectAnswers = ["Ag", "Fe", "Cu"]
            }
        };
        var questionCollection = new QuestionCollection(triviaQuestions);
        var viewModel = new GameViewModel(questionCollection);

        // Act
        viewModel.Questions[0].SelectedAnswer = "Water"; // Select correct answer for first question

        // Assert
        Assert.AreEqual(1, viewModel.Score); // Score should be 1 for correct answer
    }

    [TestMethod]
    public void GameViewModel_ResetGame_UpdatesScoreAndClearsAnswers()
    {
        // Arrange
        var triviaQuestions = new[]
        {
            new TriviaQuestion
            {
                Category = new TriviaCategory { Id = 1, Name = "Science" },
                Type = TriviaQuestionType.MultipleChoice,
                Difficulty = TriviaQuestionDifficulty.Easy,
                Question = "What is H2O?",
                CorrectAnswer = "Water",
                IncorrectAnswers = ["Air", "Fire", "Earth"]
            },
            new TriviaQuestion
            {
                Category = new TriviaCategory { Id = 2, Name = "History" },
                Type = TriviaQuestionType.TrueFalse,
                Difficulty = TriviaQuestionDifficulty.Medium,
                Question = "World War II ended in 1945.",
                CorrectAnswer = "True",
                IncorrectAnswers = ["False"]
            },
            new TriviaQuestion
            {
                Category = new TriviaCategory { Id = 1, Name = "Science" },
                Type = TriviaQuestionType.MultipleChoice,
                Difficulty = TriviaQuestionDifficulty.Hard,
                Question = "What is the chemical symbol for gold?",
                CorrectAnswer = "Au",
                IncorrectAnswers = ["Ag", "Fe", "Cu"]
            }
        };
        var questionCollection = new QuestionCollection(triviaQuestions);
        var viewModel = new GameViewModel(questionCollection);

        // Act
        viewModel.Questions[0].SelectedAnswer = "Water"; // Select correct answer for first question
        Assert.AreEqual(1, viewModel.Score); // Score should be 1 for correct answer
        viewModel.ResetGameCommand.Execute(null); // Reset the game

        // Assert
        Assert.AreEqual(0, viewModel.Score); // Score should be 1 for correct answer
        Assert.IsNull(viewModel.Questions[0].SelectedAnswer); // Selected answer should be cleared
    }

    [TestMethod]
    public void GameViewModel_SelectQuestion_UpdatesSelection()
    {
        // Arrange
        var triviaQuestions = new[]
        {
            new TriviaQuestion
            {
                Category = new TriviaCategory { Id = 1, Name = "Science" },
                Type = TriviaQuestionType.MultipleChoice,
                Difficulty = TriviaQuestionDifficulty.Easy,
                Question = "What is H2O?",
                CorrectAnswer = "Water",
                IncorrectAnswers = ["Air", "Fire", "Earth"]
            },
            new TriviaQuestion
            {
                Category = new TriviaCategory { Id = 2, Name = "History" },
                Type = TriviaQuestionType.TrueFalse,
                Difficulty = TriviaQuestionDifficulty.Medium,
                Question = "World War II ended in 1945.",
                CorrectAnswer = "True",
                IncorrectAnswers = ["False"]
            },
            new TriviaQuestion
            {
                Category = new TriviaCategory { Id = 1, Name = "Science" },
                Type = TriviaQuestionType.MultipleChoice,
                Difficulty = TriviaQuestionDifficulty.Hard,
                Question = "What is the chemical symbol for gold?",
                CorrectAnswer = "Au",
                IncorrectAnswers = ["Ag", "Fe", "Cu"]
            }
        };
        var questionCollection = new QuestionCollection(triviaQuestions);
        var viewModel = new GameViewModel(questionCollection);

        // Act
        viewModel.Questions[2].IsSelected = true;

        // Assert
        Assert.AreEqual(viewModel.Questions[2], viewModel.SelectedQuestion); // Selected question should be the third one
    }

    [TestMethod]
    public void GameViewModel_SelectQuestion_DeselectsOtherSelection()
    {
        // Arrange
        var triviaQuestions = new[]
        {
            new TriviaQuestion
            {
                Category = new TriviaCategory { Id = 1, Name = "Science" },
                Type = TriviaQuestionType.MultipleChoice,
                Difficulty = TriviaQuestionDifficulty.Easy,
                Question = "What is H2O?",
                CorrectAnswer = "Water",
                IncorrectAnswers = ["Air", "Fire", "Earth"]
            },
            new TriviaQuestion
            {
                Category = new TriviaCategory { Id = 2, Name = "History" },
                Type = TriviaQuestionType.TrueFalse,
                Difficulty = TriviaQuestionDifficulty.Medium,
                Question = "World War II ended in 1945.",
                CorrectAnswer = "True",
                IncorrectAnswers = ["False"]
            },
            new TriviaQuestion
            {
                Category = new TriviaCategory { Id = 1, Name = "Science" },
                Type = TriviaQuestionType.MultipleChoice,
                Difficulty = TriviaQuestionDifficulty.Hard,
                Question = "What is the chemical symbol for gold?",
                CorrectAnswer = "Au",
                IncorrectAnswers = ["Ag", "Fe", "Cu"]
            }
        };
        var questionCollection = new QuestionCollection(triviaQuestions);
        var viewModel = new GameViewModel(questionCollection);

        // Act
        viewModel.Questions[2].IsSelected = true;
        viewModel.Questions[0].IsSelected = true;

        // Assert
        Assert.IsFalse(viewModel.Questions[2].IsSelected); // Third question should be deselected
        Assert.IsTrue(viewModel.Questions[0].IsSelected); // First question should be selected
        Assert.AreEqual(viewModel.Questions[0], viewModel.SelectedQuestion); // Selected question should be the first one
    }
}
