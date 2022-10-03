using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WebApi.Entities;
using WebApi.Helpers.Common;
using WebApi.Helpers.DatabaseUtils;
using WebApi.Helpers.Utils;
using WebApi.Models;
using WebApi.Models.Transaction.AML;
using WebApi.Models.Transaction.LRC;
using WebApi.Models.Transaction.MB;

namespace WebApi.Services
{
    public interface IApiService
    {
        public TransactionReponse GetWorkingDate();
        public TransactionReponse CallFrontOffice(TransactionRequest model, string txcode, bool isMapping = true, Users user = null);
        public TransactionReponse CallFrontOffice(JObject model, String session, string txcode, bool isMapping = true, Users user = null);
        public TransactionReponse CallFunction(TransactionRequest model, string txcode, Users user = null);
        public TransactionReponse CallTransferImage(MB1000 model, Users user = null);
        public TransactionReponse CallTransferFile(FileTransferRequest model);
        public Task<TransactionReponse> AMLScanningServiceAsync(ScanRequest model);
        public Task<TransactionReponse> AMLCheckingServiceAsync(CheckRequest model);
    }

    public class ApiService : IApiService
    {
        private readonly DataContext _context;
        private readonly AppSettings _appSettings;
        private readonly DbUtils dbUtils;
        private readonly ILogger<ApiService> _logger;

        public ApiService(DataContext context, IOptions<AppSettings> appSettings, ILogger<ApiService> logger)
        {
            _logger = logger;
            _context = context;
            _appSettings = appSettings.Value;
            dbUtils = new DbUtils(_context);
        }

        public TransactionReponse GetWorkingDate()
        {
            JObject jsonResult = new JObject();
            if (string.IsNullOrEmpty(GlobalVariable.O9_GLOBAL_TXDT)) return new TransactionReponse(Codetypes.Err_Not_Started);
            TransactionReponse response = new TransactionReponse(Codetypes.Code_Success_Trans);
            jsonResult.Add(GlobalVariable.WorkingDate, GlobalVariable.O9_GLOBAL_TXDT);
            response.SetResult(jsonResult);

            return response;
        }

        public TransactionReponse CallFrontOffice(TransactionRequest model, string txcode, bool isMapping = true, Users user = null)
        {
            if (user == null)
            {
                if (String.IsNullOrEmpty(model.SESSIONID)) return new TransactionReponse(Codetypes.Err_RequireSessionID);
                user = dbUtils.GetUsersBySession(model.SESSIONID);
            }
            if (user == null) return new TransactionReponse(Codetypes.Err_Unauthorized);
            if (string.IsNullOrEmpty(GlobalVariable.O9_GLOBAL_TXDT)) return new TransactionReponse(Codetypes.Err_Not_Started);
            JObject txbody = JObject.Parse(JsonConvert.SerializeObject(model));
            string strResult = O9Utils.GenJsonFrontOfficeRequest(user, txcode, txbody, GlobalVariable.O9_GLOBAL_TXDT, isMapping: isMapping);
            if (string.IsNullOrEmpty(strResult)) return new TransactionReponse(Codetypes.Err_Unknown);
            return O9Utils.AnalysisFrontOfficeResult(strResult);
        }

        public TransactionReponse CallFrontOffice(JObject model, String session, string txcode, bool isMapping = true, Users user = null)
        {
            if (user == null)
            {
                if (String.IsNullOrEmpty(session)) return new TransactionReponse(Codetypes.Err_RequireSessionID);
                user = dbUtils.GetUsersBySession(session);
            }
            if (user == null) return new TransactionReponse(Codetypes.Err_Unauthorized);
            if (string.IsNullOrEmpty(GlobalVariable.O9_GLOBAL_TXDT)) return new TransactionReponse(Codetypes.Err_Not_Started);
            JObject txbody = JObject.Parse(JsonConvert.SerializeObject(model));
            string strResult = O9Utils.GenJsonFrontOfficeRequest(user, txcode, txbody, GlobalVariable.O9_GLOBAL_TXDT, isMapping: isMapping);
            if (string.IsNullOrEmpty(strResult)) return new TransactionReponse(Codetypes.Err_Unknown);
            return O9Utils.AnalysisFrontOfficeResult(strResult);
        }


        public TransactionReponse CallFunction(TransactionRequest model, string txproc, Users user = null)
        {
            if (user == null)
            {
                if (String.IsNullOrEmpty(model.SESSIONID)) return new TransactionReponse(Codetypes.Err_RequireSessionID);
                user = dbUtils.GetUsersBySession(model.SESSIONID);
            }
            if (user == null) return new TransactionReponse(Codetypes.Err_Unauthorized);
            if (string.IsNullOrEmpty(GlobalVariable.O9_GLOBAL_TXDT)) return new TransactionReponse(Codetypes.Err_Not_Started);
            JObject txbody = JObject.Parse(JsonConvert.SerializeObject(model));
            string strResult = O9Utils.GenJsonFunctionRequest(user, "", txbody, txproc);
            if (string.IsNullOrEmpty(strResult)) return new TransactionReponse(Codetypes.Err_Unknown);
            return O9Utils.AnalysisFunctionResult(strResult);
        }

