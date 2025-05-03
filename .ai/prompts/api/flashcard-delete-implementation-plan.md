# API Endpoint Implementation Plan: DELETE /flashcards/{id}

## 1. Endpoint Overview
This endpoint is designed to delete a flashcard by its unique identifier. It removes the flashcard record from the database if the flashcard exists and the user is authenticated and authorized to delete it.

## 2. Request Details
- HTTP Method: DELETE
- URL Structure: /flashcards/{id}
- Parameters:
  - Required: 
    - id (route parameter): The unique identifier of the flashcard to be deleted.
- Request Body: None

## 3. Types Used
- No new DTO is required as the flashcard id is provided via the route.
- Use the existing generic Response class for responses that do not carry any payload.

## 4. Data Flow
1. The API receives a DELETE request with a flashcard id.
2. The authentication middleware ensures the request is from an authenticated user. (to be implemented later - assume that user is authenticated and the userId = 1)
3. The controller/endpoint extracts the flashcard id from the route.
4. The service layer (new method DeleteFlashcardAsync in FlashcardService) is called:
   - Validate the id parameter to ensure it is a valid identifier (greater than 0).
   - Perform deletion, look for flashcard with the provided id that belongs to the authenticated user
5. On success, the API returns a 204 No Content response.

## 5. Security Considerations
- Authentication: Ensure the user is authenticated before processing the request. Respond with 401 if not. (to be implemented later - assume that user is authenticated and the userId = 1)
- Authorization: When deleting a flashcard, look for a flashcard with the provided id that belongs to the authenticated user.
- Input Validation: Validate the id parameter to ensure it is a valid identifier.

## 6. Error Handling
- 401 Unauthorized: If the user is not logged in.
- 404 Not Found: If no flashcard exists with the provided id or if the flashcard does not belong to the authenticated user.
- 500 Internal Server Error: For any unforeseen issues during deletion.

## 7. Performance Considerations
- Use a transactional operation to maintain database integrity.
- Leverage EF Core's AsNoTracking() for read-only operations where applicable.
- Optimize the deletion query to affect only the necessary row.

## 8. Implementation Steps
1. Extend the FlashcardService and add a new method named DeleteFlashcardAsync(long id) that:
   - Validates the id parameter to ensure it is a valid identifier (greater than 0).
   - Performs deletion looking for flashcard with the provided id that belongs to the authenticated user
2. Create or update the controller/endpoint to route DELETE /flashcards/{id}:
   - Extract the flashcard id from the route.
   - Invoke the DeleteFlashcardAsync method.
   - Return 204 No Content on success.
3. Ensure FluentValidation rules are applied at the service layer if needed (e.g., validating id is > 0).
4. Write unit tests in Memoraid.Tests.Unit to cover:
   - Successful deletion.
   - Deletion of non-existing flashcards (404).