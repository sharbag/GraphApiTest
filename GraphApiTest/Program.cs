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
            var accessToken = aap.GetAccessTokenCertificateBased();

            //var graphserviceClient = new GraphServiceClient("https://graph.microsoft.com/",
            var graphserviceClient = new GraphServiceClient("https://graph.microsoft.com/v1.0/",
                new DelegateAuthenticationProvider(
                    (requestMessage) =>
                    {
                        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
                        return Task.FromResult(0);
                    }));


            var userService = new UserService(graphserviceClient);
            var allUsers = userService.GetUsers();
            Console.WriteLine("Liste aller Nutzer:");
            foreach(var user in allUsers)
            {
                Console.WriteLine(String.Format("{0}", user.UserPrincipalName));

                User concretUser = null;
                try
                {
                    concretUser = userService.GetUser(user.UserPrincipalName);

                    if (concretUser.AssignedPlans != null)
                    {
                        foreach (var license in user.AssignedLicenses)
                        {
                            Console.WriteLine(String.Format("Assigned License: {0}", license.SkuId));

                            if (license.DisabledPlans != null)
                            {
                                foreach (var disabledPlan in license.DisabledPlans)
                                {
                                    Console.WriteLine(String.Format("Disabled Plan: {0}", disabledPlan));
                                }
                            }
                        }
                    }
                }
                catch { }                
                Console.WriteLine("\n");
            }

            


            var licenseService = new LicenseService(graphserviceClient);
            var availiableLicenses = licenseService.SubscribedSkus();
            Console.WriteLine("\n\nListe der Licensen und zugehörigen Plänen:");
            foreach (var license in availiableLicenses)
            {
                Console.WriteLine(String.Format("skuPartNumber: {0}", license.SkuPartNumber));
                Console.WriteLine(String.Format("skuId: {0}", license.SkuId));

                foreach(var servicePlan in license.ServicePlans)
                {
                    Console.WriteLine(String.Format("servicePlanName: {0} | servicePlanId: {1}", servicePlan.ServicePlanName, servicePlan.ServicePlanId));
                }
                Console.WriteLine("\n");
            }


            Console.WriteLine("Press [enter] to quit...");
            Console.ReadLine();
        }

    }
}