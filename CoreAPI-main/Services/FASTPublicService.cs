using Apache.NMS;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using WebApi.Entities;
using WebApi.Helpers.Common;
using WebApi.Helpers.DatabaseUtils;
using WebApi.Helpers.Utils;
using WebApi.Models;
using WebApi.Models.FASTPublic;

namespace WebApi.Services
{
    public interface IFASTPublicService
    {
        public FASTReponse FASTLogin(PublicLoginRequest request);
        public FASTReponse AccountInquiry(AccountInquiryRequest request);
    }
    public class FASTPublicService : IFASTPublicService
    {
        private readonly DataContext _context;
        private readonly DbUtils dbUtils;
        private readonly ILogger<FASTPublicService> _logger;
        private readonly IApiService _apiService;
        private readonly AppSettings _appSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FASTPublicService(DataContext context, ILogger<FASTPublicService> logger, IApiService apiService, IOptions<AppSettings> appSettings, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _context = context;
            dbUtils = new DbUtils(_context);
            _apiService = apiService;
            _appSettings = appSettings.Value;
            _httpContextAccessor = httpContextAccessor;
        }
        public string generatePublicJwtToken(SUserpublic user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(GlobalVariable.Identity, user.username),
                    new Claim(GlobalVariable.UserType, GlobalVariable.UserPublic)
                }),
                Expires = DateTime.UtcNow.AddDays(_appSettings.ExpiryDateJWT),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public FASTReponse FASTLogin(PublicLoginRequest request)
        {
            FASTReponse reponse = new FASTReponse();
            try
            {
                string username = request.username;
                string password = O9Encrypt.MD5Encrypt(request.password);
                SUserpublic fastUser = _context.SUserpublics.SingleOrDefault(x => x.username == username && x.password == password);
                if (fastUser == null)
                {
                    reponse.SetError(4, 11, "Authentication error. Please try again.");
                    return reponse;
                }
                if (!string.IsNullOrEmpty(fastUser.usernamecore) && !string.IsNullOrEmpty(fastUser.passwordcore))
                {
                    JsonLoginResponse clsJsonLoginResponse;
                    JsonLoginRequest loginRequest = new JsonLoginRequest
                    {
                        L = fastUser.usernamecore,
                        P = fastUser.passwordcore,
                        A = false
                    };
                    string strResult = O9Utils.GenJsonBodyRequest(loginRequest, GlobalVariable.UMG_LOGIN, "", EnmCacheAction.NoCached, EnmSendTypeOption.Synchronize, "-1", MsgPriority.Normal);
                    clsJsonLoginResponse = JsonConvert.DeserializeObject<JsonLoginResponse>(strResult);
                    if (clsJsonLoginResponse != null)
                    {
                        if (string.IsNullOrEmpty(clsJsonLoginResponse.E))
                        {
                            Users users = new Users(clsJsonLoginResponse.USRID, clsJsonLoginResponse.BRANCHID, clsJsonLoginResponse.BRANCHCD, fastUser.usernamecore, loginRequest.P, clsJsonLoginResponse.UUID, clsJsonLoginResponse.LANG, clsJsonLoginResponse.BUSDATE);
                            dbUtils.AddUser(users);
                        }
                        else
                        {
                            TransactionReponse reponseLoginCore = O9Utils.AnalysisLoginResult(clsJsonLoginResponse.E, clsJsonLoginResponse.WSNAME);
                            reponse.SetError(4, 11, reponseLoginCore.messagedetail);
                            return reponse;
                        }
                    }
                    else
                    {
                        reponse.SetError(1, 41, "CBS is not available");
                        return reponse;
                    }
                }
                string token = generatePublicJwtToken(fastUser);
                JObject data = new JObject { { "token", token } };
                reponse.SetData(data);
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                reponse.SetError(1, 131, "Internal server error");
                return reponse;
            }

        }

        public FASTReponse AccountInquiry(AccountInquiryRequest request)
        {
            FASTReponse reponse = new FASTReponse();
            try
            {
                SUserpublic userRequest = (SUserpublic)_httpContextAccessor.HttpContext.Items["User"];
                Users userCore = _context.Users.SingleOrDefault(x => x.usrname == userRequest.usernamecore);
                FUNC_FAST_ACNO transactionRequest = new FUNC_FAST_ACNO
                {
                    ACCOUNT = request.account_no,
                    SESSIONID = userCore.ssesionid
                };
                var responseCore = _apiService.CallFunction(transactionRequest, GlobalVariable.FUNC_FAST_ACNO);              
                if(responseCore.errorcode == 0)
                {
                    JObject jObjectResultCore = JObject.FromObject(responseCore.result);
                    string ERRORCODE = jObjectResultCore.SelectToken("ERRORCODE").ToString();
                    if (ERRORCODE == "0")
                    {
                        JObject jObjectAccount = (JObject)jObjectResultCore.SelectToken("RESULT");
                        jObjectAccount.Add("bank_code", GlobalVariable.Sender_ParticipantCode);
                        JObject jObjectReponse = new JObject
                        {
                            { "result", "success" },
                            { "product_code", "FAST" },
                            { "account_data", jObjectAccount }
                        };
                        reponse.SetData(jObjectReponse);
                        return reponse;
                    }
                    else
                    {
                        reponse.SetError(1, 65, "The account Number is not found. Please check the entered data.");
                        return reponse;
                    }
                }
                else
                {
                    reponse.SetError(1, 41, "CBS is not available");
                    return reponse;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                reponse.SetError(1, 131, "Internal server error");
                return reponse;
            }
        }
    }
}
