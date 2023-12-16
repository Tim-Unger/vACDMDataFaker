using System.Text.Json.Serialization;

namespace VacdmDataFaker
{
    internal class ApiStatus
    {
        [JsonPropertyName("msg")]
        internal string Message;
    }
}
