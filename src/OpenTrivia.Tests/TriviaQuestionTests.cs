namespace OpenTrivia.Tests;

[TestClass]
public class TriviaQuestionTests
{
    [TestMethod]
    public void TriviaQuestion_ShouldInitilizeProperties()
    {
        // Arrange
        var category = new TriviaCategory() { Name = "Science" };
        var type = TriviaQuestionType.MultipleChoice;
        var difficulty = TriviaQuestionDifficulty.Easy;
        var question = "What is the chemical symbol for water?";
        var correctAnswer = "H2O";
        var incorrectAnswers = new List<string> { "CO2", "O2", "NaCl" };

        // Act
        TriviaQuestion triviaQuestion = new()
        {
            Category = category,
            Type = type,
            Difficulty = difficulty,
            Question = question,
            CorrectAnswer = correctAnswer,
            IncorrectAnswers = incorrectAnswers
        };

        // Assert
        Assert.AreEqual(category, triviaQuestion.Category);
        Assert.AreEqual(type, triviaQuestion.Type);
        Assert.AreEqual(difficulty, triviaQuestion.Difficulty);
        Assert.AreEqual(question, triviaQuestion.Question);
        Assert.AreEqual(correctAnswer, triviaQuestion.CorrectAnswer);
        CollectionAssert.AreEqual(incorrectAnswers, triviaQuestion.IncorrectAnswers);
    }

    [TestMethod]
    public void TriviaQuestion_ConstructWith_ShouldInitilizeProperties()
    {
        // Arrange
        var category = new TriviaCategory() { Name = "Science" };
        var type = TriviaQuestionType.MultipleChoice;
        var difficulty = TriviaQuestionDifficulty.Easy;
        var question = "What is the chemical symbol for water?";
        var correctAnswer = "H2O";
        var incorrectAnswers = new List<string> { "CO2", "O2", "NaCl" };

        var expectedDifficulty = TriviaQuestionDifficulty.Hard;

        // Act
        var triviaQuestion = new TriviaQuestion()
        {
            Category = category,
            Type = type,
            Difficulty = difficulty,
            Question = question,
            CorrectAnswer = correctAnswer,
            IncorrectAnswers = incorrectAnswers
        }
        with
        {
            Difficulty = expectedDifficulty
        };

        // Assert
        Assert.AreEqual(category, triviaQuestion.Category);
        Assert.AreEqual(type, triviaQuestion.Type);
        Assert.AreEqual(expectedDifficulty, triviaQuestion.Difficulty);
        Assert.AreEqual(question, triviaQuestion.Question);
        Assert.AreEqual(correctAnswer, triviaQuestion.CorrectAnswer);
        CollectionAssert.AreEqual(incorrectAnswers, triviaQuestion.IncorrectAnswers);
    }

}
