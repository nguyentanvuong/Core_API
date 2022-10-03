using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Entities;
using WebApi.Helpers.Common;
using WebApi.Helpers.DatabaseUtils;
using WebApi.Helpers.Utils;
using WebApi.Models;
using WebApi.Models.System;

namespace WebApi.Services
{
    public interface ISystemService
    {
        public TransactionReponse GetListBank();
        public TransactionReponse GetListParameter();
        public TransactionReponse GetListSystemCode();
        public TransactionReponse GetListSearchFuntion();
        public TransactionReponse GetListForm();       
        public TransactionReponse GetListMenu(string username);
        public TransactionReponse AddSystemBank(JObject jObject);
        public TransactionReponse ModifySystemBank(JObject jObject);
        public TransactionReponse ViewBank(JObject model);
        public TransactionReponse DeleteBank(JObject model);
        public TransactionReponse LookupData(LookupDataRequest model);
        public TransactionReponse MultiGetInfor(GetInforDataRequest model);
        public TransactionReponse GetMenuInvokeByRoleid(JObject model);
        public TransactionReponse UpdateMenuInvoke(ListRoleInvoke model);
        public TransactionReponse AddRoleDetail(AddRoleDetailRequest role);
        public TransactionReponse GetUserListByRoleID(JObject role);
        public TransactionReponse DeleteRoleDetail(AddRoleDetailRequest role);
        public TransactionReponse GetReportForDashboardPage();
        public TransactionReponse GetListRoleDetail();
        public TransactionReponse GetMenuInvoke();
        public TransactionReponse SwitchModeFAST();

        public TransactionReponse AddSystemParameter(AddParam param);
        public TransactionReponse ModifySystemParameter(AddParam model);
        public TransactionReponse ViewParameter(ViewParam param);
        public TransactionReponse DeleteParameter(ViewParam param);

        public TransactionReponse GetListSystemCode1();
        public TransactionReponse ViewSystemCode(ViewSystemCode systemcode);
        public TransactionReponse AddSystemCode(AddSystemCode systemcode);
        public TransactionReponse ModifySystemCode(AddSystemCode systemcode);
        public TransactionReponse DeleteSystemCode(ViewSystemCode systemcode);

    }
    public class SystemService : ISystemService
    {
        private readonly DataContext _context;
        private readonly AppSettings _appSettings;
        private readonly DbUtils dbUtils;
        public readonly ILogger<SystemService> _logger;

        public SystemService(DataContext context, IOptions<AppSettings> appSettings, ILogger<SystemService> logger)
        {
            _context = context;
            _appSettings = appSettings.Value;
            dbUtils = new DbUtils(_context);
            _logger = logger;
        }

