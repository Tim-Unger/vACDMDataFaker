using System.Text.Json;

namespace VacdmDataFaker.Vacdm
{
    internal class TaskRunner
    {
        private static bool _isInitialized = false;

        internal static Config Config { get; set; }

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

            var jsonOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            Config =
                JsonSerializer.Deserialize<Config>(
                    File.ReadAllText($"{Environment.CurrentDirectory}/config.json"),
                    jsonOptions
                ) ?? throw new InvalidDataException();

            Console.WriteLine($"[{DateTime.UtcNow:s}Z] [INFO] Read Config");

            while (true)
            {
                var nowRunner = DateTime.UtcNow;
                Console.WriteLine($"[{nowRunner:s}Z] [INFO] Running Tasks");

                await VacdmPilotFaker.RunUpdate();

                Console.WriteLine(
                    $"[{nowRunner:s}Z] [INFO] Success, updated pilots, Next update at: {nowRunner.AddMinutes(10):HH:mm}Z"
                );

                await Task.Delay(TimeSpan.FromMinutes(10));
            }
        }
    }
}
