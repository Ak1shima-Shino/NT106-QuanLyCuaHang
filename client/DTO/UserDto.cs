using Newtonsoft.Json;
using System.Collections.Generic;

namespace RetailClient.DTO
{
    public class UserDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("roles")]
        public List<string> Roles { get; set; }
    }
}
