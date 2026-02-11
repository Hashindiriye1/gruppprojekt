using System.Net;
using System.Text.Json;

namespace CarWashBooking.API;

public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var body = JsonSerializer.Serialize(new { error = "An error occurred processing your request.", detail = ex.Message });
            await context.Response.WriteAsync(body);
        }
    }
}
