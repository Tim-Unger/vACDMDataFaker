using Microsoft.AspNetCore.Mvc;
using System.Net;
using VacdmDataFaker.Vacdm;

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

            var authenticateSuccess = ApiAuthenticator.AuthenticateUser(context);

            if (authenticateSuccess != HttpStatusCode.OK)
            {
                Console.WriteLine($"[{DateTime.UtcNow:s}] [WARN] Authetification for POST Request failed");

                return "unauthorized";
            }

            try
            {
                var now = DateTime.UtcNow;

                Console.WriteLine($"[{now:s}] [INFO] Adding {count} through POST");

                FlowMeasureFaker.FakeMeasures(count);

                return "success";
            }
            catch (Exception ex)
            {
                var now = DateTime.UtcNow;

                Console.WriteLine($"[{now:s}] [WARN] Post failed: {ex.InnerException}");

                return "error, see logs";
            }
        }

        [HttpDelete("/delete/all")]
        [HttpDelete("/delete/{count}")]
        public string Delete(int? count)
        {
            var context = HttpContext;

            var authenticateSuccess = ApiAuthenticator.AuthenticateUser(context);

            if (authenticateSuccess != HttpStatusCode.OK)
            {
                return authenticateSuccess.ToString().ToLower();
            }

            try
            {
                if (count is null)
                {
                    FlowMeasureFaker.DeleteMeasures(null);

                    Console.WriteLine($"[{DateTime.UtcNow:s}] [INFO] Deleted all Flow Measures");

                    return "success";
                }

                if (count <= 0)
                {
                    Console.WriteLine($"[{DateTime.UtcNow:s}] [WARN] Delete failed, invalid count was provided");

                    return "invalid count value";
                }

                FlowMeasureFaker.DeleteMeasures(count);

                Console.WriteLine($"[{DateTime.UtcNow:s}Z] [INFO] Deleted {count} measures through DELETE");

                return "success";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.UtcNow:s}] [WARN] DELETE failed: {ex.InnerException}");

                return "error, see logs";
            }
        }
    }
}
