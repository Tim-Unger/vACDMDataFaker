using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace VacdmDataFaker.Vacdm.Controllers
{
    [ApiController]
    [Route("api")]
    public class VacdmController : Microsoft.AspNetCore.Mvc.Controller
    {
        [HttpGet("/data")]
        public async Task<JsonResult> Get()
        {
            var client = new HttpClient();

#if RELEASE
            if (TaskRunner.Config.Url is null)
            {
                Console.WriteLine($"[{DateTime.UtcNow:s}] [FATAL] Variable VACDM_URL was not provided");

                throw new MissingMemberException();
            }

            var url = TaskRunner.Config.Url;
#else
            var url = TaskRunner.Config.Url;
#endif

            var data = await client.GetFromJsonAsync<List<VacdmPilot>>(
                url
            );

            return new JsonResult(data);
        }

        [HttpPost("/add/{count}")]
        public HttpStatusCode Post(int? count)
        {
            var context = HttpContext;

            var authenticateSuccess = ApiAuthenticator.AuthenticateUser(context);

            if(authenticateSuccess != HttpStatusCode.OK)
            {
                return authenticateSuccess;
            }

            try
            {
                var now = DateTime.UtcNow;

                var addCount = count ?? 10;

                VacdmPilotFaker.FakePilots(addCount);

                Console.WriteLine($"[{now:s}] [INFO] Added {count} Pilots through DELETE");

                return HttpStatusCode.OK;

            }
            catch ( Exception ex )
            {
                var now = DateTime.UtcNow;

                Console.WriteLine($"[{now:s}] [WARN] Post failed: {ex.InnerException}");

                return HttpStatusCode.InternalServerError;
            }
        }

        [HttpDelete("/delete/all")]
        [HttpDelete("/delete/{count}")]
        public async Task<string> Delete(int? count)
        {
            var context = HttpContext;

            var authenticateSuccess = ApiAuthenticator.AuthenticateUser(context);

            if (authenticateSuccess != HttpStatusCode.OK)
            {
                return authenticateSuccess.ToString().ToLower();
            }

            try
            {
                if(count is null)
                {
                    await VacdmPilotFaker.DeleteAllAsync();

                    Console.WriteLine($"[{DateTime.UtcNow:s}] [INFO] Deleted all Pilots through DELETE");

                    return "success";
                }

                if(count <= 0)
                {
                    Console.WriteLine($"[{DateTime.UtcNow:s}] [WARN] Delete failed, invalid count was provided");

                    return "invalid count value";
                }

                //count can not be null
                await VacdmPilotFaker.DeleteCountAsync((int)count);

                Console.WriteLine($"[{DateTime.UtcNow:s}Z] [INFO] Deleted {count} pilots through DELETE");

                return "success";
            }
            catch ( Exception ex )
            {
                Console.WriteLine($"[{DateTime.UtcNow:s}] [WARN] DELETE failed: {ex.InnerException}");

                return "error, see logs";
            }
        }
    }
}
