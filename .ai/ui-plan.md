# UI Architecture for Memoraid

## 1. UI Framework Overview
A modular React-based UI utilizing functional components, hooks, shadcn/ui for reusable elements, and Tailwind CSS for coherent styling. Emphasis is placed on accessibility, responsive design, and secure user interactions.

## 2. List of Views

- **Register View**
  - **View Path:** /register
  - **Primary Purpose:** Enable user registration so that they can later log in and use authorized features of the app.
  - **Key Information to Display:** Email/password fields, validation messages, and submit action feedback.
  - **Key View Components:** Input fields, Button, Form, Toast notifications for success/error messages.
  - **UX, Accessibility, and Security Considerations:** Inline field validations, accessible labels, HTTPS secure submissions.

- **Login View**
  - **View Path:** /login
  - **Primary Purpose:** Enable user login.
  - **Key Information to Display:** Email/password fields, validation messages, and submit action feedback.
  - **Key View Components:** Input fields, Button, Form, Toast notifications for success/error messages.
  - **UX, Accessibility, and Security Considerations:** Clear form validations, accessible labels, HTTPS secure submissions.

- **Flashcard Generation**
  - **View Path:** /generate
  - **Primary Purpose:** Allow users to paste text (up to 10000 characters) and generate flashcards via AI.
  - **Key Information to Display:** Large text input, dynamically generated flashcards with “Front” and “Back” fields, and possibility to inline Accept/Reject/Edit options. Edit option opens a Modal that contains form with two fields "front" (max 500 characters) and "back" (max 200 characters). One button "Submit flashcards" to bulk save all accepted flashcards.
  - **Key View Components:** TextArea for pasting text, Cards for generated flashcards, Inline Accept, Reject, and "Submit flashcards" Buttons, Modal for editing.
  - **UX, Accessibility, and Security Considerations:** Inline error messaging, keyboard-accessible modals, visual feedback and accessible controls, Toast notifications for success/error messages.

- **My Flashcards**
  - **View Path:** /flashcards
  - **Primary Purpose:** Manage user-owned flashcards via CRUD operations.
  - **Key Information to Display:** Paginated list of flashcards Card components with "front" and "back" fields, plus action buttons for editing and deletion of each flashcard. Also includes a button to manually create new flashcards which opens a modal with a form to create a new flashcard with "front" (max 500 characters) and "back" (max 200 characters) fields.
  - **Key View Components:** Card, Pagination, Action Buttons (Create, Edit, Delete), Confirmation Prompts, Creation Modal, Toast notifications for success/error messages.
  - **UX, Accessibility, and Security Considerations:** Accessible layouts, clear interactive elements with focus states, confirmation dialogs for destructive actions.

- **Study Mode**
  - **View Path:** /study
  - **Primary Purpose:** Support spaced repetition study sessions by showing flashcards and gathering user rating feedback.
  - **Key Information to Display:** Flashcard (initially with only the Front), interactive mechanism to reveal the Back and record user rating.
  - **Key View Components:** Cards for flashcards, Reveal/Rating Buttons, Progress Indicator.
  - **UX, Accessibility, and Security Considerations:** Responsive design, clear call-to-action for revealing answers, keyboard navigability and accessible rating controls.

- **Account Management**
  - **View Path:** /account
  - **Primary Purpose:** Manage user account details including password change and secure deletion.
  - **Key Information to Display:** Forms for password updates (provide current and new password), secure account deletion with re-authentication prompt.
  - **Key View Components:** Form Inputs, Modal dialogs for deletion confirmation, Buttons.
  - **UX, Accessibility, and Security Considerations:** Masked password inputs, accessible error messages, secure prompts with explicit user consent, Toast notifications for success/error messages.

## 3. User Journey Map
1. **Entry Point:** User accesses the application.
   - If unauthenticated, redirected to /login.
2. **Authentication:** User registers/logs in via the Register/Login views.
3. **AI Flashcard Generation:** Authenticated users are automatically redirected to /generate.
   - Paste text → AI generates flashcards → User reviews and accepts/rejects/edits flashcards. Then clicks "Submit flashcards" to save accepted ones in bulk.
4. **Flashcard Management:** User enters /flashcards to view all of his saved flashcards.
   - Users visit /flashcards to create, view, edit, or delete flashcards.
5. **Study Mode:** User enters /study to review saved flashcards using spaced repetition.
   - Reveal answers and rate his performance on each flashcard.
6. **Account Updates:** User accesses /account to change password or delete account.

## 4. Navigation Layout and Structure
A universal layout with a sticky top navigation bar is shared across all views. Navigation links include:
- Generate Flashcards (/generate)
- My Flashcards (/flashcards)
- Study Mode (/study)
- Account Management (/account)
- Login (/login - only for unauthenticated users)
- Register (/register - only for unauthenticated users)

## 5. Key Components
- **RegisterForm**: Provides inputs for email and password, with validation and submission feedback.
- **LogInForm**: Provides inputs for email and password, with validation and submission feedback.
- **TextArea & Input:** Reusable input components with inline error handling.
- **Button:** Reusable button component with different styles for primary, secondary, and destructive actions.
- **Flashcard:** Displays flashcard content as a card with front and back sides (with a flip animation upon back field reveal).
- **Modal:** Centralized, accessible modal for flashcard editing and account actions with ability to display different content based on context.
- **EditFlashcardModal:** Specific modal for editing flashcards with form fields for front and back content.
- **CreateFlashcardModal:** Modal for creating new flashcards with form fields for front and back content.
- **FlashcardsList:** Manages paginated display of flashcards in the My Flashcards view.
- **LoadingIndicator:** Integrated within actionable components to indicate processing states.
- **ToastNotification (Sonner):** Global notifications for success and error messages across views.