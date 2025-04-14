using Memoraid.WebApi.Configuration;
using Memoraid.WebApi.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.Configure<ApplicationOptions>(builder.Configuration);

builder.Services.AddDbContext<MemoraidDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"))
        .UseSnakeCaseNamingConvention());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/dbtest", (MemoraidDbContext dbContext) =>
{
    var flashcards = dbContext.Flashcards.ToList();
    var flashcardAIGenerations = dbContext.FlashcardAIGenerations.ToList();
    var users = dbContext.Users.ToList();

    return Results.Ok(new
    {
        Flashcards = flashcards,
        FlashcardAIGenerations = flashcardAIGenerations,
        Users = users
    });
});

app.Run();
