# Project Onboarding: Memoraid

## Welcome

Welcome to the Memoraid project! Memoraid is an AI-powered web application for creating and studying flashcards efficiently. This platform simplifies and accelerates the creation of educational flashcards by leveraging AI to automatically generate high-quality content from user-supplied text.

## Project Overview & Structure

The core functionality revolves around AI-powered flashcard generation and management. The project is organized as a monorepo with the following key components/modules: a React frontend (`src/app`), a .NET backend (`src/api`), and a containerized development environment (`docker-compose.yaml`).

## Core Modules

### `src/app`

-   **Role:** This module is the frontend of the application, built with React. It is responsible for rendering the user interface, managing user interactions, and communicating with the backend API to provide a seamless user experience.
-   **Structure:** It is organized into pages for distinct functionalities like flashcard generation (`Generate.tsx`) and management, a set of reusable UI components, and a dedicated API layer (`api.ts`) for handling backend communication.
-   **Recent Focus:** Recent development has been heavily focused on improving the user experience through UI/UX enhancements like loaders and improved styling. There has also been a significant effort to ensure mobile responsiveness and to strengthen the CI/CD pipeline with linting and end-to-end tests.
-   **Relationships:** This module is tightly coupled with `src/api/Memoraid.WebApi`, consuming its endpoints for all data-related operations. It also relies on the Firebase service defined in `docker-compose.yaml` for authentication.

### `src/api/Memoraid.WebApi`

-   **Role:** As the .NET backend API, this module is the core of the application. It handles all business logic, manages data persistence through a database, and integrates with the external Open Router AI service for flashcard generation.
-   **Structure:** The API is built around key services like `FlashcardService`, structured request/response models, and a central `Program.cs` file for configuring services, authentication, and database connections. Its architecture supports flashcard management, user authentication, and AI-powered content generation.
-   **Recent Focus:** The primary focus has been on architectural improvements, notably migrating the authentication system to Firebase. Development has also centered on expanding API capabilities with new endpoints and enhancing the CI/CD workflow for more robust deployments.
-   **Relationships:** This module serves as the backend for the `src/app` frontend. It connects to the PostgreSQL database for data persistence and integrates with the Open Router API for AI-powered features.

### `src/api/Memoraid.Tests.Unit`

-   **Role:** This module contains a suite of unit tests for the backend API. Its main purpose is to ensure the reliability and correctness of the API's services and validation logic, thereby maintaining code quality and stability.
-   **Structure:** The tests are organized in a structure that mirrors the `Memoraid.WebApi` project, with tests grouped by services and validators. This organization makes it easy to locate tests corresponding to specific application components.
-   **Recent Focus:** Recent work has been dedicated to expanding test coverage in tandem with new feature development in the API. This includes writing tests for the new authentication flow, mocking external services like Open Router, and ensuring that all new API endpoints are thoroughly tested.
-   **Relationships:** This module directly tests the `src/api/Memoraid.WebApi` project, ensuring its components function as expected.

## Key Contributors

-   **Kamil Kapliński:** As the sole contributor to date, Kamil has been responsible for the entire project, from the initial setup to the implementation of all features. His work spans the full stack, including the .NET backend, the React frontend, the containerized development environment, and the CI/CD pipeline. His deep involvement across all modules makes him the primary point of contact for any questions.

## Overall Takeaways & Recent Focus

1.  **Architectural Evolution:** The project has recently undergone a significant architectural shift with the migration from .net authentication to Firebase, indicating a focus on leveraging managed services to streamline development.
2.  **CI/CD and Automation:** There is a strong emphasis on automation, with a robust CI/CD pipeline in place for continuous deployment and a containerized development environment for consistency.
3.  **End-to-End Testing:** The introduction of end-to-end tests with Playwright highlights a growing focus on ensuring the quality and reliability of the user-facing features.
4.  **UI/UX Refinement:** Recent frontend development has been geared towards improving the user experience, with a focus on styling, loaders, and mobile responsiveness.

## Potential Complexity/Areas to Note

