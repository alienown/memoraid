# View Implementation Plan: Login View

## 1. Overview
This view enables users to log in by providing their email and password. It displays a couple of input fields with inline validation and provides feedback using toast notifications. The component securely submits the credentials to retrieve a JWT token and, on success, redirects the user to the flashcard generation view.

## 2. View Routing
- The view will be accessible at the path: `/login`.

## 3. Component Structure
- **Login** (Single Component): A consolidated component that includes:
  - Input fields for email and password.
  - A submit button to initiate login.
  - Inline field validation messages.
  - Toast notifications for API feedback.
  - Handling of loading state and redirection upon successful login.

## 4. Component Details
### Login
- **Description**: A self-contained component managing user input, validation, and API communication for login.
- **Main Elements**:
  - HTML form with email and password Input shadcn components.
  - A shadcn Button component for form submission.
  - A sonner shadcn component for displaying success/error notifications:
- **Events Handled**:
  - OnChange events for inputs to update fields state.
  - OnSubmit event for form submission triggering field validation and the API call.
- **Validation Conditions**:
  - Email: Must be provided and match a valid email format.
  - Password: Must be provided.
- **Types**:
  - Uses the `LoginUserRequest` DTO for API calls and the `LoginUserResponse` DTO for responses.
  - Local state ViewModel includes:
    - email: string
    - password: string
    - emailError: string | undefined
    - passwordError: string | undefined
    - isSubmitting: boolean
- **Props**: As a top-level component, it does not require external props

### Sonner (Toast)
- **Component Description:** A third-party library for displaying toast notifications.
- **Example usage:**: 
  - toast.success(`Generated ${flashcards.length} flashcards successfully!`);
  - toast.error("Failed to connect to the server. Please try again later.");

## 5. Types
- **LoginUserRequest (DTO)**:
  - email: string (required; valid email format)
  - password: string (required)
- **LoginUserResponse (DTO)**:
  - token: string
- **Local ViewModel**:
  - email: string
  - password: string
  - errors: { email?: string; password?: string }
  - isSubmitting: boolean

## 6. State Management
- Use React's useState hook to manage email, password, validation errors, and submission state.
- Optionally, encapsulate login logic in a custom hook (e.g., useLogin) within the same file if it simplifies the component.

## 7. API Integration
- Use the provided API client from `api.ts` to call the `loginUser` endpoint.
- Request:
  - Type: `LoginUserRequest`
- Expected Response:
  - Type: `ResponseOfLoginUserResponse` containing the JWT token.
- The API call is triggered on form submission

## 8. User Interactions
- User fills in email and password.
- On form submission:
  - Validate both fields.
  - If valid, disable the submit button and indicate loading.
  - Call the login API; on success, redirect to the flashcard generation view.
  - On failure, display a general error message (for error without propertyName) via toast notification and/or specific field errors for field errors.

## 9. Conditions and Validation
- Ensure email and password fields are not empty.
- Email input type should be set to "email" for proper validation. Other than that no additional email validation needed.
- Inline validation errors are displayed near the corresponding input.
- Submission triggers validation for both fields.

## 10. Handling Errors
- API errors are caught and displayed using toast notifications.
- Generic fallback toast error message for network errors.
- Maintain field error state to highlight specific invalid fields.

## 11. Implementation steps
1. Create the `Login` component as a single functional component.
2. Implement local state using useState for email, password, errors, and submission state.
3. Define input handlers to update state on change.
4. Implement the onSubmit handler to trigger validation and call the `loginUser` API and handle success and error flows.
5. Integrate toast notifications to provide user feedback.
6. On successful authentication, extract the JWT token and don't do anything with it. Just redirect to the flashcard generation view. Storing JWT token will be implemented later on.
7. Apply Tailwind styles to manage the layout and presentation, ensuring accessibility standards are met.
