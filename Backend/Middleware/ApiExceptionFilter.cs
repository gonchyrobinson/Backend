using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Backend.Middleware
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var code = HttpStatusCode.InternalServerError;
            string errorType = "ServerError";
            string? details = null;

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

            var result = new ObjectResult(new
            {
                error = errorType,
                message = exception.Message,
                details
            })
            {
                StatusCode = (int)code
            };

            context.Result = result;
            context.ExceptionHandled = true;
        }
    }
}
