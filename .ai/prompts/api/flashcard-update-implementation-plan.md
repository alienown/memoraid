# API Endpoint Implementation Plan: Update Flashcard

## 1. Endpoint Overview
Implement the PUT /flashcards/{id} endpoint to allow users to update the front and back content of an existing flashcard. The endpoint will validate input, confirm flashcard ownership, update the record, and return a 204 No Content response upon success.

## 2. Request Details
- **HTTP Method:** PUT
- **URL Structure:** /flashcards/{id}
- **Route Parameter:** 
  - id (long) – the unique identifier of the flashcard to update.
- **Request Body:**
  ```json
  {
    "front": "string, required, max 500 chars",
    "back": "string, required, max 200 chars"
  }
  ```
  *Note:* Although reference types in the request model remain optional in code, validation will enforce presence and maximum length.

## 3. Types Used
- **Request DTO:** Already exists: `UpdateFlashcardRequest`
- **Response:** Use a non-generic Response (which will have no data on success).
- **Entity:** Flashcard (from existing EF Core model)

## 4. Data Flow
1. The client calls PUT /flashcards/{id} with the request body.
2. The endpoint controller (or minimal API route handler) passes the id and request object to the FlashcardService.
3. The service validates the request using FluentValidation.
4. The service checks whether the flashcard exists and is owned by the current user.
5. If found, update the flashcard’s Front and Back fields (last modified on/by fields are already handled by EF Core interceptors - no need to set them explicitly), and save changes via EF Core.
6. Return a Response with an HTTP 204 status code.

## 5. Security Considerations
- **Authentication:** Ensure user is authenticated (e.g., via JWT) - this will be implemented later on. Assume user is authenticated for this task.
- **Authorization:** Validate that the flashcard belongs to the authenticated user (assume userId = 1).
- **Input Validation:** Use FluentValidation to enforce string lengths and required fields.

## 6. Error Handling
- **400 Bad Request:** For syntactic validation errors in the request.
- **401 Unauthorized:** If the user is not authenticated. (this will be implemented later on)
- **404 Not Found:** If the flashcard does not exist or is not owned by the user.
- **422 Unprocessable Entity:** For business logic violations (e.g., violation of maximum allowed character lengths).

## 7. Performance Considerations
- Use AsNoTracking() for read operations but ensure proper EF Core tracking for updates.
- Optimize the update operation by limiting the update to only the necessary fields.
- Rely on EF Core’s change tracking to minimize unnecessary database calls.

## 8. Implementation Steps
1. **Implement Fluent Validator:**
   - Create a FluentValidation validator for `UpdateFlashcardRequest` that checks:
     - The `front` property is not null and not empty and is ≤ 500 characters.
     - The `back` property is not null and not empty and is ≤ 200 characters.

2. **Extend FlashcardService:**
   - Add a new method `UpdateFlashcardAsync(UpdateFlashcardRequest request)`.
   - Within this method:
     - Validate the request using the validator.
     - Query the flashcard for the given id and current user.
     - If not found, return a Response with a constructed error (Error code e.g., `FlashcardNotFound`).
     - Update the flashcard fields.
     - Save changes to the database and return a Response with no content.

3. **Define the API Endpoint:**
   - In the minimal API routing configuration or controller, implement the PUT route.
   - Extract the id from the route, bind the request body to `UpdateFlashcardRequest`, and call the service’s update method.
   - Return an HTTP 204 No Content response when successful, handling error responses as needed.

4. **Testing:**
   - Write unit tests covering:
     - Successful update.
     - Update failure when flashcard is not found.
     - Validation failures for request body.
