using Microsoft.Net.Http.Headers;
using System.Net;
using VerticalSliceMinimalApi.Domain.Account.Exceptions;

namespace VerticalSliceMinimalApi.API.Middleware;

public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionMiddleware>();
    }

    public static Task EventSchedulingErrorResponseAsync(this HttpResponse response,
      Exception businessException)
    {
        var (httpStatusCode, eventId) = GetResponseCode(businessException.GetType().Name);
        var message = $"{{\"code\": {eventId},\"message\":\"{businessException.Message}\"}}";
        response.Clear();
        response.StatusCode = (int)httpStatusCode;
        response.ContentType = "application/json";

        response.GetTypedHeaders().CacheControl =
          new CacheControlHeaderValue { NoStore = true, NoCache = true };

        response.WriteAsync(message);
        return Task.FromResult(response.StatusCode);
    }

    private static (HttpStatusCode, EventId) GetResponseCode(string exception)
    {
        return exception switch
        {
            // BadRequest
            nameof(ArgumentException) => (HttpStatusCode.BadRequest, LoggingEvents.ArgumentException),
            nameof(AccountNotFoundException) => (HttpStatusCode.BadRequest, LoggingEvents.AccountNotFoundException),


            // Conflict
            nameof(AccountAlreadyExistsException) => (HttpStatusCode.Conflict, LoggingEvents.AccountAlreadyExistsException),

            //Default
            _ => (HttpStatusCode.InternalServerError, LoggingEvents.Unknown)
        };
    }
}