        public TransactionReponse GetListSystemCode()
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                List<SSystemcode> systemcodes = _context.SSystemcodes.ToList();
                JObject responseObject = dbUtils.FormatCdlist(systemcodes);
                GlobalVariable.CDLIST = responseObject;
                reponse.SetCode(Codetypes.Code_Success);
                reponse.SetResult(responseObject);
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                reponse.SetCode(new CodeDescription(9997, ex.Message));
                return reponse;
            }
        }

        public TransactionReponse GetListSearchFuntion()
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                List<SSearch> searches = _context.SSearches.ToList();
                JObject responseObject = dbUtils.FormatSearchFunction(searches);
                GlobalVariable.SearchFunction = responseObject;
                reponse.SetCode(Codetypes.Code_Success);
                reponse.SetResult(responseObject);
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                reponse.SetCode(new CodeDescription(9997, ex.Message));
                return reponse;
            }
        }

        public TransactionReponse GetListForm()
        {
            try
            {
                TransactionReponse reponse = new TransactionReponse();
                List<SForm> forms = _context.SForms.OrderBy(x => x.order).ToList();
                JObject responseObject = dbUtils.FormatSForm(forms);
                GlobalVariable.SForm = responseObject;
                reponse.SetCode(Codetypes.Code_Success);
                reponse.SetResult(responseObject);
                return reponse;
            }
            catch
            {
                return null;
            }
        }

        public TransactionReponse GetListMenu(string username)
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                var listmenu = _context.VMenuUsers.Where(x => x.username == username).Distinct().OrderBy(x => x.menuorder).ToList();
                var result = dbUtils.FormatMenu(listmenu, "0");
                reponse.SetCode(Codetypes.Code_Success);
                reponse.SetResult(new JObject { { "data", result } });
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                reponse.SetCode(new CodeDescription(9997, ex.Message));
                return reponse;
            }
        }

        public TransactionReponse GetListBank()
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                string sqlreponse = DbFactory.GetDataTableFromStoredProcedure("SYS_GET_BANK");
                if (sqlreponse == null)
                {
                    reponse.SetCode(Codetypes.Err_Unknown);
                    return reponse;
                }
                JArray array = JArray.Parse(sqlreponse);
                JObject obj = new JObject
                {
                    { "data", array }
                };
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
        
        public TransactionReponse AddSystemBank(JObject jObject)
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                string bankcode = Utility.GetValueJObjectByKey(jObject, "bankcode");
                if (string.IsNullOrEmpty(bankcode))
                {
                    reponse.SetCode(Codetypes.Err_RequireBankCode);
                    return reponse;
                }
                int checkbank = _context.SBanks.Where(x => x.bankcode == bankcode).Count();
                if (checkbank > 0)
                {
                    reponse.SetCode(Codetypes.Err_BankCodeExists);
                    return reponse;
                }
                SBank bank = new SBank();
                bank = (SBank)Utility.SetValueObject(jObject, bank);
                if (bank == null)
                {
                    reponse.SetCode(Codetypes.Err_InputFormat);
                    return reponse;
                }
                _context.SBanks.Add(bank);
                _context.SaveChanges();
                JObject obj = Utility.FromatJObject(bank);
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

        public TransactionReponse ModifySystemBank(JObject jObject)
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                string bankid = Utility.GetValueJObjectByKey(jObject, "bankid");
                if (string.IsNullOrEmpty(bankid))
                {
                    reponse.SetCode(Codetypes.Err_RequireBankID);
                    return reponse;
                }
                SBank bank = _context.SBanks.SingleOrDefault(x => x.bankid == Int32.Parse(bankid));
                if (bank == null)
                {
                    reponse.SetCode(Codetypes.Err_DataNotFound);
                    return reponse;
                }
                bank = (SBank)Utility.SetValueObject(jObject, bank);
                if (bank == null)
                {
                    reponse.SetCode(Codetypes.Err_InputFormat);
                    return reponse;
                }
                _context.SaveChanges();
                JObject obj = Utility.FromatJObject(bank);
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

        public TransactionReponse ViewBank(JObject model)
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                string bankid = Utility.GetValueJObjectByKey(model, "bankid");
                if (string.IsNullOrEmpty(bankid))
                {
                    reponse.SetCode(Codetypes.Err_RequireBankID);
                    return reponse;
                }
                SBank bank = _context.SBanks.SingleOrDefault(x => x.bankid == Int32.Parse(bankid));
                if (bank == null)
                {
                    reponse.SetCode(Codetypes.Err_DataNotFound);
                    return reponse;
                }
                JObject obj = Utility.FromatJObject(bank);
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

        public TransactionReponse DeleteBank(JObject model)
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                string bankid = Utility.GetValueJObjectByKey(model, "bankid");
                if (string.IsNullOrEmpty(bankid))
                {
                    reponse.SetCode(Codetypes.Err_RequireBankID);
                    return reponse;
                }
                SBank bank = _context.SBanks.SingleOrDefault(x => x.bankid == Int32.Parse(bankid));
                if (bank == null)
                {
                    reponse.SetCode(Codetypes.Err_DataNotFound);
                    return reponse;
                }
                _context.SBanks.Remove(bank);
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

        public string GenerateSelectClauseLookupData(List<SLoopkup> paramlist)
        {
            string select = "SELECT";
            foreach (var lookup in paramlist)
            {
                select += " " + lookup.colunmname + " AS " + lookup.key + ",";
            }
            return select[0..^1] + " FROM " + paramlist.First().tablename;
        }

        public TransactionReponse LookupData(LookupDataRequest model)
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                string sql = "";
                var loopkup = _context.SLoopkups.Where(x => x.lookupid == model.lookupid).ToList();
                var query = loopkup.Where(x => x.key == "query").ToList();
                if (query.Count == 1)
                {
                    var listparam = loopkup.Where(x => x.key == "param").ToList();
                    if (listparam.Count > 0)
                    {
                        sql = query.First().query;
                        foreach (var para in listparam)
                        {
                            string strvalue = "";
                            foreach (KeyValuePair<string, JToken> keyValuePair in model.param)
                            {
                                string key = keyValuePair.Key;
                                if (key == para.colunmname)
                                {
                                    string value = keyValuePair.Value.ToString();
                                    strvalue = value;
                                    break;
                                }
                            }
                            string strreplace = "{" + para.colunmname + "}";
                            sql = sql.Replace(strreplace, strvalue);
                        }
                    }
                    else
                    {
                        sql = query.FirstOrDefault().query;
                    }

                }
                else if (query.Count == 0)
                {
                    var tablenamecount = loopkup.GroupBy(x => x.tablename).Select(x => x.FirstOrDefault()).Count();
                    if (tablenamecount > 1)
                    {
                        reponse.SetCode(Codetypes.FormatCodeDescription(Codetypes.Err_LookupConfigInvalid, new string[] { "look up", model.lookupid }));
                        return reponse;
                    }
                    else if (tablenamecount == 1)
                    {
                        var columns = loopkup.Where(x => x.key.ToLower() != "query" && x.key.ToLower() != "param").ToList();
                        if (columns.Count < 2) return null;
                        else
                        {
                            sql = GenerateSelectClauseLookupData(columns);
                        }
                        JObject jquery = new JObject();
                        var listparam = loopkup.Where(x => x.key == "param").ToList();
                        if (listparam.Count > 0 && model.param != null)
                        {
                            int counttable = listparam.GroupBy(x => x.tablename).Select(x => x.FirstOrDefault()).Count();
                            if (counttable > 1) return null;
                            foreach (var para in listparam)
                            {
                                string paramvalue = Utility.GetValueJObjectByKey(model.param, para.colunmname);
                                if (string.IsNullOrEmpty(paramvalue)) return null;
                                jquery.Add(para.colunmname, paramvalue);
                            }
                        }
                        sql += DbUtils.GenerateWhereSQLCondition(jquery);
                    }
                }
                if (string.IsNullOrEmpty(sql)) return null;
                string datastring = DbFactory.GetDataTableFromSQL(sql);
                JArray array = JArray.Parse(datastring);
                reponse.SetCode(Codetypes.Code_Success);
                JObject obj = new JObject
                {
                    { "data", array }
                };
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

        public JObject GetInforData(string getinforid, JObject paramlist)
        {
            string sql = "";
            var getinforlist = _context.SGetinfors.Where(x => x.inforid == getinforid).ToList();
            var query = getinforlist.Where(x => x.key == "query" || x.type == "query").ToList();
            if (query.Count == 1)
            {
                var parameters = getinforlist.Where(x => x.type != "query").ToList();
                sql += query.First().value;
                foreach (SGetinfor getinforparam in parameters)
                {
                    foreach (KeyValuePair<string, JToken> keyValuePair in paramlist)
                    {
                        string key = keyValuePair.Key;
                        if (key == getinforparam.key)
                        {
                            string value = keyValuePair.Value.ToString();
                            getinforparam.value = value;
                            break;
                        }
                    }
                    string strreplace = "{" + getinforparam.key + "}";
                    string strvalue = getinforparam.value;
                    if (string.IsNullOrEmpty(getinforparam.value))
                    {
                        strvalue = "";
                    }
                    sql = sql.Replace(strreplace, strvalue);
                }
            }
            if (string.IsNullOrEmpty(sql)) return new JObject();
            string result = DbFactory.GetDataTableFromSQL(sql);
            JArray temparray = JArray.Parse(result);
            if (temparray.Count > 1) throw new FormatException("Get info [" + getinforid + "] have greater than 1 result");
            temparray = Utility.FormatKeyJArray(temparray);
            JObject jObject = (JObject)temparray.FirstOrDefault();
            return jObject;
        }

        public TransactionReponse MultiGetInfor(GetInforDataRequest model)
        {
            try
            {
                TransactionReponse reponse = new TransactionReponse();
                string[] multigetinfor = model.inforid.Split("|");
                JObject jsonreponse = new JObject();
                foreach (string id in multigetinfor)
                {
                    jsonreponse.Merge(GetInforData(id, model.param));
                }
                reponse.SetCode(Codetypes.Code_Success);
                reponse.SetResult(jsonreponse);
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new TransactionReponse(new CodeDescription(9997, ex.Message));
            }
        }

        public TransactionReponse GetMenuInvokeByRoleid(JObject model)
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
                Dictionary<string, object> para = new Dictionary<string, object>
                {
                    { "ROLEID", roleid }
                };
                string sqlreponse = DbFactory.GetDataTableFromStoredProcedure("ROLE_GETMENUFORINVOKE", para);
                JArray array = JArray.Parse(sqlreponse);
                reponse.SetCode(Codetypes.Code_Success);
                reponse.SetResult(new JObject { { "data", array } });
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                reponse.SetCode(new CodeDescription(9997, ex.Message));
                return reponse;
            }
        }

        public TransactionReponse UpdateMenuInvoke(ListRoleInvoke model)
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                if (model.data.Count == 0)
                {
                    reponse.SetCode(Codetypes.Err_InputFormat);
                    return reponse;
                }
                JArray roles = JArray.FromObject(model.data);
                string inputroles = roles.ToString();
                Dictionary<string, object> para = new Dictionary<string, object>
                {
                    { "JSON", inputroles }
                };
                string sqlreponse = DbFactory.GetVariableFromStoredProcedure("ROLE_UPDATEMENUROLES", para);
                if (sqlreponse == "0")
                {
                    reponse.SetCode(Codetypes.Code_Success);
                    return reponse;
                }
                else
                {
                    reponse.SetCode(Codetypes.Err_DatabaseError);
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

        public TransactionReponse GetReportForDashboardPage()
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                string sqlreponse = DbFactory.GetDataTableFromSQL("GetReportForDashboardPage");
                JObject rawData = (JObject)JArray.Parse(sqlreponse).First();
                string outgoingchartStr = rawData.SelectToken("outgoingchart").ToObject<string>();
                string incomingchartStr = rawData.SelectToken("incomingchart").ToObject<string>();
                JArray outgoingchart = (string.IsNullOrEmpty(outgoingchartStr)) ? new JArray() : JArray.Parse(outgoingchartStr);
                JArray incomingchart = (string.IsNullOrEmpty(incomingchartStr)) ? new JArray() : JArray.Parse(incomingchartStr);
                int sumuser = rawData.SelectToken("sumuser").ToObject<int>();
                int[] outgoingchartArray = new int[12], incomingchartArray = new int[12];
                int sumoutgoing = 0, sumincoming = 0;
                for (int i = 0; i < 12; i++)
                {
                    foreach (var outgoing in outgoingchart)
                    {
                        int month = outgoing.SelectToken("month").ToObject<int>();
                        if (month == (i + 1))
                        {
                            outgoingchartArray[i] = outgoing.SelectToken("count").ToObject<int>();
                            break;
                        }
                    }
                    sumoutgoing += outgoingchartArray[i];
                    foreach (var incoming in incomingchart)
                    {
                        int month = incoming.SelectToken("month").ToObject<int>();
                        if (month == (i + 1))
                        {
                            incomingchartArray[i] = incoming.SelectToken("count").ToObject<int>();
                            break;
                        }
                    }
                    sumincoming += incomingchartArray[i];
                }
                int sumTransaction = sumincoming + sumoutgoing;
                JObject reponseObject = new JObject
                {
                    {"sumuser" , sumuser},
                    {"sumtransaction" , sumTransaction},
                    {"sumoutgoing" , sumoutgoing},
                    {"sumincoming" , sumincoming},
                    {"outgoingchart" , JArray.FromObject(outgoingchartArray)},
                    {"incomingchart" , JArray.FromObject(incomingchartArray)},
                    {"transactionchart" , JArray.FromObject(new int[]{ sumincoming, sumoutgoing})},
                };
                reponse.SetCode(Codetypes.Code_Success);
                reponse.SetResult(reponseObject);
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                reponse.SetCode(new CodeDescription(9997, ex.Message));
                return reponse;
            }
        }

        public TransactionReponse GetListRoleDetail()
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                var roleDetails = _context.SUserroledetails.ToList();
                JObject jObject = new JObject { { "data", JArray.FromObject(roleDetails) } };
                reponse.SetCode(Codetypes.Code_Success);
                reponse.SetResult(jObject);
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                reponse.SetCode(new CodeDescription(9997, ex.Message));
                return reponse;
            }
        }

        public TransactionReponse AddRoleDetail(AddRoleDetailRequest role)
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                SUsrac usrac = _context.SUsracs.SingleOrDefault(x => x.username == role.usrname);
                if (usrac == null)
                {
                    reponse.SetCode(Codetypes.Err_UserNotExists);
                    return reponse;
                }
                SUserrole userrole = _context.SUserroles.SingleOrDefault(x => x.roleid == role.roleid);
                if (userrole == null)
                {
                    reponse.SetCode(Codetypes.Err_RoleNotExist);
                    return reponse;
                }
                SUserroledetail sUserroledetail = _context.SUserroledetails.SingleOrDefault(x => x.usrname == role.usrname && x.roleid == role.roleid);
                if (sUserroledetail != null)
                {
                    reponse.SetCode(Codetypes.Err_RoleDetailExist);
                    return reponse;
                }
                SUserroledetail userroledetail = new SUserroledetail
                {
                    usrname = role.usrname,
                    roleid = role.roleid,
                    description = role.description
                };
                _context.SUserroledetails.Add(userroledetail);
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

        public TransactionReponse DeleteRoleDetail(AddRoleDetailRequest role)
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                SUserroledetail userroledetail = _context.SUserroledetails.SingleOrDefault(x => x.usrname == role.usrname && x.roleid == role.roleid);
                if(userroledetail == null)
                {
                    reponse.SetCode(Codetypes.Err_DataNotFound);
                    return reponse;
                }
                int countRoleDetail = _context.SUserroledetails.Where(x => x.usrname == role.usrname).Count();
                if(countRoleDetail <= 1)
                {
                    reponse.SetCode(Codetypes.Err_DeleteRoleDetail);
                    return reponse;
                }
                _context.SUserroledetails.Remove(userroledetail);
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

        public TransactionReponse GetUserListByRoleID(JObject role)
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                string roleid = role.GetValueJObjectByKey("roleid");
                if (string.IsNullOrEmpty(roleid))
                {
                    reponse.SetCode(Codetypes.Err_RequireRoleID);
                    return reponse;
                }
                Dictionary<string, object> param = new Dictionary<string, object>
                {
                    {"ROLEID" , int.Parse(roleid) }
                };
                string sqlReponse = DbFactory.GetVariableFromStoredProcedure("GetAllUserByRoleID", param);
                JArray array = JArray.Parse(sqlReponse);
                JObject jObject = new JObject
                {
                    {"data" , array }
                };
                _context.SaveChanges();
                reponse.SetCode(Codetypes.Code_Success);
                reponse.SetResult(jObject);
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                reponse.SetCode(new CodeDescription(9997, ex.Message));
                return reponse;
            }
        }

        public void getmenu(JObject result, List<VMenu> menu, List<SUserrole> role, List<VRoleDetail> roleDetails, string level)
        {
            var tempmenu = menu.Where(x => x.menuparent == level && x.cmdtype == "M").OrderBy(y => y.menuorder).ToList();

            for (int i = 0; i < tempmenu.Count; i++)
            {
                JArray menuorder = new JArray();

                var tempmenu2 = menu.Where(x => x.menuparent == tempmenu[i].menuid).OrderBy(y => y.menuid).ToList();
                if (tempmenu2.Count > 0)
                {
                    foreach (var item in tempmenu2)
                    {
                        if (item.cmdtype.Equals("M"))
                        {
                            getmenu(result, menu, role, roleDetails, tempmenu[i].menuid);
                        }
                    }
                }

                for (int j = 0; j < role.Count; j++)
                {
                    JObject temp = new JObject();

                    temp.Add("roleid", role[j].roleid);
                    temp.Add("rolename", role[j].rolename);

                    var currentdetail = roleDetails.Where(x => x.roleid == role[j].roleid && x.menuid == tempmenu[i].menuid).ToList();
                    if (currentdetail.Count > 0)

                        temp.Add("self", currentdetail[0].invoke);
                    else
                        temp.Add("self", false);

                    foreach (var item in tempmenu2)
                    {
                        if (!item.cmdtype.Equals("M"))
                        {
                            var currentdetailbutton = roleDetails.Where(x => x.roleid == role[j].roleid && x.menuid == item.menuid).ToList();
                            if (currentdetailbutton.Count > 0)

                                temp.Add(item.menuid, currentdetailbutton[0].invoke);
                            else
                                temp.Add(item.menuid, false);
                        }
                    }

                    menuorder.Add(temp);
                }
                if (!result.ContainsKey(tempmenu[i].menuid))
                    result.Add(tempmenu[i].menuid, menuorder);
            }

        }

        public TransactionReponse GetMenuInvoke()
        {
            try
            {
                JObject json = new JObject();
                TransactionReponse reponse = new TransactionReponse();
                var listmenu = _context.VMenu.Distinct().OrderBy(x => x.menuorder).ToList();

                var formatmenu = dbUtils.FormatMenuOnly(listmenu, "0");
                var array = dbUtils.FormatMenuFlat(listmenu);

                var listrole = _context.SUserroles.Distinct().OrderBy(x => x.roleid).ToList();

                var role = dbUtils.FormatSUserRole(listrole);

                var roledetail = _context.VRoleDetail.Distinct().OrderBy(x => x.roleid).ToList();

                getmenu(json, listmenu, listrole, roledetail, "0");

                JObject result = new JObject();

                result.Add("table", json);
                result.Add("role", role);
                result.Add("menu", formatmenu);
                result.Add("menudetail", array);

                reponse.SetCode(Codetypes.Code_Success);
                reponse.SetResult(result);
                return reponse;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public TransactionReponse SwitchModeFAST()
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                string sqlreponse = DbFactory.GetVariableFromStoredProcedure("SwitchModeFAST");
                JObject result = new JObject
                {
                    { "currentmode", sqlreponse }
                };
                reponse.SetCode(Codetypes.Code_Success);
                reponse.SetResult(result);
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                reponse.SetCode(new CodeDescription(9997, ex.Message));
                return reponse;
            }
        }

        public TransactionReponse GetListParameter()
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                List<SParam> parameter = _context.SParams.ToList();
                List<AddParam> paramList = new List<AddParam>();
                foreach (SParam param in parameter)
                {
                    AddParam addParam = new AddParam
                    {
                        PARGRP = param.pargrp,
                        PARNAME = param.parname,
                        PARVAL = param.parval,
                        MVAL = param.mval,
                        DESCR = param.descr,
                        PARCODE = param.parcode
                    };

                    paramList.Add(addParam);
                }
                JArray array = JArray.FromObject(paramList);
                JObject Parameter = new JObject
                {
                    { "data", array }
                };
                reponse.SetCode(Codetypes.Code_Success);
                reponse.SetResult(Parameter);
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                reponse.SetCode(new CodeDescription(9997, ex.Message));
                return reponse;
            }
        }

        public TransactionReponse AddSystemParameter(AddParam param)
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {              
                SParam sparam = _context.SParams.SingleOrDefault(x => x.parname == param.PARNAME);
                if (sparam != null)
                {
                    reponse.SetCode(Codetypes.Err_ParameterNameExists);
                    return reponse;
                }
                SParam parameter = new SParam
                {
                    pargrp = param.PARGRP,                  
                    parname = param.PARNAME,                   
                    parval = param.PARVAL,
                    descr = param.DESCR,
                    mval = param.MVAL,
                    parcode = param.PARCODE
                };
                _context.SParams.Add(parameter);
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

        public TransactionReponse ModifySystemParameter(AddParam model)
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                SParam sparam = _context.SParams.SingleOrDefault(x => x.parname == model.PARNAME);
                if (sparam == null)
                {
                    reponse.SetCode(Codetypes.Err_DataNotFound);
                    return reponse;
                }
                sparam.pargrp = model.PARGRP;
                sparam.parname = model.PARNAME;
                sparam.parval = model.PARVAL;
                sparam.descr = model.DESCR;
                sparam.mval = model.MVAL;
                sparam.parcode = model.PARCODE;

                JObject obj = Utility.FromatJObject(sparam);
                _context.SaveChanges();
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

        public TransactionReponse ViewParameter(ViewParam param)
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                SParam sparam = _context.SParams.SingleOrDefault(x => x.parname == param.PARNAME);
                if (sparam == null)
                {
                    reponse.SetCode(Codetypes.Err_DataNotFound);
                    return reponse;
                }
                JObject obj = Utility.FromatJObject(sparam);
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

        public TransactionReponse DeleteParameter(ViewParam model)
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                SParam param = _context.SParams.SingleOrDefault(x => x.parname == model.PARNAME);
                if (param == null)
                {
                    reponse.SetCode(Codetypes.Err_DataNotFound);
                    return reponse;
                }
                _context.SParams.Remove(param);
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

        public TransactionReponse GetListSystemCode1()
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                List<SSystemcode> systemcodes = _context.SSystemcodes.ToList();
                List<AddSystemCode> addSystemCodes = new List<AddSystemCode>();
                foreach (SSystemcode systemcode1 in systemcodes)
                {
                    AddSystemCode addSystem = new AddSystemCode
                    {
                        cdid = systemcode1.cdid,
                        cdname = systemcode1.cdname,
                        caption = systemcode1.caption,
                        cdgrp = systemcode1.cdgrp,
                        cdidx = systemcode1.cdidx,
                        isvisible = bool.TrueString
                    };

                    addSystemCodes.Add(addSystem);
                }
                JArray array = JArray.FromObject(addSystemCodes);
                JObject objsystemcode = new JObject
                {
                    { "data", array }
                };
                reponse.SetCode(Codetypes.Code_Success);
                reponse.SetResult(objsystemcode);
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                reponse.SetCode(new CodeDescription(9997, ex.Message));
                return reponse;
            }
        }

        public TransactionReponse ViewSystemCode(ViewSystemCode systemcode)
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                
                SSystemcode systemcode1 = _context.SSystemcodes.SingleOrDefault(x => x.cdid == systemcode.cdid && x.cdname == systemcode.cdname && x.cdgrp == systemcode.cdgrp);
                if (systemcode1 == null)
                {
                    reponse.SetCode(Codetypes.Err_DataNotFound);
                    return reponse;
                }
                JObject obj = Utility.FromatJObject(systemcode1);
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

        public TransactionReponse AddSystemCode(AddSystemCode systemcode)
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                SSystemcode systemcode1 = _context.SSystemcodes.SingleOrDefault(x => x.cdid == systemcode.cdid && x.cdname == systemcode.cdname && x.cdgrp == systemcode.cdgrp);
                if (systemcode1 != null)
                {
                    reponse.SetCode(Codetypes.Err_DataNotFound);
                    return reponse;
                }
                SSystemcode systemcode2 = new SSystemcode
                {
                    cdid = systemcode.cdid,
                    cdname = systemcode.cdname,
                    caption = systemcode.caption,
                    cdgrp = systemcode.cdgrp,
                    cdidx = systemcode.cdidx,
                    isvisible = bool.Parse(systemcode.isvisible = bool.TrueString)
                };
                _context.SSystemcodes.Add(systemcode2);
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

        public TransactionReponse ModifySystemCode(AddSystemCode systemcode)
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                SSystemcode systemcode1 = _context.SSystemcodes.SingleOrDefault(x => x.cdid == systemcode.cdid && x.cdname == systemcode.cdname && x.cdgrp == systemcode.cdgrp);
                if (systemcode1 == null)
                {
                    reponse.SetCode(Codetypes.Err_DataNotFound);
                    return reponse;
                }
                systemcode1.cdid = systemcode.cdid;
                systemcode1.cdname = systemcode.cdname;
                systemcode1.caption = systemcode.caption;
                systemcode1.cdgrp = systemcode.cdgrp;
                systemcode1.cdidx = systemcode.cdidx;
                systemcode1.isvisible = bool.Parse(systemcode.isvisible = bool.TrueString);

                 JObject obj = Utility.FromatJObject(systemcode1);
                _context.SaveChanges();
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

        public TransactionReponse DeleteSystemCode(ViewSystemCode systemcode)
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                SSystemcode systemcode1 = _context.SSystemcodes.SingleOrDefault(x => x.cdid == systemcode.cdid && x.cdname == systemcode.cdname && x.cdgrp == systemcode.cdgrp);
                if (systemcode1 == null)
                {
                    reponse.SetCode(Codetypes.Err_DataNotFound);
                    return reponse;
                }
                _context.SSystemcodes.Remove(systemcode1);
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