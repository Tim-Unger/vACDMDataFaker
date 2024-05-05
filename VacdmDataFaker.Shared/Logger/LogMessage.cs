using System.Text.Json.Serialization;

namespace VacdmDataFaker.Shared
{
    public enum LogLevel
    {
        Info,
        Warning,
        Error,
        Fatal
    }

    public class LogMessage
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public LogLevel LogLevel { get; set; }

        public DateTime LogTime { get; set; }

        public string Message { get; set; }
    }
}
