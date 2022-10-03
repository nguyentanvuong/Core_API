using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using WebApi.Models;
using WebApi.Entities;
using Apache.NMS;
using WebApi.Helpers.Common;
using WebApi.Helpers.Utils;
using WebApi.Helpers.DatabaseUtils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Linq;
using WebApi.Models.User;
using Utility = WebApi.Helpers.Utils.Utility;
using WebApi.Models.Util;
using WebApi.Helpers.Services;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace WebApi.Services
{
    public interface IUserService
    {
        TransactionReponse Authenticate(LoginRequest model);
        public MultilTransactionReponse MultiAuthenticate(MultiLoginRequest model);
        JObject Checkpool();
        public IEnumerable<Users> GetAll();
        public Users GetUsersBySession(string session);
        public SUsrac GetUserWebByUsername(string username);
        public string generateJwtToken(Users user);
        public TransactionReponse LoginUser(UserLoginRequest model);
        public TransactionReponse GetListUserWeb();
        public TransactionReponse GetListPolicy();
        public TransactionReponse GetListUserRole();
        public TransactionReponse ChangePasswordUser(ChangePasswordRequest model);
        public TransactionReponse AddUser(AddUserRequest model);
        public TransactionReponse ResetPassword(ResetPasswordRequest model);
        public TransactionReponse ModifyUser(JObject model);
        public TransactionReponse ViewUser(JObject model);
        public TransactionReponse DeleteUser(JObject model);
        public TransactionReponse ViewPolicy(JObject model);
        public TransactionReponse DeletePolicy(JObject model);
        public TransactionReponse InsertPolicy(JObject model);
        public TransactionReponse UpdatePolicy(JObject model);
        public TransactionReponse ViewUserRole(JObject model);
        public TransactionReponse AddUserRole(JObject model);
        public TransactionReponse ModifyUserRole(JObject model);
        public TransactionReponse DeleteUserRole(JObject model);
        public SUserpublic GetUserPublicByUsername(string username);

    }

    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly AppSettings _appSettings;
        private readonly DbUtils dbUtils;
        public readonly ILogger<UserService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(DataContext context, IOptions<AppSettings> appSettings, ILogger<UserService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _appSettings = appSettings.Value;
            dbUtils = new DbUtils(_context);
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public TransactionReponse Authenticate(LoginRequest model)
        {
            JsonLoginResponse clsJsonLoginResponse;
            TransactionReponse authenticate = null;

            JsonLoginRequest loginRequest = new JsonLoginRequest();
            loginRequest.L = model.Username;
            loginRequest.P = model.encrypt ? model.Password : O9Encrypt.MD5Encrypt(model.Password);
            loginRequest.A = false;

            string strResult = O9Utils.GenJsonBodyRequest(loginRequest, GlobalVariable.UMG_LOGIN, "", EnmCacheAction.NoCached, EnmSendTypeOption.Synchronize, "-1", MsgPriority.Normal);

            clsJsonLoginResponse = JsonConvert.DeserializeObject<JsonLoginResponse>(strResult);
            if (clsJsonLoginResponse != null)
            {
                authenticate = new TransactionReponse();
                if (string.IsNullOrEmpty(clsJsonLoginResponse.E))
                {
                    Users users = new Users(clsJsonLoginResponse.USRID, clsJsonLoginResponse.BRANCHID, clsJsonLoginResponse.BRANCHCD, model.Username, loginRequest.P, clsJsonLoginResponse.UUID, clsJsonLoginResponse.LANG, clsJsonLoginResponse.BUSDATE);
                    dbUtils.AddUser(users);

                    O9Utils.UpdateWorkingDate(clsJsonLoginResponse.BUSDATE);

                    JObject loginValue = new JObject();
                    loginValue.Add(GlobalVariable.LoginName, users.usrname);
                    loginValue.Add(GlobalVariable.WorkingDate, clsJsonLoginResponse.BUSDATE);
                    loginValue.Add(GlobalVariable.Session, users.ssesionid);
                    loginValue.Add(GlobalVariable.BranchCode, users.usrbranch);

                    if (_appSettings.UsingJWT)
                    {
                        string token = generateJwtToken(users);
                        loginValue.Add(GlobalVariable.Token, token);
                    }

                    authenticate.SetCode(Codetypes.Code_Success_Login);
                    authenticate.SetResult(loginValue);
                }
                else
                {
                    authenticate = O9Utils.AnalysisLoginResult(clsJsonLoginResponse.E, clsJsonLoginResponse.WSNAME);
                }
            }
            return authenticate;
        }

        public MultilTransactionReponse MultiAuthenticate(MultiLoginRequest model)
        {
            JsonLoginResponse clsJsonLoginResponse;
            MultilTransactionReponse multil = new MultilTransactionReponse();
            multil.result = new List<TransactionReponse>();
            int count = model.User.Count;
            for (int i = 0; i < count; i++)
            {
                TransactionReponse authenticate = new TransactionReponse();
                JsonLoginRequest loginRequest = new JsonLoginRequest();
                loginRequest.L = model.User[i].Username;
                loginRequest.P = loginRequest.P = model.User[i].encrypt ? model.User[i].Password : O9Encrypt.MD5Encrypt(model.User[i].Password);
                loginRequest.A = false;

                string strResult = O9Utils.GenJsonBodyRequest(loginRequest, GlobalVariable.UMG_LOGIN, "", EnmCacheAction.NoCached, EnmSendTypeOption.Synchronize, "-1", MsgPriority.Normal);
                clsJsonLoginResponse = JsonConvert.DeserializeObject<JsonLoginResponse>(strResult);
                if (clsJsonLoginResponse != null)
                {
                    if (string.IsNullOrEmpty(clsJsonLoginResponse.E))
                    {
                        Users users = new Users(clsJsonLoginResponse.USRID, clsJsonLoginResponse.BRANCHID, clsJsonLoginResponse.BRANCHCD, model.User[i].Username, loginRequest.P, clsJsonLoginResponse.UUID, clsJsonLoginResponse.LANG, clsJsonLoginResponse.BUSDATE);
                        dbUtils.AddUser(users);

                        O9Utils.UpdateWorkingDate(clsJsonLoginResponse.BUSDATE);

                        JObject loginValue = new JObject();
                        loginValue.Add(GlobalVariable.LoginName, model.User[i].Username);
                        loginValue.Add(GlobalVariable.WorkingDate, clsJsonLoginResponse.BUSDATE);
                        loginValue.Add(GlobalVariable.Session, users.ssesionid);
                        loginValue.Add(GlobalVariable.BranchCode, users.usrbranch);
                        if (_appSettings.UsingJWT)
                        {
                            string token = generateJwtToken(users);
                            loginValue.Add(GlobalVariable.Token, token);
                        }
                        authenticate.SetCode(Codetypes.Code_Success_Login);
                        authenticate.SetResult(loginValue);
                    }
                    else
                    {
                        authenticate = O9Utils.AnalysisLoginResult(clsJsonLoginResponse.E, clsJsonLoginResponse.WSNAME);
                        JObject loginValue = new JObject();
                        loginValue.Add(GlobalVariable.LoginName, model.User[i].Username);
                        authenticate.SetResult(loginValue);
                    }
                }
                else
                {
                    authenticate = new TransactionReponse();
                    JObject loginValue = new JObject();
                    loginValue.Add(GlobalVariable.LoginName, model.User[i].Username);
                    authenticate.SetResult(loginValue);
                    authenticate.SetCode(Codetypes.Err_Authenticate);
                }
                multil.result.Add(authenticate);
            }
            return multil;
        }

        public JObject Checkpool()
        {
            JObject poolInfor = new JObject();
            poolInfor.Add("Available", O9Client.activeMQ.AvailableCount());
            poolInfor.Add("Using", O9Client.activeMQ.InUsingCount());
            poolInfor.Add("total", O9Client.activeMQ.TotalCount());
            return poolInfor;
        }

        public IEnumerable<Users> GetAll()
        {
            return _context.Users;
        }

        public Users GetUsersBySession(string session)
        {
            return dbUtils.GetUsersBySession(session);
        }

        public string generateJwtToken(Users user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(GlobalVariable.Identity, user.ssesionid),
                    new Claim(GlobalVariable.UserType, GlobalVariable.FromCore)
                }),
                Expires = DateTime.UtcNow.AddDays(_appSettings.ExpiryDateJWT),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public TransactionReponse ChangePasswordUser(ChangePasswordRequest model)
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                if (model.oldpass == model.newpass) return new TransactionReponse(Codetypes.Err_ChangePassword);
                string oldPassword = model.encrypt ? model.oldpass : O9Encrypt.MD5Encrypt(model.oldpass);
                string newPassword = model.encrypt ? model.newpass : O9Encrypt.MD5Encrypt(model.newpass);
                SUsrac user = _context.SUsracs.SingleOrDefault(x => x.username == model.username);
                if (user == null || user.password != oldPassword)
                {
                    reponse = new TransactionReponse();
                    reponse.SetCode(Codetypes.Err_IncorrectPassword);
                    return reponse;
                }
                else if (user.password == oldPassword)
                {
                    //update pass
                    user.password = newPassword;
                    _context.SaveChanges();

                    reponse = new TransactionReponse();
                    reponse.SetCode(Codetypes.Code_Success);
                    JObject jObject = new JObject
                    {
                        { "ischange", 1 }
                    };
                    reponse.SetResult(jObject);
                }
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                reponse.SetCode(new CodeDescription(9997, ex.Message));
                return reponse;
            }
        }

        public TransactionReponse GetListUserWeb()
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                List<SUsrac> usracs = _context.SUsracs.ToList();
                List<GetUserReponse> userList = new List<GetUserReponse>();
                foreach (SUsrac user in usracs)
                {
                    GetUserReponse userReponse = new GetUserReponse();
                    userReponse.address = user.address;
                    userReponse.birthday = user.birthday == null ? "" : Utility.ConvertDateTimeToString((DateTime)user.birthday);
                    userReponse.branchid = user.branchid;
                    userReponse.corelgname = user.corelgname;
                    userReponse.coreuserid = user.coreuserid;
                    userReponse.datecreated = user.datecreated == null ? "" : Utility.ConvertDateTimeToString((DateTime)user.datecreated);
                    userReponse.deptid = user.deptid;
                    userReponse.email = user.email;
                    userReponse.expiretime = user.expiretime == null ? "" : Utility.ConvertDateTimeToStringDatetime((DateTime)user.expiretime);
                    userReponse.failnumber = user.failnumber;
                    userReponse.firstname = user.firstname;
                    userReponse.gender = user.gender;
                    userReponse.islogin = user.islogin;
                    userReponse.isshow = user.isshow;
                    userReponse.lastname = user.lastname;
                    userReponse.phone = user.phone;
                    userReponse.policyid = user.policyid;
                    userReponse.productid = user.productid;
                    userReponse.status = user.status;
                    userReponse.usercreated = user.usercreated;
                    userReponse.userlevel = user.userlevel;
                    userReponse.usermodified = user.usermodified;
                    userReponse.username = user.username;
                    userList.Add(userReponse);
                }
                JArray array = JArray.FromObject(userList);
                JObject users = new JObject
                {
                    { "data", array }
                };
                reponse.SetCode(Codetypes.Code_Success);
                reponse.SetResult(users);
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                reponse.SetCode(new CodeDescription(9997, ex.Message));
                return reponse;
            }

        }

        public TransactionReponse LoginUser(UserLoginRequest model)
        {
            TransactionReponse authenticate = new TransactionReponse();
            try
            {
                UserLoginReponse userLoginReponse = new UserLoginReponse();
                string password = model.encrypt ? model.password : O9Encrypt.MD5Encrypt(model.password);
                SUsrac user = _context.SUsracs.SingleOrDefault(x => x.username == model.username);
                if (user == null || user.password != password)
                {
                    authenticate = new TransactionReponse();
                    authenticate.SetCode(Codetypes.Err_Authenticate);
                    return authenticate;
                }
                else if (user.password == password)
                {
                    authenticate = new TransactionReponse();
                    if (user.status != "N")
                    {
                        authenticate.SetCode(Codetypes.Err_UserStatus);
                        return authenticate;
                    }
                    if (user.expiretime != null)
                    {
                        DateTime expiryDate = (DateTime)user.expiretime;
                        if (expiryDate < DateTime.Now)
                        {
                            authenticate.SetCode(Codetypes.UMG_INVALID_EXPDT);
                            return authenticate;
                        }
                    }
                    JObject loginValue = new JObject();

                    if (_appSettings.UsingJWT)
                    {
                        string token = generateJwtTokenUserWeb(user);
                        userLoginReponse.token = token;
                        userLoginReponse.token_type = "bearer";
                    }

                    authenticate.SetCode(Codetypes.Code_Success_Login);
                    userLoginReponse.branchid = user.branchid;
                    userLoginReponse.deptid = user.deptid;
                    userLoginReponse.expiretime = user.expiretime == null ? "" : Utility.ConvertDateTimeToStringDatetime((DateTime)user.expiretime);
                    userLoginReponse.firstname = user.firstname;
                    userLoginReponse.lastname = user.lastname;
                    userLoginReponse.policyid = user.policyid;
                    userLoginReponse.userlevel = user.userlevel;
                    userLoginReponse.username = user.username;
                    userLoginReponse.status = user.status;
                    userLoginReponse.gender = user.gender;
                    userLoginReponse.address = user.address;
                    userLoginReponse.email = user.email;
                    userLoginReponse.birthday = user.birthday == null ? "" : Utility.ConvertDateTimeToString((DateTime)user.birthday);
                    userLoginReponse.phone = user.phone;
                    userLoginReponse.datecreated = user.datecreated == null ? "" : Utility.ConvertDateTimeToStringDatetime((DateTime)user.datecreated);
                    userLoginReponse.usercreated = user.usercreated;
                    userLoginReponse.coreuserid = user.coreuserid;
                    userLoginReponse.corelgname = user.corelgname;
                    userLoginReponse.productid = user.productid;
                    userLoginReponse.isshow = user.isshow;
                    userLoginReponse.failnumber = user.failnumber;
                    userLoginReponse.cdlist = GlobalVariable.CDLIST;
                    userLoginReponse.searchfunc = GlobalVariable.SearchFunction;
                    userLoginReponse.form = GlobalVariable.SForm;
                    userLoginReponse.fastmode = _context.SParams.SingleOrDefault(x=> x.parcode == "API" && x.pargrp == "FAST" &&x.parname=="MODE")?.parval;
                    var listmenu = _context.VMenuUsers.Where(x => x.username == user.username).Distinct().OrderBy(x => x.menuorder).ToList();
                    var result = dbUtils.FormatMenu(listmenu, "0");
                    userLoginReponse.menu = result;

                    loginValue = (JObject)JToken.FromObject(userLoginReponse);
                    authenticate.SetResult(loginValue);

                    //update last login date
                    user.lastlogintime = DateTime.Now;
                    user.islogin = true;
                    _context.SaveChanges();
                }
                return authenticate;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                authenticate.SetCode(new CodeDescription(9997, ex.Message));
                return authenticate;
            }
        }

        public SUsrac GetUserWebByUsername(string username)
        {
            return dbUtils.GetUserWebByUsername(username);
        }

        public SUserpublic GetUserPublicByUsername(string username)
        {
            return dbUtils.GetUserPublicByUsername(username);
        }

        public string generateJwtTokenUserWeb(SUsrac user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(GlobalVariable.Identity, user.username),
                    new Claim(GlobalVariable.UserType, GlobalVariable.FromWeb)
                }),
                Expires = DateTime.UtcNow.AddDays(_appSettings.ExpiryDateJWT),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public TransactionReponse AddUser(AddUserRequest model)
        {
            TransactionReponse reponse = new TransactionReponse();
            SUsrac userRequest = (SUsrac)_httpContextAccessor.HttpContext.Items["User"];
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>
                {
                    { "USERNAME", model.Username },
                    { "EMAIL", model.Email },
                    { "POLICYID", model.Policyid },
                    { "DEPTID", model.Deptid },
                    { "BRANCHID", model.Branchid }
                };
                string checkadduser = DbFactory.GetVariableFromStoredProcedure(GlobalVariable.SYS_CHECK_SUSER_ADD, param);
                switch (int.Parse(checkadduser))
                {
                    case 1:
                        reponse.SetCode(Codetypes.Err_UserExists);
                        return reponse;
                    case 2:
                        reponse.SetCode(Codetypes.Err_UserEmailExists);
                        return reponse;
                    case 3:
                        reponse.SetCode(Codetypes.Err_PolicyIdInvalid);
                        return reponse;
                    case 4:
                        reponse.SetCode(Codetypes.Err_InvalidDeptId);
                        return reponse;
                    case 5:
                        reponse.SetCode(Codetypes.Err_InvalidBranchId);
                        return reponse;
                    default:
                        break;
                }
                string newpass = Utility.GenPassword(8);
                SUsrac newUser = new SUsrac
                {
                    username = model.Username,
                    address = model.Address,
                    birthday = Utility.IsDate(model.Birthday) ? Utility.ConvertStringToDateTime(model.Birthday) : null,
                    branchid = model.Branchid,
                    datecreated = DateTime.Now,
                    password = O9Encrypt.MD5Encrypt(newpass),
                    firstname = model.Firstname,
                    lastname = model.Lastname,
                    gender = model.Gender,
                    email = model.Email,
                    phone = model.Phone,
                    status = GlobalVariable.UserStatus,
                    usercreated = userRequest.username,
                    expiretime = Utility.IsDate(model.Expiretime) ? Utility.ConvertStringToDateTime(model.Expiretime) : null,
                    deptid = model.Deptid,
                    userlevel = model.Userlevel,
                    productid = model.Productid,
                    policyid = model.Policyid
                };

                IpcEmailtemplate emailtemplate = dbUtils.GetEmailTemplate(1, "ANY", "FAST_RESETNEWPASS");
                if (emailtemplate != null)
                {
                    string htmlContent = string.Format(emailtemplate.content, model.Username, newpass);
                    EmailService.SendMail(model.Email, "", emailtemplate.title, htmlContent, "", "", "", ref reponse);
                }
                _context.SUsracs.Add(newUser);
                SUserroledetail userroledetail = new SUserroledetail
                {
                    roleid = 1,
                    usrname = newUser.username,
                    description = "Default role"
                };
                _context.SUserroledetails.Add(userroledetail);
                int result = _context.SaveChanges();
                if (result == 2)
                {
                    reponse.SetCode(Codetypes.Code_Success);
                    JObject obj = Utility.FromatJObject(newUser, new string[] { "password" });
                    reponse.SetResult(obj);
                    return reponse;
                }
                else
                {
                    reponse.SetCode(new CodeDescription(9997, "Add user fail"));
                    return reponse;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                reponse.SetCode(new CodeDescription(9997, ex.Message));
                return reponse;
            }
        }

        public TransactionReponse ResetPassword(ResetPasswordRequest model)
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                SUsrac usrac = _context.SUsracs.FirstOrDefault(x => x.username == model.username && x.email == model.email);
                if (usrac == null)
                {
                    reponse.SetCode(Codetypes.Err_UserNotExists);
                    return reponse;
                }
                if (string.IsNullOrEmpty(model.code))
                {
                    GlobalVariable.AuthenCodeList.RemoveAll(x => x.Username == model.username);
                    string code = Utility.GenAuthenticataCode(6);
                    AuthenCodeInfo authenCodeInfo = new AuthenCodeInfo
                    {
                        Code = code,
                        Email = model.email,
                        Username = model.username,
                        Expirytime = DateTime.Now.AddSeconds(120)
                    };
                    GlobalVariable.AuthenCodeList.Add(authenCodeInfo);
                    IpcEmailtemplate emailtemplate = dbUtils.GetEmailTemplate(1, "ANY", "FAST_RESETPASS");
                    if (emailtemplate != null)
                    {
                        string[] codes = code.ToCharArray().Select(c => c.ToString()).ToArray();
                        string htmlContent = string.Format(emailtemplate.content, new string[] { model.username, codes[0], codes[1], codes[2], codes[3], codes[4], codes[5] });
                        if (EmailService.SendMail(model.email, "", emailtemplate.title, htmlContent, "", "", "", ref reponse))
                        {
                            reponse.SetCode(Codetypes.Code_Success);
                            return reponse;
                        }
                        else
                        {
                            return reponse;
                        }
                    }
                    reponse.SetCode(Codetypes.Code_Success);
                    return reponse;
                }
                else
                {
                    AuthenCodeInfo authenCodeInfo = GlobalVariable.AuthenCodeList.SingleOrDefault(x => x.Username == model.username && x.Email == model.email && x.Code == model.code);
                    if (authenCodeInfo == null)
                    {
                        reponse.SetCode(Codetypes.Err_IncorrectAuthenCode);
                        return reponse;
                    }
                    if (authenCodeInfo.Expirytime < DateTime.Now)
                    {
                        GlobalVariable.AuthenCodeList.Remove(authenCodeInfo);
                        reponse.SetCode(Codetypes.Err_AuthenCodeExpried);
                        return reponse;
                    }
                    else
                    {
                        GlobalVariable.AuthenCodeList.Remove(authenCodeInfo);
                        string newpass = Utility.GenPassword(8);
                        usrac.password = O9Encrypt.MD5Encrypt(newpass);
                        IpcEmailtemplate emailtemplate = dbUtils.GetEmailTemplate(1, "ANY", "FAST_RESETNEWPASS");
                        if (emailtemplate != null)
                        {
                            string htmlContent = string.Format(emailtemplate.content, model.username, newpass);
                            EmailService.SendMail(model.email, "", emailtemplate.title, htmlContent, "", "", "", ref reponse);
                        }
                        _context.SaveChanges();
                        reponse.SetCode(Codetypes.Code_Success);
                        return reponse;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                reponse.SetCode(new CodeDescription(9997, ex.Message));
                return reponse;
            }
        }

        public TransactionReponse ModifyUser(JObject model)
        {
            TransactionReponse reponse = new TransactionReponse();
            SUsrac userRequest = (SUsrac)_httpContextAccessor.HttpContext.Items["User"];
            try
            {
                string username = Utility.GetValueJObjectByKey(model, "username");
                if (string.IsNullOrEmpty(username))
                {
                    reponse.SetCode(Codetypes.Err_RequireUsername);
                    return reponse;
                }
                string email = Utility.GetValueJObjectByKey(model, "email");
                string deptid = Utility.GetValueJObjectByKey(model, "deptid");
                string branchid = Utility.GetValueJObjectByKey(model, "branchid");
                string policyid = Utility.GetValueJObjectByKey(model, "policyid");
                Dictionary<string, object> param = new Dictionary<string, object>
                {
                    { "USERNAME", username },
                    { "EMAIL", email },
                    { "POLICYID", policyid },
                    { "DEPTID", deptid },
                    { "BRANCHID", branchid }
                };
                string checkmodifyuser = DbFactory.GetVariableFromStoredProcedure(GlobalVariable.SYS_CHECK_SUSER_MODIFY, param);
                switch (int.Parse(checkmodifyuser))
                {
                    case 1:
                        reponse.SetCode(Codetypes.Err_UserNotExists);
                        return reponse;
                    case 2:
                        reponse.SetCode(Codetypes.Err_UserEmailExists);
                        return reponse;
                    case 3:
                        reponse.SetCode(Codetypes.Err_PolicyIdInvalid);
                        return reponse;
                    case 4:
                        reponse.SetCode(Codetypes.Err_InvalidDeptId);
                        return reponse;
                    case 5:
                        reponse.SetCode(Codetypes.Err_InvalidBranchId);
                        return reponse;
                    default:
                        break;
                }
                SUsrac usrac = _context.SUsracs.SingleOrDefault(x => x.username == username);
                if (usrac == null)
                {
                    reponse.SetCode(Codetypes.Err_UserNotExists);
                    return reponse;
                }
                usrac = (SUsrac)Utility.SetValueObject(model, usrac);
                if (usrac == null)
                {
                    reponse.SetCode(Codetypes.Err_InputFormat);
                    return reponse;
                }
                usrac.datemodified = DateTime.Now;
                usrac.usermodified = userRequest.username;
                _context.SaveChanges();
                JObject obj = Utility.FromatJObject(usrac, new string[] { "password" });
                reponse.SetCode(Codetypes.Code_Success);
                reponse.SetResult(obj);
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                reponse.SetCode(new CodeDescription(9997, ex.Message));
                return reponse;
            }
        }

        public TransactionReponse ViewUser(JObject model)
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                string username = Utility.GetValueJObjectByKey(model, "username");
                if (string.IsNullOrEmpty(username))
                {
                    reponse.SetCode(Codetypes.Err_RequireUsername);
                    return reponse;
                }
                SUsrac user = _context.SUsracs.SingleOrDefault(x => x.username == username);
                if (user == null)
                {
                    reponse.SetCode(Codetypes.Err_DataNotFound);
                    return reponse;
                }
                JObject obj = Utility.FromatJObject(user, new string[] { "password" });
                if (obj == null)
                {
                    return null;
                }
                reponse.SetCode(Codetypes.Code_Success);
                reponse.SetResult(obj);
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                reponse.SetCode(new CodeDescription(9997, ex.Message));
                return reponse;
            }
        }

        public TransactionReponse DeleteUser(JObject model)
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                string username = Utility.GetValueJObjectByKey(model, "username");
                if (string.IsNullOrEmpty(username))
                {
                    reponse.SetCode(Codetypes.Err_RequireUsername);
                    return reponse;
                }
                SUsrac user = _context.SUsracs.SingleOrDefault(x => x.username == username);
                if (user == null)
                {
                    reponse.SetCode(Codetypes.Err_DataNotFound);
                    return reponse;
                }
                _context.SUsracs.Remove(user);
                _context.SaveChanges();
                reponse.SetCode(Codetypes.Code_Success);
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                reponse.SetCode(new CodeDescription(9997, ex.Message));
                return reponse;
            }
        }

        public TransactionReponse GetListPolicy()
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                List<SUserpolicy> userpolicies = _context.SUserpolicies.ToList();
                List<GetPolicyResponse> policyList = new List<GetPolicyResponse>();
                foreach (SUserpolicy userpolicy in userpolicies)
                {
                    GetPolicyResponse policyReponse = new GetPolicyResponse();

                    policyReponse.baseonpolicy = userpolicy.baseonpolicy;
                    policyReponse.contractid = userpolicy.contractid;
                    policyReponse.datecreate = userpolicy.datecreate == null ? "" : Utility.ConvertDateTimeToStringDatetime((DateTime)userpolicy.datecreate);
                    policyReponse.datemodify = userpolicy.datemodify == null ? "" : Utility.ConvertDateTimeToStringDatetime((DateTime)userpolicy.datemodify);
                    policyReponse.descr = userpolicy.descr;
                    policyReponse.effrom = Utility.ConvertDateTimeToStringDatetime((DateTime)userpolicy.effrom);
                    policyReponse.efto = userpolicy.efto == null ? "" : Utility.ConvertDateTimeToStringDatetime((DateTime)userpolicy.efto);
                    policyReponse.isbankedit = userpolicy.isbankedit;
                    policyReponse.iscorpedit = userpolicy.iscorpedit;
                    policyReponse.isdefault = userpolicy.isdefault;
                    policyReponse.lginfr = userpolicy.lginfr;
                    policyReponse.lginto = userpolicy.lginto;
                    policyReponse.llkoutthrs = userpolicy.llkoutthrs;
                    policyReponse.minpwdlen = userpolicy.minpwdlen;
                    policyReponse.policyid = userpolicy.policyid;
                    policyReponse.pwccplxsn = userpolicy.pwccplxsn;
                    policyReponse.pwdagemax = userpolicy.pwdagemax;
                    policyReponse.pwdcplx = userpolicy.pwdcplx;
                    policyReponse.pwdcplxlc = userpolicy.pwdcplxlc;
                    policyReponse.pwdcplxsc = userpolicy.pwdcplxsc;
                    policyReponse.pwdcplxuc = userpolicy.pwdcplxuc;
                    policyReponse.pwdft = userpolicy.pwdft;
                    policyReponse.pwdhis = userpolicy.pwdhis;
                    policyReponse.resetlkout = userpolicy.resetlkout;
                    policyReponse.timelginrequire = userpolicy.timelginrequire;
                    policyReponse.usercreate = userpolicy.usercreate;
                    policyReponse.usermodify = userpolicy.usermodify;

                    policyList.Add(policyReponse);
                }
                JArray array = JArray.FromObject(policyList);
                JObject policies = new JObject
                {
                    { "data", array }
                };
                reponse.SetCode(Codetypes.Code_Success);
                reponse.SetResult(policies);
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                reponse.SetCode(new CodeDescription(9997, ex.Message));
                return reponse;
            }

        }

        public TransactionReponse ViewPolicy(JObject model)
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                string policyid = Utility.GetValueJObjectByKey(model, "policyid");
                if (string.IsNullOrEmpty(policyid))
                {
                    reponse.SetCode(Codetypes.Err_RequirePolicyID);
                    return reponse;
                }
                SUserpolicy userpolicy = _context.SUserpolicies.SingleOrDefault(x => x.policyid == int.Parse(policyid));
                if (userpolicy == null)
                {
                    reponse.SetCode(Codetypes.Err_DataNotFound);
                    return reponse;
                }
                JObject obj = Utility.FromatJObject(userpolicy);
                if (obj == null)
                {
                    return null;
                }
                reponse.SetCode(Codetypes.Code_Success);
                reponse.SetResult(obj);
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                reponse.SetCode(new CodeDescription(9997, ex.Message));
                return reponse;
            }
        }

        public TransactionReponse DeletePolicy(JObject model)
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                string policyid = Utility.GetValueJObjectByKey(model, "policyid");
                if (string.IsNullOrEmpty(policyid))
                {
                    reponse.SetCode(Codetypes.Err_RequirePolicyID);
                    return reponse;
                }
                SUserpolicy userpolicy = _context.SUserpolicies.SingleOrDefault(x => x.policyid == int.Parse(policyid));
                if (userpolicy == null)
                {
                    reponse.SetCode(Codetypes.Err_DataNotFound);
                    return reponse;
                }
                if ((bool)userpolicy.isdefault)
                {
                    reponse.SetCode(Codetypes.Err_CanNotDeletePolicy);
                    return reponse;
                }
                _context.SUserpolicies.Remove(userpolicy);
                _context.SaveChanges();
                reponse.SetCode(Codetypes.Code_Success);
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                reponse.SetCode(new CodeDescription(9997, ex.Message));
                return reponse;
            }
        }

        public TransactionReponse InsertPolicy(JObject model)
        {
            TransactionReponse reponse = new TransactionReponse();
            SUsrac userRequest = (SUsrac)_httpContextAccessor.HttpContext.Items["User"];
            try
            {
                string policyid = Utility.GetValueJObjectByKey(model, "policyid");
                if (string.IsNullOrEmpty(policyid))
                {
                    reponse.SetCode(Codetypes.Err_RequirePolicyID);
                    return reponse;
                }
                SUserpolicy userpolicy = _context.SUserpolicies.SingleOrDefault(x => x.policyid == int.Parse(policyid));
                if (userpolicy != null)
                {
                    reponse.SetCode(Codetypes.Err_PolicyExist);
                    return reponse;
                }
                SUserpolicy newpolicy = new SUserpolicy();
                newpolicy = (SUserpolicy)Utility.SetValueObject(model, newpolicy);
                if (newpolicy == null) return null;
                if ((bool)newpolicy.isdefault)
                {
                    var updatedefault = _context.SUserpolicies.Where(x => x.isdefault == true).ToList();
                    updatedefault.ForEach(x => x.isdefault = false);
                }
                newpolicy.datecreate = DateTime.Now;
                newpolicy.usercreate = userRequest.username;
                _context.SUserpolicies.Add(newpolicy);
                _context.SaveChanges();
                JObject obj = Utility.FromatJObject(newpolicy);
                reponse.SetCode(Codetypes.Code_Success);
                reponse.SetResult(obj);
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                reponse.SetCode(new CodeDescription(9997, ex.Message));
                return reponse;
            }
        }

        public TransactionReponse UpdatePolicy(JObject model)
        {
            TransactionReponse reponse = new TransactionReponse();
            SUsrac userRequest = (SUsrac)_httpContextAccessor.HttpContext.Items["User"];
            try
            {
                string policyid = Utility.GetValueJObjectByKey(model, "policyid");
                if (string.IsNullOrEmpty(policyid))
                {
                    reponse.SetCode(Codetypes.Err_RequirePolicyID);
                    return reponse;
                }
                SUserpolicy userpolicy = _context.SUserpolicies.SingleOrDefault(x => x.policyid == int.Parse(policyid));
                if (userpolicy == null)
                {
                    reponse.SetCode(Codetypes.Err_DataNotFound);
                    return reponse;
                }
                userpolicy = (SUserpolicy)Utility.SetValueObject(model, userpolicy);
                if (userpolicy == null) return null;
                if ((bool)userpolicy.isdefault)
                {
                    var updatedefault = _context.SUserpolicies.Where(x => x.isdefault == true && x.policyid != int.Parse(policyid)).ToList();
                    updatedefault.ForEach(x => x.isdefault = false);
                }
                userpolicy.datemodify = DateTime.Now;
                userpolicy.usermodify = userRequest.username;
                _context.SaveChanges();
                JObject obj = Utility.FromatJObject(userpolicy);
                reponse.SetCode(Codetypes.Code_Success);
                reponse.SetResult(obj);
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                reponse.SetCode(new CodeDescription(9997, ex.Message));
                return reponse;
            }
        }

        public TransactionReponse GetListUserRole()
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                List<SUserrole> userRoles = _context.SUserroles.ToList();
                List<GetUserRoleResponse> userRoleList = new List<GetUserRoleResponse>();
                foreach (SUserrole userrole in userRoles)
                {
                    GetUserRoleResponse userRoleReponse = new GetUserRoleResponse
                    {
                        contractno = userrole.contractno,
                        datecreated = userrole.datecreated == null ? "" : Utility.ConvertDateTimeToStringDatetime((DateTime)userrole.datecreated),
                        datemodified = userrole.datemodified == null ? "" : Utility.ConvertDateTimeToStringDatetime((DateTime)userrole.datemodified),
                        isshow = userrole.isshow,
                        roledescription = userrole.roledescription,
                        roleid = userrole.roleid,
                        rolename = userrole.rolename,
                        serviceid = userrole.serviceid,
                        status = userrole.status,
                        usercreated = userrole.usercreated,
                        usermodified = userrole.usermodified,
                        usertype = userrole.usertype
                    };

                    userRoleList.Add(userRoleReponse);
                }
                JArray array = JArray.FromObject(userRoleList);
                JObject userroles = new JObject
                {
                    { "data", array }
                };
                reponse.SetCode(Codetypes.Code_Success);
                reponse.SetResult(userroles);
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                reponse.SetCode(new CodeDescription(9997, ex.Message));
                return reponse;
            }

        }

        public TransactionReponse ViewUserRole(JObject model)
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                string roleid = Utility.GetValueJObjectByKey(model, "roleid");
                if (string.IsNullOrEmpty(roleid))
                {
                    reponse.SetCode(Codetypes.Err_RequireRoleID);
                    return reponse;
                }
                SUserrole userrole = _context.SUserroles.SingleOrDefault(x => x.roleid == int.Parse(roleid));
                if (userrole == null)
                {
                    reponse.SetCode(Codetypes.Err_DataNotFound);
                    return reponse;
                }
                JObject obj = Utility.FromatJObject(userrole);
                if (obj == null)
                {
                    return null;
                }
                reponse.SetCode(Codetypes.Code_Success);
                reponse.SetResult(obj);
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                reponse.SetCode(new CodeDescription(9997, ex.Message));
                return reponse;
            }
        }

        public TransactionReponse AddUserRole(JObject model)
        {
            TransactionReponse reponse = new TransactionReponse();
            SUsrac userRequest = (SUsrac)_httpContextAccessor.HttpContext.Items["User"];
            try
            {
                SUserrole newrole = new SUserrole();
                newrole = (SUserrole)Utility.SetValueObject(model, newrole);
                if (newrole == null) return null;
                newrole.datecreated = DateTime.Now;
                newrole.usercreated = userRequest.username;
                _context.SUserroles.Add(newrole);
                _context.SaveChanges();
                JObject obj = Utility.FromatJObject(newrole);
                reponse.SetCode(Codetypes.Code_Success);
                reponse.SetResult(obj);
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                reponse.SetCode(new CodeDescription(9997, ex.Message));
                return reponse;
            }
        }

        public TransactionReponse ModifyUserRole(JObject model)
        {
            TransactionReponse reponse = new TransactionReponse();
            SUsrac userRequest = (SUsrac)_httpContextAccessor.HttpContext.Items["User"];
            try
            {
                string roleid = Utility.GetValueJObjectByKey(model, "roleid");
                if (string.IsNullOrEmpty(roleid))
                {
                    reponse.SetCode(Codetypes.Err_RequireRoleID);
                    return reponse;
                }
                SUserrole userrole = _context.SUserroles.SingleOrDefault(x => x.roleid == int.Parse(roleid));
                if (userrole == null)
                {
                    reponse.SetCode(Codetypes.Err_DataNotFound);
                    return reponse;
                }
                userrole = (SUserrole)Utility.SetValueObject(model, userrole);
                if (userrole == null) return null;
                userrole.datemodified = DateTime.Now;
                userrole.usermodified = userRequest.username;
                _context.SaveChanges();
                JObject obj = Utility.FromatJObject(userrole);
                reponse.SetCode(Codetypes.Code_Success);
                reponse.SetResult(obj);
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                reponse.SetCode(new CodeDescription(9997, ex.Message));
                return reponse;
            }
        }

        public TransactionReponse DeleteUserRole(JObject model)
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                string roleid = Utility.GetValueJObjectByKey(model, "roleid");
                if (string.IsNullOrEmpty(roleid))
                {
                    reponse.SetCode(Codetypes.Err_RequireRoleID);
                    return reponse;
                }
                SUserrole userrole = _context.SUserroles.SingleOrDefault(x => x.roleid == int.Parse(roleid));
                if (userrole == null)
                {
                    reponse.SetCode(Codetypes.Err_DataNotFound);
                    return reponse;
                }
                _context.SUserroles.Remove(userrole);
                _context.SaveChanges();
                reponse.SetCode(Codetypes.Code_Success);
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                reponse.SetCode(new CodeDescription(9997, ex.Message));
                return reponse;
            }
        }

    }
}