using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using System.Collections.Generic;
using TDL.APIs.Responses;

namespace TDL.APIs.Configurations
{
    public class ApiVersioningErrorResponseProvider : DefaultErrorResponseProvider
    {
        public override IActionResult CreateResponse(ErrorResponseContext context)
        {
            var errorWrapper = new ErrorResponseWrapper
            {
                Errors = new List<ErrorResponse>
                {
                    new ErrorResponse
                    {
                        Message = context.Message,
                        MessageDetail = context.MessageDetail
                    }
                }
            };

            return new ObjectResult(errorWrapper)
            {
                StatusCode = context.StatusCode
            };
        }
    }
}
