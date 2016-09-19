using GraphApiTest.Helpers;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GraphApiTest.Services
{
    public class LicenseService
    {
        private GraphServiceClient _graphClient;

        public LicenseService (GraphServiceClient graphClient)
        {
            _graphClient = graphClient;
        }

        public User SetLicense(string userPrincipalName, Guid licenseSkuId, IList<Guid> disabledPlansGuids)
        {
            var assignedLicense = new AssignedLicense();
            //assignedLicense.AdditionalData = new Dictionary<string, object>();
            assignedLicense.SkuId = licenseSkuId;
            assignedLicense.DisabledPlans = disabledPlansGuids;

            var request = _graphClient.Users[userPrincipalName].AssignLicense(new List<AssignedLicense> { assignedLicense }, new List<Guid>());
            var response = request.Request().PostAsync();
            //try
            //{
                response.Wait();
            //}
            //catch { }
            if(response.Exception == null)
                return response.Result;

            return null;
        }

        public IList<SubscribedSku> SubscribedSkus()
        {
            var skus = _graphClient.SubscribedSkus.Request().GetAsync();
            skus.Wait();
            return skus.Result.ToList();
        }
    }
}
