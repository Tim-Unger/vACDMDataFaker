using System.Net.Http.Headers;
using System.Net;
using System.Text;

namespace VacdmDataFaker.Shared
{
    public static partial class Authenticator
    {
        public static HttpStatusCode AuthenticateUser(AuthenticationHeaderValue authenticationHeaderValue, bool isCid)
        {
            try
            {
                var credentialBytes = Convert.FromBase64String(authenticationHeaderValue.Parameter!);
                
                var credentialsRaw = Encoding.UTF8.GetString(credentialBytes).Split(":");

                var authStatusCode = isCid ? ReadVacdmConfig(credentialsRaw[0], credentialsRaw[1]) : ReadEcfmpConfig(credentialsRaw[0], credentialsRaw[1]);

                if (authStatusCode != HttpStatusCode.OK)
                {
                    return HttpStatusCode.Unauthorized;
                }

                return HttpStatusCode.OK;
            }
            catch
            {
                throw new InvalidDataException();
            }
        }
    }
}