-   **High-Change Files:** The `Program.cs` and `docker-compose.yaml` files have high change rates, indicating their central role in the application's configuration and a potential need for careful coordination when making changes.
-   **Complex Frontend Component:** The `Generate.tsx` component is a complex, stateful component that orchestrates the entire flashcard generation workflow, making it a critical area to understand for frontend development.
-   **Deployment Workflow:** The `.github/workflows/deploy.yml` file, while well-structured, involves multiple services and secrets, requiring a thorough understanding before making any changes to the deployment process.

## Questions for the Team

1.  What is the long-term vision for Memoraid beyond the current MVP features?
2.  Are there any established coding style guides or architectural patterns that I should be aware of for the frontend and backend respectively?
3.  Could you walk me through the typical workflow for developing a new feature, from ticket to deployment?
4.  What is the strategy for end-to-end testing, and how are the Playwright tests maintained?
5.  Are there any plans for more comprehensive logging, monitoring, or error tracking?
6.  What was the primary motivation for migrating from .NET authentication to Firebase, and are there any plans to leverage other Firebase services in the future?
7.  The `.ai/api-plan.md` file is a great resource, but are there any established processes for ensuring it stays in sync with the implementation?
8.  Could you provide more context on the decision to use Open Router for AI model integration? Are there any specific models that have proven to be most effective?
9.  What is the strategy for managing database schema changes, and are there any tools or processes in place beyond the `run-migrations.sh` script?

## Next Steps

1.  **Familiarize Yourself with the Core Components:** Start by reviewing the `docker-compose.yaml` file to understand how the services are orchestrated. Then, dive into `src/api/Memoraid.WebApi/Program.cs` to see how the backend is configured, and `src/app/src/pages/generate/Generate.tsx` to understand the core user workflow.
2.  **Run the Application Locally:** Use the `docker-compose up` command to get the full stack running on your local machine and explore the application's functionality.
3.  **Review the Testing Suites:** Run the backend unit tests (`dotnet test`) and the frontend end-to-end tests (`yarn e2e`) to familiarize yourself with the testing setup and the existing test cases.
4. **Explore the flashcard generation flow:** Start at `src/app/src/pages/generate/Generate.tsx` and trace the user interaction down 
through the API call to `src/api/Memoraid.WebApi/Services/FlashcardService.cs` and the AI integration.
5.  **Analyze the database setup:** Look at the `src/database/migrations/` directory to understand how the database schema is defined 
and evolved.
6. **Review the deployment workflow:** Examine `.github/workflows/deploy.yml` to understand how the application is built, 
containerized, and deployed to Render.
7.  **Enhance the Documentation:** Consider contributing to the documentation by adding more detailed comments to the `docker-compose.yaml` and `.github/workflows/deploy.yml` files, as they are complex and have a high change rate.

## Development Environment Setup

1.  **Prerequisites:** Docker Desktop. Alternatively, for a manual setup: .NET 9.x SDK, Node.js v22.11.0+, Yarn, PostgreSQL, Firebase CLI.
2.  **Dependency Installation:** For frontend, run `yarn install` in `src/app`. For backend, dependencies are managed by .NET and restored on build.
3.  **Building the Project (if applicable):** `docker-compose build` or for frontend `yarn build` in `src/app` and for backend `dotnet build` in `src/api`.
4.  **Running the Application/Service:** `docker-compose up`. The application will be available at http://localhost:7002.
5.  **Running Tests:** 
    - Frontend: `yarn test` in `src/app`
    - E2E: `yarn e2e` in `src/app`
    - Backend: `dotnet test` in `src/api`
6.  **Common Issues:** Common issues section not found in checked files.

## Helpful Resources

- **Documentation:** The primary documentation is the `README.md` in the root of the project.
- **Issue Tracker:** An issue tracker link was not found in the checked files, but it is likely managed through GitHub Issues for the repository.
- **Contribution Guide:** A contribution guide was not found in the checked files.
- **Communication Channels:** A communication channel link was not found in the checked files.
- **Learning Resources:** Specific learning resources section not found.
