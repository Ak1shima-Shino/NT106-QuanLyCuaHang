using Newtonsoft.Json;

namespace RetailClient.DTO
{
    public class ApiErrorResponse
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}