namespace VacdmDataFaker.Shared
{
    public static partial class Logger
    {
        public static LogMessage LogInfo(string message) => LogInfoAndPost(message);

        public static LogMessage LogWarning(string message) => LogWarningAndPost(message);

        public static LogMessage LogError(string message) => LogErrorAndPost(message);

        public static LogMessage LogFatal(string message) => LogFatalAndPost(message);
    }
}
