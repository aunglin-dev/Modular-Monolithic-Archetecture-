using System.Net;
using LoginSolution.Shared.Domain.Exceptions;
using LoginSolution.Shared.Domain.Models.Common;
using DomainValidationException = LoginSolution.Shared.Domain.Exceptions.ValidationException;

namespace LoginSolution.IBanking.Api.Middleware;

public sealed class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IWebHostEnvironment _environment;
    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IWebHostEnvironment environment) { _next = next; _logger = logger; _environment = environment; }
    public async Task InvokeAsync(HttpContext context)
    {
        try { await _next(context); }
        catch (Exception ex)
        {
            var (status, message, errors) = ex switch
            {
                DomainValidationException validation => (HttpStatusCode.BadRequest, validation.Message, validation.Errors),
                UnauthorizedException => (HttpStatusCode.Unauthorized, ex.Message, Array.Empty<string>()),
                NotFoundException => (HttpStatusCode.NotFound, ex.Message, Array.Empty<string>()),
                _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.", _environment.IsDevelopment() ? new[] { ex.Message } : Array.Empty<string>())
            };
            if (status == HttpStatusCode.InternalServerError) _logger.LogError(ex, "Unhandled API exception");
            context.Response.StatusCode = (int)status;
            await context.Response.WriteAsJsonAsync(ApiResponseModel.Failure(message, errors));
        }
    }
}
