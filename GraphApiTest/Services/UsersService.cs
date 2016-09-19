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
        private GraphServiceClient _graphClient;
        
        public UserService(GraphServiceClient graphClient)
        {
            _graphClient = graphClient;
        }

        public User GetMe()
        {
            var user = _graphClient.Me.Request().GetAsync();
            user.Wait();
            return user.Result;
        }

        // Get all users.
        public IList<User> GetUsers()
        {
            // Get users.
            var users = _graphClient.Users.Request().GetAsync();
            users.Wait();
            return users.Result.ToList();
        }

        public User GetUser(string userPrincipalName)
        {
            var user = _graphClient.Users[userPrincipalName].Request().GetAsync();
            user.Wait();
            return user.Result;
        }
    }
}