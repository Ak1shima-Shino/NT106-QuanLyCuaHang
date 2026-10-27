using Newtonsoft.Json;

namespace RetailClient.DTO
{
    public class ForgotPasswordRequest
    {
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
