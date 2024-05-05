namespace VacdmDataFaker.Shared
{
    public static partial class Logger
    {
        private static LogMessage LogFatalAndPost(string message)
        {
            var now = DateTime.UtcNow;

            Console.WriteLine($"[{now:s}Z] [INFO] {message}");

            return new LogMessage()
            {
                LogLevel = LogLevel.Fatal,
                LogTime = now,
                Message = message
            };
        }
    }
}
