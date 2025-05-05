# API Endpoint Implementation Plan: User Registration (POST /users/register)

## 1. Endpoint Overview
The user registration endpoint enables new users to create an account by providing a valid email and password. It ensures that the email is unique, the password is securely stored, and all validation rules are enforced.

## 2. Request Details
- HTTP Method: POST
- URL Structure: /users/register
- Parameters:
  - Required (in request body):
    - email: string, must be a valid email address.
    - password: string, cannot be null or empty.
- Request Body Example:
  {
    "email": "user@example.com",
    "password": "SecurePassword123"
  }

## 3. Types Used
- Request DTO: RegisterUserRequest defined in Memoraid.WebApi/Requests.
- Response DTO: Response (generic Response<T> if data is needed, or non-generic Response when no data is returned).

## 4. Data Flow
1. The API endpoint receives the POST /users/register request with the JSON payload.
2. The controller forwards the request to the user service.
3. The user service applies FluentValidation on the RegisterUserRequest.
4. If valid, the password hash with new user record is created in the database.
5. On success, the endpoint returns a 201 Created response with a corresponding Response body.
6. In case of errors (syntactic or business rule violations), the service throws validation exceptions handled by FluentValidationExceptionMiddleware.

## 5. Security Considerations
- Ensure all inputs are validated to prevent injection attacks.
- Hash passwords using a secure algorithm before storing them.
- Enforce HTTPS for all communications.
- Use proper error messages that do not reveal sensitive information.

## 6. Error Handling
- 400 Bad Request: Returned when the incoming JSON is syntactically incorrect.
- 422 Unprocessable Entity: Returned when business logic fails (e.g., email already exists).
- Use FluentValidationExceptionMiddleware to automatically handle and transform validation errors into a 422 status with detailed error messages.

## 7. Performance Considerations
- Optimize database queries using the built-in capabilities of EF Core.
- Encrypt and hash passwords asynchronously if needed.

## 8. Implementation Steps
1. Create a FluentValidation validator for RegisterUserRequest to enforce email format and password presence.
2. Develop or update the UserService to handle:
   - Validation of request via FluentValidator (field requireness, email format, email does not exist yet).
   - Encrypt/hash the provided password.
   - Insert the new user into the database.
3. Integrate the user service with the minimal API endpoint.
4. Ensure proper HTTP response codes are returned (201 on success, 400/422 on errors).
5. Write corresponding unit tests using NUnit and EF Core InMemory database.
6. Review for security best practices and update CI/CD pipelines if necessary.

