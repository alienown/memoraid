# Memoraid Test Plan

## 1. Test Objectives
- Validate overall system functionality and business logic.
- Ensure reliability, stability, and performance meet MVP criteria.
- Confirm usability and accessibility of the user interface.
- Verify security and integration with key third-party services.
- Ensure efficient interaction between the React frontend, .NET API backend, and PostgreSQL database.

## 2. Scope of Testing
- **In Scope:**
  - Integration between services (authentication via Firebase, AI module via Open Router API).
  - API endpoints and business logic implemented in .NET.
  - UI and user flow validations in React components, routing and data fetching.
- **Out of Scope:**
  - Extensive load testing beyond MVP requirements.
  - Non-core features not directly affecting flashcard creation or review.
  - Detailed performance testing of third-party services provided as-is.

## 3. Functionalities to be Tested
- AI-Generated Flashcards: Validate copy-paste input, auto-generation, and review interface (accept, reject, edit) with proper validation.
- Manual Flashcard Operations: Test creation, listing, editing, and deletion with proper validations.
- User Account Management: Verify registration, login, password change, and secure account deletion.
- Study Mode Integration: Ensure flashcards are presented for study with reveal and rating mechanisms.
- Data Logging: Confirm that analytics data on flashcard acceptance and creation are accurately logged.

## 4. Assumptions and Constraints
- Development is following a minimal viable product (MVP) approach.
- Teams will be using predefined CI/CD pipelines utilizing GitHub Actions.
- Limited resources are available leading to focus on high-impact areas.
- Test environments will mimic production as closely as possible within constraints.
- Third-party integrations (Firebase, Open Router API) are considered stable.

## 5. Test Strategy
- **Risk-Based Approach:** Focus on critical workflows such as flashcard generation, authentication, and database operations.
- **Shift-Left Testing:** Begin testing early within the development cycle (unit tests, integration tests) to catch issues quickly.
- **Exploratory Testing:** Complement automated tests with exploratory sessions to surface unanticipated issues.
- Utilize automated regression suites alongside manual sanity checks for major releases.

## 6. Test Design and Coverage Approach
- **Requirements-Based Testing:** Derive test cases directly from functional and non-functional requirements.
- **Code-Path Analysis:** Use unit tests targeting core logic in business services and validators.
- **Model-Based Testing:** For complex interactions, particularly in the spaced repetition and flashcard generation algorithms.
- **Integration Testing:** For end-to-end scenarios involving multiple components (e.g., API to database, API to third-party services).
- Ensure high coverage on:
  - API input validations using FluentValidation.
  - Integration points: UI with backend, backend with database, backend with third-party services.
  - UI components and routing in the React application.

## 7. Environment and Infrastructure
- **Environments:**  
  - **Development:** Local developer environments with EF Core InMemory for unit tests.
  - **Staging/Pre-Prod:** Environment close to production for integration tests and user acceptance testing.
- **Test Data:**  
  - Use synthetic datasets to simulate real usage across API and UI layers.
  - Maintain separate test databases with controlled migrations.
- **Containerization:**  
  - Utilize Docker images for consistent deployment in CI/CD pipelines where applicable.

## 8. Test Tools and Frameworks
- **Unit & Integration Testing:** NUnit with Shouldly assertions. Docker containers for running services used in integration tests.
- **Mocking:** Moq for isolating dependencies during tests.
- **UI Testing:** React testing library & vitest for unit testing and React testing library & playwright for end-to-end testing.
- **API Testing:** Postman/Swagger for endpoint verification.
- **Continuous Integration:** GitHub Actions to run automated tests on every commit.

## 9. Entry and Exit Criteria
- **Entry Criteria:**  
  - Build is compiled with no critical errors.
  - Test environments are provisioned and stable.
  - All core functionality is feature complete.
  - Test data is set up and aligned with the MVP scope.
- **Exit Criteria:**  
  - All high-priority test cases pass.
  - No critical or high severity bugs remain open.
  - Regression test suite has been executed successfully.

