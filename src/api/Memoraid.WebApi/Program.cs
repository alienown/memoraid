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
using System.Text;
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

builder.Services.AddDbContext<MemoraidDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"))
        .UseSnakeCaseNamingConvention()
        .AddInterceptors(new SaveEntityBaseInterceptor()));

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

builder.Services
    .AddAuthentication()
    .AddJwtBearer(options =>
    {
        var key = Encoding.ASCII.GetBytes(builder.Configuration.GetRequiredSection("Jwt:Secret").Value!);
        var issuer = builder.Configuration.GetRequiredSection("Jwt:Issuer").Value!;
        var audience = builder.Configuration.GetRequiredSection("Jwt:Audience").Value!;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// Add services
builder.Services.AddScoped<IFlashcardGenerationService, FlashcardGenerationService>();
builder.Services.AddScoped<IFlashcardService, FlashcardService>();
builder.Services.AddScoped<IUserService, UserService>();

// Add validators
builder.Services.AddScoped<IValidator<GenerateFlashcardsRequest>, GenerateFlashcardsRequestValidator>();
builder.Services.AddScoped<IValidator<CreateFlashcardsRequest>, CreateFlashcardsRequestValidator>();
builder.Services.AddScoped<IValidator<GetFlashcardsRequest>, GetFlashcardsRequestValidator>();
builder.Services.AddScoped<IValidator<long>, DeleteFlashcardRequestValidator>();
builder.Services.AddScoped<IValidator<UpdateFlashcardRequest>, UpdateFlashcardRequestValidator>();
builder.Services.AddScoped<IValidator<RegisterUserRequest>, RegisterUserRequestValidator>();
builder.Services.AddScoped<IValidator<LoginUserRequest>, LoginUserRequestValidator>();

var app = builder.Build();

app.UseMiddleware<FluentValidationExceptionMiddleware>();

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
.Produces<Response<GenerateFlashcardsResponse>>();

app.MapPost("/flashcards", async (CreateFlashcardsRequest request, IFlashcardService flashcardService) =>
{
    await flashcardService.CreateFlashcardsAsync(request);

    return Results.Created("/flashcards", new Response());
})
.WithName("CreateFlashcards")
.Produces<Response>();

app.MapGet("/flashcards", async ([AsParameters] GetFlashcardsRequest request, IFlashcardService flashcardService) =>
{
    var result = await flashcardService.GetFlashcardsAsync(request);

    return Results.Ok(result);
})
.WithName("GetFlashcards")
.Produces<Response<GetFlashcardsResponse>>();

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
.Produces<Response>();

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
.Produces<Response>();

app.MapPost("/users/register", async (RegisterUserRequest request, IUserService userService) =>
{
    var result = await userService.RegisterUserAsync(request);
    return Results.Created("/users/register", result);
})
.WithName("RegisterUser")
.Produces<Response>();

app.MapPost("/users/login", async (LoginUserRequest request, IUserService userService) =>
{
    var response = await userService.LoginUserAsync(request);

    if (!response.IsSuccess)
    {
        var invalidCredentials = response.Errors.All(x => x.Code == IUserService.ErrorCodes.InvalidCredentials);

        return invalidCredentials
            ? Results.Json(response, statusCode: StatusCodes.Status401Unauthorized)
            : Results.UnprocessableEntity(response);
    }

    return Results.Ok(response);
})
.WithName("LoginUser")
.Produces<Response<LoginUserResponse>>();

app.Run();
