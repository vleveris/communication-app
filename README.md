# Communication App

This is a .NET 9 Web API for managing communication templates and customers, and for sending communications.

## Architecture

The application follows a Repository-Service-Controller pattern:

*   **Controllers**: Handle HTTP requests and responses.
*   **Services**: Contain the core logic.
*   **Repositories**: data access layer.

## Setup

1.  **Prerequisites:**
    *   .NET 9 SDK
2.  **Installation:**
    *   Clone the repository.
    *   Navigate to the solution directory (`CommunicationApp`).
    *   Run `dotnet restore` to install dependencies.
    *   Run `dotnet ef database update --project CommunicationApp/CommunicationApp.csproj` to apply database migrations.
3.  **Running the application:**
    *   Run `dotnet run --project CommunicationApp/CommunicationApp.csproj` from the solution directory.
    *   The API will be available at `http://localhost:5107`.

## API Documentation

The API is documented using Swagger. Once the application is running, you can access the Swagger UI at `http://localhost:5107/swagger`.

### Authentication

To access the protected endpoints, you first need to obtain a JWT token by sending a POST request to `/api/auth/token`. Include the returned token in the `Authorization` header of your subsequent requests as a Bearer token.

### Endpoints

*   **Customers** (`/api/customers`)
    *   `GET /`: Get all customers.
    *   `GET /{id}`: Get a specific customer.
    *   `POST /`: Create a new customer.
    *   `PUT /{id}`: Update a customer.
    *   `DELETE /{id}`: Delete a customer.
*   **Templates** (`/api/templates`)
    *   `GET /`: Get all templates.
    *   `GET /{id}`: Get a specific template.
    *   `POST /`: Create a new template.
    *   `PUT /{id}`: Update a template.
    *   `DELETE /{id}`: Delete a template.
*   **Communication** (`/api/communication`)
    *   `POST /send?customerId={customerId}&templateId={templateId}`: Send a communication to a customer using a template.
*   **Placeholders** (`/api/placeholder`)
    *   `GET /`: Get all available placeholders and their descriptions.
