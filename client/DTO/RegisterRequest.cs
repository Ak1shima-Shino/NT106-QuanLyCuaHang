using Newtonsoft.Json;

namespace RetailClient.DTO
{
    public class RegisterRequest
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
