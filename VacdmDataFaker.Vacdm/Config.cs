using System.Text.Json.Serialization;
using VacdmDataFaker.Shared;

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

        [JsonPropertyName("requireAuthenticationForLogs")]
        public bool RequireAuthenticationForLogs { get; set; } = true;

        [JsonPropertyName("allowNonDevCids")]
        public bool AllowNonDevCids { get; set; } = false;

        [JsonPropertyName("airports")]
        public List<string> Airports { get; set; } =
            new() { "EDDF", "EDDS", "EDDK", "EDDM", "EDDL", "EDDH", "EDDB" };

        //Casting Shared.VacdmConfig to Config
        public static implicit operator Config(VacdmConfig v)
        {
            return new Config()
            {
                Cid = v.Cid,
                Password = v.Password,
                Url = v.Url,
                UpdateAutomatically = v.UpdateAutomatically,
                UpdateInterval = v.UpdateInterval,
                MinimumAmount = v.MinimumAmount,
                MaximumAmount = v.MaximumAmount,
                DeleteAllOnStartup = v.DeleteAllOnStartup,
                RequireAuthenticationForLogs = v.RequireAuthenticationForLogs,
                AllowNonDevCids = v.AllowNonDevCids,
                Airports = v.Airports,
            };
        }
    }
}
