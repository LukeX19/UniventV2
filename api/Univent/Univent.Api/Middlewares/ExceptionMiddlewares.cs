using System.Net;
using System.Text.Json;
using Univent.Api.Models;
using Univent.App.Exceptions;
using Univent.Infrastructure.Exceptions;

namespace Univent.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex) when (ex is EntityNotFoundException || ex is AWSFileNotFoundException)
            {
                await HandleCustomExceptionAsync(context, ex, HttpStatusCode.NotFound);
            }
            catch (Exception ex) when (ex is EntityAlreadyExistsException || ex is NameConflictException
                || ex is NameConflictWithStatusException || ex is AccountAlreadyExistsException || ex is StatusConflictException
                || ex is EventMaximumParticipantsReachedException || ex is EventAuthorEnrollmentException)
            {
                await HandleCustomExceptionAsync(context, ex, HttpStatusCode.Conflict);
            }
            catch (InvalidCredentialsException ex)
            {
                await HandleCustomExceptionAsync(context, ex, HttpStatusCode.Unauthorized);
            }
            catch (InvalidFileFormatException ex)
            {
                await HandleCustomExceptionAsync(context, ex, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                await HandleCustomExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
            }
        }

        private async Task HandleCustomExceptionAsync(HttpContext context, Exception ex, HttpStatusCode httpStatusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)httpStatusCode;
            var error = new Error
            {
                StatusCode = context.Response.StatusCode,
                Message = ex.Message
            };

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var serializedError = JsonSerializer.Serialize(error, options);

            await context.Response.WriteAsync(serializedError);
        }
    }
}
