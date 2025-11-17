# ECommerceApi

Complete **.NET 8 ECommerce API backend**.

##  Features

- Product Management (CRUD)
- Category Management
- User Registration & Login (JWT Auth)
- Admin Panel APIs
- Order Management
- Entity Framework Core (Code-First)
- MSSQL Database
- Clean Architecture Structure

---

##  Quickstart Setup

Follow these steps to run the project locally:

### Install Requirements
- Install **.NET SDK 8** (or 7)
- Install **SQL Server / SQL Server Express**
- Install **dotnet-ef** tool:
  ```bash
  dotnet tool install --global dotnet-ef --version 8.0.4
  
  dotnet ef database update

Default admin login:
  "Email": "admin@store.com",
  "Password": "Admin123"
  
Customer Login
   "Email": "cust@store.com"
   "Password": "Cust12345"

