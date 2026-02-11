using Tudormobile.OpenTrivia;
namespace SimpleConsoleApp;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var httpClient = new HttpClient();
        var client = new OpenTriviaClient(httpClient);

        var query = client.GetQuestionsAsync(amount: 10);

        query.Wait();

        Console.WriteLine($"IsSuccess: {query.Result.IsSuccess}");
        Console.WriteLine($"Questions: {query.Result.Data?.Count}");
        query.Result.Data?.ForEach(q =>
        {
            Console.WriteLine();
            Console.WriteLine($"Question: {q.Question}");
            Console.WriteLine($"Category: {q.Category}");
            Console.WriteLine($"Difficulty: {q.Difficulty}");
            Console.WriteLine($"Type: {q.Type}");
            Console.WriteLine($"Correct Answer: {q.CorrectAnswer}");
            Console.WriteLine($"Incorrect Answers: {string.Join(", ", q.IncorrectAnswers)}");
            Console.WriteLine();
        });
    }
}

/* Program Output:
Hello, World!
{
"Global Quote": {
    "01. symbol": "IBM",
    "02. open": "125.0000",
    "03. high": "126.5000",
    "04. low": "124.5000",
    "05. price": "125.7500",
    "06. volume": "3500000",
    "07. latest trading day": "2023-10-20",
    "08. previous close": "124.8000",
    "09. change": "0.9500",
    "10. change percent": "0.7615%"
}
}
*/
