using Newtonsoft.Json;

namespace RetailClient.DTO
{
    public class RegisterResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("user_id")]
        public int UserId { get; set; }
    }
}
