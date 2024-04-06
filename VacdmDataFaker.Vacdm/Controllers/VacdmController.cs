
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

            var data = await client.GetFromJsonAsync<List<VacdmPilot>>(
                "https://vacdm.tim-u.me/api/v1/pilots"
            );

            return new JsonResult(data);
        }
    }
}
