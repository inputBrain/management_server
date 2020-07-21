using System;
using Management.Server.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Management.Server.Host
{
    public class ErrorHandlingFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;

            var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>()
                                .CreateLogger(nameof(ErrorHandlingFilter));
            logger.LogError(exception.Message, exception);

            if (typeof(Exception) == typeof(Error))
            {
                var errorResponse = Error.Create(exception.Message);

                context.HttpContext.Response.WriteAsync(
                    Utf8Json.JsonSerializer.ToJsonString(
                        errorResponse
                    )
                );
                context.ExceptionHandled = true;
            }
        }
    }
}