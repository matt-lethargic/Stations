using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Stations.Core.SharedKernel.Exceptions;
using Stations.Web.Models;

namespace Stations.Web.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (DomainValidationException ex)
            {
                _logger.LogWarning($"Validation failed");
                await HandleValidationExceptionAsync(httpContext, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            string data = JsonConvert.SerializeObject(new
            {
                StatusCode = context.Response.StatusCode,
                Message = "Internal Server Error."
            });

            return context.Response.WriteAsync(data);
        }


        private static Task HandleValidationExceptionAsync(HttpContext context, DomainValidationException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;


            var error = new ValidationErrorModel()
            {
                Errors = exception.Errors.Select(x => new ValidationErrorMessageModel(x.Key, x.Value)).ToList()
            };

            string data = JsonConvert.SerializeObject(error);

            return context.Response.WriteAsync(data);
        }
    }
}
