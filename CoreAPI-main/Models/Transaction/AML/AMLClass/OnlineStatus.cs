using System;
using System.Xml.Serialization;
using WebApi.Helpers.Common;

namespace WebApi.Models.Transaction.AML
{
    [XmlRoot(Namespace = "http://tempuri.org/")]
    public class OnlineStatus
    {
        public string AuthUser { get; set; }
        public string AuthPass { get; set; }
        public string Username { get; set; }
        public string BrCode { get; set; }
        public string SysName { get; set; }
        public string ScanOption { get; set; }
        public string UniqueNo { get; set; }
        public string scanID { get; set; }
        public string rtnResult { get; set; }

        public OnlineStatus()
        {

        }

        public OnlineStatus(string username, string brCode, string sysName, string scanOption, string scanID)
        {
            AuthUser = GlobalVariable.AMLAuthUser;
            AuthPass = GlobalVariable.AMLAuthPswd;
            UniqueNo = Guid.NewGuid().ToString();
            Username = username;
            BrCode = brCode;
            SysName = sysName;
            ScanOption = scanOption;
            this.scanID = scanID;
        }
    }
}
