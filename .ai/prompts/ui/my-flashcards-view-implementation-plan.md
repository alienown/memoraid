# View Implementation Plan: My Flashcards

## 1. Overview
Create a "My Flashcards" view that allows authenticated users to manage their own flashcards. This view displays a paginated list of flashcards (showing the front by default with an option to reveal the back) and offers actions to create, edit, or delete a flashcard.

## 2. View Routing
- Route: `/flashcards`

## 3. Component Structure
- **MyFlashcardsPage** (container page)
  - **FlashcardsList** (list/grid of flashcard cards)
    - **FlashcardListItem** (individual card component)  
  - **Pagination** (navigates pages)
  - **CreateFlashcardModal** (modal for creating a new flashcard)
  - **EditFlashcardModal** (modal for editing an existing flashcard)
  - **ConfirmationDialog** (for delete confirmation)

## 4. Component Details
### MyFlashcardsPage
- **Description:** Main page container that loads and displays user flashcards; manages create/edit/delete actions and pagination.
- **Main Elements:** Header with "Create" button, list of flashcards, pagination control.
- **Events Handled:**  
  - Page load (fetch flashcards via GET API)  
  - Open create modal  
  - Open edit modal (when user clicks edit on a card)  
  - Launch delete confirmation and remove flashcard via DELETE API   
- **Types:**
  - Uses `GetFlashcardsResponse` from the API
- **Props:** None; it manages its own state.

### FlashcardsList
- **Description:** Displays a list of flashcards.
- **Main Elements:** List of `FlashcardListItem` components.
- **Events Handled:** None directly; it receives flashcards as props and renders them.
- **Types:** `FlashcardListProps`
- **Props:**
  - flashcards: FlashcardListItem[] (array of flashcard data)
  - onEdit: (id: number) => void (callback for edit action)
  - onDelete: (id: number) => void (callback for delete action)

### FlashcardListItem
- **Description:** Displays an individual flashcard showing “front” text; toggles to reveal “back” text on user action.
- **Main Elements:** Card header (with action buttons: edit, delete, toggle answer), content section.
- **Events Handled:**  
  - Click to toggle back visibility  
  - Edit action button clicks  
  - Delete action button clicks  
- **Types:**  
  - `FlashcardListItemProps`  
- **Props:** 
  - front: string
  - back: string
  - onEdit: () => void  
  - onDelete: () => void  

### CreateFlashcardModal
- **Description:** Modal to create a flashcard.
- **Main Elements:** Form with text areas for front and back.
- **Events:** 
  - On input change.
  - On submit, validate fields and if valid run onSave callback passing flashcard data to it
- **Validation:** Front: max 500 chars, Back: max 200 chars, both required.
- **Types:** `CreateFlashcardData` (contains front and back fields).
- **Props:**:
  - isOpen: boolean
  - onSave callback
  - onCancel callback

### EditFlashcardModal
- **Description:** Modal to modify a chosen flashcard.
- **Main Elements:** Form with text areas for front and back.
- **Events:** 
  - On input change.
  - On submit, validate fields and if valid run onSave callback passing updated data to it
- **Validation:** Front: max 500 chars, Back: max 200 chars, both required.
- **Types:** `EditFlashcardData` (contains front and back fields).
- **Props:**:
  - isOpen: boolean
  - Flashcard to edit
  - onSave callback
  - onCancel callback

### Pagination
- **Description:** Component to navigate through pages of flashcards.
- **Main Elements:** Page number display, next/previous buttons.
- **Events Handled:**
  - Change of page selection triggers new API requests.
- **Types:**
  - PaginationData (currentPage: number, totalItems: number, pageSize: number)
- **Props:**
  - currentPage: number
  - totalItems: number
  - pageSize: number
  - onPageChange: (page: number) => void

### ConfirmationDialog
- **Description:** Reusable dialog to confirm actions such as deletion.
- **Main Elements:** Message text, Confirm and Cancel buttons.
- **Events Handled:**  
  - Confirm action
- **Props:**  
  - isOpen: boolean
  - message: string  
  - onConfirm: () => void  
  - onCancel: () => void

## 5. Types
- **ResponseOfGetFlashcardsResponse** (result of fetching flashcards - taken from `api.ts`)
  - data: GetFlashcardsResponse;
  - isSuccess: boolean;
  - errors: Error[];
- **GetFlashcardsResponse:** (data field of `ResponseOfGetFlashcardsResponse` from `api.ts`)
  - items: FlashcardsListItem[]
  - total: number
- **FlashcardListProps** (for FlashcardsList component)
  - flashcards: FlashcardListItem[] (array of flashcard data)
  - onEdit: (id: number) => void (callback for edit action)
  - onDelete: (id: number) => void (callback for delete action)
