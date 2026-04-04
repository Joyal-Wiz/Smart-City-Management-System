using Microsoft.Extensions.Logging;
using SmartCity.Application.DTOs;
using SmartCity.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace SmartCity.API.Middleware
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
                // 🔥 Log the exception
                _logger.LogError(ex, "Unhandled exception occurred");

                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string message = exception.Message;

            // 🔥 Map exception types to status codes
            switch (exception)
            {
                case NotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    break;

                case BadRequestException:
                    statusCode = HttpStatusCode.BadRequest;
                    break;

                case UnauthorizedAccessException:
                    statusCode = HttpStatusCode.Unauthorized;
                    break;

                case InvalidOperationException:
                case ArgumentException:
                    statusCode = HttpStatusCode.BadRequest;
                    break;
            }

            var response = new ApiResponse<object>
            {
                Success = false,
                Message = message,
                Data = null,
                Errors = new List<string> { message }
            };

            var json = JsonSerializer.Serialize(response);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(json);
        }
    }
}