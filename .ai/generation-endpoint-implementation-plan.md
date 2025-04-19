# API Endpoint Implementation Plan: Generate Flashcards via AI

## 1. Endpoint Overview
Implement a POST endpoint at `/flashcards/generate` to generate flashcard proposals using AI. The endpoint accepts user-supplied text (up to 10000 characters) and returns a series of flashcards along with a generation ID.

## 2. Request Details
- HTTP Method: POST
- URL Structure: /flashcards/generate
- Parameters: None in URL; request body only.
- Request Body:
  - Required:
    - sourceText (string, maximum 10000 characters)
  - Note: The request class property remains optional per guidelines.

## 3. Types Used
- Request DTO: GenerateFlashcardsRequest (contains SourceText)
- Response DTO: GenerateFlashcardsResponse (contains a list of Flashcard DTOs and GenerationId)

## 4. Response Details
- Success (200 OK): Returns a JSON object with:
  - flashcards: Array of objects; each containing:
    - front (string, max 500 characters)
    - back (string, max 200 characters)
  - generationId: integer
- Error Responses:
  - 400: Syntactic validation errors
  - 401: Unauthenticated (won't be implemented now)
  - 422: Business rule violations (e.g., exceeding character limits)

## 5. Data Flow
1. Client sends POST request with JSON payload including sourceText.
2. Controller/endpoint receives request and passes it to a dedicated service.
3. The service validates the input (using FluentValidation) and processes the request, invoking AI generation logic.
4. The service interacts with the database to record the generation process (FlashcardAIGeneration entity).
5. A validated response, including flashcards and a generationId, is returned to the client.

## 6. Security Considerations
- Input validation is implemented using FluentValidation to mitigate injection attacks.

## 7. Error Handling
- 400 Bad Request: Return when syntactic validations (e.g., missing sourceText) fail.
- 401 Unauthorized: Reserved for future authentication implementation.
- 422 Unprocessable Entity: Return when business rules are violated (e.g., sourceText length exceeds 10000 characters).

## 8. Performance Considerations
- Validate request payloads to reduce unnecessary processing.
- Optimize AI generation service to run asynchronously if processing time is long.
- Utilize efficient Entity Framework Core practices (e.g., AsNoTracking for read-only queries) where applicable.

## 9. Implementation Steps
1. Define and register a FluentValidation validator for GenerateFlashcardsRequest ensuring:
   - sourceText is not null and its length is ≤ 10000 characters.
2. Add a dedicated service (e.g., IFlashcardGenerationService) that encapsulates validation, AI logic and flashcard creation and generation logging in the database.
3. Implement the POST endpoint in a minimal API route mapping:
   - Map the route `/flashcards/generate`.
   - Inject dependencies (generation service).
   - Process the request and handle validation errors accordingly.
4. Create unit tests using NUnit and Moq for:
   - GenerateFlashcardsRequestValidator
   - FlashcardGenerationService