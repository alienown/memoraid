# Product Requirements Document (PRD) - Memoraid

## 1. Product Overview
Memoraid is a web-based flashcard platform designed to simplify and accelerate the creation of high-quality educational flashcards. The application leverages AI to automatically generate flashcards from user-supplied text using a dedicated AI-generation view while providing a separate "My Flashcards" view for manual creation, review, editing, and deleting. The system also includes user account management and integrates with an existing spaced repetition algorithm for effective study sessions.

## 2. User Problem
Creating educational flashcards manually is a time-consuming process. Users who wish to benefit from spaced repetition are often discouraged by the effort required to manually produce high-quality flashcards. Memoraid addresses this pain point by automating a large portion of the flashcard creation process while still providing the flexibility for manual input and corrections.

## 3. Functional Requirements
1. AI-Generated Flashcards
   - Provide a dedicated view where authenticated users can copy-paste large blocks of text (maximum 10000 characters).
   - Automatically generate flashcards from the text. Each flashcard comprises two fields:
     - Front: Required, maximum 500 characters.
     - Back: Required, maximum 200 characters.
   - Present generated flashcards in an AI-generation review interface with options to accept, reject, or modify (via an edit modal).

2. Manual Flashcard Creation and CRUD Operations
   - Provide a separate "My Flashcards" view for authenticated users to view and manage their flashcards.
   - Allow users to create, view, edit, and delete flashcards they own.
   - Enforce validation rules (fields required and character limits) on both the frontend and backend.

3. User Account Management
   - Implement email and password registration and login.
   - Enable password changes (requires entry of the current password).
   - Provide secure account deletion that includes reauthentication and a confirmation prompt.

4. Study Mode Integration
   - Integrate the flashcards with an external spaced repetition algorithm.
   - Present flashcards in study mode by showing only the "Front" and revealing the "Back” upon user interaction.
   - Allow users to rate how well they remembered the answer to adjust future study sessions.

5. Data Logging for Analytics
   - Track metrics such as:
     - The percentage of AI-generated flashcards accepted by users.
     - The percentage of total flashcards created using AI.
   - Store these logs in a database for ongoing performance and usage analysis.

6. Authentication and Authorization
   - All flashcard functionalities (AI-generation and CRUD operations) are available only to authenticated users.
   - Users can only view, edit, or delete flashcards that they own.

## 4. Product Boundaries
Included in MVP:
- AI-driven flashcard generation with copy-paste functionality.
- Manual creation and complete CRUD operations for flashcards.
- Flashcard review interface with inline accepting and rejecting and a modal for flashcard modifications.
- Basic user account management (registration, login, password change, secure account deletion).
- Integration with an existing spaced repetition algorithm for study mode.

Not included in MVP:
- Development of a custom advanced spaced repetition algorithm (e.g., SuperMemo, Anki).
- Support for importing multiple document formats (PDF, DOCX, etc.).
- Facilitating sharing of flashcard sets between users.
- Integration with external educational platforms.
- Mobile applications (initially web-only).

## 5. User Stories

### US-001: User Registration and Login
- Description: As a new user, I want to register with my email and password so that I can securely access my flashcards.
- Acceptance Criteria:
  - The registration form collects an email and password.
  - Both fields are validated for requireness.
  - Email format is validated.
  - User data is securely stored.
  - A login form allows access upon successful registration.

### US-002: AI Flashcard Generation
- Description: As a logged in user, I want to paste input text and have the application generate flashcards automatically, so that I can save time.
- Acceptance Criteria:
  - Users can paste a block of text of maximum 10000 characters into a dedicated input field.
  - The system generates flashcards with a "Front" (max 500 characters) and "Back" (max 200 characters).
  - Generated flashcards are listed with options to review them.

### US-003: Review and Acceptance of AI-Generated Flashcards
- Description: As a logged in user, I want to review AI-generated flashcards and either accept, reject, or edit them before saving.
- Acceptance Criteria:
  - A review interface presents all AI-generated flashcards.
  - Each flashcard can be accepted or rejected inline or modified using an edit modal.
  - Only accepted flashcards are saved to the user's account when confirmed.

### US-004: Viewing Flashcards
- Description: As a logged in user, I want to view a list of only my flashcards so that I can manage my study material securely.
- Acceptance Criteria:
  - Users can see a paginated list or grid of flashcards that they own.
  - Each flashcard displays the "Front" by default.
  - Clicking a flashcard reveals the "Back".
  - Each flashcard has options to edit or delete it.
  - The view consists of a button to create a new flashcard, which opens the manual creation form.

### US-005: Manual Flashcard Creation
- Description: As a logged in user, I want to manually create flashcards so that I can add content not covered by AI.
- Acceptance Criteria:
  - User flashcards list includes a button to create a new flashcard.
  - A form allows manual entry of flashcard "Front" (max 500 characters) and "Back" (max 200 characters). Both fields are required.
  - Flashcards are saved and immediately visible in the user's flashcard list.

### US-006: Edit Flashcards
- Description: As a logged in user, I want to edit my flashcards so that I can correct errors or update information.
- Acceptance Criteria:
  - User flashcards list includes a button to edit the flashcard.
  - Users can open an edit modal or inline editor for a flashcard.
  - A form allows manual entry of flashcard "Front" (max 500 characters) and "Back" (max 200 characters). Both fields are required.
  - Changes are saved and reflected in the flashcard list.

### US-007: Delete Flashcards
- Description: As a logged in user, I want to delete my flashcards so that I can remove outdated or incorrect cards.
- Acceptance Criteria:
  - Each flashcard on the user flashcards list has a delete option.
  - Deletion is confirmed by a prompt before the flashcard is permanently removed.
  - The flashcard is removed from the display list upon successful deletion.

### US-008: Study Mode Integration
- Description: As a logged in user, I want to study my flashcards using a spaced repetition system so that I can efficiently retain knowledge.
- Acceptance Criteria:
  - Study mode view presents flashcards based on the spaced repetition algorithm.
  - The "Front" is shown first; the "Back" is revealed upon user action.
  - Users can rate their recall performance, which is logged to adjust future study sessions.
  - User can exit study mode any time and return to the home screen.

### US-009: Password Change
- Description: As a logged in user, I want to change my password securely so that I can maintain my account security.
- Acceptance Criteria:
  - A password change form that requires entering the current password and a new password.
  - The system validates the current password before allowing the change.

### US-010: Secure Account Deletion
- Description: As a logged in user, I want to delete my account securely so that I can safely remove my data if I choose to stop using the application.
- Acceptance Criteria:
  - The account deletion process requires reauthentication (e.g., entering the password again).
  - A confirmation prompt is presented to prevent accidental deletion.
  - Upon confirmation, all user data (flashcards) is removed.

## 6. Success Metrics
1. AI Flashcard Acceptance Rate
   - 75% or more of AI-generated flashcards are accepted by users.
2. AI Usage in Flashcard Creation
   - At least 75% of flashcards saved by users are generated via AI.