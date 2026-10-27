using Newtonsoft.Json;

namespace RetailClient.DTO
{
    public class ResetPasswordRequest
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("new_password")]
        public string NewPassword { get; set; }
    }
}
