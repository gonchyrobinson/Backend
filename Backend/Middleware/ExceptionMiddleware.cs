using System.Net;
using System.Text.Json;

namespace Backend.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            string errorType = "ServerError";
            string? details = null;

            // Puedes personalizar el mapeo según tus excepciones
            if (exception.GetType().Name.Contains("ValidationException"))
            {
                code = HttpStatusCode.BadRequest;
                errorType = "ValidationError";
            }
            else if (exception.GetType().Name.Contains("NotFoundException"))
            {
                code = HttpStatusCode.NotFound;
                errorType = "NotFound";
            }
            else if (exception.GetType().Name.Contains("AppException"))
            {
                code = HttpStatusCode.BadRequest;
                errorType = "AppError";
            }

            var result = JsonSerializer.Serialize(new
            {
                error = errorType,
                message = exception.Message,
                details
            });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
