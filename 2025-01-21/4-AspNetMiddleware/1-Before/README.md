# IntroductionToDotNetAndWebApi-UniPartners-2025
ASP.NET Middlewares (Request/Response pipeline)

## Een WebApi service maken

Maak een nieuw ASP.NET WebApi project aan:

```
dotnet new webapi --name WebApiController
```

Maak de gegenereerde template-code wat schoon totdat je dit overhoudt:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/status", () =>
{
    return Results.Ok();
}).WithName("GetWeatherForecast");

app.Run();
```

Voeg een 'rest.http' bestand toe en test het status endpoint:

```py
@baseUrl = http://localhost:????

### GeefStatus
GET {{baseUrl}}/status
```

Ga terug naar 'Program.cs' en voeg de volgende twee Middleware klassen toe:

```csharp
class FirstMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<FirstMiddleware> _logger;

    public FirstMiddleware(
        RequestDelegate next,
        ILogger<FirstMiddleware> logger)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        _logger.LogInformation($"{nameof(FirstMiddleware)} - Request");

        // Call the next delegate/middleware in the pipeline.
        await _next(context);

        _logger.LogInformation($"{nameof(FirstMiddleware)} - Response");
    }
}
```

```csharp
class SecondMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<SecondMiddleware> _logger;

    public SecondMiddleware(
        RequestDelegate next,
        ILogger<SecondMiddleware> logger)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        _logger.LogInformation($"{nameof(SecondMiddleware)} - Request");

        // Call the next delegate/middleware in the pipeline.
        await _next(context);

        _logger.LogInformation($"{nameof(SecondMiddleware)} - Response");
    }
}
```

Registreer nu de twee Middleware klassen:

```csharp
app.UseMiddleware<FirstMiddleware>();
app.UseMiddleware<SecondMiddleware>();
```

Bij het uitvoeren van een request zal je nu de volgorde van bewerkingen zien via de console-logging:

```
info: FirstMiddleware[0]
      FirstMiddleware - Request
info: SecondMiddleware[0]
      SecondMiddleware - Request
info: SecondMiddleware[0]
      SecondMiddleware - Response
info: FirstMiddleware[0]
      FirstMiddleware - Response
```

Middleware kan je gebruiken om de request/response pipeline te beïnvloeden en dus requests en responses te onderzoeken en eventueel te wijzigen:

Voeg de volgende lijnen toe aan je 'FirstMiddleware' en 'FirstMiddleware':

```csharp
context.Response.Headers.Add("X-First-Middleware", $"{Guid.NewGuid()}");
```

```csharp
context.Response.Headers.Add("X-Second-Middleware", $"{Guid.NewGuid()}");
```

```csharp
_logger.LogInformation($"{context.Request.Protocol} {context.Request.Scheme}://{context.Request.Host}{context.Request.Path}");
```

```csharp
_logger.LogInformation($"{context.Response.StatusCode}");
```