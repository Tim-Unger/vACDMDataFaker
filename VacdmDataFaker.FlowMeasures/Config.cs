using System.Text.Json.Serialization;
using VacdmDataFaker.Shared;

namespace VacdmDataFaker.FlowMeasures
{
    public class Config
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("updateAutomatically")]
        public bool UpdateAutomatically { get; set; } = true;

        [JsonPropertyName("updateInterval")]
        public int UpdateInterval { get; set; } = 10;

        [JsonPropertyName("initialAmount")]
        public int InitialAmount { get; set; } = 10;

        [JsonPropertyName("requireAuthenticationForLogs")]
        public bool RequireAuthenticationForLogs { get; set; } = true;

        public static implicit operator Config(EcfmpConfig v)
        {
            return new Config()
            {
                Username = v.Username,
                Password = v.Password,
                UpdateAutomatically = v.UpdateAutomatically,
                UpdateInterval = v.UpdateInterval,
                InitialAmount = v.InitialAmount,
                RequireAuthenticationForLogs = v.RequireAuthentificationForLogs
            };
        }
    }
}
