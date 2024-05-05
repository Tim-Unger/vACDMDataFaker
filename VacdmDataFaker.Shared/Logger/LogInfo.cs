namespace VacdmDataFaker.Shared
{
    public static partial class Logger
    {
        private static LogMessage LogInfoAndPost(string message)
        {
            var now = DateTime.UtcNow;

            Console.WriteLine($"[{now:s}Z] [INFO] {message}");

            return new LogMessage()
            {
                LogLevel = LogLevel.Info,
                LogTime = now,
                Message = message,
            };
        }
    }
}
