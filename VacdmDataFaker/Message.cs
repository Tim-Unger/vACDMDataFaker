using System.Text.Json.Serialization;

namespace VacdmDataFaker.Vacdm
{
    internal class ApiStatus
    {
        [JsonPropertyName("msg")]
        internal string Message;
    }
}
