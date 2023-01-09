using Newtonsoft.Json;

namespace TDL.APIs.Responses
{
    public class BaseResponse
    {
        [JsonProperty("apiVersion")]
        public string ApiVersion { get; set; }

        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }

        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }
    }
}
