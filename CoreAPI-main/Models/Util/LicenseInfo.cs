using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Util
{
    public class LicenseInfo
    {
        public LicenseInfo()
        {
        }
        public string BankName { get; set; }
        public string ExpiryDate { get; set; }
        public List<string> ListModule { get; set; }
        public string ListModuleMD5 { get; set; }
    }
}
