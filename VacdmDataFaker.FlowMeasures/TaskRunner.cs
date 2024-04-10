using System.Text.Json;

namespace VacdmDataFaker.FlowMeasures
{
    public static class TaskRunner
    {
        private static bool _isInitialized = false;

        internal static Config Config = new();

        internal static async Task Run()
        {
            if (_isInitialized)
            {
                var nowErr = DateTime.UtcNow;

                Console.WriteLine($"[{nowErr:s}Z] [FATAL] TaskRunnter was trying to be initialized but is already running");

                throw new InvalidOperationException("TaskRunner is already running");
            }

            _isInitialized = true;

            var now = DateTime.UtcNow;

            Console.WriteLine($"[{now:s}Z] [INFO] TaskRunner initialized");

#if RELEASE
            Config = ReadConfig.FromEnv();
#else
            var rawConfig = File.ReadAllText(
                $"{Environment.CurrentDirectory}/config.json"
            );

            Config = JsonSerializer.Deserialize<Config>(rawConfig)!;
#endif

            if (!Config.UpdateAutomatically)
            {
                Console.WriteLine(
                    $"[{DateTime.UtcNow:s}Z] [INFO] Automatic updates disabled, data can only be updated through the API"
                );

                Console.WriteLine($"[{DateTime.UtcNow:s}Z] [INFO] TaskRunner stopped");

                return;
            }

            while (true)
            {

                var nowRunner = DateTime.UtcNow;
                Console.WriteLine($"[{nowRunner:s}Z] [INFO] Running Tasks");

                FlowMeasureFaker.FakeMeasures(Config.InitialAmount);

                var updateInterval = Config.UpdateInterval;

                Console.WriteLine($"[{nowRunner:s}Z] [INFO] Success, added 10 Measures, Next Update at: {nowRunner.AddMinutes(updateInterval):HH:mm}Z");

                await Task.Delay(TimeSpan.FromMinutes(updateInterval));
            }
        }
    }
}
