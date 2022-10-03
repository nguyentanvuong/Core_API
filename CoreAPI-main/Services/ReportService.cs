using DevExpress.DataAccess.Json;
using DevExpress.XtraReports.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using WebApi.Entities;
using WebApi.Helpers.Common;
using WebApi.Helpers.DatabaseUtils;
using WebApi.Models;
using Utility = WebApi.Helpers.Utils.Utility;

namespace WebApi.Services
{
    public interface IReportService
    {
        public XtraReport BindDataReport(XtraReport report, Dictionary<string, object> reportParams);
        public TransactionReponse GetListReport();
        public TransactionReponse GetReportToken(JObject reportName);
    }
    public class ReportService : IReportService
    {
        private readonly DataContext _context;
        const string ReportNamespace = "WebApi.Reports.";
        private static readonly ILogger _logger = Log.ForContext(typeof(ReportService));
        private readonly AppSettings _appSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReportService(DataContext context, IOptions<AppSettings> appSettings, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _appSettings = appSettings.Value;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Set data source và các parameter cho report
        /// </summary>
        /// <param name="report"></param>
        /// <param name="reportParams"></param>
        /// <returns>XtraReport</returns>
        public XtraReport BindDataReport(XtraReport report, Dictionary<string, object> reportParams)
        {
            try
            {
                string reportName = report.Name;
                var reportInfomation = _context.SReports.SingleOrDefault(x => x.reportcode == reportName);
                if (reportInfomation != null)
                {
                    report = (XtraReport)Utility.GetInstance(ReportNamespace + reportName);
                    if (report == null) throw new FormatException("Report doesn't exist.");
                    foreach (KeyValuePair<string, object> reportParam in reportParams)
                    {
                        if (report.Parameters[reportParam.Key] != null)
                            report.Parameters[reportParam.Key].Value = Convert.ChangeType(reportParam.Value, report.Parameters[reportParam.Key].Type);
                        else
                        {
                            DevExpress.XtraReports.Parameters.Parameter parameter = new DevExpress.XtraReports.Parameters.Parameter
                            {
                                Name = reportParam.Key,
                                Description = reportParam.Key,
                                Value = reportParam.Value,
                                AllowNull = true,
                                Type = typeof(string)
                            };
                            report.Parameters.Add(parameter);
                        }
                    }
                    report.RequestParameters = false;
                    JObject jObjectDataSource = new JObject();
                    var reportDatas = _context.SReportdatas.Where(x => x.reportcode == reportInfomation.reportcode).ToList();
                    foreach (var reportData in reportDatas)
                    {
                        if (reportData.sourcetype.ToUpper() == "SP")
                        {
                            var reportSPParams = _context.SReportparams.Where(x => x.spname == reportData.spname).ToList();
                            var paramSP = GetParamSPReport(reportSPParams, reportParams);
                            string spDataSource = DbFactory.GetDataTableFromStoredProcedure(reportData.spname, paramSP);
                            if (reportData.outputtype.ToUpper() == "OBJECT")
                            {
                                JArray arrayReponse = JArray.Parse(spDataSource);
                                JObject objectReponse = new JObject();
                                if (arrayReponse.Count == 1)
                                {
                                    objectReponse = (JObject)arrayReponse.First();
                                }
                                else if (arrayReponse.Count > 1) throw new FormatException("Stored procedure " + reportData.spname + " return greater than 1 result.");
                                jObjectDataSource.Add(reportData.spname.ToLower(), objectReponse);
                            }
                            else if (reportData.outputtype.ToUpper() == "ARRAY")
                            {
                                JArray arrayReponse = JArray.Parse(spDataSource);
                                jObjectDataSource.Add(reportData.spname.ToLower(), arrayReponse);
                            }
                        }
                    }
                    var jsonDataSource = new JsonDataSource
                    {
                        JsonSource = new CustomJsonSource(jObjectDataSource.ToString())
                    };
                    jsonDataSource.Fill();
                    report.DataSource = jsonDataSource;
                    return report;
                }
                else
                {
                    throw new FormatException("Report doesn't exist.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("{0}.{1}\n{2}", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()));
                throw new FormatException(ex.Message);
            }
        }

        private static Dictionary<string, object> GetParamSPReport(List<SReportparam> reportSPParams, Dictionary<string, object> reportParams)
        {
            Dictionary<string, object> returnParam = new Dictionary<string, object>();
            if (reportSPParams.Count == 0) return null;
            foreach (var reportSPParam in reportSPParams)
            {
                string key = reportSPParam.paramname;
                string type = reportSPParam.paramtype;
                string defaultValue = reportSPParam.defaultvalue;
                try
                {
                    foreach (var reportParam in reportParams)
                    {
                        if (reportParam.Key.ToLower() == key.ToLower())
                        {
                            switch (type)
                            {
                                case "N":
                                    if (!string.IsNullOrEmpty((string)reportParam.Value)) returnParam.Add(key, int.Parse((string)reportParam.Value));
                                    else returnParam.Add(key, int.Parse(string.IsNullOrEmpty(defaultValue) ? "0" : defaultValue));
                                    break;
                                default:
                                    if (!string.IsNullOrEmpty((string)reportParam.Value)) returnParam.Add(key, (string)reportParam.Value);
                                    else returnParam.Add(key, string.IsNullOrEmpty(defaultValue) ? "" : defaultValue);
                                    break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new FormatException(string.Format("Error in parameter '{0}':{1}", key, ex.Message));
                }
            }
            return (returnParam.Count > 0) ? returnParam : null;
        }

        public TransactionReponse GetListReport()
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                List<SReport> reports = _context.SReports.ToList();
                JArray array = JArray.FromObject(reports);
                JObject reportlist = new JObject
                {
                    { "data", array }
                };
                reponse.SetCode(Codetypes.Code_Success);
                reponse.SetResult(reportlist);
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                return new TransactionReponse(new CodeDescription(9997, ex.Message));
            }
        }

        public string generateReportJwtToken(SUsrac user, string reportName)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(GlobalVariable.Identity, user.username),
                    new Claim("report", reportName)
                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public TransactionReponse GetReportToken(JObject reportName)
        {
            string reportNameStr = Utility.GetValueJObjectByKey(reportName, "reportname");
            if(string.IsNullOrEmpty(reportNameStr)) return new TransactionReponse(Codetypes.Err_RequireReportName);
            SReport sReport = _context.SReports.SingleOrDefault(x => x.reportcode == reportNameStr);
            if(sReport == null) return new TransactionReponse(Codetypes.Err_ReportNotExist);
            SUsrac userRequest = (SUsrac)_httpContextAccessor.HttpContext.Items["User"];
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                string token = generateReportJwtToken(userRequest, reportNameStr);
                JObject tokenObject = new JObject
                {
                    { "token", token }
                };
                reponse.SetCode(Codetypes.Code_Success);
                reponse.SetResult(tokenObject);
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                return new TransactionReponse(new CodeDescription(9997, ex.Message));
            }
        }
    }
}
