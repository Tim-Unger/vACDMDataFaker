namespace VacdmDataFaker.Shared
{
    interface ILogger
    {
        LogMessage LogInfo(string message);

        LogMessage LogWarning(string message);

        LogMessage LogError(string message);

        LogMessage LogFatal(string message);
    }
}
