using System.Text.Json;
using VacdmDataFaker.Shared;

namespace VacdmDataFaker.FlowMeasures
{
    public static class TaskRunner
    {
        private static bool _isInitialized = false;

        internal static Config Config = new();

        internal static List<LogMessage> LogMessages = new();

        internal static async Task Run()
        {
            if (_isInitialized)
            {
                LogMessages.Add(Logger.LogFatal("TaskRunner was trying to be initialized but is already running"));

                throw new InvalidOperationException("TaskRunner is already running");
            }

            _isInitialized = true;

            LogMessages.Add(Logger.LogInfo("TaskRunnter initialized"));

#if RELEASE
            Config = ConfigReader.ReadEcfmpConfig();
#else
            var rawConfig = File.ReadAllText(
                $"{Environment.CurrentDirectory}/config.json"
            );

            Config = JsonSerializer.Deserialize<Config>(rawConfig)!;
#endif

            if (!Config.UpdateAutomatically)
            {
                LogMessages.Add(Logger.LogInfo("Automatic updates disabled, data can only be updated through the API"));
                LogMessages.Add(Logger.LogInfo("TaskRunner stopped"));
                return;
            }

            while (true)
            {
                LogMessages.Add(Logger.LogInfo("Running Tasks"));

                FlowMeasureFaker.FakeMeasures(Config.InitialAmount);

                var updateInterval = Config.UpdateInterval;

                LogMessages.Add(Logger.LogInfo($"Success, added 10 Measures, Next Update at: {DateTime.UtcNow.AddMinutes(updateInterval):HH:mm}Z"));

                await Task.Delay(TimeSpan.FromMinutes(updateInterval));
            }
        }
    }
}
