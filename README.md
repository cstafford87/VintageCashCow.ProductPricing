# VintageCashCow Product Pricing API

A .NET 9 Web API for managing product pricing with discount application and price history tracking.

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Visual Studio 2022 or VS Code

## Running the Application

### API Only

**Visual Studio:**
1. Open `VintageCashCow.ProductPricing.sln`
2. Set `VintageCashCow.ProductPricing.Api` as startup project
3. Press `F5`

### Running with Blazor App

1. Open `VintageCashCowBlazorServer.sln`
2. Set `VintageCashCow.WebUI.Server` as startup project
3. Ensure API is running
4. Configure API base URL is required 
5. Press `F5`

**Blazor Configuration:**
Configure the API base URL in your Blazor app:

builder.Services.AddScoped(sp => new HttpClient 
{ 
    BaseAddress = new Uri("http://localhost:5095") 
});

**Test the API:** Use Swagger UI at `http://localhost:5095/swagger`

### Demo Application
https://vccproductmanagement-server.azurewebsites.net/

