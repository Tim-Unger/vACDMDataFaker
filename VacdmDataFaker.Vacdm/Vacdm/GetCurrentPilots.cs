using System.Text.Json;

namespace VacdmDataFaker.Vacdm
{
    public static partial class VacdmPilotFaker
    {
        internal static async Task<IEnumerable<string>> GetCurrentPilots()
        {
            var url = TaskRunner.Config.Url;

            var currentPilotsRaw = await Client.GetStringAsync(
                url
            );

            if (currentPilotsRaw is null)
            {
                Console.WriteLine(
                    $"[{DateTime.UtcNow:s}Z] [FATAL] Could not fetch current pilots from vACDM"
                );
                throw new InvalidDataException();
            }

            return JsonSerializer
                .Deserialize<List<VacdmPilot>>(currentPilotsRaw)!
                .Select(x => x.Callsign);
        }
    }
}
