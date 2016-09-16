using System;
using GraphApiTest.Services;
using Microsoft.Graph;
using GraphApiTest.Helpers;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace GraphApiTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var aap = new AzureAuthenticationProvider();
            var accessToken = aap.GetAccessToken();

            var graphserviceClient = new GraphServiceClient("https://graph.microsoft.com/currentServiceVersion",
                new DelegateAuthenticationProvider(
                    (requestMessage) =>
                    {
                        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", accessToken);

                        return Task.FromResult(0);
                    }));


            UserService userService = new UserService(graphserviceClient);

            var me = userService.GetMe();
            me.Wait();

            var users = userService.GetUsers();
            users.Wait();

            var user = userService.GetUser("azure.admin@ptdev.onmicrosoft.com");
            user.Wait();

            Console.WriteLine("Press [enter] to quit...");
            Console.ReadLine();
        }

    }
}