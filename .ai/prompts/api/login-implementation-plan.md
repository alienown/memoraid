# API Endpoint Implementation Plan: POST /users/login

## 1. Endpoint Overview
This endpoint authenticates a user by verifying their email and password and returns a JWT token. It is designed to support secure login, ensuring that credentials are validated both syntactically and business-wise before generating a token.

## 2. Request Details
- **HTTP Method:** POST
- **URL Structure:** /users/login
- **Parameters:**
  - **Required:** 
    - email (string) – must be a valid email address
    - password (string) – non-empty
  - **Optional:** (None; the request class properties are optional but will be validated at the service level.)
- **Request Body:** A JSON object corresponding to the LoginUserRequest:
  ```json
  {
    "email": "user@example.com",
    "password": "securepassword"
  }
  ```

## 3. Types Used
- **Request DTO:** LoginUserRequest
- **Response DTO:** LoginUserResponse (embedded in Response<LoginUserResponse>)
- **Wrapper:** Response<T> for success and non-generic Response for no-data responses
- **Validation:** A new FluentValidator for LoginUserRequest will be implemented to enforce required and formatted email and non-empty password.

## 4. Data Flow
1. The client POSTs credentials to /users/login.
2. The minimal API endpoint receives the data and passes it to the service layer.
3. The LoginUserRequest is validated using FluentValidation. If validation fails, a 422 status is returned.
4. The service queries the Users table using the provided email.
5. If the user is not found, a 401 Unauthorized is returned.
6. If the user is found, the provided password is verified against the stored hashed password.
7. If the credentials match, a JWT is generated and wrapped in a LoginUserResponse.
8. The endpoint returns a 200 OK response with the JWT. Otherwise, a 401 Unauthorized is returned for invalid credentials.

## 5. Security Considerations
- **Password Verification:** Use secure hash comparison to verify passwords.
- **JWT Generation:** Ensure that the JWT signing key is securely stored and accessed.
- **Error Messages:** Do not reveal specific authentication failure reasons.
- **Input Sanitization:** Use FluentValidation at the service level to prevent injection attacks.

## 6. Error Handling
- **400 (Bad Request):** Returned for syntactic or malformed JSON payloads.
- **401 (Unauthorized):** Returned if the credentials (email/password) do not match.
- **422 (Unprocessable Entity):** Returned from FluentValidation when business rules or required formats are not met.

## 7. Performance Considerations
- Use asynchronous calls for database access.
- Only retrieve the user record necessary for authentication.
- Consider caching public keys or configuration for JWT generation to reduce overhead.

## 8. Implementation Steps
1. **Create Validator:**  
   - Implement a FluentValidator for LoginUserRequest to ensure email and password are provided and valid.
2. **Extend Service Layer:**  
   - Create or extend a method (e.g., LoginUserAsync) in the UserService to:
     - Retrieve the user by email.
     - Verify the password using the stored salted hash.
     - Generate a JWT upon successful authentication.
3. **Configure JWT Provider:**  
   - Set up JWT token generation using the recommended ASP.NET Core minimal APIs approach and secure key management. See official docs: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/security?view=aspnetcore-9.0
4. **Implement Endpoint:**  
   - Create a minimal API endpoint (in Program.cs or a dedicated endpoints file) listening on POST /users/login.
   - Inject the authentication service and validator.
   - Handle responses according to the outlined status codes.
5. **Testing:**
   - Write unit tests covering all scenarios for:
      - Validator to ensure correct validation logic.
      - Service method to ensure correct user retrieval and password verification.