# AI Rules for Memoraid

Memoraid is a web-based flashcard platform designed to simplify and accelerate the creation of high-quality educational flashcards. The application leverages AI to automatically generate flashcards from user-supplied text using a dedicated AI-generation view while providing a separate "My Flashcards" view for manual creation, review, editing, and deleting. The system also includes user account management and integrates with an existing spaced repetition algorithm for effective study sessions.

# Database

The application uses a PostgreSQL database.

## Project structure
- /src/database: root directory containing database-related files and scripts
  - /migrations: migration scripts
    - /scripts: migration scripts
    - /rollback-scripts: rollback scripts for each migration

## PostgreSQL guidelines
- Use PostgreSQL naming conventions. Table names should be lowercased and plural. Constraint and index naming convention are as follows:
  {tablename}_{columnname(s)}_{suffix}, where the suffix is one of the following:
  - pkey for a Primary Key constraint
  - key for a Unique constraint
  - excl for an Exclusion constraint
  - idx for any other kind of index
  - fkey for a Foreign key
  - check for a Check constraint

# Backend

## Project structure

- /src/api: source code for the backend application
  - /Memoraid.WebApi: ASP.NET Core Web API application
    - /Configuration: configuration classes initialized from appsettings.json
    - /Requests: request classes for API endpoints
    - /Responses: response classes for API endpoints
    - /Services: service classes for business logic, authentication and authorization, and data access
    - /Validation: validation classes for request models using Fluent Validation
    - /Persistence: Entity Framework Core data access configuration
      - /Entities: entity classes representing database tables
      - /EntityConfigurations: entity configuration classes for Fluent API configurations
  - /Memoraid.Tests.Unit: unit tests

## Guidelines for .NET

### Code Style and Structure

- Prefer LINQ and lambda expressions for collection operations
- Use UPPERCASE for constants
- Use C#'s expressive syntax (e.g., null-conditional operators, string interpolation)
- Use 'var' for implicit typing when the type is obvious
- Use dependency injection with scoped lifetime for request-specific services and singleton for stateless services
- Do NOT use primary constructors

### Error Handling and Validation
  - Use exceptions for exceptional cases, not for control flow
  - Use Fluent Validation for model validation

### ASP NET Core Web API

- Use minimal APIs to reduce boilerplate code

### Entity Framework Core

- Implement eager loading with Include() to avoid N+1 query problems for entity relationships
- Use migrations for database schema changes and version control with proper naming conventions
- Apply appropriate tracking behavior (AsNoTracking() for read-only queries) to optimize performance
- Implement query optimization techniques like compiled queries for frequently executed database operations
- Use value conversions for complex property transformations and proper handling of custom data types

### Testing
  - Write unit tests using NUnit
  - Use Moq for mocking dependencies in unit tests
  - In each test write the Arrange, Act, Assert comments for clarity
  - When mocking dependencies, 
  - Use Shouldly for assertions

# Frontend

## Project structure

- /src/app: root directory containing source code and configuration files for the React application
  - /src: the actual source code for the application
    - /assets: static assets like images and fonts
    - /components: contains reusable components
      - /ui: shadcn components
    - /pages: page components
    - /lib: code used by third-party libraries and APIs

## Guidelines for REACT

### React coding standards

- Use functional components with hooks instead of class components
- Implement React.memo() for expensive components that render often with the same props
- Utilize React.lazy() and Suspense for code-splitting and performance optimization
- Use the useCallback hook for event handlers passed to child components to prevent unnecessary re-renders
- Prefer useMemo for expensive calculations to avoid recomputation on every render
- Implement useId() for generating unique IDs for accessibility attributes
- Use the new use hook for data fetching in React 19+ projects
- Use useTransition for non-urgent state updates to keep the UI responsive

### React Router

- Use createBrowserRouter instead of BrowserRouter for better data loading and error handling
- Implement lazy loading with React.lazy() for route components to improve initial load time
- Use the useNavigate hook instead of the navigate component prop for programmatic navigation
- Leverage loader and action functions to handle data fetching and mutations at the route level
- Implement error boundaries with errorElement to gracefully handle routing and data errors
- Use relative paths with dot notation (e.g., "../parent") to maintain route hierarchy flexibility
- Utilize the useRouteLoaderData hook to access data from parent routes
- Implement fetchers for non-navigation data mutations
- Use route.lazy() for route-level code splitting with automatic loading states
- Implement shouldRevalidate functions to control when data revalidation happens after navigation

## Guidelines for Styling

### Tailwind

- Use the @layer directive to organize styles into components, utilities, and base layers
- Implement Just-in-Time (JIT) mode for development efficiency and smaller CSS bundles
- Use arbitrary values with square brackets (e.g., w-[123px]) for precise one-off designs
- Leverage the @apply directive in component classes to reuse utility combinations
- Implement the Tailwind configuration file for customizing theme, plugins, and variants
- Use component extraction for repeated UI patterns instead of copying utility classes
- Leverage the theme() function in CSS for accessing Tailwind theme values
- Implement dark mode with the dark: variant
- Use responsive variants (sm:, md:, lg:, etc.) for adaptive designs
- Leverage state variants (hover:, focus:, active:, etc.) for interactive elements