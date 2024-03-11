using System.Text.Json.Serialization;

namespace VacdmDataFaker.FlowMeasures
{
    public class Config
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