        public TransactionReponse CallTransferImage(MB1000 model, Users user = null)
        {
            try
            {
                if (user == null)
                {
                    if (String.IsNullOrEmpty(model.SESSIONID)) return new TransactionReponse(Codetypes.Err_RequireSessionID);
                    user = dbUtils.GetUsersBySession(model.SESSIONID);
                }
                if (user == null) return new TransactionReponse(Codetypes.Err_Unauthorized);
                if (string.IsNullOrEmpty(GlobalVariable.O9_GLOBAL_TXDT)) return new TransactionReponse(Codetypes.Err_Not_Started);
                string fileName = model.CUSCODE + "." + model.FILETYPE;
                string directory = _appSettings.ImageTranferPath;
                string path = directory + "/" + fileName;
                if (!"png|jpg|PNG|JPG".Contains(model.FILETYPE)) return new TransactionReponse(Codetypes.Err_Image_Extensions);
                if (!Directory.Exists(directory)) return new TransactionReponse(Codetypes.Err_Folder_Config);
                if (File.Exists(path)) return new TransactionReponse(Codetypes.Err_Image_Duplicate);
                byte[] imageBytes = Convert.FromBase64String(model.BASE64STRING);
                FileStream fs = File.Create(path);
                fs.Write(imageBytes, 0, imageBytes.Length);
                fs.Close();
                return new TransactionReponse(Codetypes.Code_Success_Trans);
            }
            catch (IOException)
            {
                return new TransactionReponse(Codetypes.Err_AccessFolder);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.ToString());
                return new TransactionReponse(Codetypes.Err_Unknown);
            }
        }

        public TransactionReponse CallTransferFile(FileTransferRequest model)
        {
            try
            {
                string fileName = model.MSGCODE + ".103";
                string directory = _appSettings.TransferFilePath;
                string path = directory + "/" + fileName;
                if (!Directory.Exists(directory)) return new TransactionReponse(Codetypes.Err_Folder_Config);
                if (File.Exists(path)) return new TransactionReponse(Codetypes.Err_MT103_Duplicate);
                System.IO.File.WriteAllText(@path, model.CONTENT);
                return new TransactionReponse(Codetypes.Code_Success_Trans);
            }
            catch (IOException)
            {
                return new TransactionReponse(Codetypes.Err_AccessFolder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new TransactionReponse(Codetypes.Err_Unknown);
            }
        }

        public async Task<TransactionReponse> AMLScanningServiceAsync(ScanRequest model)
        {
            TransactionReponse transactionReponse = new TransactionReponse();
            OnlineScan onlineScan = new OnlineScan(model.SearchCountry, model.SearchDOB, model.SysName, model.PassportVerify, model.SearchName, model.ScanOption,
                                                model.SearchID, model.PassExpiryDate, model.Username, model.PassExpiryDtVerify, model.RiskFactors, model.Content,
                                                model.BrCode, model.SecurityNo1, model.SecurityNo2);
            var requestXml = XmlUtils.AddAMLElement(XmlUtils.GenerateXML<OnlineScan>(onlineScan, true, "tem", "http://tempuri.org/"));
            var httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(10);
            httpClient.DefaultRequestHeaders.Add("SOAPAction", "http://tempuri.org/iAMLWS/OnlineScan");
            var stringContent = new StringContent(requestXml.InnerXml, Encoding.UTF8, "text/xml");
            try
            {
                var respone = await httpClient.PostAsync(_appSettings.AMLConfigure.GetScanURL(), stringContent);
                string responseContent = await respone.Content.ReadAsStringAsync();
                XmlDocument XmlDoc = new XmlDocument();
                XmlDoc.LoadXml(responseContent);
                XmlDoc = XmlUtils.RemoveAMLElement(XmlDoc, "s:Envelope");
                XmlDoc = XmlUtils.RemoveAMLElement(XmlDoc, "s:Body");
                JObject result = XmlUtils.XmlToJsonObject(XmlDoc.InnerXml, false);
                transactionReponse.SetResult(result);
                if (respone.IsSuccessStatusCode) transactionReponse.SetCode(Codetypes.Code_Success_Trans);
                else transactionReponse.SetCode(Codetypes.Err_AML_Exception);
                return transactionReponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new TransactionReponse(new CodeDescription(9997, ex.Message));
            }
        }

        public async Task<TransactionReponse> AMLCheckingServiceAsync(CheckRequest model) 
        {
            TransactionReponse transactionReponse = new TransactionReponse();
            OnlineStatus onlineStatus = new OnlineStatus(model.Username, model.BrCode, model.SysName, model.ScanOption, model.ScanID);
            var requestXml = XmlUtils.AddAMLElement(XmlUtils.GenerateXML<OnlineStatus>(onlineStatus, true, "tem", "http://tempuri.org/"));
            var httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(10);
            httpClient.DefaultRequestHeaders.Add("SOAPAction", "http://tempuri.org/iAMLStatus/OnlineStatus");
            var stringContent = new StringContent(requestXml.InnerXml, Encoding.UTF8, "text/xml");
            try
            {
                string url = _appSettings.AMLConfigure.GetCheckURL();
                var respone = await httpClient.PostAsync(url, stringContent);
                string responseContent = await respone.Content.ReadAsStringAsync();
                XmlDocument XmlDoc = new XmlDocument();
                XmlDoc.LoadXml(responseContent);
                XmlDoc = XmlUtils.RemoveAMLElement(XmlDoc, "s:Envelope");
                XmlDoc = XmlUtils.RemoveAMLElement(XmlDoc, "s:Body");
                JObject result = XmlUtils.XmlToJsonObject(XmlDoc.InnerXml, false);
                transactionReponse.SetResult(result);
                if (respone.IsSuccessStatusCode) transactionReponse.SetCode(Codetypes.Code_Success_Trans);
                else transactionReponse.SetCode(Codetypes.Err_AML_Exception);
                return transactionReponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new TransactionReponse(new CodeDescription(9997, ex.Message));
            }
        }

    }
}