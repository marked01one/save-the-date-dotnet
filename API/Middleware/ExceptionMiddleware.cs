using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.Middleware
{
  public class ExceptionMiddleware
  {
    private readonly RequestDelegate next;
    private readonly ILogger<Exception> logger;
    private readonly IHostEnvironment env;
    public ExceptionMiddleware(RequestDelegate next, ILogger<Exception> logger, IHostEnvironment env)
    {
      this.env = env;
      this.logger = logger;
      this.next = next;
    }

    // Because of ASP.NET, the method for middleware must be called `InvokeAsync()`
    // Middleware goes sequentially, always calling `next` (RequestDelegate)
    // `HttpContext` will allow us to access the current HTTP request that's passing through the middleware
    public async Task InvokeAsync(HttpContext context)
    {
      try
      {
        await this.next(context);
      }
      catch (Exception ex)
      {
        // Log the caught error
        this.logger.LogError(ex, ex.Message);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

        // Create exception response based on whether it's development or production
        var response = this.env.IsDevelopment() 
          ? new ApiException(
            statusCode: context.Response.StatusCode,
            message: ex.Message,
            details: ex.StackTrace?.ToString())
          : new ApiException(
            statusCode: context.Response.StatusCode,
            message: ex.Message,
            details: "Internal Server Error");

        var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};

        var json = JsonSerializer.Serialize(response, options);

        await context.Response.WriteAsync(json);
      }
    }
  }
}