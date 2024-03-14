namespace VacdmDataFaker.FlowMeasures
{
    public class TaskRunner
    {
        private static bool _isInitialized = false;

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

            while (true)
            {

                var nowRunner = DateTime.UtcNow;
                Console.WriteLine($"[{nowRunner:s}Z] [INFO] Running Tasks");

                FlowMeasureFaker.FakeMeasures(10);

                Console.WriteLine($"[{nowRunner:s}Z] [INFO] Success, added 10 Measures, Next Update at: {nowRunner.AddMinutes(10):HH:mm}Z");

                await Task.Delay(TimeSpan.FromMinutes(10));
            }
        }
    }
}
