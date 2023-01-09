using Newtonsoft.Json;

namespace TDL.APIs.Responses
{
    public class ErrorResponse
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("messageDetail")]
        public string MessageDetail { get; set; }
    }
}
