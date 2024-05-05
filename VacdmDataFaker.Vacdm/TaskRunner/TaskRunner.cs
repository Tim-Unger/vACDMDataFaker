using System.Text.Json;
using VacdmDataFaker.Shared;

namespace VacdmDataFaker.Vacdm
{
    internal class TaskRunner
    {
        private static bool _isInitialized = false;

        private static bool _isFirstLoad = true;

        private static readonly List<int> _devCids =
            new()
            {
                10000000,
                10000001,
                10000002,
                10000003,
                10000004,
                10000005,
                10000006,
                10000007,
                10000008,
                10000009,
                100000010
            };

        internal static List<LogMessage> LogMessages { get; set; } = new();

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
            //NORELEASE fix cast
            Config = ConfigReader.ReadVacdmConfig();
#else
            var jsonOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            Config =
                JsonSerializer.Deserialize<Config>(
                    File.ReadAllText($"{Environment.CurrentDirectory}/Data/config.json"),
                    jsonOptions
                ) ?? throw new InvalidDataException();

            //This is Checked in ReadVacdmConfig() in Release but we check it here in Debug as well, to prevent any accidents
            if (!Config.AllowNonDevCids && !_devCids.Any(x => x == Config.Cid))
            {
                Console.WriteLine($"[{DateTime.UtcNow:s}Z] [FATAL] Program is not allowed to run with a non-dev CID");
                throw new InvalidDataException();
            }
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
