using Newtonsoft.Json;

public class MeResponse
{
    [JsonProperty("success")]
    public bool Success { get; set; }

    [JsonProperty("user")]
    public UserDto User { get; set; }
}