﻿using System.Text.Json;

namespace VacdmDataFaker.Vacdm
{
    public partial class VacdmPilotFaker
    {
        internal static async Task<IEnumerable<string>> GetCurrentPilots()
        {
            var currentPilotsRaw = await Client.GetStringAsync(
                "https://vacdm.tim-u.me/api/v1/pilots"
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
