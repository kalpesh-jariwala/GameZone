using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Core.Certificates
{
    public class Connections
    {
        public string[] Urls { get; set; }
        public X509Certificate2 Certificate { get; set; }
        public string DatabaseName { get; set; }

        public Connections(string connectionName, string dbName)
        {
            string databaseName = dbName;
            string[] localUrls = new string[] { "http://127.0.0.1:8081", "http://127.0.0.1:8080" };
            string[] localCertificates = new string[] { "blah" };
            X509Store x509Store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            x509Store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            X509Certificate2Collection collection = x509Store.Certificates;
            if (connectionName == "local")
            {
                DatabaseName = databaseName;

                Urls = localUrls;
                Certificate = null;
                foreach (var cert in collection)
                {
                    foreach (var localCertificate in localCertificates)
                    {
                        if (cert.FriendlyName == string.Concat("CN=", localCertificate))
                        {
                            Certificate = new X509Certificate2(cert);
                            break;
                        }
                    }
                }
            }
        }
    }
}
