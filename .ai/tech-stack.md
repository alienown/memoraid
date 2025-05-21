Frontend - React with Vite:

- React 19 provides interactivity where needed.
- TypeScript 5 ensures static typing and better IDE support.
- Tailwind 4 allows for convenient application styling.
- Shadcn/ui offers a library of accessible React components to build the UI.

Backend - .NET API and PostgreSQL as a database:

- minimal API built with .NET 9
- FluentValidation for validating API requests and responses
- Entity Framework Core for easy database access and migrations
- NUnit and Shouldly for testing

Auth - Firebase Authentication:
- Firebase Authentication for user management and authentication

AI - Communication with models via Open Router API:
- single point of entry for multiple AI models

Testing:

- Backend Testing:
  - NUnit framework with Shouldly for assertions
  - EF Core InMemory database for unit tests
  - Moq for mocking dependencies
  - Docker containers for running services used in integration tests

- Frontend Testing:
  - React Testing Library with Vitest for component unit testing
  - Playwright with React Testing Library for end-to-end testing

- API Testing:
  - Postman/Swagger for manual endpoint verification

CI/CD and Hosting:

- GitHub Actions for creating CI/CD pipelines and running automated tests.
- DigitalOcean for hosting applications via Docker images.
