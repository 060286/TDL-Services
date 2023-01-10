using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using TDL.APIs.Responses;
using TDL.Infrastructure.Utilities;

namespace TDL.APIs.Configurations
{
    /// <summary>
    /// A customized class for configuring the ApiBehaviorOptions.
    /// </summary>
    public static class ApiBehaviorOption
    {
        public static IMvcBuilder ConfigureApiBehaviorOptions(this IMvcBuilder builder)
        {
            Guard.ThrowIfNull<ArgumentNullException>(builder, nameof(builder));

            builder.Services.Configure<ApiBehaviorOptions>(opts =>
            {
                opts.InvalidModelStateResponseFactory = CustomErrorResponse;
            });

            return builder;
        }

        private static BadRequestObjectResult CustomErrorResponse(ActionContext actionContext)
        {
            var errorResponseWrapper = new ErrorResponseWrapper
            {
                Errors = actionContext.ModelState
                    .Where(modelError => modelError.Value.Errors.Count > 0)
                    .Select(modelError => new ErrorResponse
                    {
                        Key = modelError.Key,
                        Message = modelError.Value.Errors.Select(x => x.ErrorMessage).FirstOrDefault()
                    }).ToList()
            };

            return new BadRequestObjectResult(errorResponseWrapper);
        }
    }
}
