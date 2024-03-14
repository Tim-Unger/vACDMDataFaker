using System.Text.Json.Serialization;

namespace VacdmDataFaker.Vacdm
{
    public class Position
    {
        [JsonPropertyName("lat")]
        public float Latitude { get; set; }

        [JsonPropertyName("lon")]
        public float Longitude { get; set; }
    }
}
