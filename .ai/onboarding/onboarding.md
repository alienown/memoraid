# Project Onboarding: Memoraid

## Welcome

Welcome to the Memoraid project! Memoraid is an AI-powered web application for creating and studying flashcards efficiently. This platform simplifies and accelerates the creation of educational flashcards by leveraging AI to automatically generate high-quality content from user-supplied text.

## Project Overview & Structure

The project is a monorepo containing the backend API, a frontend web application, and database management scripts. The entire stack is orchestrated using Docker, as defined in the `docker-compose.yaml` file.

The core functionality revolves around AI-powered flashcard generation and management. The project is organized with the following key components:

-   **`src/api`**: A .NET/C# solution containing the backend REST API (`Memoraid.WebApi`) and its corresponding unit tests (`Memoraid.Tests.Unit`). It handles all business logic and data persistence.
-   **`src/app`**: A TypeScript-based React single-page application built with Vite. It provides the user interface for interacting with the flashcard data.
-   **`src/database`**: Contains Dockerized SQL migration and data seeding scripts.
-   **`docker-compose.yaml`**: The central file for running the entire application stack (backend, frontend, and database) in a containerized environment.

## Core Modules

### `src/api/Memoraid.WebApi` (Backend)

-   **Role:** As the .NET backend API, this module is the core of the application. It handles all business logic, manages data persistence through a database, and integrates with the external Open Router AI service for flashcard generation.
-   **Structure & Key Files:** The API is built around key services, structured request/response models, and a central `Program.cs` for configuration.
    -   `Program.cs`: Entry point, service registration, and middleware configuration.
    -   `Services/`: Contains the core business logic (e.g., `FlashcardService`, `OpenRouterService`).
    -   `Persistence/`: Handles database interactions using Entity Framework Core.
    -   `Requests/` & `Responses/`: Defines the API's data contracts.
    -   `Memoraid.WebApi.csproj`: Lists project dependencies like `FluentValidation`, `Npgsql` (PostgreSQL).
-   **Recent Focus:** The primary focus has been on architectural improvements, notably migrating the authentication system to Firebase. Development has also centered on expanding API capabilities with new endpoints and enhancing the CI/CD workflow.

### `src/app` (Frontend)

-   **Role:** This module is the frontend of the application, built with React. It is responsible for rendering the user interface, managing user interactions, and communicating with the backend API.
-   **Structure & Key Files:** It is organized into pages for distinct functionalities, reusable UI components, and a dedicated API layer.
    -   `src/App.tsx`: The main component that structures the application.
    -   `src/pages/`: Contains components for different application pages (e.g., `generate/Generate.tsx`, `flashcards/Flashcards.tsx`).
    -   `src/components/`: Reusable UI components.
    -   `src/api/`: Contains the auto-generated API client (`api.ts`) for communicating with the backend.
    -   `package.json`: Lists dependencies like `react`, `react-router-dom`, `tailwindcss`, and `axios`.
    -   `vite.config.ts`: The build configuration for the frontend application.
-   **Recent Focus:** Recent development has been heavily focused on improving the user experience through UI/UX enhancements like loaders, improved styling, a hamburger menu, and mobile responsiveness. There has also been a significant effort to strengthen the CI/CD pipeline with linting and end-to-end tests.

### `src/api/Memoraid.Tests.Unit`

-   **Role:** This module contains a suite of unit tests for the backend API. Its main purpose is to ensure the reliability and correctness of the API's services and validation logic.
-   **Structure:** The tests are organized in a structure that mirrors the `Memoraid.WebApi` project, with tests grouped by services and validators.
-   **Recent Focus:** Recent work has been dedicated to expanding test coverage in tandem with new feature development in the API. This includes writing tests for the new authentication flow and mocking external services like Open Router.

### `src/database`

-   **Role:** Manages the database schema and initial data. It uses a Docker container to run migrations, ensuring a consistent database environment.
-   **Structure & Key Files:**
    -   `migrations/scripts/`: Contains SQL scripts for creating and altering database tables.
    -   `test-data-seed/`: Scripts for populating the database with test data.
    -   `Dockerfile`: Defines the environment for running the migration scripts.
-   **Recent Focus:** Recent work has focused on improving the deployment process for database migrations by using a dedicated Docker image, ensuring a more reliable and consistent deployment pipeline.

## Key Contributors

