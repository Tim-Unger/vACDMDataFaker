using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;
using VacdmDataFaker.Shared;

namespace VacdmDataFaker.FlowMeasures.Controllers
{
    [ApiController]
    [Route("api")]
    public class FlowMeasureController : Controller
    {
        [HttpGet("/data")]
        public JsonResult Get() => new(FlowMeasureFaker.FlowMeasures);

        [HttpPost("/add/{count}")]
        public string Post(int count)
        {
            var context = HttpContext;

            var authHeader = AuthenticationHeaderValue.Parse(
                 context.Request.Headers["Authorization"]
             );

            var authenticateSuccessCode = Authenticator.AuthenticateUser(authHeader, false);

            if (authenticateSuccessCode != HttpStatusCode.OK)
            {
                TaskRunner.LogMessages.Add(Logger.LogError("Authentication for POST Request failed"));
                return "unauthorized";
            }

            try
            {
                FlowMeasureFaker.FakeMeasures(count);

                TaskRunner.LogMessages.Add(Logger.LogInfo($"Added {count} through POST"));
                return "success";
            }
            catch (Exception ex)
            {
                TaskRunner.LogMessages.Add(Logger.LogError($"Post failed: {ex.InnerException}"));

                return "error, see logs";
            }
        }

        [HttpDelete("/delete/all")]
        [HttpDelete("/delete/{count}")]
        public string Delete(int? count)
        {
            var context = HttpContext;

            var authHeader = AuthenticationHeaderValue.Parse(
                 context.Request.Headers["Authorization"]
             );

            var authenticateSuccess = Authenticator.AuthenticateUser(authHeader, false);

            if (authenticateSuccess != HttpStatusCode.OK)
            {
                return authenticateSuccess.ToString().ToLower();
            }

            try
            {
                if (count is null)
                {
                    FlowMeasureFaker.DeleteMeasures(null);

                    TaskRunner.LogMessages.Add(Logger.LogInfo("Deleted all Flow Measures"));
                    return "success";
                }

                if (count <= 0)
                {
                    TaskRunner.LogMessages.Add(Logger.LogWarning("Delete failed, invalid count was provided"));
                    return "invalid count value";
                }

                FlowMeasureFaker.DeleteMeasures(count);

                TaskRunner.LogMessages.Add(Logger.LogInfo("Deleted {count} measures through DELETE"));
                return "success";
            }
            catch (Exception ex)
            {
                TaskRunner.LogMessages.Add(Logger.LogError($"DELETE failed: {ex.InnerException}"));
                return "error, see logs";
            }
        }
    }
}
