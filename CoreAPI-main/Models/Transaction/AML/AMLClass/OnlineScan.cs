using System;
using System.Xml.Serialization;
using WebApi.Helpers.Common;

namespace WebApi.Models.Transaction.AML
{
    [XmlRoot(Namespace = "http://tempuri.org/")]
    public class OnlineScan
    {
        public string AuthUser { get; set; }
        public string AuthPswd { get; set; }
        public string Username { get; set; }
        public string BrCode { get; set; }
        public string SysName { get; set; }
        public string ScanOption { get; set; }
        public string UniqueNo { get; set; }
        public string SearchName { get; set; }
        public string SearchCountry { get; set; }
        public string SearchDOB { get; set; }
        public string SearchID { get; set; }
        public string PassportVerify { get; set; }
        public string SecurityNo1 { get; set; }
        public string PassExpiryDtVerify { get; set; }
        public string PassExpiryDate { get; set; }
        public string SecurityNo2 { get; set; }
        public string RiskFactors { get; set; }
        public string Content { get; set; }
        public string RtnHit { get; set; }
        public string RtnScanID { get; set; }
        public string RtnEnCryptScanID { get; set; }

        public OnlineScan()
        {

        }


        public OnlineScan(string searchCountry, string searchDOB, string sysName, string passportVerify, string searchName, string scanOption, string searchID, string passExpiryDate, string username, string passExpiryDtVerify, string riskFactors, string content, string brCode, string securityNo1, string securityNo2)
        {
            AuthUser = GlobalVariable.AMLAuthUser;
            AuthPswd = GlobalVariable.AMLAuthPswd;
            UniqueNo = Guid.NewGuid().ToString();
            RtnHit = "1";
            RtnScanID = "0";
            RtnEnCryptScanID = "";
            SearchCountry = searchCountry;
            SearchDOB = searchDOB;
            SysName = sysName;
            PassportVerify = passportVerify;
            SearchName = searchName;
            ScanOption = scanOption;
            SearchID = searchID;
            PassExpiryDate = passExpiryDate;
            Username = username;
            PassExpiryDtVerify = passExpiryDtVerify;
            RiskFactors = riskFactors;
            Content = content;
            BrCode = brCode;
            SecurityNo1 = securityNo1;
            SecurityNo2 = securityNo2;
        }

        public OnlineScan(string authUser, string authPswd, string uniqueNo, string rtnHit, string rtnScanID, string rtnEnCryptScanID, string searchCountry, string searchDOB, string sysName, string passportVerify, string searchName, string scanOption, string searchID, string passExpiryDate, string username, string passExpiryDtVerify, string riskFactors, string content, string brCode, string securityNo1, string securityNo2)
        {
            AuthUser = authUser;
            AuthPswd = authPswd;
            UniqueNo = uniqueNo;
            RtnHit = rtnHit;
            RtnScanID = rtnScanID;
            RtnEnCryptScanID = rtnEnCryptScanID;
            SearchCountry = searchCountry;
            SearchDOB = searchDOB;
            SysName = sysName;
            PassportVerify = passportVerify;
            SearchName = searchName;
            ScanOption = scanOption;
            SearchID = searchID;
            PassExpiryDate = passExpiryDate;
            Username = username;
            PassExpiryDtVerify = passExpiryDtVerify;
            RiskFactors = riskFactors;
            Content = content;
            BrCode = brCode;
            SecurityNo1 = securityNo1;
            SecurityNo2 = securityNo2;
        }
    }
}
