using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace VacdmDataFaker.FlowMeasures.Controllers
{
    [ApiController]
    [Route("api")]
    public class FlowMeasureController : Controller
    {
        [HttpGet("/data")]
        [HttpGet("/")]
        public JsonResult Get() => new(FlowMeasureFaker.FlowMeasures);

        [HttpPost("/data/fake/{count}")]
        public HttpResponseMessage PostCount(int count) => Post(count);

        [HttpPost("/data/fake")]
        public HttpResponseMessage Post(int? count)
        {
            var authHeader = AuthenticationHeaderValue.Parse(
                HttpContext.Request.Headers["Authorization"]
            );
            var credentialBytes = Convert.FromBase64String(authHeader.Parameter!);
            var credentialsRaw = Encoding.UTF8.GetString(credentialBytes).Split(":");

#if RELEASE
            var config = GetConfigFromEnv();
#else
            var rawConfig = System.IO.File.ReadAllText(
                $"{Environment.CurrentDirectory}/config.json"
            );

            var config = JsonSerializer.Deserialize<Config>(rawConfig);
#endif

            if (config?.Password is null)
            {
                var now = DateTime.UtcNow;

                Console.WriteLine($"[{now:s}] [WARN] Post failed as config is not valid");

                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

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

                Console.WriteLine($"[{now:s}] [WARN] Post failed as password or username were invalid");

                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            try
            {
                var now = DateTime.UtcNow;

                var addCount = count ?? 10;

                Console.WriteLine($"[{now:s}] [INFO] Adding {addCount} through Post");

                FlowMeasureFaker.FakeMeasures(addCount);

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch ( Exception ex )
            {
                var now = DateTime.UtcNow;

                Console.WriteLine($"[{now:s}] [WARN] Post failed: {ex.InnerException}");

                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        private static Config GetConfigFromEnv()
        {
            var config = new Config();

            var envUsername = Environment.GetEnvironmentVariable("ECFMP_USER");

            if(envUsername is null)
            {
                Console.WriteLine(
                    $"[{DateTime.UtcNow:s}Z] [FATAL] Variable ECFMP_USER was not provided"
                );

                throw new MissingFieldException();
            }

            config.Username = envUsername;

            var envPassword = Environment.GetEnvironmentVariable("ECFMP_PASSWORD");

            if (envPassword is null)
            {
                Console.WriteLine(
                    $"[{DateTime.UtcNow:s}Z] [FATAL] Variable ECFMP_PASSWORD was not provided"
                );

                throw new MissingFieldException();
            }

            config.Password = envPassword;

            return config;
        }
    }
}
