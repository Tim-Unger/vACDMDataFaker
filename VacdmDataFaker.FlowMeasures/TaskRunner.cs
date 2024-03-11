namespace VacdmDataFaker.FlowMeasures
{
    public class TaskRunner
    {
        internal static async Task Run()
        {
            while (true)
            {
                FlowMeasureFaker.FakeMeasures(10);

                Console.WriteLine("--");
                Console.WriteLine($"Next Update at: {DateTime.UtcNow.AddMinutes(10).ToLongTimeString()}Z");
                await Task.Delay(TimeSpan.FromMinutes(10));
            }
        }
    }
}
