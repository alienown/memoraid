# REST API Plan

## 1. Resources
- USERS → corresponds to the USERS table.
- FLASHCARDS → corresponds to the FLASHCARDS table.
- FLASHCARD_AI_GENERATIONS → corresponds to the FLASHCARD_AI_GENERATIONS table.
- AUTH → for session management and JWT-based authentication.

## 2. Endpoints

### USERS Resource

*Note: User registration and authentication are now handled by Firebase Authentication.*

- **PUT /users/password**
  - Description: Change user password.
  - Request:
    {
      "currentPassword": "string, required",
      "newPassword": "string, required"
    }
  - Success response:
    - 204 No Content
  - Error responses:
    - 400 for syntactic validation errors
    - 401 if unauthenticated or invalid credentials

- **DELETE /users**
  - Description: Secure account deletion with all user data.
  - Request body: none
  - Success response:
    - 200 Ok with response: `Response`
  - Error responses:
    - 400 for syntactic validation errors
    - 401 if unauthenticated

### FLASHCARDS Resource

- **GET /flashcards**
  - Description: Retrieve a paginated list of user-owned flashcards.
  - Query Parameters: pageNumber (Optional, default 1), pageSize (Optional, default 10, maximum 50)
  - Success response:
    - 200 OK with response: Response<GetFlashcardsResponse>
  - Error responses:
    - 401 if unauthenticated

- **POST /flashcards**
  - Description: Create one or multiple flashcards manually or via AI-generation input.
  - Request:
    {
      "flashcards": [
        {
          "front": "string, required, max 500 chars",
          "back": "string, required, max 200 chars",
          "source": "enum, required, one of ['Manual', 'AIFull', 'AIEdited']",
          "generationId": "big integer, optional, required if source is 'AIFull' or 'AIEdited', should be null if source is 'Manual'. The generationId must be the same for all AI-generated flashcards in the request."
        },
      // ... additional flashcards ...
      ]
    }
  - Success response:
    - 201 Created with response: Response
  - Error responses:
    - 400 for syntactic validation errors
    - 401 if unauthenticated
    - 422 for business rule violations (front or back too long, etc.)

- **PUT /flashcards/{id}**
  - Description: Update flashcard content.
  - Success response:
    - 200 OK with response: Response
  - Error responses:
    - 400 for syntactic validation errors
    - 401 if unauthenticated
    - 404 if flashcard not found or not owned by the user
    - 422 for business rule violations (front or back too long, etc.)

- **DELETE /flashcards/{id}**
  - Description: Delete a specified flashcard.
  - Success response:
    - 200 OK with response: Response
  - Error responses:
    - 401 if unauthenticated
    - 404 if flashcard not found

### FLASHCARD_AI_GENERATIONS Resource

- **POST /flashcards/generate**
  - Description: Generate flashcard proposals using AI based on user-supplied text.
  - Request:
    {
      "sourceText": "string, required, maximum 10000 characters"
    }
  - Success response:
    - 200 OK with response: Response<GenerateFlashcardsResponse>
  - Error responses:
    - 400 for syntactic validation errors
    - 401 if unauthenticated
    - 422 for business rule violations (sourceText too long, etc.)

## 3. Authentication and Authorization

- Firebase Authentication.
- All endpoints require Authorization header with Firebase token.
- Authorization checks ensure that users can only access or modify their own flashcards.
- API should return 401 for unauthenticated requests.
- Security measures include HTTPS.

## 4. Validation and Business Logic

- **Validation Conditions:**
  - USERS:
    - Email must be valid and unique.
    - Password is required.
  - FLASHCARDS:
    - Front is required and must not exceed 500 characters.
    - Back is required and must not exceed 200 characters.
    - GenerationId is required for AI-generated flashcards (AIFull, AIEdited) and must be null for manual ones. The GenerationId must be the same for all AI-generated flashcards in the request.
  - FLASHCARD_AI_GENERATIONS:
    - SourceText is limited to 10000 characters.
    - Generated flashcards must conform to the flashcard constraints.
- **Business Logic Implementation:**
  - User authentication and account management are handled by Firebase Authentication.
  - Flashcard creation and editing enforce input validation via FluentValidation.
  - AI flashcard generation endpoint integrates with an external AI service (e.g., via Open Router API) and returns a set of candidate flashcards for review.
  - Review allow inline acceptance/rejection or modification via an edit modal to match the PRD requirements.
  - Use pagination for list endpoints to optimize performance.
  - All endpoints return appropriate HTTP status codes (e.g., 200, 201, 400, 401, 404) and error messages reflecting validation and business rule violations.

## 5. Types

- The Response type has the following JSON structure:
  - For a non-generic Response:
    {
      "Errors": [ { "Code": "string", "Message": "string", "PropertyName": "string | null" }, ... ],
      "IsSuccess": boolean
    }
  - For a generic Response<T>:
    {
      "Data": T, // an instance of a class of type T
      "Errors": [ { "Code": "string", "Message": "string", "PropertyName": "string | null" }, ... ],
      "IsSuccess": boolean
    }
