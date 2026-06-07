# E-Commerce Backend API

A production-style RESTful API built with ASP.NET Core 8 
and Clean Architecture.

## Tech Stack

- ASP.NET Core 8 Web API
- Entity Framework Core 8
- SQL Server
- JWT Authentication
- Clean Architecture
- Repository Pattern
- Serilog
- FluentValidation

## Features

- JWT Authentication with Role-based Authorization (Admin/User)
- Product & Category Management
- Shopping Cart
- Order Management with Stock Control
- Global Exception Handling
- Structured Logging with Serilog
- Pagination, Filtering & Sorting

## Architecture
- ECommerce.Domain          → Entities, Interfaces
- ECommerce.Application     → Business Logic, DTOs, Services
- ECommerce.Infrastructure  → EF Core, Repositories
- ECommerce.API             → Controllers, Middleware

## How to Run

1. Clone the repository
   git clone https://github.com/pranavsharma14/ecommerce-backend-api.git

2. Update connection string in appsettings.json
   "DefaultConnection": "Server=localhost;Database=CleanAPIDb;..."

3. Configure JWT secret key
   - For local development, use user secrets:
     dotnet user-secrets set "JwtSettings:SecretKey" "YOUR_SECRET_KEY"
   - For production or hosting, use environment variables:
     Windows: setx JwtSettings__SecretKey "YOUR_SECRET_KEY"
     Linux/macOS: export JwtSettings__SecretKey="YOUR_SECRET_KEY"

4. Run migrations
   Update-Database -StartupProject ECommerce.API

5. Run the project
   dotnet run --project ECommerce.API

## JWT Secret and Existing Users
- The JWT secret is loaded from `JwtSettings:SecretKey`.
- In development, the project uses user secrets outside the repository.
- Changing the secret does not affect user records in the database.
- Changing the secret invalidates previously issued JWT tokens, so users must log in again.

## API Endpoints

### Auth
- POST /api/auth/register
- POST /api/auth/login
- GET  /api/auth/me

### Products
- GET    /api/products
- GET    /api/products/{id}
- POST   /api/products        [Authorize]
- PUT    /api/products/{id}   [Authorize]
- DELETE /api/products/{id}   [Authorize]

### Categories
- GET    /api/categories
- GET    /api/categories/{id}
- POST   /api/categories      [Admin]
- PUT    /api/categories/{id} [Admin]
- DELETE /api/categories/{id} [Admin]

### Cart
- GET    /api/cart            [Authorize]
- POST   /api/cart/items      [Authorize]
- PUT    /api/cart/items/{id} [Authorize]
- DELETE /api/cart/items/{id} [Authorize]
- DELETE /api/cart            [Authorize]

### Orders
- GET  /api/order             [Authorize]
- GET  /api/order/{id}        [Authorize]
- POST /api/order             [Authorize]
- PUT  /api/order/{id}        [Admin]
- GET  /api/order/all         [Admin]
