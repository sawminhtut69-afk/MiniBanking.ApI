# Mini Banking API

A simple Banking Backend API built with C# and ASP.NET Core.

## Features

- Customer CRUD
- Bank Account CRUD
- Deposit
- Withdraw
- Transfer
- Transaction History
- SQL Server Database
- Entity Framework Core
- Swagger API Documentation

## Technologies

- C#
- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Swagger / OpenAPI
- Git & GitHub

## Database

The project uses SQL Server with Entity Framework Core.

Main tables:

- Customers
- BankAccounts
- Transactions

## API Endpoints

### Customer

`text
GET    /api/Customer
GET    /api/Customer/{id}
POST   /api/Customer
PUT    /api/Customer/{id}
DELETE /api/Customer/{id}
Bank Account
GET    /api/BankAccount
GET    /api/BankAccount/{id}
POST   /api/BankAccount
PUT    /api/BankAccount/{id}
DELETE /api/BankAccount/{id}
Transaction
GET  /api/Transaction
POST /api/Transaction/deposit
POST /api/Transaction/withdraw
POST /api/Transaction/transfer
How to Run
1. Clone the repository.
2. Open the project folder in VS Code.
3. Make sure SQL Server is running.
4. Update the connection string in:
appsettings.json
Run database migrations:
dotnet ef database update
Start the API:
dotnet run
Open Swagger:
http://localhost:5203/swagger
Project Structure
MiniBanking.API
│
├── Controllers
│   ├── CustomerController.cs
│   ├── BankAccountController.cs
│   └── TransactionController.cs
│
├── Data
│   └── BankingDbContext.cs
│
├── Models
│   ├── Customer.cs
│   ├── BankAccount.cs
│   └── Transaction.cs
│
├── Migrations
│
├── Program.cs
├── appsettings.json
├── MiniBanking.API.csproj
└── README.md
Purpose
This project was created as a learning project to practice backend development, REST APIs, CRUD operations, database integration, Entity Framework Core, and basic banking transaction logic