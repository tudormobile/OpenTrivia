using Tudormobile.OpenTrivia;
using Tudormobile.OpenTrivia.UI.ViewModels;

namespace OpenTrivia.UI.Tests.ViewModels;

[TestClass]
public class QuestionCollectionTests
{
    [TestMethod]
    public void QuestionCollection_Constructor_InitializesWithTriviaQuestions()
    {
        // Arrange
        var triviaQuestions = new[]
        {
            new TriviaQuestion
            {
                Category = new TriviaCategory { Id = 1, Name = "General Knowledge" },
                Type = TriviaQuestionType.MultipleChoice,
                Difficulty = TriviaQuestionDifficulty.Easy,
                Question = "What is the capital of France?",
                CorrectAnswer = "Paris",
                IncorrectAnswers = ["London", "Berlin", "Madrid"]
            },
            new TriviaQuestion
            {
                Category = new TriviaCategory { Id = 2, Name = "Science" },
                Type = TriviaQuestionType.TrueFalse,
                Difficulty = TriviaQuestionDifficulty.Medium,
                Question = "The Earth is flat.",
                CorrectAnswer = "False",
                IncorrectAnswers = ["True"]
            },
            new TriviaQuestion
            {
                Category = new TriviaCategory { Id = 3, Name = "History" },
                Type = TriviaQuestionType.MultipleChoice,
                Difficulty = TriviaQuestionDifficulty.Hard,
                Question = "Who was the first President of the United States?",
                CorrectAnswer = "George Washington",
                IncorrectAnswers = ["Thomas Jefferson", "John Adams", "Benjamin Franklin"]
            }
        };

        // Act
        var questionCollection = new QuestionCollection(triviaQuestions);

        // Assert
        Assert.IsNotNull(questionCollection);
        Assert.HasCount(3, questionCollection);

        // Verify sequential question numbers
        Assert.AreEqual(1, questionCollection[0].QuestionNumber);
        Assert.AreEqual(2, questionCollection[1].QuestionNumber);
        Assert.AreEqual(3, questionCollection[2].QuestionNumber);

        // Verify question content
        Assert.AreEqual("What is the capital of France?", questionCollection[0].Question);
        Assert.AreEqual("The Earth is flat.", questionCollection[1].Question);
        Assert.AreEqual("Who was the first President of the United States?", questionCollection[2].Question);
    }

    [TestMethod]
    public void QuestionCollection_Constructor_NullTriviaQuestions_ThrowsArgumentNullException()
    {
        // Arrange, Act & Assert
        Assert.ThrowsExactly<ArgumentNullException>(() => new QuestionCollection(null!));
    }
}
