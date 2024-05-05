using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;
using VacdmDataFaker.Shared;
using LogLevel = VacdmDataFaker.Shared.LogLevel;

namespace VacdmDataFaker.FlowMeasures.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        [HttpGet("/logs")]
        public JsonResult Get()
        {
            var config = TaskRunner.Config;

            if (config.RequireAuthenticationForLogs)
            {
                var context = HttpContext;

                try
                {
                    var authHeader = AuthenticationHeaderValue.Parse(
                        context.Request.Headers["Authorization"]
                    );

                    var authenticateSuccess = Authenticator.AuthenticateUser(authHeader, false);

                    if (authenticateSuccess != HttpStatusCode.OK)
                    {
                        TaskRunner.LogMessages.Add(
                            Logger.LogError("Authentication for GET Logs Request failed")
                        );
                        return new JsonResult("unauthorized");
                    }
                }
                catch
                {
                    TaskRunner.LogMessages.Add(
                        Logger.LogError("Authentication for GET Logs Request failed")
                    );
                    return new JsonResult("unauthorized");
                }
            }

            return new JsonResult(TaskRunner.LogMessages);
        }

        [HttpGet("/logs/{logLevelStringOrInt}")]
        public JsonResult GetLevel(string logLevelStringOrInt)
        {
            if (!int.TryParse(logLevelStringOrInt, out var logLevelInt))
            {
                logLevelInt = logLevelStringOrInt.ToLower() switch
                {
                    "info" or "information" => 0,
                    "warn" or "warning" => 1,
                    "err" or "error" => 2,
                    "fatal" => 3,
                    _ => throw new InvalidDataException("Provided string was not a valid LogLevel")
                };
            }

            var logLevel = (LogLevel)logLevelInt;

            var config = TaskRunner.Config;

            if (config.RequireAuthenticationForLogs)
            {
                var context = HttpContext;

                var authHeader = AuthenticationHeaderValue.Parse(
                    context.Request.Headers["Authorization"]
                );

                if (authHeader is null)
                {
                    TaskRunner.LogMessages.Add(
                        Logger.LogError("Authentication for GET Log Request failed")
                    );
                    return new JsonResult("unauthorized");
                }

                var authenticateSuccess = Authenticator.AuthenticateUser(authHeader, false);

                if (authenticateSuccess != HttpStatusCode.OK)
                {
                    TaskRunner.LogMessages.Add(
                        Logger.LogError("Authentication for GET Log Request failed")
                    );
                    return new JsonResult("unauthorized");
                }
            }

            return new JsonResult(TaskRunner.LogMessages.Where(x => x.LogLevel == logLevel));
        }
    }
}
