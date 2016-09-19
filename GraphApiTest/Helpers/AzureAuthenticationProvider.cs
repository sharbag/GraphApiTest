using Microsoft.Graph;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Configuration;
using System.Threading.Tasks;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;

namespace GraphApiTest.Helpers
{
    public class AzureAuthenticationProvider : IAuthenticationProvider
    {

        // Properties used to get and manage an access token.
        private string clientId = ConfigurationManager.AppSettings["ida:AppId"];
        private string clientSecret = ConfigurationManager.AppSettings["ida:AppPassword"];

        public string GetAccessToken()
        {
            AuthenticationContext authContext = new AuthenticationContext("https://login.windows.net/7e34a101-4f3e-45c7-943e-1bfeb9530099/oauth2/token");
            var creds = new ClientCredential(clientId, clientSecret);

            var authResult = authContext.AcquireTokenAsync("https://graph.microsoft.com/", creds);
            authResult.Wait();
            return authResult.Result.AccessToken;
        }

        public string GetAccessTokenCertificateBased()
        {
            AuthenticationContext authenticationContext = new AuthenticationContext("https://login.microsoftonline.com/7e34a101-4f3e-45c7-943e-1bfeb9530099/oauth2/token", false);
            string certfile = @"C:\Users\nusswalt\OneDrive - ProTechnology\Projects\RTL\RTL-Test.pfx";
            var cert = new X509Certificate2(certfile, "Dresden.2016", X509KeyStorageFlags.MachineKeySet);
            var cac = new ClientAssertionCertificate(clientId, cert);
            var authResult = authenticationContext.AcquireTokenAsync("https://graph.microsoft.com/", cac);
            authResult.Wait();
            return authResult.Result.AccessToken;
        }

        public async Task AuthenticateRequestAsync(HttpRequestMessage request)
        {
            AuthenticationContext authContext = new AuthenticationContext("https://login.windows.net/ptdev.onmicrosoft.com/oauth2/token");

            var creds = new ClientCredential(clientId, clientSecret);

            var authResult = await authContext.AcquireTokenAsync("https://graph.microsoft.com/", creds);
            request.Headers.Add("Authorization", "Bearer " + authResult.AccessToken);
        }
    }
}
