# Project Onboarding: Memoraid

## Welcome

Welcome to the Memoraid project! This document provides a comprehensive overview to help you get started. Memoraid is a flashcard application designed to help users study and memorize information effectively. It features a .NET-based backend API and a modern React frontend.

## Project Overview & Structure

The project is a monorepo containing three main parts: a backend API (`src/api`), a frontend web application (`src/app`), and database management scripts (`src/database`). The entire stack is orchestrated using Docker, as defined in the `docker-compose.yaml` file.

- **`src/api`**: A .NET/C# solution containing the backend REST API (`Memoraid.WebApi`) and its corresponding unit tests (`Memoraid.Tests.Unit`). It handles all business logic and data persistence.
- **`src/app`**: A TypeScript-based React single-page application built with Vite. It provides the user interface for interacting with the flashcard data.
- **`src/database`**: Contains Dockerized SQL migration and data seeding scripts, suggesting a container-based database setup.
- **`docker-compose.yaml`**: The central file for running the entire application stack (backend, frontend, and database) in a containerized environment.

## Core Modules

### `Memoraid.WebApi (Backend)`

- **Role:** Serves as the backend API for the application. It manages flashcards, handles user requests, and interacts with the database. It also integrates with an external AI service (OpenRouter) for generating flashcard content.
- **Key Files/Areas:**
  - `Program.cs`: Entry point, service registration, and middleware configuration.
  - `Services/`: Contains the core business logic (e.g., `FlashcardService`, `OpenRouterService`).
  - `Persistence/`: Handles database interactions.
  - `Requests/` & `Responses/`: Defines the API's data contracts.
  - `Memoraid.WebApi.csproj`: Lists project dependencies like `FluentValidation`, `Npgsql` (PostgreSQL).
- **Recent Focus:** Recent work has focused on improving the deployment process and API robustness. This includes making the CORS configuration more flexible and handling more authentication errors.

### `app (Frontend)`

- **Role:** Provides the complete user interface for the Memoraid application. Users can view, create, edit, and delete flashcards through this interface.
- **Key Files/Areas:**
  - `src/App.tsx`: The main component that structures the application.
  - `src/pages/`: Contains components for different application pages (e.g., login, dashboard).
  - `src/components/`: Reusable UI components.
  - `src/api/`: Contains the auto-generated API client for communicating with the backend.
  - `package.json`: Lists dependencies like `react`, `react-router-dom`, `tailwindcss`, and `axios`.
  - `vite.config.ts`: The build configuration for the frontend application.
- **Recent Focus:** Recent development has heavily focused on UI/UX improvements. This includes making the application mobile-responsive, adding a hamburger menu, implementing loaders for better user feedback, and other styling enhancements. End-to-end tests have also been updated to reflect these UI changes.

### `database`

- **Role:** Manages the database schema and initial data. It uses a Docker container to run migrations, ensuring a consistent database environment.
- **Key Files/Areas:**
  - `migrations/scripts/`: Contains SQL scripts for creating and altering database tables.
  - `test-data-seed/`: Scripts for populating the database with test data.
  - `Dockerfile`: Defines the environment for running the migration scripts.
- **Recent Focus:** Recent work has focused on improving the deployment process for database migrations by using a dedicated Docker image, ensuring a more reliable and consistent deployment pipeline.

## Key Contributors

Based on recent commits, the primary contributor to the project is:

- **Kamil Kapliński / alienown**: Appears to be the lead developer, working across the entire stack, including backend, frontend, and infrastructure setup.

## Overall Takeaways & Recent Focus

The project is actively being developed with a strong focus on improving the user experience and robustness of the deployment pipeline. Key recent initiatives, according to the commit history, include:

- **UI/UX Enhancements:** A significant amount of recent work has gone into improving the user interface, including adding a mobile-friendly view, a hamburger menu for navigation, and other styling and usability improvements like loaders and custom fonts.
- **CI/CD Pipeline Automation:** There has been a heavy focus on setting up and refining the GitHub Actions workflows for continuous integration and deployment. This includes fixing action paths, ensuring deployments wait for completion, and using a dedicated Docker image for database migrations.
- **Code Quality and Testing:** Recent commits show an emphasis on improving code quality by adding linting to the pull request process and adjusting end-to-end tests to match UI changes.
- **Configuration and Infrastructure:** Work has been done to correctly pass build arguments in Docker Compose and to make the CORS configuration more flexible.

## Potential Complexity/Areas to Note

- **API Generation:** The frontend uses a code generator (`apiGenerator.js`) to create a client from the backend's OpenAPI specification. Understanding this workflow is crucial when making API changes.
- **Docker Environment:** The `docker-compose.yaml` file orchestrates multiple services. Familiarity with Docker is essential for running the project and debugging environment-related issues.
- **Database Migrations:** Database changes are managed through SQL scripts and applied via a Docker container. This process must be followed carefully to avoid schema inconsistencies.

## Questions for the Team

1. What is the process for updating the frontend API client after making changes to the backend API?
2. Are there any established coding style guides or conventions beyond what is enforced by the linter?
3. What is the long-term vision for the AI-powered flashcard generation feature?
4. How is application configuration (e.g., API keys for OpenRouter) managed in production?
5. What are the priorities for the next development cycle?
6. Is there a preferred branching strategy for new features or bug fixes?
7. What is the current state of user authentication and authorization?

## Next Steps

1. **Set up the development environment:** Follow the instructions in the `README.md` to build and run the application using Docker Compose.
2. **Explore the running application:** Log in, create a few flashcards manually, and use the AI generation feature to understand the user workflow.
3. **Review the API documentation:** With the application running, navigate to the OpenAPI endpoint (`/openapi/v1.json`) to see the available API endpoints and their specifications.
4. **Trace a feature:** Follow the code path for a simple feature, like deleting a flashcard, from the frontend component to the backend service and database query.
5. **Run the tests:** Execute the backend unit tests and the frontend end-to-end tests to ensure everything is working correctly on your local machine.

## Development Environment Setup

The project is designed to be run using Docker.

1.  **Prerequisites:** Ensure you have Docker and Docker Compose installed on your system.
2.  **Configuration:** Create a `.env` file in the `src/app` directory if it doesn't exist, and populate it with the necessary environment variables as defined in the project's documentation.
3.  **Build & Run:** From the root directory, run the command `docker-compose up --build`. This will build the Docker images for the frontend, backend, and database, and start all the services.
4.  **Accessing the App:** Once the containers are running, the frontend should be accessible at `http://localhost:7002` and the backend API at `http://localhost:7000`.

## Helpful Resources

- **Project README:** [README.md](file:///c%3A/Programs/source/10xdevs/memoraid/README.md)
- **Backend API Documentation:** Available at `/openapi/v1.json` on the running backend service (e.g., `http://localhost:7000/openapi/v1.json`).
- **Frontend README:** [src/app/README.md](file:///c%3A/Programs/source/10xdevs/memoraid/src/app/README.md)
