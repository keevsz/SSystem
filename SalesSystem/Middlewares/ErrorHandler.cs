using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace SalesSystem.Middlewares
{
    public class ErrorHandler
    {
        private readonly RequestDelegate _next;

        public ErrorHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = GetStatusCode(exception);
            
            string errorMessage = "Error";
            if (exception.InnerException != null)
            {
                errorMessage += $": {exception.InnerException.Message}";
            }
            else
            {
                errorMessage += $": {exception.Message}";
            }

            var errorResponse = new
            {
                error = "Error",
                message = errorMessage
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }

        private static int GetStatusCode(Exception exception)
        {
            if (exception is DbUpdateException)
            {
                return (int)HttpStatusCode.BadRequest;
            }

            return (int)HttpStatusCode.InternalServerError;
        }
    }
}
