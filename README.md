# Taskify Project Documentation

## Overview

The Taskify project is a task management application consisting of a Blazor-based client application, a web API built with ASP.NET Core, a data access layer using Entity Framework Core, and test projects for backend services. The project facilitates task management functionalities such as listing tasks, creating, updating, and deleting tasks. It communicates with the Taskify API via RESTful endpoints.

## Project Structure

### Client

Blazor application providing a user interface for task management. It offers capabilities to view, create, update, and delete tasks. Communication with the Taskify API is through REST calls.

### DataAccess

Class library handling database connections, data models, DbContext classes, and migrations. Utilizes Entity Framework Core with an SQLite database.

### Server

ASP.NET Core Web API implementing CRUD operations for tasks. Built using the OpenAPI standard with Swagger for API documentation. Includes controllers exposing endpoints for task management, a TaskService, and a TaskStatusUpdaterService to manage task status updates.

### Server.Tests

Test project containing unit tests for the TaskService and TaskStatusUpdaterService within the Server project. These tests cover logic and edge cases.

#### Notes

The TaskStatusUpdaterService test is still a work in progress. I left it for last and couldn't figure out how to create proper mocks for it to run in the time left.

## Setup and Running the Project

### Prerequisites

* .NET SDK installed
* SQLite database engine

### Steps

* Clone the repository
* Restore dependencies: `dotnet restore`
* Set up database migrations (if required): dotnet ef database update
* Run the applications using dotnet run or your preferred IDE.
  * Starting projects should be `Client` and `Server`

## API Endpoints and Interactions

See available endpoints in the [swagger file](./swagger.json)

Sample API Interaction:

* GET /api/tasks
  * Response: List of tasks in the specified format.

## Backend Logic Implementation

The backend logic is divided into:

### Client-side

Blazor application handling user interactions and making REST API calls.

### Server-side

Employs MVC architecture with controllers, services, and models to maintain separation of concerns. Implements API endpoints for task management and background service for automated status updates.

The TaskService manages task-related operations like creation, retrieval, updating, and deletion.

TaskStatusUpdaterService runs periodically to update task statuses based on due dates.

## UI

The project incorporates a Blazor-based user interface providing a user-friendly task management experience. It offers functionalities to view, create, update, and delete tasks, enhancing the overall user experience.

## Improvements in the future

* Fix UI view bug: on the view modal, the modal data is one click behind. For example:
  * If you load the page and click on TaskA, the modal loads empty. If you then click on TaskB, the modal loads with TaskA.
  * If you load the page and click on TaskC, on the first click the modal is empty and on the second click it shows TaskC details.
* Add Logging
* Change the way UI shows errors (currently through alerts)
* Expand on unit tests
* Make date picker open on today by default.
