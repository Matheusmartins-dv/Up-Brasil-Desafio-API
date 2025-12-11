using Application.Common.Behaviors;

namespace Api.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        if (exception.GetType().Name.Contains("AlreadyExist")) 
        {
             context.Response.StatusCode = StatusCodes.Status409Conflict;
        }
        else 
        {
             context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        }

        var response = new ApiResponse<object>(
            message: exception.Message 
        );

        var jsonResponse = System.Text.Json.JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(jsonResponse);
    }
}
