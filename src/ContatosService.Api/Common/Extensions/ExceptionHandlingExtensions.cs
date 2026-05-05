using ContatosService.Api.Common.Errors;
using ContatosService.Application.Common.Exceptions;
using ContatosService.Domain.Common;
using Microsoft.AspNetCore.Diagnostics;

namespace ContatosService.Api.Common.Extensions;

public static class ExceptionHandlingExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                var (statusCode, message) = MapearErro(exception);

                if (exception is not DomainException and not NotFoundException and not ConflictException)
                {
                    var logger = context.RequestServices
                        .GetRequiredService<ILoggerFactory>()
                        .CreateLogger("GlobalExceptionHandling");

                    logger.LogError(exception, "Erro não tratado na requisição.");
                }

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(new ErrorResponse(statusCode, message));
            });
        });

        return app;
    }

    private static (int StatusCode, string Message) MapearErro(Exception? exception)
    {
        return exception switch
        {
            DomainException domainException => (StatusCodes.Status400BadRequest, domainException.Message),
            NotFoundException notFoundException => (StatusCodes.Status404NotFound, notFoundException.Message),
            ConflictException conflictException => (StatusCodes.Status409Conflict, conflictException.Message),
            _ => (StatusCodes.Status500InternalServerError, "Erro interno do servidor.")
        };
    }
}
