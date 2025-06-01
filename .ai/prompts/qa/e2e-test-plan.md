# Memoraid E2E Test Plan (MVP)

This plan focuses on verifying the core user stories using Playwright with a Page Object Model approach.

## 1. Test Framework
- Utilize Playwright for automation.
- Structure tests using the Page Object Model for maintainability.
- Keep tests concise—cover key functionalities without exhaustive edge-case coverage.

## 2. Key Scenarios

### 2.1 Login
- Verify login form accepts a valid email and password.
- Test login flow with valid credentials and redirection to the flashcard generation page.

### 2.2 AI-Generated Flashcards
- Input a valid block of text and trigger AI-driven flashcard generation.
- Confirm flashcards are displayed with front-content. Check if back content shows on user interaction.
- Check inline acceptance and editing using modal dialogs.
- Bulk submit accepted flashcards.
- Check if accepted flashcards are visible in the flashcard list.

### 2.3 Manual Flashcard Operations
- Test creation of a new flashcard via the manual entry modal.
- Verify that editing via edit modal reflects updates immediately.
- Check deletion prompts confirmation and successfully removes a flashcard.

## 3. Page Object Model Structure
- Create separate page objects for each major view:
  - LoginPage
  - RegistrationPage
  - FlashcardGenerationPage (includes AI-generation flows)
  - FlashcardModal (for manual creation/editing)
- Reuse objects across tests to reduce duplication and ease maintenance.

## 4. Test Execution
- Define clear Arrange/Act/Assert steps inside each test.
- Focus on critical paths to ensure quick feedback during MVP development.
- Use test user credentials for authentication so that there is no need to create new users in each test run:
  - Email: accessed via `process.env.TEST_USER_EMAIL`
  - Password: accessed via `process.env.TEST_USER_PASSWORD`