using System.Net;
using System.Text.Json;

namespace VacdmDataFaker.Shared
{
    public static partial class Authenticator
    {
        private static HttpStatusCode ReadVacdmConfig(string cid, string password)
        {
            if (!int.TryParse(cid, out var cidParsed))
            {
                return HttpStatusCode.UnsupportedMediaType;
            }

            var passedConfig = new VacdmConfig()
            {
                Cid = cidParsed,
                Password = password
            };

#if RELEASE
            var configCid = int.Parse(Environment.GetEnvironmentVariable("VACDM_CID")!);
            var configPassword = Environment.GetEnvironmentVariable("VACDM_PASSWORD")!;
#else
            var rawConfig = File.ReadAllText(
                $"{Environment.CurrentDirectory}/config.json"
            );

            var config = JsonSerializer.Deserialize<VacdmConfig>(rawConfig);

            if(config is null)
            {
                return HttpStatusCode.InternalServerError;
            }

            var configCid = config.Cid;
            var configPassword = config.Password;
#endif

            var currentConfig = new VacdmConfig()
            {
                Cid = configCid,
                Password = configPassword
            };

            if(currentConfig.Cid == passedConfig.Cid && currentConfig.Password == passedConfig.Password)
            {
                return HttpStatusCode.OK;
            }

            return HttpStatusCode.Unauthorized;
        }
    }
}
