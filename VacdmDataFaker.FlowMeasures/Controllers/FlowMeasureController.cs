using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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

            var rawConfig = System.IO.File.ReadAllText($"{Environment.CurrentDirectory}/config.json");

            var config = JsonSerializer.Deserialize<Config>(rawConfig);

            if(config?.Password is null)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

            var passedConfig = new Config()
            {
                Username = credentialsRaw[0],
                Password = credentialsRaw[1]
            };

            if(config.Password != passedConfig.Password || config.Username != passedConfig.Username)
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            try
            {
                FlowMeasureFaker.FakeMeasures(count ?? 10);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }

            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}
