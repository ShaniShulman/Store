# WEB API .NET 8 Project

## Overview

This project is a WEB API built with .NET 8, adhering to REST architecture principles. The API provides various endpoints for managing users, products, categories, and orders. Detailed API documentation can be found [here](https://localhost:44382/swagger/v1/swagger.json).

## Project Structure

The project is divided into several layers, each with a specific responsibility. This separation of concerns promotes maintainability, scalability, and testability.

### Layers

1. **DTO (Data Transfer Objects)**
   - Contains records used for data transfer between layers. 
   - Ensures that only necessary data is exposed to clients, promoting security and reducing payload size.
   
   Example:
   ```csharp
   public record OrderDTO(DateTime OrderDate, double? OrderSum, int UserId, List<OrderItemDTO> OrderItems);
   ```

2. **Entities**
   - Represents the core business objects and data models used within the application.
   - Mapped directly to the database tables using Entity Framework Core.

3. **Repositories**
   - Handles data access logic.
   - Communicates with the database and performs CRUD operations.

4. **Services**
   - Contains business logic.
   - Calls repositories to fetch or persist data as needed.
   - Implements various business rules and validations.

5. **Controllers**
   - Exposes API endpoints.
   - Uses services to handle incoming HTTP requests and return appropriate responses.

### AutoMapper

AutoMapper is used to handle the conversion between different layers, ensuring a clean and maintainable codebase.

### Dependency Injection (DI)

Dependency Injection is used to manage dependencies between layers. This promotes loose coupling and makes the system more testable and maintainable.

## Asynchronous Programming

Async/await is used throughout the project to handle asynchronous operations. This ensures better scalability and responsiveness, especially under high load conditions.

## Database

The project uses SQL database with Code First approach. To create the database, use the following commands:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## Configuration

The project uses configuration files to manage various settings and secrets. This ensures that configuration is externalized and can be easily changed without modifying the code.

## Error Handling

All errors are handled using a custom error middleware. Errors are logged using a logger, and fatal errors are sent via email.

## Request Logging

Every request to the system is logged for rating and analysis purposes.

## Clean Code

The project adheres to clean code principles to ensure readability, maintainability, and extensibility.
