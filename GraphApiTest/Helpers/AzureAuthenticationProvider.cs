using Microsoft.Graph;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Configuration;
using System.Threading.Tasks;
using System.Net.Http;

namespace GraphApiTest.Helpers
{
    public class AzureAuthenticationProvider : IAuthenticationProvider
    {

        // Properties used to get and manage an access token.
        private string clientId = ConfigurationManager.AppSettings["ida:AppId"];
        private string clientSecret = ConfigurationManager.AppSettings["ida:AppPassword"];

        public string GetAccessToken()
        {
            AuthenticationContext authContext = new AuthenticationContext("https://login.windows.net/ptdev.onmicrosoft.com/oauth2/token");

            var creds = new ClientCredential(clientId, clientSecret);

            var authResult = authContext.AcquireTokenAsync("https://graph.microsoft.com/", creds);
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
