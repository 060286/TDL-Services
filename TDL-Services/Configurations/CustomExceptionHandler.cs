using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Threading.Tasks;
using TDL.APIs.Responses;
using TDL.Infrastructure.Constants;
using TDL.Infrastructure.Exceptions;

namespace TDL.APIs.Configurations
{
    public class CustomExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandler> _logger;

        public CustomExceptionHandler(RequestDelegate next, ILogger<CustomExceptionHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch(Exception ex)
            {
                await HandleException(httpContext, ex);
            }
        }

        private async Task HandleException(HttpContext context, Exception exception)
        {
            context.Response.ContentType = ConfigurationConstant.DefaultContentType;
            switch(exception)
            {
                case BusinessLogicException _:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case NotFoundException _:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                case UnsupportedTypeException _:
                    context.Response.StatusCode = (int)HttpStatusCode.UnsupportedMediaType;
                    break;

                case UnauthorizedAccessException _:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;

                case DBConcurrencyException _:
                    context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                    break;

                default:
                    _logger.LogError(exception, exception.Message);
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            ErrorResponseWrapper errorResponseWrapper = new ErrorResponseWrapper
            {
                Errors = new List<ErrorResponse>
                {
                    new ErrorResponse
                    {
                        Message = exception.Message,
                        MessageDetail = exception.InnerException?.Message
                    }
                }
            };

            await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponseWrapper));
        }
    }
}
