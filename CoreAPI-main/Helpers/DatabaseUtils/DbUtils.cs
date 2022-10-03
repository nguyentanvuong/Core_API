using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WebApi.Entities;
using WebApi.Helpers.Common;
using WebApi.Helpers.Services;
using WebApi.Helpers.Utils;
using WebApi.Models;
using WebApi.Models.FAST;

namespace WebApi.Helpers.DatabaseUtils
{
    public class DbUtils
    {
        private readonly DataContext _dataContext;

        public DbUtils(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Users GetUsersBySession(string sessionId)
        {
            return _dataContext.Users.SingleOrDefault(x => x.ssesionid == sessionId);
        }
        public SUsrac GetUserWebByUsername(string username)
        {
            return _dataContext.SUsracs.SingleOrDefault(x => x.username == username);
        }

        public SUserpublic GetUserPublicByUsername(string username)
        {
            return _dataContext.SUserpublics.SingleOrDefault(x => x.username == username);
        }

        public bool AddUser(Users users)
        {
            try
            {
                Users current = _dataContext.Users.SingleOrDefault(x => x.usrid == users.usrid);
                if (current != null)
                {
                    current.usrbranch = users.usrbranch;
                    current.ssesionid = users.ssesionid;
                    current.usrpass = users.usrpass;
                    _dataContext.Users.Update(current);
                }
                else
                {
                    _dataContext.Users.Add(users);
                }
                _dataContext.SaveChanges();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public void AddOrReplaceListFASTAccount(List<SFastaccount> fastAccounts)
        {
            var currentAccount = _dataContext.SFastaccounts.ToList();
            foreach (SFastaccount fastAccount in fastAccounts)
            {
                SFastaccount current = currentAccount.SingleOrDefault(x => x.accnum == fastAccount.accnum);
                if (current != null)
                {
                    current.accname = fastAccount.accname;
                    current.accnum = fastAccount.accnum;
                    current.balance = fastAccount.balance;
                    current.branchnum = fastAccount.branchnum;
                    current.currency = fastAccount.currency;
                    current.status = fastAccount.status;
                    current.type = fastAccount.type;
                    _dataContext.SFastaccounts.Update(current);
                }
                else
                {
                    _dataContext.SFastaccounts.Add(fastAccount);
                }
            }
            _dataContext.SaveChanges();
        }

        public JObject FormatCdlist(List<SSystemcode> systemcodes)
        {
            List<string> cdnames = systemcodes.Select(x => x.cdname).Distinct().ToList<string>();
            JObject responseObject = new JObject();
            foreach (string cdname in cdnames)
            {
                JObject cdidArray = new JObject();
                foreach (SSystemcode systemcode in systemcodes)
                {
                    if (systemcode.cdname == cdname)
                    {
                        cdidArray.Add(systemcode.cdid, systemcode.caption);
                    }
                }
                responseObject.Add(cdname, cdidArray);
            }
            return responseObject;
        }

        ///<summary>
        /// Tạo câu điều kiện lọc dữ liệu SQL
        ///</summary>
        public static string GenerateWhereSQLCondition(JObject jObject)
        {
            if (jObject == null)
            {
                return "";
            }
            int countChild = jObject.Children().Count();
            if (countChild == 0)
            {
                return "";
            }
            else
            {
                string condition = " WHERE ";
                int countProcess = 0;
                foreach (KeyValuePair<string, JToken> keyValuePair in jObject)
                {
                    string key = keyValuePair.Key;
                    string value = keyValuePair.Value.ToString();
                    if (countProcess != countChild - 1)
                    {
                        condition += key + " LIKE '%" + value + "%' AND ";
                    }
                    else
                    {
                        condition += key + " LIKE '%" + value + "%' ";
                    }
                    countProcess++;
                }
                return condition;
            }
        }

        ///<summary>
        /// Tạo câu điều kiện lọc dữ liệu LINQ
        ///</summary>
        public string GenerateWhereConditionLinq(JObject jObject, string searchfunc)
        {
            if (jObject == null || string.IsNullOrEmpty(searchfunc))
            {
                return "1=1";
            }
            int countChild = jObject.Children().Count();
            if (countChild == 0)
            {
                return "1=1";
            }
            else
            {
                List<SSearch> searches = _dataContext.SSearches.Where(x => x.searchfunc == searchfunc).ToList();
                if (searches.Count == 0)
                {
                    return "1=1";
                }
                string condition = "";
                int countProcess = 0;
                foreach (KeyValuePair<string, JToken> keyValuePair in jObject)
                {
                    string key = keyValuePair.Key;
                    SSearch search = searches.SingleOrDefault(x => x.ftag.ToUpper() == key.ToUpper());
                    if (search != null)
                    {
                        string type = search.type.ToUpper();
                        string value = keyValuePair.Value.ToString();
                        if (countProcess != countChild - 1)
                        {
                            if (type == GlobalVariable.TypeSearchNumber)
                            {
                                condition += string.Format("{0}=={1} && ", key.ToLower(), value);
                            }
                            else
                            {
                                condition += string.Format("string(object({0}))" + ".Contains( \"{1}\") && ", key.ToLower(), value);
                            }
                        }
                        else
                        {
                            if (type == GlobalVariable.TypeSearchNumber)
                            {
                                condition += string.Format("{0}=={1}", key.ToLower(), value);
                            }
                            else
                            {
                                condition += string.Format("string(object({0}))" + ".Contains( \"{1}\")", key.ToLower(), value);
                            }
                        }
                    }
                    countProcess++;
                }
                if (condition[^3..].Contains("&&"))
                {
                    condition = condition[0..^4];
                }
                return condition;
            }
        }

        public JObject FormatSearchFunction(List<SSearch> searches)
        {
            List<string> searchcodes = searches.Select(x => x.searchfunc).Distinct().ToList<string>();
            JObject responseObject = new JObject();
            foreach (string searchcode in searchcodes)
            {
                JArray searchArray = new JArray();
                foreach (SSearch search in searches)
                {
                    if (search.searchfunc == searchcode)
                    {
                        searchArray.Add(JObject.FromObject(search));
                    }
                }
                JArray arraysort = new JArray(searchArray.OrderBy(obj => (int)obj["order"]));
                responseObject.Add(searchcode, arraysort);
            }
            return responseObject;
        }

        public IpcEmailtemplate GetEmailTemplate(int bankid, string channelid, string trancode)
        {
            return _dataContext.IpcEmailtemplates.SingleOrDefault(x => x.bankid == bankid && x.channelid == channelid && x.trancode == trancode);
        }

        public JObject FormatSForm(List<SForm> forms)
        {
            List<string> formnames = forms.Select(x => x.formname).Distinct().ToList<string>();
            JObject responseObject = new JObject();
            foreach (string formname in formnames)
            {
                JArray array = new JArray();
                foreach (SForm form in forms)
                {
                    if (form.formname == formname)
                    {
                        array.Add(JObject.FromObject(form));
                    }
                }
                JArray arraysort = new JArray(array.OrderBy(obj => (int)obj["order"]));
                responseObject.Add(formname, arraysort);
            }
            return responseObject;
        }
       

        public JArray FormatMenu(List<VMenuUser> menus, string parent)
        {
            JArray response = new JArray();
            var listroot = menus.Where(x => x.menuparent == parent).ToList();
            for (int i = 0; i < listroot.Count; i++)
            {
                JObject menu = new JObject();
                menu.Add("name", listroot[i].menuid);
                menu.Add("description", "");
                menu.Add("icon", listroot[i].icon);
                menu.Add("path", listroot[i].menupath);
                menu.Add("type", listroot[i].type);
                menu.Add("menuparent", listroot[i].menuparent);
                menu.Add("pageid", listroot[i].pageid);
                menu.Add("actionid", listroot[i].actionid);
                menu.Add("cmdtype", listroot[i].cmdtype);
                menu.Add("caption", listroot[i].caption);
                menu.Add("actionsetting", listroot[i].actionsetting);
                menu.Add("searchfunc", listroot[i].searchfunc);
                var listsub = menus.Where(x => x.menuparent == listroot[i].menuid && x.cmdtype == "M").ToList();
                if (listsub.Count > 0)
                {
                    JArray submenu = FormatMenu(menus, listroot[i].menuid);
                    menu.Add("sub", submenu);
                }
                var listbutton = menus.Where(x => x.menuparent == listroot[i].menuid && x.cmdtype != "M").ToList();
                if (listbutton.Count > 0)
                {
                    JArray subbutton = FormatMenu(menus, listroot[i].menuid);
                    menu.Add("button", subbutton);
                }

                response.Add(menu);
            }
            return response;
        }

        public TransactionReponse GetNewFastToken(FASTGetTokenRequest model)
        {
            TransactionReponse transactionReponse = new TransactionReponse();
            SToken token = _dataContext.STokens.SingleOrDefault(x => x.bankid == "2" && x.varname == "FASTTOKEN" && x.varext == "FAST");
            JObject response = new JObject();
            if (token == null)
            {
                response = RestClientService.CallRestAPI(GlobalVariable.FASTRestfulURL + GlobalVariable.URLGetToken, "post", model).Result;
                if (response == null || (int)response["status"]["code"] != 0)
                {
                    transactionReponse.SetCode(Codetypes.Err_Unknown);
                    transactionReponse.SetResult(response);
                    return transactionReponse;
                }
                string strnewtoken = (string)response["data"]["token"];
                token = new SToken
                {
                    bankid = "2",
                    varname = "FASTTOKEN",
                    varext = "FAST",
                    varvalue = strnewtoken,
                    vardate = JWTUtils.GetExpiryTimeJWT(strnewtoken),
                    description = "Token FAST"
                };
                _dataContext.STokens.Add(token);
            }
            string oldtoken = token.varvalue.Split(" ").Last();
            DateTime? expirytime = JWTUtils.GetExpiryTimeJWT(oldtoken);
            if (expirytime == null || expirytime.Value.AddMinutes(5) < DateTime.Now)
            {
                response = RestClientService.CallRestAPI(GlobalVariable.FASTRestfulURL + GlobalVariable.URLGetToken, "post", model).Result;
                if (response == null || (int)response["status"]["code"] != 0)
                {
                    transactionReponse.SetCode(Codetypes.Err_Unknown);
                    transactionReponse.SetResult(response);
                    return transactionReponse;
                }
                string strnewtoken = (string)response["data"]["token"];
                token.varvalue = strnewtoken;
                token.vardate = JWTUtils.GetExpiryTimeJWT(strnewtoken);
            }
            _dataContext.SaveChanges();
            GlobalVariable.CurrentFastToken = token;
            transactionReponse.SetCode(Codetypes.Code_Success);
            transactionReponse.SetResult(response);
            return transactionReponse;
        }

        public string CreateMessageXML(string trancode, JObject jOjectData)
        {
            string result = "";
            try
            {
                var inputxml = _dataContext.Ipcoutputdefinexmls.Where(x => x.ipctrancode == trancode).OrderBy(x => x.fieldno).ToList();
                foreach (var field in inputxml)
                {
                    switch (field.fieldstyle)
                    {
                        case "VALUE":
                            result += field.valuename.ToString();
                            break;
                        case "TAG":
                            object tag;
                            if (string.IsNullOrEmpty(field.valueobject)) tag = "";
                            else tag = jOjectData.SelectToken(field.valueobject);
                            if (string.IsNullOrEmpty(Convert.ToString(tag)))
                                result += String.Format("<{0}></{0}>", field.fieldname);
                            else
                                result += String.Format("<{0}><![CDATA[{1}]]></{0}>", field.fieldname, Convert.ToString(tag));
                            break;
                        case "XMLDATE":
                            JToken xmldate = jOjectData.SelectToken(field.valueobject);
                            if (xmldate.Type == JTokenType.Date)
                            {
                                var dateTime = DateTime.Parse(jOjectData.SelectToken(field.valueobject).ToString());
                                string formatdt = Utility.ConvertDateTimeToStringDatetime(dateTime, field.formatobject);
                                result += String.Format("<{0}>{1}</{0}>", field.fieldname, formatdt);
                            }
                            else
                            {
                                string dateStr = Convert.ToString(xmldate);
                                var date = Utility.ConvertStringToDateTime(dateStr);
                                result += String.Format("<{0}>{1}</{0}>", field.fieldname, Utility.ConvertDateTimeToStringDatetime(date, field.formatobject));
                            }
                            break;
                        case "ARRAY1":
                            break;
                        case "ARRAY2":
                            break;
                        case "ARRAY12":
                            break;
                        case "DATATABLE":
                            break;
                        case "HASHTABLE":
                            break;
                        case "XML":
                            string valueString = "";
                            if (string.IsNullOrEmpty(field.valueobject))
                            {
                                valueString = field.defaultvalue;
                                result += String.Format("<{0}>{1}</{0}>", field.fieldname, XmlUtils.FormatEscapeXMLContent(valueString));
                                break;
                            }
                            var valueJOject = jOjectData.SelectToken(field.valueobject);
                            if ((valueJOject == null || string.IsNullOrEmpty(valueJOject.ToString())) && !string.IsNullOrEmpty(field.defaultvalue)) valueString = field.defaultvalue;
                            else valueString = valueJOject.ToString();
                            result += String.Format("<{0}>{1}</{0}>", field.fieldname, XmlUtils.FormatEscapeXMLContent(valueString));
                            break;
                        case "XMLHIDE":
                            if (string.IsNullOrEmpty(field.valueobject)) break;
                            object xmlhide = jOjectData.SelectToken(field.valueobject);
                            if (string.IsNullOrEmpty(Convert.ToString(xmlhide)))
                                result += string.Empty;
                            else
                                result += String.Format("<{0}>{1}</{0}>", field.fieldname, XmlUtils.FormatEscapeXMLContent(xmlhide.ToString()));
                            break;
                        case "XMLATTRIBUTE":
                            string id = string.Empty;
                            string val = string.Empty;
                            JObject attribute = (JObject)jOjectData.SelectToken(field.valueobject);
                            foreach (var entry in attribute)
                            {
                                if (entry.Key.ToUpper().Equals("VALUE"))
                                {
                                    val = entry.Value.ToString();
                                }
                                else
                                {
                                    id = id + " " + entry.Key + "=" + "\"" + entry.Value + "\"";
                                }
                            }
                            result += string.Format("<{0}{1}>{2}</{0}>", field.fieldname, id, val);
                            break;
                        case "FORMAT":
                            //sua lai sau
                            string text1 = jOjectData.SelectToken(field.valueobject.Split("|")[0])?.ToString();
                            string text2 = jOjectData.SelectToken(field.valueobject.Split("|")[1])?.ToString();
                            result += string.Format(field.formatobject, text1, text2);
                            break;
                        case "SQL":
                            string value = jOjectData.SelectToken(field.valueobject)?.ToString();
                            string sql = string.Format(field.formatobject, (string.IsNullOrEmpty(value) ? "" : value));
                            result += String.Format("<{0}>{1}</{0}>", field.fieldname, DbFactory.GetVariableFromSQL(sql));
                            break;
                    }
                }
            }
            catch(Exception)
            {
                throw;
            }
            return result;
        }


        public JArray FormatMenuOnly(List<VMenu> menus, string parent)
        {
            JArray response = new JArray();
            var listroot = menus.Where(x => x.menuparent == parent && x.cmdtype == "M").ToList();
            for (int i = 0; i < listroot.Count; i++)
            {
                JObject menu = new JObject();
                menu.Add("name", listroot[i].menuid);
                menu.Add("description", "");
                menu.Add("icon", listroot[i].icon);
                menu.Add("path", listroot[i].menupath);
                menu.Add("type", listroot[i].type);
                menu.Add("menuparent", listroot[i].menuparent);
                menu.Add("cmdtype", listroot[i].cmdtype);
                menu.Add("caption", listroot[i].caption);
                var listsub = menus.Where(x => x.menuparent == listroot[i].menuid && x.cmdtype == "M").ToList();
                if (listsub.Count > 0)
                {
                    JArray submenu = FormatMenuOnly(menus, listroot[i].menuid);
                    menu.Add("sub", submenu);
                }
                response.Add(menu);
            }
            return response;
        }

        public JObject FormatMenuFlat(List<VMenu> menus)
        {
            JObject response = new JObject();
            for (int i = 0; i < menus.Count; i++)
            {
                JObject menu = new JObject();
                menu.Add("name", menus[i].menuid);
                menu.Add("description", "");
                menu.Add("icon", menus[i].icon);
                menu.Add("path", menus[i].menupath);
                menu.Add("type", menus[i].type);
                menu.Add("menuparent", menus[i].menuparent);
                menu.Add("cmdtype", menus[i].cmdtype);
                menu.Add("caption", menus[i].caption);
                response.Add(menus[i].menuid, menu);
            }
            return response;
        }

        public JArray FormatSUserRole(List<SUserrole> userrole)
        {
            JArray response = new JArray();
            for (int i = 0; i < userrole.Count; i++)
            {
                JObject menu = new JObject();
                menu.Add("roleid", userrole[i].roleid);
                menu.Add("rolename", userrole[i].rolename);
                menu.Add("rolecaption", userrole[i].roledescription);
                menu.Add("status", userrole[i].status);
                response.Add(menu);
            }
            return response;
        }

        public JObject MappingJSONObject(string trancode, object objectSource, List<Ipcmappingmsg> mappingFields = null)
        {
            JObject jObjectSource = JObject.FromObject(objectSource);
            if (mappingFields == null) mappingFields = _dataContext.Ipcmappingmsgs.Where(x => x.ipctrancode == trancode).OrderBy(x => x.fieldno).ToList();
            JObject response = new JObject();
            foreach (var mappingField in mappingFields)
            {
                string fieldtype = mappingField.fieldtype;
                JToken jTokenSource = jObjectSource.SelectToken(mappingField.sourcefield);
                string stringSource = (jTokenSource == null) ? "" : jTokenSource.ToString();
                switch (fieldtype)
                {
                    case "VALUE":
                        response.Add(mappingField.destfield, stringSource);
                        break;
                    case "DEFAULT":
                        response.Add(mappingField.destfield, mappingField.defaultvalue);
                        break;
                    case "DATE":
                        if (string.IsNullOrEmpty(stringSource) || (!Utility.IsDate(stringSource) && jTokenSource.Type != JTokenType.Date))
                        {
                            response.Add(mappingField.destfield, "");
                        }
                        else if(jTokenSource.Type == JTokenType.Date)
                        {
                            var dateTime = DateTime.Parse(stringSource);
                            string formatdt = Utility.ConvertDateTimeToStringDatetime(dateTime, mappingField.fieldformat);
                            response.Add(mappingField.destfield, formatdt);
                        }
                        else 
                        {
                            var getdt = Utility.ConvertStringToDateTime(stringSource);
                            string formatdt = Utility.ConvertDateTimeToStringDatetime(getdt, mappingField.fieldformat);
                            response.Add(mappingField.destfield, formatdt);
                        }
                        break;
                    case "DATE_NOW":
                        string formatdtnull = Utility.ConvertDateTimeToStringDatetime(DateTime.Now, mappingField.fieldformat);
                        response.Add(mappingField.destfield, formatdtnull);
                        break;
                    case "TO_LOWER":
                        response.Add(mappingField.destfield, stringSource.ToLower());
                        break;
                    case "JSON_ATTRIBUTE":
                        string[] list = mappingField.sourcefield.Split("|");
                        string result = string.Format(mappingField.fieldformat, jObjectSource.SelectToken(list[0]).ToString(), jObjectSource.SelectToken(list[1]).ToString());
                        JObject attribute = JObject.Parse(result);
                        response.Add(mappingField.destfield, attribute);
                        break;
                    case "SQL":
                        string sql = (string.IsNullOrEmpty(stringSource))? mappingField.fieldformat : string.Format(mappingField.fieldformat, stringSource);
                        response.Add(mappingField.destfield, DbFactory.GetVariableFromSQL(sql));
                        break;
                }
            }
            return response;
        }
    }
}