-   **Kamil Kapliński (alienown):** As the primary contributor to date, Kamil has been responsible for the entire project, from the initial setup to the implementation of all features, spanning the full stack.

## Overall Takeaways & Recent Focus

1.  **Architectural Evolution:** The project has recently undergone a significant architectural shift with the migration from .NET authentication to Firebase.
2.  **UI/UX Refinement:** A significant amount of recent work has gone into improving the user interface, including mobile responsiveness, loaders, and styling enhancements.
3.  **CI/CD and Automation:** There is a strong emphasis on automation, with a robust CI/CD pipeline for continuous deployment and a containerized development environment for consistency.
4.  **End-to-End Testing:** The introduction of end-to-end tests with Playwright highlights a growing focus on ensuring the quality and reliability of the user-facing features.

## Potential Complexity/Areas to Note

-   **High-Change Files:** The `Program.cs` and `docker-compose.yaml` files have high change rates, indicating their central role in the application's configuration.
-   **Complex Frontend Component:** The `src/app/src/pages/generate/Generate.tsx` component is a complex, stateful component that orchestrates the entire flashcard generation workflow.
-   **API Generation:** The frontend uses a code generator (`apiGenerator.js`) to create a client from the backend's OpenAPI specification. Understanding this workflow is crucial when making API changes.
-   **Database Migrations:** Database changes are managed through SQL scripts and applied via a Docker container. This process must be followed carefully to avoid schema inconsistencies.
-   **Deployment Workflow:** The `.github/workflows/deploy.yml` file involves multiple services and secrets, requiring a thorough understanding before making changes.

## Questions for the Team

1.  What is the long-term vision for Memoraid beyond the current MVP features?
2.  Are there any established coding style guides or architectural patterns that I should be aware of?
3.  What is the process for updating the frontend API client after making changes to the backend API?
4.  What is the strategy for end-to-end testing, and how are the Playwright tests maintained?
5.  What was the primary motivation for migrating from .NET authentication to Firebase?
6.  Could you provide more context on the decision to use Open Router for AI model integration?
7.  How is application configuration (e.g., API keys for OpenRouter) managed in production?
8.  What is the strategy for managing database schema changes beyond the `run-migrations.sh` script?
9.  Is there a preferred branching strategy for new features or bug fixes?

## Next Steps

1.  **Set up the development environment:** Follow the instructions in the "Development Environment Setup" section below to build and run the application using Docker Compose.
2.  **Explore the running application:** Log in, create a few flashcards manually, and use the AI generation feature to understand the user workflow.
3.  **Review the API documentation:** With the application running, navigate to the OpenAPI endpoint (`/openapi/v1.json`) to see the available API endpoints and their specifications.
4.  **Familiarize Yourself with the Core Components:** Review `docker-compose.yaml` to understand service orchestration, `src/api/Memoraid.WebApi/Program.cs` for backend configuration, and `src/app/src/pages/generate/Generate.tsx` for the core user workflow.
5.  **Run the tests:** Execute the backend unit tests and the frontend end-to-end tests to ensure everything is working correctly.
6.  **Trace a feature:** Follow the code path for a simple feature, like deleting a flashcard, from the frontend component to the backend service and database query.
7.  **Analyze the database setup:** Look at the `src/database/migrations/` directory to understand how the database schema is defined and evolved.

## Development Environment Setup

The project is designed to be run using Docker.

1.  **Prerequisites:** Docker Desktop must be installed. For an optional manual setup, you would need: .NET 9.x SDK, Node.js v22.11.0+, Yarn, PostgreSQL, and the Firebase CLI.
2.  **Configuration:** Create a `.env` file in the `src/app` directory if it doesn't exist, and populate it with the necessary environment variables.
3.  **Build & Run:** From the root directory, run `docker-compose up --build`. This will build the Docker images and start all services.
4.  **Accessing the App:** The frontend will be available at `http://localhost:7002` and the backend API at `http://localhost:7000`.
5.  **Running Tests:**
    -   Frontend: `yarn test` in `src/app`
    -   E2E: `yarn e2e` in `src/app`
    -   Backend: `dotnet test` in `src/api`

## Helpful Resources

-   **Project README:** [README.md](./README.md)
-   **Frontend README:** [src/app/README.md](./src/app/README.md)
-   **Backend API Specification (OpenAPI):** Available at `/openapi/v1.json` on the running backend service (e.g., `http://localhost:7000/openapi/v1.json`). 