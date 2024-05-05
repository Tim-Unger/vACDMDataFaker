using System.Net;
using System.Text.Json;

namespace VacdmDataFaker.Shared
{
    public static partial class Authenticator
    {
        private static HttpStatusCode ReadEcfmpConfig(string username, string password)
        {
            var passedConfig = new EcfmpConfig()
            {
                Username = username,
                Password = password
            };

#if RELEASE
            var configUsername = Environment.GetEnvironmentVariable("ECFMP_USER")!;
            var configPassword = Environment.GetEnvironmentVariable("ECFMP_PASSWORD")!;
#else
            var rawConfig = File.ReadAllText(
                $"{Environment.CurrentDirectory}/Data/config.json"
            );

            var config = JsonSerializer.Deserialize<EcfmpConfig>(rawConfig);

            if(config is null)
            {
                return HttpStatusCode.BadRequest;
            }

            var configUsername = config.Username;
            var configPassword = config.Password;
#endif
            var currentConfig = new EcfmpConfig()
            {
                Username = configUsername,
                Password = configPassword
            };

            if (currentConfig.Username == passedConfig.Username && currentConfig.Password == passedConfig.Password)
            {
                return HttpStatusCode.OK;
            }

            return HttpStatusCode.Unauthorized;
        }
    }
}
