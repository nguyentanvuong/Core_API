using System.Collections.Generic;
using WebApi.Models;

namespace WebApi.Helpers.Common
{
    public class AppSettings
    {
        public string Port { get; set; }
        public string Secret { get; set; }
        public string DatabaseProvider { get; set; }
        public string DBConnectionString { get; set; }
        public string TransferFilePath { get; set; }
        public string ImageTranferPath { get; set; }
        public string Memcached { get; set; }
        public bool UsingJWT { get; set; }
        public int PoolConnection { get; set; }
        public CoreConfig Configure { get; set; }
        public AMLConfig AMLConfigure { get; set; }
        public List<LoginRequest> DefaultUser { get; set; }
        public int ExpiryDateJWT { get; set; }
        public bool UsingLicense { get; set; }
        public bool UsingFullLog { get; set; } = false;
        public bool UsingEncrypt { get; set; }
    }
}