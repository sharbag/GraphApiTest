using GraphApiTest.Helpers;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GraphApiTest.Services
{
    public class UserService
    {
        private GraphServiceClient graphClient;
        public UserService()
        {
            graphClient = new GraphServiceClient("https://graph.microsoft.com/currentServiceVersion", new AzureAuthenticationProvider());
        }

        public UserService(GraphServiceClient graphClient)
        {
            this.graphClient = graphClient;
        }

        public async Task<User> GetMe()
        {
            var user = await graphClient
                     .Me
                     .Request()
                     .GetAsync();
            return user;
        }

        public void SetLicense(string userPrincipalName)
        {
            var assignedLicense = new AssignedLicense();
            assignedLicense.SkuId = Guid.NewGuid();
            assignedLicense.DisabledPlans = new List<Guid> {
                Guid.NewGuid()
            };

            var assignedLicenses = new List<AssignedLicense>();
            assignedLicenses.Add(assignedLicense);
            
            graphClient.Users[userPrincipalName].AssignLicense(assignedLicenses, null).Request();
        }

        // Get all users.
        public async Task<IGraphServiceUsersCollectionPage> GetUsers()
        {
            // Get users.
            var users = await graphClient.Users.Request().GetAsync();          
            return users;
        }

        public async Task<User> GetUser(string userPrincipalName)
        {
            var user = await graphClient.Users[userPrincipalName].Request().GetAsync();
            return user;
        }
    }
}