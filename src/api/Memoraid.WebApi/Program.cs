using FluentValidation;
using Memoraid.WebApi.Configuration;
using Memoraid.WebApi.Middleware;
using Memoraid.WebApi.OpenApi;
using Memoraid.WebApi.Persistence;
using Memoraid.WebApi.Persistence.Interceptors;
using Memoraid.WebApi.Requests;
using Memoraid.WebApi.Responses;
using Memoraid.WebApi.Services;
using Memoraid.WebApi.Services.OpenRouter;
using Memoraid.WebApi.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:7002")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<DocumentTransformer>()
        .AddSchemaTransformer<ResponseSchemaTransformer>()
        .AddSchemaTransformer<NonNullableEnumSchemaTransformer>();
});

builder.Services.Configure<ApplicationOptions>(builder.Configuration);

builder.Services.AddDbContext<MemoraidDbContext>((sp, options) =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"))
        .UseSnakeCaseNamingConvention()
        .AddInterceptors(new SaveEntityBaseInterceptor(sp.GetRequiredService<IUserContext>()));
});

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddHttpClient<IOpenRouterService, OpenRouterService>((serviceProvider, client) =>
{
    var options = serviceProvider.GetRequiredService<Microsoft.Extensions.Options.IOptions<ApplicationOptions>>().Value;
    client.BaseAddress = new Uri(options.OpenRouter.ApiBaseUrl);
    client.Timeout = TimeSpan.FromSeconds(60);
});

builder.Services.AddScoped<IOpenRouterService>(serviceProvider =>
{
    var options = serviceProvider.GetRequiredService<Microsoft.Extensions.Options.IOptions<ApplicationOptions>>().Value;

    if (options.OpenRouter.UseMock)
    {
        return new MockOpenRouterService();
    }

    return serviceProvider.GetRequiredService<OpenRouterService>();
});

builder.Services
    .AddAuthentication()
    .AddJwtBearer(options =>
    {
        var useEmulator = builder.Configuration.GetValue<bool>("Firebase:UseEmulator");
        var firebaseProjectId = builder.Configuration.GetRequiredSection("Firebase:ProjectId").Value;
        var authority = builder.Configuration.GetRequiredSection("Firebase:Auth:Authority").Value;

        options.MapInboundClaims = false;
        options.Authority = authority;
        options.RequireHttpsMetadata = !useEmulator;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = $"https://securetoken.google.com/{firebaseProjectId}",
            ValidateAudience = true,
            ValidAudience = firebaseProjectId,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            RequireSignedTokens = !useEmulator,
        };
    });

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthorization();

// Add services
builder.Services.AddScoped<IFlashcardGenerationService, FlashcardGenerationService>();
builder.Services.AddScoped<IFlashcardService, FlashcardService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserContext, UserContext>();

// Add validators
builder.Services.AddScoped<IValidator<GenerateFlashcardsRequest>, GenerateFlashcardsRequestValidator>();
builder.Services.AddScoped<IValidator<CreateFlashcardsRequest>, CreateFlashcardsRequestValidator>();
builder.Services.AddScoped<IValidator<GetFlashcardsRequest>, GetFlashcardsRequestValidator>();
builder.Services.AddScoped<IValidator<long>, DeleteFlashcardRequestValidator>();
builder.Services.AddScoped<IValidator<UpdateFlashcardRequest>, UpdateFlashcardRequestValidator>();

var app = builder.Build();

app.UseMiddleware<FluentValidationExceptionMiddleware>();
app.UseMiddleware<UserNotAuthenticatedExceptionMiddleware>();

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/flashcards/generate", async (GenerateFlashcardsRequest request, IFlashcardGenerationService generationService) =>
{
    var result = await generationService.GenerateFlashcardsAsync(request);

    return Results.Ok(new Response<GenerateFlashcardsResponse>(result));
})
.WithName("GenerateFlashcards")
.Produces<Response<GenerateFlashcardsResponse>>()
.RequireAuthorization();

app.MapPost("/flashcards", async (CreateFlashcardsRequest request, IFlashcardService flashcardService) =>
{
    await flashcardService.CreateFlashcardsAsync(request);

    return Results.Created("/flashcards", new Response());
})
.WithName("CreateFlashcards")
.Produces<Response>()
.RequireAuthorization();

app.MapGet("/flashcards", async ([AsParameters] GetFlashcardsRequest request, IFlashcardService flashcardService) =>
{
    var result = await flashcardService.GetFlashcardsAsync(request);

    return Results.Ok(result);
})
.WithName("GetFlashcards")
.Produces<Response<GetFlashcardsResponse>>()
.RequireAuthorization();

app.MapDelete("/flashcards/{id}", async (long id, IFlashcardService flashcardService) =>
{
    var response = await flashcardService.DeleteFlashcardAsync(id);

    if (!response.IsSuccess)
    {
        var flashcardNotFound = response.Errors.All(x => x.Code == IFlashcardService.ErrorCodes.FlashcardNotFound);

        return flashcardNotFound ? Results.NotFound(response) : Results.UnprocessableEntity(response);
    }

    return Results.Ok(response);
})
.WithName("DeleteFlashcard")
.Produces<Response>()
.RequireAuthorization();

app.MapPut("/flashcards/{id}", async (long id, UpdateFlashcardRequest request, IFlashcardService flashcardService) =>
{
    var response = await flashcardService.UpdateFlashcardAsync(id, request);

    if (!response.IsSuccess)
    {
        var flashcardNotFound = response.Errors.All(x => x.Code == IFlashcardService.ErrorCodes.FlashcardNotFound);

        return flashcardNotFound ? Results.NotFound(response) : Results.UnprocessableEntity(response);
    }

    return Results.Ok(response);
})
.WithName("UpdateFlashcard")
.Produces<Response>()
.RequireAuthorization();

app.MapDelete("/users", async (IUserService userService) =>
{
    var response = await userService.DeleteUserAsync();

    if (!response.IsSuccess)
    {
        return Results.UnprocessableEntity(response);
    }

    return Results.Ok(response);
})
.WithName("DeleteUser")
.Produces<Response>()
.RequireAuthorization();

app.Run();
