using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace VacdmDataFaker.Vacdm
{
    internal class ApiAuthenticator
    {
        internal static HttpStatusCode AuthenticateUser(HttpContext httpContext)
        {
            var authHeader = AuthenticationHeaderValue.Parse(
                 httpContext.Request.Headers["Authorization"]
             );
            var credentialBytes = Convert.FromBase64String(authHeader.Parameter!);

            var credentialsRaw = Encoding.UTF8.GetString(credentialBytes).Split(":");

#if RELEASE
            var config = TaskRunner.Config;
#else
            var rawConfig = File.ReadAllText(
                $"{Environment.CurrentDirectory}/config.json"
            );

            var config = JsonSerializer.Deserialize<Config>(rawConfig);
#endif
            if (!int.TryParse(credentialsRaw[0], out var cidParsed))
            {
                Console.WriteLine($"[{DateTime.UtcNow:s}] [FATAL] Provided CID was not a number");

                return HttpStatusCode.Unauthorized;
            }

            var passedConfig = new Config()
            {
                Cid = cidParsed,
                Password = credentialsRaw[1]
            };

            if (
                config.Password != passedConfig.Password || config.Cid != passedConfig.Cid
            )
            {
                var now = DateTime.UtcNow;

                Console.WriteLine(
                    $"[{now:s}] [WARN] Post failed as password or username were invalid"
                );

                return HttpStatusCode.Unauthorized;
            }

            return HttpStatusCode.OK;
        }
    }
}
