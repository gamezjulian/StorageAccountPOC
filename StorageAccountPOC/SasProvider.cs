using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageAccountPOC
{
    public class SasProvider
    {
        private string token;
        public string SASToken
        {
            get
            {
                if (string.IsNullOrEmpty(this.token))
                {
                    this.token = this.GetAccountSASToken();
                }

                return this.token;
            }
        }

        public SasProvider()
        {

        }

        private string GetAccountSASToken()
        {
            var ConnectionString = ConfigurationManager.AppSettings["StorageAccountConnectionString"];
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);

            SharedAccessAccountPolicy policy = new SharedAccessAccountPolicy()
            {
                Permissions = SharedAccessAccountPermissions.Read |
                                SharedAccessAccountPermissions.Create |
                                SharedAccessAccountPermissions.Write |
                                SharedAccessAccountPermissions.List |
                                SharedAccessAccountPermissions.Add |
                                SharedAccessAccountPermissions.Delete |
                                SharedAccessAccountPermissions.Update,
                Services = SharedAccessAccountServices.Table,
                ResourceTypes = SharedAccessAccountResourceTypes.Object | SharedAccessAccountResourceTypes.Container | SharedAccessAccountResourceTypes.Service,
                SharedAccessExpiryTime = DateTime.UtcNow.AddHours(24),
                Protocols = SharedAccessProtocol.HttpsOnly
            };

            return storageAccount.GetSharedAccessSignature(policy);
        }
    }
}
