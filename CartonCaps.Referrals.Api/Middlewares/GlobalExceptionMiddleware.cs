namespace CartonCaps.Referrals.Api.Middlewares
{
    using CartonCaps.Referrals.Api.Contracts.Common;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using System.Net;
    using System.Text.Json;

    public sealed class GlobalExceptionMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(ILogger<GlobalExceptionMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
            {
                context.Response.StatusCode = StatusCodes.Status499ClientClosedRequest;
            }
            catch (Exception ex)
            {
                
                var (statusCode, code, message) = MapException(ex);

                _logger.LogError(ex,
                    "Unhandled exception. Path={Path} Status={StatusCode} Code={Code}", context.Request.Path, statusCode, code);

                if (context.Response.HasStarted)
                    throw; 

                context.Response.Clear();
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = statusCode;

                var response = new ApiResponse<object>
                {
                    Success = false,
                    Status = statusCode,
                    Data = null,
                    Errors =
                    new List<ApiError>()
                    {
                        new ApiError
                        {
                        Code = code,
                        Message = message,
                        }
                    }
                };

                var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                await context.Response.WriteAsync(json);
            }
        }

        private static (int StatusCode, string Code, string Message) MapException(Exception ex)
        {
            
            return ex switch
            {
                ArgumentException => (StatusCodes.Status400BadRequest, "BadRequest", ex.Message),

                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized", "Unauthorized."),

                KeyNotFoundException => (StatusCodes.Status404NotFound, "NotFound", "Resource not found."),

                // fallback
                _ => (StatusCodes.Status500InternalServerError, "ServerError", "An unexpected error occurred.")
            };
        }
    }

}
