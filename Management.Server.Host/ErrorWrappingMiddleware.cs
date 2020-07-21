using System;
using System.Net;
using System.Threading.Tasks;
using Management.Server.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Management.Server.Host
{

    internal class ErrorWrappingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorWrappingMiddleware> _logger;


        public ErrorWrappingMiddleware(RequestDelegate next, ILogger<ErrorWrappingMiddleware> logger)
        {
            _next = next;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public async Task Invoke(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            var json = "";
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            try
            {
                await _next.Invoke(context);
            }
            catch (ErrorException errorException)
            {
                context.Response.StatusCode = (int) HttpStatusCode.NotAcceptable;
                _logger.LogError(errorException, errorException.Error.Type + " : " + errorException.Error.Message);
                json = JsonConvert.SerializeObject(errorException.Error, serializerSettings);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                json = JsonConvert.SerializeObject(Error.Create("server error"),
                    serializerSettings);
            }

            if (!context.Response.HasStarted)
            {
                await context.Response.WriteAsync(json);
            }
        }
    }
}