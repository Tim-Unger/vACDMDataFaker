using System.Text.Json;
using System.Text;

namespace VacdmDataFaker.Vacdm
{
    public partial class VacdmPilotFaker
    {
        internal static async Task AddPilotAsync(VacdmPilot pilot)
        {

#if RELEASE
            var configUrl = TaskRunner.Config.Url;

            if(configUrl is null)
            {
                Console.WriteLine($"[{DateTime.UtcNow:s}] [FATAL] Variable VACDM_URL was not provided");

                throw new MissingMemberException();
            }

            //Bit stupid but we want to be consistent with var names
            var postUrl = configUrl;
#else
            var postUrl = TaskRunner.Config.Url;
#endif

            if (!postUrl.Contains("api/v1/pilots"))
            {
                postUrl += "/api/v1/pilots";
            }

            var contentRaw = JsonSerializer.Serialize<VacdmPilot>(pilot);

            var content = new StringContent(contentRaw, Encoding.UTF8, "application/json");

            var response = await Client.PostAsync(postUrl, content);

            if (response.Content != null)
            {
                var messageRaw = await response.Content.ReadAsStringAsync();

                var message = JsonSerializer.Deserialize<VacdmPilot>(messageRaw);

                if (message is not null)
                {
                    Console.WriteLine(
                        $"[{DateTime.UtcNow:s}Z] [INFO] Added fake pilot {pilot.Callsign}"
                    );
                }
            }
        }
    }
}
