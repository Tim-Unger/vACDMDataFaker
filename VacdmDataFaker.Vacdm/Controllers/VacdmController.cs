using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using VacdmDataFaker.Shared;

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
                TaskRunner.LogMessages.Add(Logger.LogFatal("Variable VACDM_URL was not provided"));

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

            var authHeader = AuthenticationHeaderValue.Parse(
                 context.Request.Headers["Authorization"]
             );

            var authenticateSuccess = Authenticator.AuthenticateUser(authHeader, true);

            if(authenticateSuccess != HttpStatusCode.OK)
            {
                return authenticateSuccess;
            }

            try
            {
                var addCount = count ?? 10;

                VacdmPilotFaker.FakePilots(addCount);

                TaskRunner.LogMessages.Add(Logger.LogInfo($"Added {count} Pilots through DELETE"));

                return HttpStatusCode.OK;

            }
            catch ( Exception ex )
            {
                var now = DateTime.UtcNow;

                TaskRunner.LogMessages.Add(Logger.LogWarning($"[{now:s}] [WARN] Post failed: {ex.InnerException}"));

                return HttpStatusCode.InternalServerError;
            }
        }

        [HttpDelete("/delete/all")]
        [HttpDelete("/delete/{count}")]
        public async Task<string> Delete(int? count)
        {
            var context = HttpContext;

            var authHeader = AuthenticationHeaderValue.Parse(
                 context.Request.Headers["Authorization"]
             );

            var authenticateSuccess = Authenticator.AuthenticateUser(authHeader, true);

            if (authenticateSuccess != HttpStatusCode.OK)
            {
                return authenticateSuccess.ToString().ToLower();
            }

            try
            {
                if(count is null)
                {
                    await VacdmPilotFaker.DeleteAllAsync();

                    TaskRunner.LogMessages.Add(Logger.LogInfo($"[{DateTime.UtcNow:s}] [INFO] Deleted all Pilots through DELETE"));

                    return "success";
                }

                if(count <= 0)
                {
                    TaskRunner.LogMessages.Add(Logger.LogWarning($"[{DateTime.UtcNow:s}] [WARN] Delete failed, invalid count was provided"));

                    return "invalid count value";
                }

                //count can not be null
                await VacdmPilotFaker.DeleteCountAsync((int)count);

                TaskRunner.LogMessages.Add(Logger.LogInfo($"[{DateTime.UtcNow:s}Z] [INFO] Deleted {count} pilots through DELETE"));

                return "success";
            }
            catch ( Exception ex )
            {
                TaskRunner.LogMessages.Add(Logger.LogWarning($"[{DateTime.UtcNow:s}] [WARN] DELETE failed: {ex.InnerException}"));

                return "error, see logs";
            }
        }
    }
}
