using System.Text.Json;
using System.Text;

namespace VacdmDataFaker.Vacdm
{
    public partial class VacdmPilotFaker
    {
        internal static async Task AddPilotAsync(VacdmPilot pilot)
        {

#if RELEASE
            var envUrl = Environment.GetEnvironmentVariable("VACDM_URL");

            if(envUrl is null)
            {
                Console.WriteLine($"[{DateTime.UtcNow:s}] [FATAL] Variable VACDM_URL was not provided");

                throw new MissingMemberException();
            }

            if(!Uri.TryCreate(envUrl, UriKind.Absolute, out var postUri)) 
            {
                Console.WriteLine($"[{DateTime.UtcNow:s}] [FATAL] Variable VACDM_URL was not a valid URL");

                throw new InvalidDataException();
            }

            //Bit stupid but we want to be consistent with var names
            var postUrl = envUrl;
#else
            var postUrl = $"https://vacdm.tim-u.me/api/v1/pilots";
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
