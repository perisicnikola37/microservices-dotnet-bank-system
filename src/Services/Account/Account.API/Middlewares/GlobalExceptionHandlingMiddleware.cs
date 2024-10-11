using System.Net;
using System.Text.Json;
using Account.Application.Exceptions;

namespace Account.API.Middlewares;

public class GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (EntityNotFoundException ex)
        {
            await HandleExceptionAsync(context, HttpStatusCode.NotFound, ex);
        }
        catch (InsufficientBalanceException ex)
        {
            await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            await HandleExceptionAsync(context, HttpStatusCode.Forbidden, ex);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, Exception ex)
    {
        logger.LogError(ex, $"Exception: {ex.Message} | Stack Trace: {ex.StackTrace}");

        var response = new
        {
            error = ex.Message,
            date = DateTime.UtcNow,
            stackTrace = ex.StackTrace
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var result = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(result);
    }
    }