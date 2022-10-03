using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WebApi.Entities;
using WebApi.Helpers.Common;
using WebApi.Helpers.Utils;
using WebApi.Models;
using ILogger = Serilog.ILogger;

namespace WebApi.Helpers.DatabaseUtils
{
    public class FASTUtils
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(FASTUtils));
        public static bool RefreshFASTToken()
        {
            try
            {
                var response = RestClientService.CallRestAPI(GlobalVariable.FASTRestfulURL + GlobalVariable.URLGetToken, "post", GlobalVariable.FASTGetTokenRequest).Result;
                if (response == null || (int)response["status"]["code"] != 0)
                {
                    return false;
                }
                string strnewtoken = (string)response["data"]["token"];
                SToken token = new SToken
                {
                    bankid = "2",
                    varname = "FASTTOKEN",
                    varext = "FAST",
                    varvalue = strnewtoken,
                    vardate = JWTUtils.GetExpiryTimeJWT(strnewtoken),
                    description = "Token FAST"
                };
                GlobalVariable.CurrentFastToken = token;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<string> CallSOAPServiceAsync(string message)
        {
            _logger.Information(string.Format("{0}.{1}: Request to " + GlobalVariable.FASTSOAPURL + " Message: \n\t{2}", MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, message));
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(message);
            var httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(GlobalVariable.TimeoutCallFASTService)
            };
            var stringContent = new StringContent(doc.InnerXml, Encoding.UTF8, "text/xml");
            var reponse = await httpClient.PostAsync(GlobalVariable.FASTSOAPURL, stringContent);
            string responseContent = await reponse.Content.ReadAsStringAsync();
            _logger.Information(string.Format("{0}.{1}: Reponse Message: \n\t{2}", MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, responseContent));
            if (!reponse.IsSuccessStatusCode)
            {
                throw new Exception("Exception when sending message to FAST: " + reponse.StatusCode + " - " + responseContent);
            }
            return responseContent;
        }

        public static JObject AnalyzeMessageFAST(string responseContent, bool isIgnoredRootNode = false)
        {
            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.LoadXml(responseContent);
            var cdata = XmlDoc.InnerText.Trim();
            if (!XmlUtils.IsValidXml(cdata))
            {
                throw new Exception(cdata);
            }
            XmlDocument xmlreponse = new XmlDocument();
            xmlreponse.LoadXml(cdata);
            xmlreponse = XmlUtils.RemoveXmlDeclaration(xmlreponse);
            JObject jObjectReponse = XmlUtils.ConvertXMLToJSON(xmlreponse, isIgnoredRootNode);
            return jObjectReponse;
        }

        public static JArray AnalyzeListMessageFAST(string responseContent, bool isIgnoredRootNode = false)
        {
            JArray arrayReponse = new JArray();
            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.LoadXml(responseContent);
            var cdata = XmlDoc.InnerText.Trim();
            if (!XmlUtils.IsValidXml(cdata))
            {
                _logger.Information("Reponse get incoming transaction: " + cdata);
                return arrayReponse;
            }
            string[] xmlStringList = cdata.Split(GlobalVariable.FASTGetIncomingSplitSeperater, StringSplitOptions.RemoveEmptyEntries);
            if (xmlStringList.Length == 0)
            {
                throw new Exception(cdata);
            }
            foreach (string xmlString in xmlStringList)
            {
                string xmlNodeString = xmlString + GlobalVariable.FASTGetIncomingSplitSeperater;
                XmlDocument xmlreponse = new XmlDocument();
                xmlreponse.LoadXml(xmlNodeString.Trim());
                xmlreponse = XmlUtils.RemoveXmlDeclaration(xmlreponse);
                JObject jObjectReponse = XmlUtils.ConvertXMLToJSON(xmlreponse, isIgnoredRootNode);
                arrayReponse.Add(jObjectReponse);
            }

            return arrayReponse;
        }

        public static string InsertLogSubTran(string tranref, long ipctransid, string sourceid, string ipctrancode, DateTime? startTime,
                                                string status, string userid, string errorcode, string errordesc = "")
        {
            Dictionary<string, object> insertLogSubTran = new Dictionary<string, object>
                {
                    {"TRANREF" , tranref},
                    {"IPCTRANSID" , ipctransid},
                    {"SOURCEID" , sourceid},
                    {"TRANCODE" , ipctrancode},
                    {"STARTTIME" , startTime},
                    {"ENDTIME" , DateTime.Now},
                    {"STATUS" , status},
                    {"USERID" , userid},
                    {"ERRORCODE" , errorcode},
                    {"ERRORDESC " , errordesc},
                };
            string result = DbFactory.GetVariableFromStoredProcedure("IPCLOGSUBTRANS_INSERT", insertLogSubTran);
            return result;
        }

        public static TransactionReponse CallFunction(JObject txbody, string txproc)
        {
            _logger.Information("CALL TO O9CORE:" + Environment.NewLine + "Request: " + txbody);
            if (string.IsNullOrEmpty(GlobalVariable.O9_GLOBAL_TXDT)) return new TransactionReponse(Codetypes.Err_Not_Started);
            string strResult = O9Utils.GenJsonFunctionRequest(GlobalVariable.O9CoreUser, "", txbody, txproc);
            if (string.IsNullOrEmpty(strResult)) return new TransactionReponse(Codetypes.Err_Unknown);
            var reponse = O9Utils.AnalysisFunctionResult(strResult);
            _logger.Information(Environment.NewLine + "Reponse: " + JsonConvert.SerializeObject(reponse));
            return reponse;
        }

        public static string GetSWIFTCodeByFastCode(string fastCode)
        {
            string sql = "SELECT BANKID FROM FAST_BANK WHERE participatecode = '"+fastCode+"';";
            return DbFactory.GetVariableFromSQL(sql);
        }

        public static string GetFASTCodeByFastCode(string SWIFTCode)
        {
            string sql = "SELECT participatecode FROM FAST_BANK WHERE BANKID = '" + SWIFTCode + "';";
            return DbFactory.GetVariableFromSQL(sql);
        }
    }
}