- **FlashcardListItemProps** (for FlashcardListItem component)
  - front: string (max 500)
  - back: string (max 200)
  - onEdit: () => void
  - onDelete: () => void
- **CreateFlashcardData:** (for creation modal)
  - front: string (required, cannot exceed 500 characters)
  - back: string (required, cannot excee 200 characters)
- **CreateFlashcardModalProps:** (for creation modal)
  - isOpen: boolean (required)
  - onSave: (data: CreateFlashcardData) => void (required)
  - onCancel: () => void (required)
- **CreateFlashcardsRequest:** (for sending API flashcard creation request - type taken from `api.ts`)  
  - front: string (required, cannot exceed 500 characters)
  - back: string (required, cannot excee 200 characters)
  - source: "Manual" (required)
  - generationId: null (required)
- **EditFlashcardData:** (for edit modal)
  - front: string (required, cannot exceed 500 characters)
  - back: string (required, cannot exceed 200 characters)
- **EditFlashcardModalProps:** (for edit modal)
  - isOpen: boolean (required)
  - flashcard: EditFlashcardData (required)
  - onSave: (data: EditFlashcardData) => void (required) (callback contains updated data)
  - onCancel: () => void (required)
- **UpdateFlashcardRequest:** (for sending API flashcard update request - type taken from `api.ts`)  
  - front: string (required, cannot exceed 500 characters)
  - back: string (required, cannot excee 200 characters)

## 6. State Management
- Local state in MyFlashcardsPage for:
  - flashcards: `GetFlashcardsResponse`
  - loading and error flags
  - current page number and total items (for pagination)
  - Modal open state for creation and editing
  - Selected flashcard for editing
- Custom hooks may be introduced for flashcards fetching and pagination management.

## 7. API Integration
- **GET /flashcards:**  
  - Request: Pass optional query params (pageNumber, pageSize)  
  - Response: `ResponseOfGetFlashcardsResponse` with flashcards list and total count  
- **POST /flashcards:**  
  - Request: `CreateFlashcardsRequest` (for manual flashcards: source must be "Manual" and generationId null)  
  - Response: 201 Created on success  
- **PUT /flashcards/{id}:**  
  - Request: UpdateFlashcardRequest (with front and back)  
  - Response: 204 No Content  
- **DELETE /flashcards/{id}:**  
  - Response: 204 No Content  
- Use the provided API client (see api.ts) for each operation with appropriate DTO types.

## 8. User Interactions
- **Viewing Flashcards:**  
  - On page load, flashcards are fetched and displayed as cards showing “front”.  
  - Clicking a flashcard toggles display of the “back”.
- **Creating a Flashcard:**  
  - A "Create" button opens the creation modal.  
  - Submission validates input and calls the POST endpoint. On success a toast is shown and the list is refreshed.
- **Editing a Flashcard:**  
  - Clicking the edit button on a card triggers the edit modal.  
  - Upon submission, the PUT endpoint is called. On success a toast is shown and the list is refreshed.
- **Deleting a Flashcard:**  
  - Clicking the delete button prompts a confirmation dialog.  
  - Upon confirmation, the DELETE endpoint is invoked. On success a toast is shown and the list is refreshed.
- **Pagination:**
  - Changing pages triggers re-fetch of flashcards for the selected page.

## 9. Conditions and Validation
- **Flashcard Field Validation:**  
  - “Front” must be non-empty and not exceed 500 characters  
  - “Back” must be non-empty and not exceed 200 characters  
- **API Conditions:**  
  - For POST: Ensure generationId is null and source is "Manual"
- **UI Validation:** Show appropriate inline field error messages if validation fails on submission.

## 10. Handling Errors
- Display toast notifications for API error responses (e.g., unauthorized, validation failures).
- Show in-page error messages for non-critical errors.
- Use a confirmation dialog to avoid accidental deletion.
- Handle network errors and display a user-friendly message.

## 11. Implementation Steps
1. Create the MyFlashcardsPage component in the `/src/app/src/pages/flashcards` folder.
2. Integrate the API client to fetch flashcards on component mount and on page change.
3. Implement the FlashcardsList view using the FlashcardListItem component. Both of these components are already used in `/generate` view. Check if it is reasonable to move them to shared catalog and reusing them.
4. Add a Pagination component at the bottom of the view; handle page navigation.
5. Implement the CreateFlashcardModal with validation and submission logic; on success, refresh the flashcards list and show a success toast.
6. Implement EditFlashcardModal for editing. This component already exists in `/generate` view. Check if it is reasonable to move it to shared catalog and reusing it. Refresh the list and show a success toast on successful edit.
7. Implement delete functionality with a ConfirmationDialog before calling DELETE API. ConfirmationDialog should be a shared component.
8. Incorporate toast notifications for success and error messaging.
9. Verify overall integration with the existing shared components and ensure no breaking changes were introduced to the Generate view. If there are any, fix them.