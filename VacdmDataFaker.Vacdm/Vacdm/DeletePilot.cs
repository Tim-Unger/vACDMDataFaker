using System.Text.Json;

namespace VacdmDataFaker.Vacdm
{
    public partial class VacdmPilotFaker
    {
        internal static async Task DeletePilotAsync(string callsign)
        {
            var deleteUrl = $"https://vacdm.tim-u.me/api/v1/pilots/{callsign}";

            var response = await Client.DeleteAsync(deleteUrl);

            if (response.Content != null)
            {
                var messageRaw = await response.Content.ReadAsStringAsync();

                var message = JsonSerializer.Deserialize<ApiStatus>(messageRaw);

                Console.WriteLine(
                    $"[{DateTime.UtcNow:s}Z] [INFO] Deleted fake pilot {callsign}"
                );
            }
        }
    }
}
