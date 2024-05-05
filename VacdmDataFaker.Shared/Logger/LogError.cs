namespace VacdmDataFaker.Shared
{
    public static partial class Logger
    {
        private static LogMessage LogErrorAndPost(string message)
        {
            var now = DateTime.UtcNow;

            Console.WriteLine($"[{now:s}Z] [INFO] {message}");

            return new LogMessage()
            {
                LogLevel = LogLevel.Error,
                LogTime = now,
                Message = message
            };
        }
    }
}
