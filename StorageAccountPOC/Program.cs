using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageAccountPOC
{
    public class Program
    {
        static void Main(string[] args)
        {
            var provider = new SasProvider();
            var storageAccountName = ConfigurationManager.AppSettings["SiteStorageAccountName"];
            var sasToken = provider.SASToken;

            StorageCredentials accountSAS = new StorageCredentials(sasToken);
            CloudStorageAccount accountWithSAS = new CloudStorageAccount(accountSAS, storageAccountName, endpointSuffix: null, useHttps: true);
            var tableClient = accountWithSAS.CreateCloudTableClient();
            var table = tableClient.GetTableReference("Users");
            table.CreateIfNotExists();

            var query = new TableQuery();
            var results = table.ExecuteQuery(query);

            Console.WriteLine(JsonConvert.SerializeObject(results));

            var tableEntity = new TableEntity("Users", "{\"name\":\"Antonio\", \"lastName\":\"Briones\"}");
            var op = TableOperation.InsertOrReplace(tableEntity);
            table.Execute(op);

            op = TableOperation.Delete(tableEntity);
            table.Execute(op);
        }
    }
}
