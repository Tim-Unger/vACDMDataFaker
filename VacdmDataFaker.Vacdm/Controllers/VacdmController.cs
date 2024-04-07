using Microsoft.AspNetCore.Mvc;

namespace VacdmDataFaker.Vacdm.Controllers
{
    [ApiController]
    [Route("api")]
    public class VacdmController : Controller
    {
        [HttpGet("/")]
        public async Task<JsonResult> Get()
        {
            var client = new HttpClient();

#if RELEASE
            var envUrl = Environment.GetEnvironmentVariable("VACDM_URL");

            if (envUrl is null)
            {
                Console.WriteLine($"[{DateTime.UtcNow:s}] [FATAL] Variable VACDM_URL was not provided");

                throw new MissingMemberException();
            }

            var url = envUrl;
#else
            url = "https://vacdm.tim-u.me/api/v1/pilots";
#endif

            var data = await client.GetFromJsonAsync<List<VacdmPilot>>(
                url
            );

            return new JsonResult(data);
        }
    }
}
