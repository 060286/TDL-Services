using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TDL.APIs.Responses;
using TDL.Infrastructure.Constants;
using TDL.Infrastructure.Extensions;

namespace TDL.APIs.Configurations
{
    /// <summary>
    /// Middleware for wraping the response in a meaning ful format
    /// </summary>
    public class CustomResponseWrapper
    {
        private readonly RequestDelegate _next;

        public CustomResponseWrapper(RequestDelegate next)
        {
            _next= next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if(HasSpecialRequest(httpContext))
            {
                await _next(httpContext);
                return;
            }

            var originalResponseBody = httpContext.Response.Body;

            httpContext.Response.OnStarting(() =>
            {
                httpContext.Response.ContentType = ConfigurationConstant.DefaultContentType;
                return Task.CompletedTask;
            });

            using (var tempStream = new MemoryStream())
            {
                httpContext.Response.Body = tempStream;

                await _next(httpContext);

                if(httpContext.Response.StatusCode != (int)HttpStatusCode.NoContent)
                {
                    await BuilResponseWrapper(httpContext, originalResponseBody);
                }

                httpContext.Response.Body = originalResponseBody;
            }
        }

        private async Task BuilResponseWrapper(HttpContext httpContext, Stream originResponseBody)
        {
            var responseBody = httpContext.Response.Body;
            responseBody.Position = 0;

            var apiVersion = httpContext.Features.Get<IApiVersioningFeature>()?.RequestedApiVersion;
            var bodyStr = await new StreamReader(responseBody).ReadToEndAsync();
            var response = TransformResult(bodyStr, apiVersion, httpContext.Response.StatusCode);

            var buffer = Encoding.UTF8.GetBytes(response);

            using (var output = new MemoryStream(buffer))
            {
                httpContext.Response.ContentLength = buffer.Length;
                await output.CopyToAsync(originResponseBody);
            }
        }

        private string TransformResult(string result, ApiVersion apiVersion, int statusCode)
        {
            const int startedHttpErrorStatusCode = 400;

            var baseResponse = new BaseResponse
            {
                IsSuccess = false,
                ApiVersion = apiVersion != null
                    ? $"{apiVersion.MajorVersion}.{apiVersion.MinorVersion ?? 0}"
                    : ConfigurationConstant.DefaultApiVersion,
                StatusCode = statusCode,
            };

            switch (statusCode)
            {
                case var _ when statusCode < startedHttpErrorStatusCode:
                    if(result.TryParse(out ErrorResponseWrapper errorWrapper1) && 
                        errorWrapper1 != null && 
                        !errorWrapper1.Errors.IsNullOrEmpty())
                    {
                        baseResponse.Data = errorWrapper1.Errors;

                        break;
                    }

                    baseResponse.IsSuccess = true;
                    baseResponse.Data = result.ToObject<object>();

                    break;

                case var _ when statusCode == (int)HttpStatusCode.Forbidden:
                    baseResponse.Data = new List<ErrorResponse>
                    {
                        new ErrorResponse
                        {
                            Message = ExceptionConstant.RestrictedResource
                        }
                    };

                    break;

                default: 
                    if(result.TryParse(out ErrorResponseWrapper errorWrapper) &&
                        errorWrapper != null &&
                        !errorWrapper.Errors.IsNullOrEmpty())
                    {
                        baseResponse.Data = errorWrapper.Errors;
                        break;
                    }

                    baseResponse.Data = result.ToObject<object>();

                    break;
            }

            return JsonConvert.SerializeObject(baseResponse);
        }

        private bool HasSpecialRequest(HttpContext httpContext)
        {
            return httpContext != null &&
                   (httpContext.Request.Path.Value.ContainInvariant("Swagger") ||
                   httpContext.Request.Path.Value.ContainInvariant("HangFire") ||
                   httpContext.Request.Path.Value.ContainInvariant("download") ||
                   httpContext.Request.Path.Value.ContainInvariant("image") ||
                   httpContext.Request.Path.Value.StartsWith("/hubs"));
        }
    }
}
