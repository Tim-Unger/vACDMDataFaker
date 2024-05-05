namespace VacdmDataFaker.Shared
{
    public static partial class Logger
    {
        private static LogMessage LogWarningAndPost(string message)
        {
            var now = DateTime.UtcNow;

            Console.WriteLine($"[{now:s}Z] [WARN] {message}");

            return new LogMessage
            {
                LogLevel = LogLevel.Warning,
                LogTime = now,
                Message = message,
            };
        }
    }
}
