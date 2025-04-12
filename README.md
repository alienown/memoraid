# Memoraid

Memoraid is a web-based flashcard platform designed to simplify and accelerate the creation of high-quality educational flashcards. Leveraging AI for rapid flashcard generation from user-supplied text, Memoraid also supports manual flashcard creation, review, editing, and deletion. It offers robust user account management and seamlessly integrates with a spaced repetition algorithm to optimize study sessions.

## Table of Contents

- [Tech Stack](#tech-stack)
- [Getting Started Locally](#getting-started-locally)
- [Available Scripts](#available-scripts)
- [Project Scope](#project-scope)
- [Project Status](#project-status)
- [License](#license)

## Tech Stack

**Frontend:**

- React 19 with Vite
- TypeScript 5
- Tailwind CSS 4
- shadcn/ui components

**Backend:**

- ASP NET Core Web API
- PostgreSQL
- Entity Framework Core for database access
- FluentValidation for request model validation
- OpenAI API integration via .NET SDK

## Getting Started Locally

### Running with Docker

TODO ...

### Running without Docker

Alternatively, you can run applications without Docker. To do that you need to install the following software and follow the setup instructions:

- Node.js (>= v22.11.0)
- .NET SDK 9.0
- PostgreSQL

#### Frontend Setup

1. Navigate to the [/src/app](./src/app) directory
2. Install dependencies:
   ```
   npm install
   ```
3. Run the development server:
   ```
   npm run dev
   ```

#### Backend Setup

1. Navigate to the [/src/api](./src/api) directory
2. Restore NuGet packages and build the project:
   ```
   dotnet restore
   dotnet build
   ```
3. Update your appsettings.json with Open AI API key
   ```
   {
    ...
    "OpenAIApiKey": "INSERT YOUR OPEN AI API KEY HERE",
    ...
   }
   ```
4. Run the API:
   ```
   dotnet run
   ```
5. Apply any pending Entity Framework migrations:
   ```
   dotnet ef database update
   ```

## Available Scripts

### Frontend

- `npm run dev` - Runs the app in development mode.
- `npm run build` - Builds the project for production.
- `npm run preview` - Runs the built app for preview.
- `npm run lint` - Lints the project files.

### Backend

- `dotnet build` - Builds the backend project.
- `dotnet run` - Runs the application.
- `dotnet ef database update` - Applies database migrations.

## Project Scope

- **AI-Driven Flashcard Generation:** Paste text to automatically generate flashcards (Front: max 500 characters, Back: max 200 characters) with options to accept, reject, or edit.
- **Manual Flashcard Creation & CRUD Operations:** Create, view, edit, and delete flashcards with proper validation.
- **Study Mode:** Utilize spaced repetition for flashcard review with performance ratings.
- **User Account Management:** Registration, login, password change, and account deletion.

## Project Status

This project is in early development stage with intention to develop MVP product with core functionalities.

## License

This project is licensed under the MIT License.
