using Newtonsoft.Json;
using System.Collections.Generic;

namespace TDL.APIs.Responses
{
    public class ErrorResponseWrapper
    {
        [JsonProperty("errors")]
        public IList<ErrorResponse> Errors { get; set; }
    }
}
