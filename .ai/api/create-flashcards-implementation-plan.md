# API Endpoint Implementation Plan: Create Flashcards

## 1. Endpoint Overview
This endpoint creates one or multiple flashcards either manually or via AI-generated input. It accepts an array of flashcards and enforces business rules—especially around source types and AI generation IDs—to ensure data consistency and integrity.

## 2. Request Details
- **HTTP Method:** POST
- **URL Structure:** /flashcards
- **Parameters:**
  - **Required:** 
    - flashcards (array, required, must contain at least one flashcard)
      - flashcard object fields:
        - front: string, required, max 500 characters
        - back: string, required, max 200 characters
        - source: string, required, one of ['Manual', 'AIFull', 'AIEdited']
  - **Optional:** 
    - generationId: number, greater than zero, required for flashcards where source is 'AIFull' or 'AIEdited' and must be the same value across all AI-generated flashcards in the request, must be null for 'Manual'. GenerationId must refer to an existing AI generation in the database that belongs to the user that is creating the flashcards.
- **Request Body Structure:**  
  {
    "flashcards": [
      {
        "front": "...",
        "back": "...",
        "source": "...",
        "generationId": ...
      },
      // ... additional flashcards ...
    ]
  }

## 3. Types Used
- CreateFlashcardsRequest (provided DTO)
- Response - a general response class for API endpoints

## 3. Response Details
- **Success (201 Created):** Returns a non-generic Response object indicating that the flashcards have been successfully created.
- **400 Bad Request:** For syntactic validation errors.
- **401 Unauthorized:** When the request is unauthenticated.
- **403 Forbidden:** When the user is not authorized to perform the operation.
- **422 Unprocessable Entity:** For business rule violations, for example inconsistent or invalid generationId values.

## 4. Data Flow
1. The API receives a POST request with the flashcards data.
2. The request is forwarded to a service layer where:
   - Request validators (using FluentValidation) check:
     - Character limits for 'front' and 'back'
     - If 'source' values belong to the allowed set
     - Presence, value validation, and consistency of 'generationId' based on the 'source'
3. After successful validation, the service maps the DTOs to Entity Framework Core entities.
4. The service then persists the flashcards in the PostgreSQL database.
5. A response of 201 Created is returned upon success.

## 5. Security Considerations
- **Authentication:** Ensure the request is authenticated (JWT) (will be implemented later).
- **Authorization:** Verify that the authenticated user is permitted to create flashcards (will be implemented later).
- **Data Validation:** Enforce size, type, and business rules through FluentValidation.
- **Input Sanitization:** Guard against injection attacks and malformed data inputs.

## 6. Error Handling
- **400 Bad Request:** For syntactic validation errors detected during FluentValidation.
- **401 Unauthorized:** If the request is unauthenticated (will be implemented later).
- **403 Forbidden:** If the user lacks permission to create flashcards (will be implemented later).
- **422 Unprocessable Entity:** For business rule violations such as inconsistent or improperly provided generationId for AI flashcards.

## 7. Performance Considerations
- Use EF Core’s optimized querying (e.g., AsNoTracking for read-only queries) where applicable.
- Validate data at the service level to avoid unnecessary database calls in case of failure.
- Consider batching transactions for multiple flashcards to reduce round-trips to the database.

## 8. Implementation Steps
1. **Define/Update DTOs:** Confirm that the CreateFlashcardsRequest DTO matches the API specification.
2. **Create FluentValidator:** Implement a FluentValidation validator for CreateFlashcardsRequest to enforce:
   - Maximum lengths for 'front' and 'back'.
   - Allowed values for 'source'.
   - GenerationId validation:
     - Must be null for 'Manual' source.
     - Must be present and consistent across all flashcards for 'AIFull' or 'AIEdited' sources.
     - Must refer to an existing AI generation in the database that belongs to the user creating the flashcards.
3. **Develop Service Layer:**
   - Implement a service method (e.g., CreateFlashcards) to handle mapping, validation, and persistence.
   - Inject the FluentValidator and invoke it inside the service method.
4. **Integrate Data Persistence:** Map the validated DTOs to EF Core entities and insert records into the flashcards table.
5. **Implement API Endpoint:**  
   - Create a minimal API endpoint (POST /flashcards) that calls the service method.
   - Return instance of Response class.
6. **Implement Error Handling:** Ensure appropriate error codes (400, 401, 403, 422) are returned based on exceptions and validation failures.
7. **Write Unit Tests:** Use NUnit and Moq to write tests covering:
    - CreateFlashcardsRequestValidator rules.
    - Service layer logic:
      - successful creation of flashcards.
      - a single test for handling validation failure (e.g., too long front).

## 9. Important notes:
- Authentication and authorization will be implemented later on. Assume the user is already authenticated and authorized. You can assume that the current UserId is 1.