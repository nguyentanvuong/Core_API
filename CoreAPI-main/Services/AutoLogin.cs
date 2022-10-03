using Apache.NMS;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using WebApi.Entities;
using WebApi.Helpers.Common;
using WebApi.Helpers.DatabaseUtils;
using WebApi.Helpers.Utils;
using WebApi.Models;

namespace WebApi.Helpers.Services
{
    public class AutoLogin
    {
        private DbUtils databaseUtils;
        private List<LoginRequest> listUser;
        private DataContext context;

        public AutoLogin(DataContext dataContext,  List<LoginRequest> users)
        {
            context = dataContext;
            listUser = users;
            databaseUtils = new DbUtils(context);
        }

        public bool LoginDefaultUser()
        {
            try
            {
                if (listUser == null || listUser.Count == 0) return false;
                JsonLoginResponse clsJsonLoginResponse;
                GlobalVariable.O9CoreLoginRequest = listUser.First();
                GlobalVariable.FASTMode = context.SParams.SingleOrDefault(x => x.parcode == "API" && x.pargrp == "FAST" && x.parname == "MODE")?.parval;
                for (int i = 0; i < listUser.Count; i++)
                {
                    JsonLoginRequest loginRequest = new JsonLoginRequest();
                    loginRequest.L = listUser[i].Username;
                    loginRequest.P = listUser[i].encrypt ? listUser[i].Password : O9Encrypt.MD5Encrypt(listUser[i].Password);
                    loginRequest.A = false;

                    string strResult = O9Utils.GenJsonBodyRequest(loginRequest, GlobalVariable.UMG_LOGIN, "", EnmCacheAction.NoCached, EnmSendTypeOption.Synchronize, "-1", MsgPriority.Normal);
                    clsJsonLoginResponse = JsonConvert.DeserializeObject<JsonLoginResponse>(strResult);
                    if (clsJsonLoginResponse != null)
                    {
                        if (string.IsNullOrEmpty(clsJsonLoginResponse.E))
                        {
                            Users users = new Users(clsJsonLoginResponse.USRID, clsJsonLoginResponse.BRANCHID, clsJsonLoginResponse.BRANCHCD, listUser[i].Username, loginRequest.P,
                                                    clsJsonLoginResponse.UUID, clsJsonLoginResponse.LANG, clsJsonLoginResponse.BUSDATE);
                            databaseUtils.AddUser(users);
                            O9Utils.UpdateWorkingDate(clsJsonLoginResponse.BUSDATE);
                        }
                    }
                }
            }
            catch (System.Exception)
            {
                return false;
            }
            return true;
        }

        public bool GetAllCdlist()
        {
            try
            {
                List<SSystemcode> systemcodes = context.SSystemcodes.ToList();
                JObject responseObject = databaseUtils.FormatCdlist(systemcodes);
                GlobalVariable.CDLIST = responseObject;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool GetAllSearchFunc()
        {
            try
            {
                List<SSearch> searches = context.SSearches.ToList();
                JObject responseObject = databaseUtils.FormatSearchFunction(searches);
                GlobalVariable.SearchFunction = responseObject;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool GetAllSForm()
        {
            try
            {
                List<SForm> forms = context.SForms.ToList();
                JObject responseObject = databaseUtils.FormatSForm(forms);
                GlobalVariable.SForm = responseObject;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool GetNewFASTToken()
        {
            try
            {
                TransactionReponse transactionReponse = databaseUtils.GetNewFastToken(GlobalVariable.FASTGetTokenRequest);
                if (transactionReponse == null || transactionReponse.errorcode != 0) return false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool GetDefaultConfigFromDatabase()
        {
            try
            {
                GlobalVariable.Sender_SWIFTCode = context.SParams.SingleOrDefault(x => x.parname == "SENDER_SWIFTCODE").parval;
                GlobalVariable.Sender_ParticipantCode = context.SParams.SingleOrDefault(x => x.parname == "SENDER_PARTICIPANTCODE").parval;
                GlobalVariable.Sender_BICFI = context.SParams.SingleOrDefault(x => x.parname == "SENDER_BICFI").parval;
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
