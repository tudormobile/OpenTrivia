using Tudormobile.OpenTrivia;
using Tudormobile.OpenTrivia.UI.ViewModels;

namespace OpenTrivia.UI.Tests.ViewModels;

[TestClass]
public class SelectableQuestionTests
{
    [TestMethod]
    public void SelectableQuestion_DefaultConstructorTest()
    {
        // Arrange & Act
        var questionNumber = 123;
        var question = new TriviaQuestion()
        {
            Category = new TriviaCategory() { Id = 1, Name = "General Knowledge" },
            Type = TriviaQuestionType.MultipleChoice,
            Difficulty = TriviaQuestionDifficulty.Easy,
            Question = "What is the capital of France?",
            CorrectAnswer = "Paris",
            IncorrectAnswers = ["London", "Berlin", "Madrid"]
        };
        var selectableQuestion = new SelectableQuestion(questionNumber, question);

        // Assert
        Assert.IsNotNull(selectableQuestion);
        Assert.IsFalse(selectableQuestion.IsSelected);
        Assert.AreEqual(questionNumber, selectableQuestion.QuestionNumber);

        // Verify the derived properties are initialized to their default values
        Assert.IsNull(selectableQuestion.IsCorrect);
        Assert.IsNull(selectableQuestion.SelectedAnswer);
        Assert.IsFalse(selectableQuestion.IsAnswered);

        // Verify that the properties from TriviaQuestion are correctly assigned
        Assert.AreEqual(question.Question, selectableQuestion.Question);
        Assert.AreEqual(question.Category, selectableQuestion.Category);
        Assert.AreEqual(question.Type, selectableQuestion.Type);
        Assert.AreEqual(question.Difficulty, selectableQuestion.Difficulty);
        Assert.AreEqual(question.CorrectAnswer, selectableQuestion.CorrectAnswer);
        CollectionAssert.AreEqual(question.IncorrectAnswers, selectableQuestion.IncorrectAnswers.ToList());
    }
    [TestMethod]
    public void SelectableQuestion_Constructor_NullTriviaQuestion_ThrowsArgumentNullException()
    {
        // Arrange
        var questionNumber = 123;
        // Act & Assert
        Assert.ThrowsExactly<ArgumentNullException>(() => new SelectableQuestion(questionNumber, null!));
    }

    [TestMethod]
    public void SelectableQuestion_Constructor_InvalidQuestionNumber_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var triviaQuestion = new TriviaQuestion()
        {
            Category = new TriviaCategory() { Id = 1, Name = "General Knowledge" },
            Type = TriviaQuestionType.MultipleChoice,
            Difficulty = TriviaQuestionDifficulty.Easy,
            Question = "What is the capital of France?",
            CorrectAnswer = "Paris",
            IncorrectAnswers = ["London", "Berlin", "Madrid"]
        };
        // Act & Assert
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new SelectableQuestion(0, triviaQuestion));
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new SelectableQuestion(-1, triviaQuestion));
    }

    [TestMethod]
    public void SelectableQuestion_AllAnswers_ReturnsCorrectly()
    {
        // Arrange
        var questionNumber = 123;
        var question = new TriviaQuestion()
        {
            Category = new TriviaCategory() { Id = 1, Name = "General Knowledge" },
            Type = TriviaQuestionType.MultipleChoice,
            Difficulty = TriviaQuestionDifficulty.Easy,
            Question = "What is the capital of France?",
            CorrectAnswer = "Paris",
            IncorrectAnswers = ["London", "Berlin", "Madrid"]
        };
        var selectableQuestion = new SelectableQuestion(questionNumber, question);
        // Act
        var allAnswers = selectableQuestion.AllAnswers;
        // Assert
        Assert.IsNotNull(allAnswers);
        Assert.HasCount(4, allAnswers);
        Assert.Contains("Paris", allAnswers);
        Assert.Contains("London", allAnswers);
        Assert.Contains("Berlin", allAnswers);
        Assert.Contains("Madrid", allAnswers);
    }

    [TestMethod]
    public void SelectableQuestion_SelectedAnswer_UpdatesIsCorrectAndIsAnswered()
    {
        // Arrange
        var questionNumber = 123;
        var question = new TriviaQuestion()
        {
            Category = new TriviaCategory() { Id = 1, Name = "General Knowledge" },
            Type = TriviaQuestionType.MultipleChoice,
            Difficulty = TriviaQuestionDifficulty.Easy,
            Question = "What is the capital of France?",
            CorrectAnswer = "Paris",
            IncorrectAnswers = ["London", "Berlin", "Madrid"]
        };
        var selectableQuestion = new SelectableQuestion(questionNumber, question);
        // Act
        selectableQuestion.SelectedAnswer = "Paris";
        // Assert
        Assert.IsTrue(selectableQuestion.IsCorrect);
        Assert.IsTrue(selectableQuestion.IsAnswered);
    }

    [TestMethod]
    public void SelectableQuestion_SelectedAnswer_WithIncorrectAnswer_UpdatesIsCorrectAndIsAnswered()
    {
        // Arrange
        var questionNumber = 123;
        var question = new TriviaQuestion()
        {
            Category = new TriviaCategory() { Id = 1, Name = "General Knowledge" },
            Type = TriviaQuestionType.MultipleChoice,
            Difficulty = TriviaQuestionDifficulty.Easy,
            Question = "What is the capital of France?",
            CorrectAnswer = "Paris",
            IncorrectAnswers = ["London", "Berlin", "Madrid"]
        };
        var selectableQuestion = new SelectableQuestion(questionNumber, question);
        // Act
        selectableQuestion.SelectedAnswer = "London";
        // Assert
        Assert.IsFalse(selectableQuestion.IsCorrect);
        Assert.IsTrue(selectableQuestion.IsAnswered);
    }

}
