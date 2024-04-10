using System.Net;
using System.Net.Http.Headers;
using System.Text;
using VacdmDataFaker.FlowMeasures;

namespace VacdmDataFaker.Vacdm
{
    internal static class ApiAuthenticator
    {
        internal static HttpStatusCode AuthenticateUser(HttpContext httpContext)
        {
            var authHeader = AuthenticationHeaderValue.Parse(
                 httpContext.Request.Headers["Authorization"]
             );
            var credentialBytes = Convert.FromBase64String(authHeader.Parameter!);

            var credentialsRaw = Encoding.UTF8.GetString(credentialBytes).Split(":");

            var config = TaskRunner.Config;

            var passedConfig = new Config()
            {
                Username = credentialsRaw[0],
                Password = credentialsRaw[1]
            };

            if (
                config.Password != passedConfig.Password || config.Username != passedConfig.Username
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
