using System.Text.Json.Serialization;

namespace VacdmDataFaker.Vacdm
{
    public class FlightPlan
    {
        [JsonPropertyName("flight_rules")]
        public string FlightRules { get; set; }

        [JsonPropertyName("departure")]
        public string Departure { get; set; }

        [JsonPropertyName("arrival")]
        public string Arrival { get; set; }
    }
}
