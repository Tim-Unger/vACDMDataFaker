using System.Text.Json;

namespace VacdmDataFaker.Vacdm
{
    public partial class VacdmPilotFaker
    {
        internal static HttpClient Client = new();

        internal static async Task RunUpdate()
        {
            Console.WriteLine($"[{DateTime.UtcNow:s}Z] [INFO] Running Update");

            var vatsimPilotsRaw = "";
            try
            {
                vatsimPilotsRaw = await Client.GetStringAsync(
                    "https://data.vatsim.net/v3/vatsim-data.json"
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"[{DateTime.UtcNow:s}Z] [FATAL] Could not fetch Vatsim Datafeed"
                );
                throw new HttpRequestException(ex.StackTrace);
            }

            var vatsimPilots = JsonSerializer.Deserialize<VatsimData>(vatsimPilotsRaw)!.pilots;

            var aerodromes = TaskRunner.Config.Airports;

            var concernedPilots = vatsimPilots
                .Where(x => x.flight_plan is not null)
                .Where(x => aerodromes.Any(y => y == x.flight_plan.departure))
                //.Where(x => x.groundspeed < 50)
                .ToList();

            var random = new Random();

            var randomIndices = new List<int>();

            for (var i = 0; i < 20; i++)
            {
                var randomNumber = random.Next(0, concernedPilots.Count - 1);
                randomIndices.Add(randomNumber);
            }

            randomIndices = randomIndices.Distinct().ToList();

            var randomPilots = randomIndices.Select(x => concernedPilots[x]).ToList();

            var fakePilots = FakePilots(randomPilots.Count);

            int index = 0;

            var updatedPilots = new List<VacdmPilot>();

            foreach (var fakePilot in fakePilots)
            {
                var vatsimPilot = randomPilots[index];

                var updatedPilot = AddDataToFakePilot(vatsimPilot, fakePilot);

                updatedPilots.Add(updatedPilot);

                index++;
            }

            var currentCallsigns = await GetCurrentPilots();

            Client.DefaultRequestHeaders.Authorization = new BasicAuthenticationHeaderValue(
                TaskRunner.Config.Cid.ToString(),
                TaskRunner.Config.Password.ToString()
            );

            if (!currentCallsigns.Any())
            {
                Console.WriteLine($"[{DateTime.UtcNow:s}Z] [INFO] No Pilots active at the moment");
            }

            int callsignCount = currentCallsigns.Count();

            var config = TaskRunner.Config;

            foreach (var currentCallsign in currentCallsigns)
            {
                //Delete Pilots until we reach the minimum amount required
                if (callsignCount <= config.MinimumAmount)
                {
                    break;
                }

                callsignCount--;

                await DeletePilotAsync(currentCallsign);
                
                await Task.Delay(100);
            }

            foreach (var pilot in updatedPilots)
            {
                //Add pilots until we reach the maximum amount (or the foreach loop completes, whatever is first)
                if(callsignCount >= config.MaximumAmount)
                {
                    return;
                }

                if (currentCallsigns.Any(x => x == pilot.Callsign))
                {
                    continue;
                }

                await AddPilotAsync(pilot);
                callsignCount++;

                await Task.Delay(100);
            }
        }
    }
}
