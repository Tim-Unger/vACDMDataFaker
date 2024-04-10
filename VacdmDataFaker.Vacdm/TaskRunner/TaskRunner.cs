using System.Text.Json;

namespace VacdmDataFaker.Vacdm
{
    internal class TaskRunner
    {
        private static bool _isInitialized = false;

        private static bool _isFirstLoad = true;

        internal static Config Config { get; set; } = new();

        internal static async Task Run()
        {
            if (_isInitialized)
            {
                var nowErr = DateTime.UtcNow;

                Console.WriteLine(
                    $"[{nowErr:s}Z] [FATAL] TaskRunner was trying to be initialized but is already running"
                );

                throw new InvalidOperationException("TaskRunner is already running");
            }

            _isInitialized = true;

            var now = DateTime.UtcNow;

            Console.WriteLine($"[{now:s}Z] [INFO] TaskRunner initialized");

#if RELEASE
            Config = ReadConfig.FromEnv();
#else
            var jsonOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            Config =
                JsonSerializer.Deserialize<Config>(
                    File.ReadAllText($"{Environment.CurrentDirectory}/config.json"),
                    jsonOptions
                ) ?? throw new InvalidDataException();
#endif

            Console.WriteLine($"[{DateTime.UtcNow:s}Z] [INFO] Read Config");

            if(_isFirstLoad)
            {
                _isFirstLoad = false;

                if (Config.DeleteAllOnStartup)
                {
                    await VacdmPilotFaker.DeleteAllAsync();

                    Console.WriteLine(
                        $"[{now:s}Z] [INFO] First run, Deleted all old Pilots"
                    );
                }
            }

            if(!Config.UpdateAutomatically)
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

                await VacdmPilotFaker.RunUpdate();

                var updateInterval = Config.UpdateInterval;

                Console.WriteLine(
                    $"[{nowRunner:s}Z] [INFO] Success, updated pilots, Next update at: {nowRunner.AddMinutes(updateInterval):HH:mm}Z"
                );

                await Task.Delay(TimeSpan.FromMinutes(updateInterval));
            }
        }
    }
}
