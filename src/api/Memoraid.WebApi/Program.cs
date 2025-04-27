using FluentValidation;
using Memoraid.WebApi.Configuration;
using Memoraid.WebApi.Middleware;
using Memoraid.WebApi.OpenApi;
using Memoraid.WebApi.Persistence;
using Memoraid.WebApi.Persistence.Interceptors;
using Memoraid.WebApi.Requests;
using Memoraid.WebApi.Responses;
using Memoraid.WebApi.Services;
using Memoraid.WebApi.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi(options =>
{
    options.AddSchemaTransformer<ResponseSchemaTransformer>();
});

builder.Services.Configure<ApplicationOptions>(builder.Configuration);

builder.Services.AddDbContext<MemoraidDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"))
        .UseSnakeCaseNamingConvention()
        .AddInterceptors(new SaveEntityBaseInterceptor()));

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddScoped<IFlashcardGenerationService, FlashcardGenerationService>();
builder.Services.AddScoped<IFlashcardService, FlashcardService>();
builder.Services.AddScoped<IValidator<GenerateFlashcardsRequest>, GenerateFlashcardsRequestValidator>();
builder.Services.AddScoped<IValidator<CreateFlashcardsRequest>, CreateFlashcardsRequestValidator>();

var app = builder.Build();

app.UseMiddleware<FluentValidationExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

//app.MapGet("/dbtest", (MemoraidDbContext dbContext) =>
//{
//    var flashcards = dbContext.Flashcards.ToList();
//    var flashcardAIGenerations = dbContext.FlashcardAIGenerations.ToList();
//    var users = dbContext.Users.ToList();

//    return Results.Ok(new
//    {
//        Flashcards = flashcards,
//        FlashcardAIGenerations = flashcardAIGenerations,
//        Users = users
//    });
//});

app.MapPost("/flashcards/generate", async (GenerateFlashcardsRequest request, IFlashcardGenerationService generationService) =>
{
    var result = await generationService.GenerateFlashcardsAsync(request);

    return Results.Ok(new Response<GenerateFlashcardsResponse>(result));
})
.WithName("GenerateFlashcards")
.Produces<Response<GenerateFlashcardsResponse>>();

app.MapPost("/flashcards", async (CreateFlashcardsRequest request, IFlashcardService flashcardService) =>
{
    await flashcardService.CreateFlashcardsAsync(request);

    return Results.Created("/flashcards", new Response());
})
.WithName("CreateFlashcards")
.Produces<Response>();

app.Run();
