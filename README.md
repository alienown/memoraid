# Memoraid

Memoraid is an AI-powered web application for creating and studying flashcards efficiently. This platform simplifies and accelerates the creation of educational flashcards by leveraging AI to automatically generate high-quality content from user-supplied text.

This application was created as a certification project for the [10xdevs](https://www.10xdevs.pl/?aidevs) course, which focuses on using AI tools in software development lifecycle. The project was inspired by original idea of 10xCards - flashcards application proposed by authors of the course - [Przemek Smyrdek](https://www.linkedin.com/in/psmyrdek/) and [Marcin Czarkowski](https://www.linkedin.com/in/mkczarkowski/). Memoraid which is in MVP stage has the same functionalities as 10xCards but with intention to be extended in the future. Big thanks to the authors of the course for their guidance and support throughout the course.

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Getting Started Locally](#getting-started-locally)
- [Available Scripts](#available-scripts)
- [Project Scope](#project-scope)
- [Project Status](#project-status)
- [License](#license)

## Overview

Memoraid addresses the time-consuming process of manually creating educational flashcards. Users who wish to benefit from spaced repetition are often discouraged by the effort required to produce high-quality flashcards. Memoraid solves this by automating a large portion of the flashcard creation process while still providing flexibility for manual input and corrections.

Application MVP is accessible at https://app-latest-1m1f.onrender.com.

## Features

### AI-Generated Flashcards
- Paste large blocks of text (up to 10,000 characters)
- Automatically generate flashcards with front (question) and back (answer) content
- Review AI-generated flashcards with options to accept, reject, or modify

### Manual Flashcard Management
- Create, view, edit, and delete flashcards
- Intuitive user interface for flashcard management
- Validation rules enforce quality standards

### User Account Management
- Secure email and password registration/login

## Tech Stack

### Frontend
- React 19 with Vite
- TypeScript 5
- Tailwind CSS 4
- Shadcn/UI component library
- React Router 7
- Firebase Authentication

### Backend
- .NET 9 Minimal API
- PostgreSQL database
- Entity Framework Core 9
- FluentValidation
- JWT Authentication via Firebase

### AI Integration
- Open Router API for AI model communication & flashcard generation

### Testing
- Backend: NUnit, Shouldly, EF Core InMemory
- Frontend: React Testing Library, Vitest
- E2E: Playwright

### CI/CD & Hosting
- GitHub Actions for CI/CD pipelines
- Docker containers
- Render hosting platform

## Getting Started Locally

### Prerequisites
- Docker Desktop
- Without using docker, you will need the following installed on your machine:
  - [.NET 9.x SDK](https://dotnet.microsoft.com/download)
  - [Node.js v22.11.0 or higher](https://nodejs.org/en/download/)
  - [Yarn package manager](https://yarnpkg.com/getting-started/install)
  - [PostgreSQL](https://www.postgresql.org/download/)
  - [Firebase CLI](https://firebase.google.com/docs/cli) with [Firebase Emulator Suite](https://firebase.google.com/docs/emulator-suite/install_and_configure)

You can run part of the application using Docker and part of them without Docker. The choice depends on your development environment and preferences.

### Run with Docker Compose

The easiest way to run the complete application stack is using Docker Compose:

```bash
# Start all services
docker-compose up
```

Access the application at http://localhost:7002

You can use demo user credentials to log in:
- **Email:**: demo@xyz.com
- **Password:**: 123456

### Manual Setup (Development)

#### Database Setup

Run migrations:

```bash
# Navigate to migrations directory
cd src/database/migrations

# Run migrations
POSTGRES_HOST="your_host" POSTGRES_USER="your_username" POSTGRES_PASSWORD="your_password" POSTGRES_DB="your_database" ./run-migrations.sh
```

#### Backend Setup

Change the connection string in [appsettings.json](src/api/Memoraid.WebApi/appsettings.json) to match your local PostgreSQL setup.

```bash
# Navigate to API directory
cd src/api

# Restore packages
dotnet restore

# Run the API
cd Memoraid.WebApi
dotnet run
```

#### Frontend Setup

```bash
# Navigate to app directory
cd src/app

# Install dependencies
yarn install

# Start development server
yarn dev
```

#### Firebase Emulator Setup

```bash
# Navigate to app directory
cd src/app

# Start Firebase emulator
yarn firebase-emulators
```

## Available Scripts

### Backend (.NET)

```bash
# Navigate to api directory
cd src/api

# Run tests
dotnet test 
```

### Frontend (React)

```bash
# Navigate to app directory
cd src/app

# Start development server
yarn dev

# Build for production
yarn build

# Run unit tests
yarn test

# Run end-to-end tests
yarn e2e

# Lint code
yarn lint

# Preview production build
yarn preview

# Generate API client from OpenAPI spec
yarn api

# Generate E2E API client from OpenAPI spec
yarn api:e2e

# Start Firebase emulators
yarn firebase-emulators
```

## Project Scope

### Current MVP Features

✅ AI-driven flashcard generation with copy-paste functionality
✅ Manual creation and CRUD operations for flashcards
✅ Flashcard review interface for accepting/rejecting AI-generated content
✅ Basic user account management

### Not Included in MVP

❌ Integration with spaced repetition algorithm for study mode
❌ Support for importing multiple document formats (PDF, DOCX)
❌ Sharing flashcard sets between users
❌ Integration with external educational platforms
❌ Mobile applications (initially web-only)

## Project Status

Memoraid is currently in active development with intention to bring more features in the future.

## License

This project is licensed under the MIT License.