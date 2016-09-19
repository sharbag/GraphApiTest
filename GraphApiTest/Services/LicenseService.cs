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

        public void SetLicense(string userPrincipalName, Guid licenseSkuId, IList<Guid> disabledPlansGuids)
        {
            var assignedLicense = new AssignedLicense();
            assignedLicense.SkuId = licenseSkuId;
            assignedLicense.DisabledPlans = disabledPlansGuids;

            var result = _graphClient.Users[userPrincipalName].AssignLicense(new List<AssignedLicense> { assignedLicense }, new List<Guid>());
            var request = result.Request();
        }

        public IList<SubscribedSku> SubscribedSkus()
        {
            var skus = _graphClient.SubscribedSkus.Request().GetAsync();
            skus.Wait();
            return skus.Result.ToList();
        }
    }
}
