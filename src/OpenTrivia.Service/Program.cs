using Tudormobile.OpenTrivia.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Register OpenTrivia client and service
builder.Services.AddTriviaService(options => options.WithRateLimitManagement(true));

// Build the application
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

// Map in the service endpoints
app.UseTriviaService(prefix: String.Empty);

// Start the application
app.Run();