## 10. Risk Assessment
- **Technical Risks:**  
  - Integration inconsistencies between the .NET API and PostgreSQL.
  - Edge cases in AI-generated flashcards not covered by automated tests.
  - Potential performance issues in high load scenarios.
- **Mitigation Strategies:**  
  - Early integration testing and gradual performance testing.
  - Monitoring and logging in production for unexpected behavior.
  - Regular review sessions with development and product teams to reprioritize testing focus.

## 11. Test Schedule and Resource Planning
- **Timeline:**  
  - Initial unit and integration tests: Ongoing during development.
  - Regression testing: Prior to every release milestone.
  - Final end-to-end testing: In the staging environment before every release.
- **Resources:**  
  - Use of CI pipelines to maximize test efficiency and reduce manual overhead.

## 12. Test Scenarios

### 12.1 Registration and Login
1. Registration:
   - Validate the registration form collects a valid email and password.
   - Confirm error messages display for invalid email formats or missing password.
   - Verify successful registration redirects to /generate page.
2. Login:
    - Test login with valid credentials and ensure redirection to /generate.
    - Validate error messages for incorrect credentials.
    - Confirm that the user cannot access flashcard generation without logging in.

### 12.2 AI-Generated Flashcards
1. Flashcard Generation:
   - Paste a valid block of text (up to 10000 characters) and trigger AI generation.
   - Assert that each generated flashcard complies with front (max 500 chars) and back (max 200 chars) constraints.
2. Review and Editing:
   - Verify inline Accept/Reject functionality for each flashcard.
   - Ensure editing via the modal updates flashcard details correctly.
   - Re-validate flashcard input constraints during the edit process.
3. Bulk Submission:
   - Test that clicking "Submit flashcards" saves all accepted flashcards.

### 12.3 Manual Flashcard Operations
1. Creation:
   - Validate the manual creation modal enforces required fields and character limits for "Front" and "Back".
   - Confirm that a new flashcard appears immediately upon creation.
2. Editing:
   - Verify that editing an existing flashcard reflects updated content.
   - Re-validate input constraints during the edit process.
3. Deletion:
   - Ensure deletion prompts a confirmation dialog before removal.
   - Confirm that the flashcard is properly removed from the UI and backend.

### 12.4 API Endpoints
1. GET /flashcards:
   - Retrieve paginated flashcards; validate default pageNumber (1) and pageSize (10) with upper limit (50).
   - Verify that only flashcards owned by the authenticated user are returned.
2. POST /flashcards:
   - Test submissions for both manual and AI-generated flashcards.
   - Enforce rules: AI flashcards require a generationId, while manual entries must have it as null. Check required fields and character limits.
3. PUT /flashcards/{id}:
   - Validate updating content respects field requireness and character limits with proper error (422) for violations.
   - Confirm that 404 errors occur when attempting to update nonexistent or unauthorized flashcards.
   - Verify that only flashcards owned by the authenticated can be updated.
4. DELETE /flashcards/{id}:
   - Test deletion endpoint, ensuring a flashcard is removed and proper HTTP status codes are returned.
   - Verify that only flashcards owned by the authenticated can be deleted.
5. Account Management Endpoints:
   - For POST /users, validate that email and password are required and email must be unique.
   - For POST /users/login, ensure correct credentials return a valid token and 200 status.

### 12.5 Error and Edge Cases
1. Input Violations:
   - Test scenarios with inputs exceeding maximum character limits in both AI-generated and manual flashcards.
   - Verify that violation triggers proper error responses (e.g., 422) and displays corresponding error messages.
2. Unauthorized Access:
   - Validate that API endpoints return 401 status for unauthenticated requests.
   - Confirm that users cannot access or modify flashcards they do not own.

# Conclusion
This test plan focuses on ensuring that the critical aspects of Memoraid are thoroughly validated while balancing the constraints of the MVP stage. The plan is adaptable to evolving requirements and emphasizes risk-based, automated, and exploratory testing practices.
