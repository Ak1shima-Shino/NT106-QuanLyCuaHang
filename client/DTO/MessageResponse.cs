using Newtonsoft.Json;

namespace RetailClient.DTO
{
    public class MessageResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}