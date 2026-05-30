using CleanAPI.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace CleanAPI.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
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

        private async Task HandleExceptionAsync(
            HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            var message = "Something went wrong";

            switch (exception)
            {
                case NotFoundException notFound:
                    statusCode = HttpStatusCode.NotFound;
                    message = notFound.Message;
                    _logger.LogWarning("Not Found: {Message}", notFound.Message);
                    break;

                case BadRequestException badRequest:
                    statusCode = HttpStatusCode.BadRequest;
                    message = badRequest.Message;
                    _logger.LogWarning("Bad Request: {Message}", badRequest.Message);
                    break;

                case ForbiddenException forbidden:
                    statusCode = HttpStatusCode.Forbidden;
                    message = forbidden.Message;
                    _logger.LogWarning("Forbidden: {Message}", forbidden.Message);
                    break;

                default:
                    _logger.LogError(exception,
                        "Unexpected error: {Message}", exception.Message);
                    break;
            }

            var response = new
            {
                status = (int)statusCode,
                message = message,
                traceId = context.TraceIdentifier,
                timestamp = DateTime.UtcNow
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }));
        }
    }
}
