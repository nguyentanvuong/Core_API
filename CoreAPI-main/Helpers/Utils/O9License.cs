using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Util;

namespace WebApi.Helpers.Utils
{
    public class O9License
    {
        public static string GenLicsence(LicenseInfo licenseInfo)
        {
            try
            {
                if (!Utility.IsDate(licenseInfo.ExpiryDate)) return null;
                string result = O9Encrypt.Encrypt(licenseInfo.BankName + licenseInfo.ExpiryDate);
                licenseInfo.ListModule.Sort();
                result += O9Encrypt.MD5Encrypt(licenseInfo.ListModule.Count == 0 ? "" : string.Join(",", licenseInfo.ListModule.ToArray()));
                result = O9Encrypt.Base64Encode(result);
                return result;
            }
            catch
            {
                return null;
            }
        }

        public static LicenseInfo GetLicsence(string licenseString)
        {
            try
            {
                LicenseInfo licenseInfo = new LicenseInfo();
                string decodeString = O9Encrypt.Base64Decode(licenseString);
                if (decodeString == null) return null;
                string bankInfoDecode = decodeString.Substring(0, decodeString.Length - 24);
                licenseInfo.ListModuleMD5 = decodeString.Substring(decodeString.Length - 24);
                string bankInfor = O9Encrypt.Decrypt(bankInfoDecode);
                licenseInfo.BankName = bankInfor.Substring(0, bankInfor.Length - 10);
                licenseInfo.ExpiryDate = bankInfor.Substring(bankInfor.Length - 10);
                return licenseInfo;
            }
            catch
            {
                return null;
            }
        }

        public static bool CheckLicense(string licenseString)
        {
            try
            {
                LicenseInfo licenseInfo = GetLicsence(licenseString);
                if (licenseInfo == null)
                {
                    return false;
                }
                DateTime expiryDate = Utility.ConvertStringToDateTime(licenseInfo.ExpiryDate);
                if (expiryDate.Date < DateTime.Now.Date)
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
