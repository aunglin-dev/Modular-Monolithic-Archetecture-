# Run MVC, API, Blazor Projects via CLI


## Terminal 1:
```
dotnet run --project Mvc --urls "https://localhost:7000;http://localhost:5000"
```
## Terminal 2:
```
dotnet run --project LoginApi/Api --urls "https://localhost:7001;http://localhost:5001"
```
## Terminal 3:

    dotnet run --project LoginApi/Blazor --urls "https://localhost:7002;http://localhost:5002"

## Run in Browser (e.g. Chrome)

**MVC:**  
https://localhost:7000  
  
**API Swagger:**  
https://localhost:7001/swagger  
  
**Blazor:**  
https://localhost:7002

## Login Credentials

**Email:** admin@example.com  
**Password:** Pass@123  
**Role:** Admin
