# ECommerceBackend

Complete .NET 8 Web API backend for the MACHINE TEST - L1.

## Quickstart

1. Install .NET SDK 8 or 7.
2. Update `src/ECommerce.Api/appsettings.json` connection string (Default uses localdb).
3. From `src/ECommerce.Api` folder run:
   ```bash
   dotnet tool install --global dotnet-ef
   dotnet restore
   dotnet ef database update
   dotnet run
   ```
4. Open Swagger: https://localhost:5001/swagger

Default admin:
- Email: admin@ecommerce.local
- Password: Admin123

