﻿using System.Text.Json.Serialization;

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
    }
}
