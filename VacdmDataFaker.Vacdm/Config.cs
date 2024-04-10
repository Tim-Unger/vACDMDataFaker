using System.Text.Json.Serialization;

namespace VacdmDataFaker.Vacdm
{
    public class Config
    {
        [JsonPropertyName("cid")]
        public int Cid { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; } = "";

        [JsonPropertyName("url")]
        public string Url { get; set; } = "";

        [JsonPropertyName("updateAutomatically")]
        public bool UpdateAutomatically { get; set; } = true;

        [JsonPropertyName("updateInterval")]
        public int UpdateInterval { get; set; } = 10;

        [JsonPropertyName("mimimumAmount")]
        public int MinimumAmount { get; set; } = 10;

        [JsonPropertyName("maximumAmount")]
        public int MaximumAmount { get; set; } = 50;

        [JsonPropertyName("deleteAllOnStartup")]
        public bool DeleteAllOnStartup { get; set; } = true;
    }
}
