using System;
using GraphApiTest.Services;
using Microsoft.Graph;
using GraphApiTest.Helpers;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Collections.Generic;

namespace GraphApiTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var aap = new AzureAuthenticationProvider();
            var accessToken = aap.GetAccessTokenCertificateBased();

            //var graphserviceClient = new GraphServiceClient("https://graph.microsoft.com/",
            var graphserviceClient = new GraphServiceClient("https://graph.microsoft.com/beta/",
                new DelegateAuthenticationProvider(
                    (requestMessage) =>
                    {
                        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
                        return Task.FromResult(0);
                    }));

            var userService = new UserService(graphserviceClient);
            var licenseService = new LicenseService(graphserviceClient);


            var allUsers = userService.GetUsers();
            Console.WriteLine("Liste aller Nutzer:");
            foreach(var user in allUsers)
            {
                Console.WriteLine(String.Format("{0}", user.UserPrincipalName));

                //User concretUser = null;
                //try
                //{
                //    concretUser = userService.GetUser(user.UserPrincipalName);

                //    if (concretUser.AssignedPlans != null)
                //    {
                //        foreach (var license in user.AssignedLicenses)
                //        {
                //            Console.WriteLine(String.Format("Assigned License: {0}", license.SkuId));

                //            if (license.DisabledPlans != null)
                //            {
                //                foreach (var disabledPlan in license.DisabledPlans)
                //                {
                //                    Console.WriteLine(String.Format("Disabled Plan: {0}", disabledPlan));
                //                }
                //            }
                //        }
                //    }
                //}
                //catch { }                
                //Console.WriteLine("\n");
            }

            


            
            var availiableLicenses = licenseService.SubscribedSkus();
            Console.WriteLine("\n\nListe der Licensen und zugehörigen Plänen:");
            int? availiableLicensesCount = 0;
            foreach (var license in availiableLicenses)
            {
                Console.WriteLine(String.Format("skuPartNumber: {0}", license.SkuPartNumber));
                Console.WriteLine(String.Format("skuId: {0}", license.SkuId));
                Console.WriteLine(String.Format("CapabilityStatus: {0}", license.CapabilityStatus));
                availiableLicensesCount = license.PrepaidUnits.Enabled - license.ConsumedUnits;
                Console.WriteLine(String.Format("availiableLicensesCount: {0}", availiableLicensesCount));

                foreach (var servicePlan in license.ServicePlans)
                {
                    Console.WriteLine(String.Format("servicePlanName: {0} | servicePlanId: {1}", servicePlan.ServicePlanName, servicePlan.ServicePlanId));
                }
                Console.WriteLine("\n");
            }

            if(availiableLicensesCount > 0)
            {
                var disabledPlans = new List<Guid> { new Guid("a23b959c-7ce8-4e57-9140-b90eb88a9e97") };
                var licensedUser = licenseService.SetLicense("kirsten.kluge@solution365.de", new Guid("6fd2c87f-b296-42f0-b197-1e91e994b900"), disabledPlans);
                if (licensedUser == null)
                {
                    Console.WriteLine("Lizenzzuweisung hat fehlgeschlagen");
                }
                else
                {
                    Console.WriteLine("Lizenz wurde erfolgreich zugewiesen");
                }
            }
            else
            {
                Console.WriteLine("Keine Lizenzen mehr verfügbar");
            }
            


            Console.WriteLine("Press [enter] to quit...");
            Console.ReadLine();

        }

    }
}