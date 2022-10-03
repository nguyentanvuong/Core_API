using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using WebApi.Helpers.Common;
using WebApi.Helpers.DatabaseUtils;
using WebApi.Services;

namespace WebApi.Reports
{
    public class CustomReportStorageWebExtension : ReportStorageWebExtension
    {
        private readonly string ReportDirectory;
        const string FileExtension = ".repx";
        private readonly IReportService _reportService;
        private readonly DataContext _context;
        private readonly ILogger<CustomReportStorageWebExtension> _logger;
        private readonly AppSettings _appSettings;
        public CustomReportStorageWebExtension(IWebHostEnvironment env, DataContext context, ILogger<CustomReportStorageWebExtension> logger, IOptions<AppSettings> appSettings, IHttpContextAccessor httpContextAccessor)
        {
            ReportDirectory = Path.Combine(env.ContentRootPath, "Reports");
            if (!Directory.Exists(ReportDirectory))
            {
                Directory.CreateDirectory(ReportDirectory);
            }
            _appSettings = appSettings.Value;
            _reportService = new ReportService(context, appSettings, httpContextAccessor);
            _context = context;
            _logger = logger;
        }

        public override bool CanSetData(string url)
        {
            // Xác định xem có thể lưu trữ báo cáo theo một URL nhất định hay không.
            // Ví dụ: đặt phương thức CanSetData trả về false đối với các báo cáo phải ở chế độ chỉ đọc trong bộ nhớ.
            // Phương thức này chỉ được gọi cho các URL hợp lệ (tức là nếu phương thức IsValidUrl trả về true) trước khi phương thức SetData được gọi. 
            return true;
        }

        public override bool IsValidUrl(string url)
        {
            // Hàm này sẽ check url report có hợp lệ hay không. Không xài
            return true;
        }

        /// <summary>
        /// Trả về report layout tương ứng với url hợp lệ
        /// </summary>
        /// <param name="url"></param>
        /// <returns>byte[]</returns>
        public override byte[] GetData(string url)
        {
            try
            {
                string[] parts = url.Split("?");
                string token = "";
                string reportName = parts[0];
                string parametersString = parts.Length > 1 ? parts[1] : String.Empty;
                var parameters = HttpUtility.ParseQueryString(parametersString);
                Dictionary<string, object> reportParam = new Dictionary<string, object>();
                foreach (string paramName in parameters.AllKeys)
                {
                    if (paramName == "token") 
                    {
                        token = parameters.Get(paramName);
                        continue;
                    }
                    string paramValue = parameters.Get(paramName);
                    reportParam.Add(paramName, paramValue);
                }
                bool checkToken = CheckTokenReport(token, reportName);
                if(!checkToken) throw new DevExpress.XtraReports.Web.ClientControls.FaultException("Unauthorized token.");
                XtraReport report = null;
                if (Directory.EnumerateFiles(ReportDirectory).
                    Select(Path.GetFileNameWithoutExtension).Contains(reportName))
                {
                    byte[] reportBytes = File.ReadAllBytes(
                        Path.Combine(ReportDirectory, reportName + FileExtension));
                    using (MemoryStream ms = new MemoryStream(reportBytes))
                        report = XtraReport.FromStream(ms);
                    report = _reportService.BindDataReport(report, reportParam);
                }
                else
                {
                    throw new DevExpress.XtraReports.Web.ClientControls.FaultException(string.Format("Could not find file [{0}] in folder [{1}]", reportName + FileExtension, ReportDirectory));
                }
                if (report != null)
                {
                    using MemoryStream ms = new MemoryStream();
                    report.SaveLayoutToXml(ms);
                    return ms.ToArray();
                }
                throw new DevExpress.XtraReports.Web.ClientControls.FaultException(string.Format("Could not find report '{0}'.", reportName));
            }
            catch (FormatException ex)
            {
                throw new DevExpress.XtraReports.Web.ClientControls.FaultException(string.Format("Could not open report '{0}' by error: {1}", url.Split("?")[0], ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}.{1}\n{2}", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()));
                throw new DevExpress.XtraReports.Web.ClientControls.FaultException(string.Format("Could not open report '{0}': {1}", url.Split("?")[0], ex.Message));
            }
        }

        private bool CheckTokenReport(string token, string reportName)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;
                if (jwtToken == null) return false;
                else 
                {
                    string reportNameToken = jwtToken.Claims.FirstOrDefault(x => x.Type == "report").Value;
                    string usernameToken = jwtToken.Claims.FirstOrDefault(x => x.Type == GlobalVariable.Identity).Value;
                    if (reportNameToken != reportName) return false;
                    if (usernameToken == null) return false;
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public override Dictionary<string, string> GetUrls()
        {
            // Trả về từ điển các URL báo cáo hiện có và tên hiển thị.
            // Phương thức này được gọi khi chạy Trình thiết kế báo cáo,
            // trước khi các hộp thoại Mở Báo cáo và Lưu Báo cáo được hiển thị và sau khi một báo cáo mới được lưu vào bộ nhớ. 
            var listReport = _context.SReports.Select(x => x.reportcode).ToList();
            var reportFiles = Directory.GetFiles(ReportDirectory, "*" + FileExtension)
                                     .Select(Path.GetFileNameWithoutExtension)
                                     .Concat(listReport)
                                     .ToDictionary(x => x);
            return reportFiles;
        }

        public override void SetData(XtraReport report, string url)
        {
            // Lưu trữ báo cáo đã chỉ định vào Bộ nhớ Báo cáo bằng cách sử dụng URL được chỉ định.
            // Phương thức này chỉ được gọi sau khi phương thức IsValidUrl và CanSetData được gọi. 
            report.SaveLayoutToXml(Path.Combine(ReportDirectory, url + FileExtension));
        }

        public override string SetNewData(XtraReport report, string defaultUrl)
        {
            // Lưu trữ báo cáo đã chỉ định bằng URL mới.
            // Phương thức IsValidUrl và CanSetData không bao giờ được gọi trước phương thức này.
            // Bạn có thể xác thực và sửa URL được chỉ định trực tiếp trong triển khai phương thức SetNewData
            // và trả về URL kết quả được sử dụng để lưu báo cáo trong bộ nhớ của bạn. 
            SetData(report, defaultUrl);
            return defaultUrl;
        }

    }
}
