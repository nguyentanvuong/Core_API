using Apache.NMS;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using WebApi.Entities;
using WebApi.Helpers.Common;
using WebApi.Models;

namespace WebApi.Helpers.Utils
{
    public class O9Utils
    {
        public static string GenJsonBodyRequest(Object objJsonBody, string functionId, string sessionid = "", EnmCacheAction isResultCaching = EnmCacheAction.NoCached, EnmSendTypeOption sendtype = EnmSendTypeOption.Synchronize, string usrId = "-1", MsgPriority priority = MsgPriority.Normal)
        {
            try
            {
                if (string.IsNullOrEmpty(functionId)) return String.Empty;
                string strRequest = String.Empty;
                string strResult = String.Empty;
                O9Client o9Client = new O9Client();
                if (objJsonBody != null) strRequest = JsonConvert.SerializeObject(objJsonBody);
                strResult = o9Client.SendString(strRequest, functionId, usrId, sessionid, EnmCacheAction.NoCached, EnmSendTypeOption.Synchronize, MsgPriority.Normal);
                o9Client = null;
                return strResult;
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }
        public static string GenJsonFunctionRequest(Users user, string ptxcode,JObject ptxbody,string ptxproc = "", string functionId = "",EnmCacheAction isCaching = EnmCacheAction.NoCached)
        {
            try
            {
                JsonFunction clsJsonFunction = new JsonFunction();
                if (string.IsNullOrEmpty(functionId)) functionId = GlobalVariable.UTIL_CALL_FUNC;
                clsJsonFunction.TXCODE = ptxcode;
                clsJsonFunction.TXPROC = ptxproc;
                clsJsonFunction.TXBODY = ptxbody;
                return GenJsonBodyRequest(new JsonFunctionMapping (clsJsonFunction), functionId, user.ssesionid, isCaching, EnmSendTypeOption.Synchronize, user.usrid.ToString());
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }
        public static string GenJsonFrontOfficeRequest(Users user, string ptxcode, JObject ptxbody, string ptxdt, string functionId = "", EnmCacheAction isResultCaching = EnmCacheAction.NoCached,
                                                        string pstatus = "C", string ptxrefid = null, string pvaluedt = null, string pusrws = null, Object papuser = null,
                                                        string papusrip = null, string papusrws = null, string papdt = null, string pisreverse = "N", int? phbranchid = null, int? prbranchid = null,
                                                        string papreason = null, JsonPosting pposting = null, JObject pifcfee = null, string pprn = null, string pid = null, bool isMapping = true)
        {
            try
            {
                JsonFrontOffice clsJsonFrontOffice = new JsonFrontOffice();
                if (string.IsNullOrEmpty(functionId)) functionId = GlobalVariable.UTIL_CALL_PROC;
                if (string.IsNullOrEmpty(pprn)) pprn = "";

                clsJsonFrontOffice.TXCODE = ptxcode;
                clsJsonFrontOffice.TXDT = ptxdt;
                clsJsonFrontOffice.BRANCHID = user.usrbranchid;
                clsJsonFrontOffice.USRID = user.usrid;
                clsJsonFrontOffice.LANG = user.lang;
                clsJsonFrontOffice.STATUS = pstatus;
                clsJsonFrontOffice.TXREFID = ptxrefid;
                clsJsonFrontOffice.VALUEDT = ptxdt;
                clsJsonFrontOffice.USRWS = pusrws;
                clsJsonFrontOffice.APUSER = papuser;
                clsJsonFrontOffice.APUSRIP = papusrip;
                clsJsonFrontOffice.APUSRWS = papusrws;
                clsJsonFrontOffice.APDT = papdt;
                clsJsonFrontOffice.ISREVERSE = pisreverse;
                clsJsonFrontOffice.HBRANCHID = phbranchid;
                clsJsonFrontOffice.RBRANCHID = prbranchid;
                clsJsonFrontOffice.TXBODY = ptxbody;
                clsJsonFrontOffice.APREASON = papreason;
                clsJsonFrontOffice.IFCFEE = pifcfee;
                clsJsonFrontOffice.PRN = pprn;
                if (string.IsNullOrEmpty(pid)) pid = Guid.NewGuid().ToString();
                clsJsonFrontOffice.ID = pid;

                if (isMapping) return GenJsonBodyRequest(new JsonFrontOfficeMapping(clsJsonFrontOffice), functionId, user.ssesionid, EnmCacheAction.NoCached, EnmSendTypeOption.Synchronize, user.usrid.ToString());
                return GenJsonBodyRequest(clsJsonFrontOffice, functionId, user.ssesionid, EnmCacheAction.NoCached, EnmSendTypeOption.Synchronize, user.usrid.ToString());
            }
            catch (Exception)
            {
                return String.Empty;
            }

        }
        public static TransactionReponse AnalysisLoginResult(string error, string wsName = "")
        {
            TransactionReponse reponse = new TransactionReponse();
            if (error.Equals("SYS_INVALID_SESSION")) {
                reponse.SetCode(Codetypes.SYS_INVALID_SESSION);
                reponse.messagedetail = reponse.messagedetail + "[" + wsName + "]";
            }
            else if (error.Equals("UMG_INVALID_LOGIN_TIME")) reponse.SetCode(Codetypes.UMG_INVALID_LOGIN_TIME);
            else if (error.Equals("UMG_INVALID_EXP_POLICY")) reponse.SetCode(Codetypes.UMG_INVALID_EXP_POLICY);
            else if (error.Equals("SYS_LOGIN_FALSE")) reponse.SetCode(Codetypes.SYS_LOGIN_FALSE);
            else if (error.Equals("SYS_LOGIN_BLOCK")) reponse.SetCode(Codetypes.SYS_LOGIN_BLOCK);
            else if (error.Equals("UMG_INVALID_STATUS")) reponse.SetCode(Codetypes.UMG_INVALID_STATUS);
            else if (error.Equals("UMG_INVALID_EXPDT")) reponse.SetCode(Codetypes.UMG_INVALID_EXPDT);
            else reponse.SetCode(Codetypes.Err_Unknown);
            return reponse;
        }
        public static TransactionReponse AnalysisFrontOfficeResult(string strResponse, bool ispare = true)
        {
            TransactionReponse transactionReponse = new TransactionReponse();
            JsonFrontOffice clsJsonFrontOffice;
            JsonFrontOfficeMapping clsJsonFrontOfficeMP;
            try
            {
                JsonResponse jsonResponse = JsonConvert.DeserializeObject<JsonResponse>(strResponse);
                if (jsonResponse.IsOK())
                {
                    if (ispare)
                    {
                        clsJsonFrontOfficeMP = JsonConvert.DeserializeObject<JsonFrontOfficeMapping>(jsonResponse.GetMessage());
                        clsJsonFrontOffice = new JsonFrontOffice(clsJsonFrontOfficeMP);
                    }
                    else
                    {
                        clsJsonFrontOffice = JsonConvert.DeserializeObject<JsonFrontOffice>(jsonResponse.GetMessage());
                    }
                    transactionReponse.SetCode(Codetypes.Code_Success_Trans);
                    if (clsJsonFrontOffice.RESULT != null) transactionReponse.SetResult(clsJsonFrontOffice.RESULT);
                    if (clsJsonFrontOffice.IBRET != null) transactionReponse.SetResult(clsJsonFrontOffice.IBRET);
                }
                else
                {   
                    if (jsonResponse.GetMessage().Contains("SYS_AUTHORIZE_VIOLATE")) transactionReponse.SetCode(Codetypes.Err_Unauthorized);
                    else transactionReponse.SetCode(new CodeDescription(9997, GetReasonString(jsonResponse.GetMessage())));
                }
                return transactionReponse;
            }
            catch (Exception)
            {
                transactionReponse.SetCode(Codetypes.Err_Unknown);
                return transactionReponse;
            }
        }
        public static TransactionReponse AnalysisFunctionResult(string strResponse)
        {
            TransactionReponse transactionReponse = new TransactionReponse();
            JsonFunction clsFunction;
            JsonFunctionMapping jsonFunctionMapping;
            try
            {
                JsonResponse jsonResponse = JsonConvert.DeserializeObject<JsonResponse>(strResponse);
                if (jsonResponse.IsOK())
                {
                    jsonFunctionMapping = JsonConvert.DeserializeObject<JsonFunctionMapping>(jsonResponse.GetMessage());
                    clsFunction = new JsonFunction(jsonFunctionMapping);
                    transactionReponse.SetCode(Codetypes.Code_Success_Trans);
                    if (clsFunction.RESULT != null) transactionReponse.SetResult(clsFunction.RESULT);
                }
                else
                {
                    transactionReponse.SetCode(new CodeDescription(9998, GetReasonString(jsonResponse.GetMessage())));
                }
                return transactionReponse;
            }
            catch (Exception)
            {
                transactionReponse.SetCode(Codetypes.Err_Unknown);
                return transactionReponse;
            }
        }
        public static JObject GetMapFieldFrontOffice(string txcode)
        {
            if (string.IsNullOrEmpty(txcode)) return null;
            JObject jsObject = null;
            string key = "O9." + O9Client.OP_MCKEY_FMAP + "." + txcode;
            string strMapField = O9Client.memCached.GetValue(key);
            if (strMapField != null && !strMapField.Equals(""))
            {
                jsObject = JsonConvert.DeserializeObject<JObject>(strMapField);
            }
            return jsObject;
        }
        public static bool JsonContains(JObject jsObject, string key)
        {
            Object jsItem = null;
            if (!string.IsNullOrEmpty(key)) jsItem = jsObject.SelectToken(key);
            if (jsItem != null) return true;
            return false;
        }
        public static void UpdateWorkingDate(string date)
        {
            if (!GlobalVariable.O9_GLOBAL_TXDT.Equals(date))
                GlobalVariable.O9_GLOBAL_TXDT = date;
        }
        public static string GetReasonString(string strResult)
        {
            string strReason = String.Empty;
            int start = strResult.IndexOf("]") + 1;
            string strEnd = " - " + "en";
            strReason = strResult.Substring(start, strResult.Length - start);
            if (strReason.EndsWith(strEnd)) strReason = strReason.Substring(0, strReason.Length - strEnd.Length);
            return strReason.Trim();
        }
        public static string ConvertTimeStampToShortString(long time)
        {
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dt = dt.AddMilliseconds(time).ToLocalTime();
            return (dt.ToString(GlobalVariable.FORMAT_SHORT_DATE));
        }
        public static string ConvertTimeStampToLongString(long time)
        {
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dt = dt.AddMilliseconds(time).ToLocalTime();
            return (dt.ToString(GlobalVariable.FORMAT_LONG_DATE));
        }
    }

    public class RandomGenerator
    {
        // Instantiate random number generator.  
        // It is better to keep a single Random instance 
        // and keep using Next on the same instance.  
        private readonly Random _random = new Random();

        // Generates a random number within a range.      
        public int RandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }

        // Generates a random string with a given size.    
        public string RandomString(int size, bool lowerCase = false)
        {
            var builder = new StringBuilder(size);

            // Unicode/ASCII Letters are divided into two blocks
            // (Letters 65–90 / 97–122):   
            // The first group containing the uppercase letters and
            // the second group containing the lowercase.  

            // char is a single Unicode character  
            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26; // A...Z or a..z: length = 26  

            for (var i = 0; i < size; i++)
            {
                var @char = (char)_random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }

        // Generates a random password.  
        // 4-LowerCase + 4-Digits + 2-UpperCase  
        public string RandomPassword()
        {
            var passwordBuilder = new StringBuilder();

            // 4-Letters lower case   
            passwordBuilder.Append(RandomString(4, true));

            // 4-Digits between 1000 and 9999  
            passwordBuilder.Append(RandomNumber(1000, 9999));

            // 2-Letters upper case  
            passwordBuilder.Append(RandomString(2));
            return passwordBuilder.ToString();
        }
    }
}
