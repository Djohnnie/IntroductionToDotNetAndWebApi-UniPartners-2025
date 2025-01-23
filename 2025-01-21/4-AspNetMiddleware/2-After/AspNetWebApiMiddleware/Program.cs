var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddTransient<Test>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseMiddleware<FirstMiddleware>();
app.UseMiddleware<SecondMiddleware>();

app.MapGet("/status", () =>
{
    return Results.Ok();
}).WithName("GetWeatherForecast");

app.Run();


class FirstMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<FirstMiddleware> _logger;
    private readonly Test _test;

    public FirstMiddleware(
        RequestDelegate next,Test test,
        ILogger<FirstMiddleware> logger)
    {
        _test = test;
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, Test test)
    {
        _logger.LogInformation($"{nameof(FirstMiddleware)} - Request");
        _logger.LogInformation($"{context.Request.Protocol} {context.Request.Scheme}://{context.Request.Host}{context.Request.Path}");

        // Call the next delegate/middleware in the pipeline.
        await _next(context);

        context.Response.Headers.Add("X-First-Middleware", $"{Guid.NewGuid()}");

        _logger.LogInformation($"{nameof(FirstMiddleware)} - Response");
        _logger.LogInformation($"{context.Response.StatusCode}");
    }
}

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

        context.Response.Headers.Add("X-Second-Middleware", $"{Guid.NewGuid()}");

        _logger.LogInformation($"{nameof(SecondMiddleware)} - Response");
    }
}


class Test
{

}