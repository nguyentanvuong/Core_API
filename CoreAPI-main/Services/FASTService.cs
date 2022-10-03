using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Threading.Tasks;
using WebApi.Entities;
using WebApi.Helpers.Common;
using WebApi.Helpers.DatabaseUtils;
using WebApi.Helpers.Utils;
using WebApi.Models;
using WebApi.Models.FAST;
using WebApi.Models.Transaction.FAST;

namespace WebApi.Services
{
    public interface IFASTService
    {
        public TransactionReponse GetListFASTBank();
        public TransactionReponse GetTransactionOutgoing(PagingRequest model);
        public TransactionReponse GetTransactionIncoming(PagingRequest model);
        public TransactionReponse GetTransactionById(JObject model);
        public TransactionReponse ViewFastBank(JObject model);
        public TransactionReponse ModifyFastBank(JObject model);
        public TransactionReponse AddFastBank(JObject model);
        public TransactionReponse DeleteFastBank(JObject model);
        public TransactionReponse FASTVerifyAccount(FASTVerifyAccountRequest model);
        public Task<TransactionReponse> MakeFullFundTransfer(JObject model);
        public Task<TransactionReponse> FASTAccountInquiry();
        public Task<TransactionReponse> FASTGetIncomingTransaction(JObject model);
        public Task<TransactionReponse> FASTGetOutgoingTransactionByPmtInfId(GetOutgoingTransactionByPmtInfIdRequest model); 
        public Task<TransactionReponse> FASTReverseTransaction(JObject model);
        public Task<TransactionReponse> FASTAcknowledgement(JObject model);
        public TransactionReponse SyncFASTOutgoingTransaction(JObject jObject);
        public TransactionReponse SyncFASTIncomingTransactionToO9Core(JObject jObject);
        public TransactionReponse GetListFASTAccount();
        public TransactionReponse GetLogsubtransByIpctransid(JObject model);
        public TransactionReponse SyncTransactionStatusToO9Core(JObject model);
    }
    public class FASTService : IFASTService
    {
        private readonly DataContext _context;
        private readonly DbUtils dbUtils;
        private readonly ILogger<FASTService> _logger;
        private readonly IApiService _apiService;

        public FASTService(DataContext context, ILogger<FASTService> logger, IApiService apiService)
        {
            _logger = logger;
            _context = context;
            dbUtils = new DbUtils(_context);
            _apiService = apiService;
        }

        public TransactionReponse GetListFASTBank()
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                List<FastBank> fastBanks = _context.FastBanks.ToList();
                List<GetListFastBankReponse> listFastBankReponses = new List<GetListFastBankReponse>();
                foreach (FastBank fastBank in fastBanks)
                {
                    GetListFastBankReponse bank = new GetListFastBankReponse
                    {
                        bankbuilding = fastBank.bankbuilding,
                        bankcountry = fastBank.bankcountry,
                        bankid = fastBank.bankid,
                        bankname = fastBank.bankname,
                        bankpostalcode = fastBank.bankpostalcode,
                        bankstreet = fastBank.bankstreet,
                        banktown = fastBank.banktown,
                        participatecode = fastBank.participatecode,
                        bicfi = fastBank.bicfi,
                    };
                    listFastBankReponses.Add(bank);
                }
                JArray array = JArray.FromObject(listFastBankReponses);
                JObject fastbanks = new JObject
                {
                    { "data", array }
                };
                reponse.SetCode(Codetypes.Code_Success);
                reponse.SetResult(fastbanks);
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new TransactionReponse(new CodeDescription(9997, ex.Message));
            }
        }

        public TransactionReponse GetTransactionOutgoing(PagingRequest model)
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                string condition = dbUtils.GenerateWhereConditionLinq(model.search, model.searchfunc);
                int rowCount = _context.VFastOutgoings.Where(condition).Count();
                List<VFastOutgoing> data = _context.VFastOutgoings.Where(condition)
                                                    .OrderByDescending(x => x.ipctransid)
                                                    .Skip(model.numberperpage * (model.page - 1))
                                                    .Take(model.numberperpage).ToList();
                PagingReponse pagingReponse = new PagingReponse
                {
                    currentpage = model.page,
                    maxpage = (int)Math.Ceiling(rowCount / (float)model.numberperpage),
                    total = rowCount,
                    data = JArray.FromObject(data)
                };

                JObject obj = JObject.FromObject(pagingReponse);
                reponse.SetCode(Codetypes.Code_Success);
                reponse.SetResult(obj);
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new TransactionReponse(new CodeDescription(9997, ex.Message));
            }
        }

        public TransactionReponse GetTransactionIncoming(PagingRequest model)
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                string condition = dbUtils.GenerateWhereConditionLinq(model.search, model.searchfunc);
                int rowCount = _context.VFastIncomings.Where(condition).Count();
                List<VFastIncoming> data = _context.VFastIncomings.Where(condition)
                                                    .OrderByDescending(x => x.ipctransid)
                                                    .Skip(model.numberperpage * (model.page - 1))
                                                    .Take(model.numberperpage).ToList();
                PagingReponse pagingReponse = new PagingReponse
                {
                    currentpage = model.page,
                    maxpage = (int)Math.Ceiling(rowCount / (float)model.numberperpage),
                    total = rowCount,
                    data = JArray.FromObject(data)
                };

                JObject obj = JObject.FromObject(pagingReponse);
                reponse.SetCode(Codetypes.Code_Success);
                reponse.SetResult(obj);
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new TransactionReponse(new CodeDescription(9997, ex.Message));
            }
        }

        public TransactionReponse GetTransactionById(JObject model)
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                string id = Utility.GetValueJObjectByKey(model, "ipctransid");
                if (string.IsNullOrEmpty(id))
                {
                    reponse.SetCode(Codetypes.Err_Requireipctranid);
                    return reponse;
                }
                Dictionary<string, object> para = new Dictionary<string, object>
                {
                    { "TRANID", id }
                };
                string sqlreponse = DbFactory.GetDataTableFromStoredProcedure("FAST_GETTRANSDETAIL", para);
                if (sqlreponse == null)
                {
                    reponse.SetCode(Codetypes.Err_Unknown);
                    return reponse;
                }
                JArray array = JArray.Parse(sqlreponse);
                JObject obj = new JObject();
                if (array.Count != 0)
                {
                    obj = (JObject)array.First();
                }
                reponse.SetCode(Codetypes.Code_Success);
                reponse.SetResult(obj);
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new TransactionReponse(new CodeDescription(9997, ex.Message));
            }
        }

        public TransactionReponse GetLogsubtransByIpctransid(JObject model)
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                string id = Utility.GetValueJObjectByKey(model, "ipctransid");
                if (string.IsNullOrEmpty(id))
                {
                    reponse.SetCode(Codetypes.Err_Requireipctranid);
                    return reponse;
                }
                Dictionary<string, object> para = new Dictionary<string, object>
                {
                    { "IPCTRANSID", id }
                };
                string sqlreponse = DbFactory.GetDataTableFromStoredProcedure("GETLOGSUBTRANSBYIPCTRANSID", para);
                if (sqlreponse == null)
                {
                    reponse.SetCode(Codetypes.Err_Unknown);
                    return reponse;
                }
                JArray array = JArray.Parse(sqlreponse);
                var jsonSettings = new JsonSerializerSettings
                {
                    DateFormatString = GlobalVariable.DatetimeFormatWithMilisecond
                };
                string jsonArray = JsonConvert.SerializeObject(array, jsonSettings);
                JObject obj = new JObject { { "data", JArray.Parse(jsonArray) } };
                reponse.SetCode(Codetypes.Code_Success);
                reponse.SetResult(obj);
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new TransactionReponse(new CodeDescription(9997, ex.Message));
            }
        }

        public TransactionReponse ViewFastBank(JObject model)
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
                FastBank bank = _context.FastBanks.SingleOrDefault(x => x.bankid == bankid);
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
                return new TransactionReponse(new CodeDescription(9997, ex.Message));
            }
        }

        public TransactionReponse ModifyFastBank(JObject model)
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
                string bicfi = Utility.GetValueJObjectByKey(model, "bicfi");
                if (string.IsNullOrEmpty(bicfi))
                {
                    reponse.SetCode(Codetypes.Err_RequireBICFI);
                    return reponse;
                }
                FastBank bank = _context.FastBanks.SingleOrDefault(x => x.bankid == bankid);
                if (bank == null)
                {
                    reponse.SetCode(Codetypes.Err_DataNotFound);
                    return reponse;
                }
                bank = (FastBank)Utility.SetValueObject(model, bank);
                if (bank == null)
                {
                    return null;
                }
                _context.SaveChanges();
                reponse.SetCode(Codetypes.Code_Success);
                reponse.SetResult(JObject.FromObject(bank));
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new TransactionReponse(new CodeDescription(9997, ex.Message));
            }
        }

        public TransactionReponse AddFastBank(JObject model)
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
                string bicfi = Utility.GetValueJObjectByKey(model, "bicfi");
                if (string.IsNullOrEmpty(bicfi))
                {
                    reponse.SetCode(Codetypes.Err_RequireBICFI);
                    return reponse;
                }
                int count = _context.FastBanks.Where(x => x.bankid == bankid).Count();
                if (count > 0)
                {
                    reponse.SetCode(Codetypes.Err_FastBankExist);
                    return reponse;
                }
                FastBank bank = new FastBank();
                bank = (FastBank)Utility.SetValueObject(model, bank);
                if (bank == null)
                {
                    return null;
                }
                _context.FastBanks.Add(bank);
                _context.SaveChanges();
                reponse.SetCode(Codetypes.Code_Success);
                reponse.SetResult(JObject.FromObject(bank));
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new TransactionReponse(new CodeDescription(9997, ex.Message));
            }
        }

        public TransactionReponse DeleteFastBank(JObject model)
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
                if (bankid == GlobalVariable.Sender_SWIFTCode)
                {
                    reponse.SetCode(Codetypes.Err_InvalidBankId);
                    return reponse;
                }
                FastBank bank = _context.FastBanks.SingleOrDefault(x => x.bankid == bankid);
                if (bank == null)
                {
                    reponse.SetCode(Codetypes.Err_DataNotFound);
                    return reponse;
                }
                var remove = _context.FastBanks.Remove(bank);
                int res = _context.SaveChanges();
                reponse.SetCode(Codetypes.Code_Success);
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new TransactionReponse(new CodeDescription(9997, ex.Message));
            }
        }

        public TransactionReponse GetListFASTAccount()
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                var listAccount = _context.SFastaccounts.ToList();
                if (listAccount.Count == 0)
                {
                    reponse.SetCode(Codetypes.Err_DataNotFound);
                    return reponse;
                }
                var jsonSettings = new JsonSerializerSettings
                {
                    DateFormatString = "dd/MM/yyyy HH:mm:ss"
                };
                string listAccountString = JsonConvert.SerializeObject(listAccount, jsonSettings);
                reponse.SetResult(new JObject { { "data", JArray.Parse(listAccountString) } });
                reponse.SetCode(Codetypes.Code_Success);
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new TransactionReponse(new CodeDescription(9997, ex.Message));
            }
        }

        #region API call to FAST RESTful Service
        public TransactionReponse FASTVerifyAccount(FASTVerifyAccountRequest model)
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                RestHeaderRequest restHeader = new RestHeaderRequest
                {
                    Authentication = "Bearer " + GlobalVariable.CurrentFastToken.varvalue.Trim()
                };
                JObject body = new JObject
                {
                    {"account_no" , model.RACNO},
                    {"participant_code" , model.BICCD}
                };
                JObject JObjectReponse = RestClientService.CallRestAPI(GlobalVariable.FASTRestfulURL + GlobalVariable.URLVerifyAccount, "post", body, restHeader).Result;
                var errorCode = JObjectReponse.SelectToken("status.code");
                if (errorCode.ToString() != "0")
                {
                    reponse.SetCode(new CodeDescription(int.Parse(JObjectReponse.SelectToken("status.errorCode").ToString()), JObjectReponse.SelectToken("status.error").ToString()));
                    return reponse;
                }
                reponse.SetCode(Codetypes.Code_Success);
                JObject accountInfor = new JObject
                {
                    { "RCTYPE", JObjectReponse.SelectToken("data.account_data.account_type")},
                    { "CRNAME", JObjectReponse.SelectToken("data.account_data.account_name")},
                    { "RCCR", JObjectReponse.SelectToken("data.account_data.account_currency")},
                    { "BICCD", JObjectReponse.SelectToken("data.account_data.bank_code")},
                    { "RACNO", JObjectReponse.SelectToken("data.account_data.account")},
                    { "ISVERIFY", 1}
                };
                reponse.SetResult(accountInfor);
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new TransactionReponse(new CodeDescription(9997, ex.Message));
            }
        }
        #endregion

        #region API call to FAST SOAP Service
        public async Task<TransactionReponse> FASTAccountInquiry()
        {
            TransactionReponse reponsetrans = new TransactionReponse();
            try
            {
                FASTGetAccountInquiryRequest soaprerquest = new FASTGetAccountInquiryRequest()
                {
                    cm_password = GlobalVariable.CMPassword,
                    cm_user_name = GlobalVariable.CMUsername,
                    participant_code = GlobalVariable.Sender_ParticipantCode,
                };

                string res = dbUtils.CreateMessageXML(GlobalVariable.FASTAccountInquiry, JObject.FromObject(soaprerquest));
                string responseContent = await FASTUtils.CallSOAPServiceAsync(res);
                JObject jObjectReponse = FASTUtils.AnalyzeMessageFAST(responseContent, true);
                List<SFastaccount> fastAccounts = new List<SFastaccount>();
                JToken curAccUSD = jObjectReponse.SelectToken("curAccUSD.Brnch");
                if (curAccUSD is JArray)
                {
                    foreach (JObject jObjectAccUSD in (JArray)curAccUSD)
                    {
                        SFastaccount accUSD = new SFastaccount
                        {
                            accnum = jObjectAccUSD.SelectToken("Acc").ToString(),
                            accname = "FAST USD Account Branch " + jObjectAccUSD.SelectToken("Nb").ToString(),
                            balance = decimal.Parse(jObjectAccUSD.SelectToken("Bal").ToString()),
                            currency = "USD",
                            opendate = DateTime.Now,
                            branchnum = jObjectAccUSD.SelectToken("Nb").ToString(),
                            status = "A",
                            type = "FAST"
                        };
                        fastAccounts.Add(accUSD);
                    }
                }
                else if (curAccUSD is JObject)
                {
                    SFastaccount accUSD = new SFastaccount
                    {
                        accnum = curAccUSD.SelectToken("Acc").ToString(),
                        accname = "FAST USD Account Branch " + curAccUSD.SelectToken("Nb").ToString(),
                        balance = decimal.Parse(curAccUSD.SelectToken("Bal").ToString()),
                        currency = "USD",
                        opendate = DateTime.Now,
                        branchnum = curAccUSD.SelectToken("Nb").ToString(),
                        status = "A",
                        type = "FAST"
                    };
                    fastAccounts.Add(accUSD);
                }
                JToken curAccKHR = jObjectReponse.SelectToken("curAccKHR.Brnch");
                if (curAccKHR is JArray)
                {
                    foreach (JObject jObjectAccKHR in (JArray)curAccKHR)
                    {
                        SFastaccount accKHR = new SFastaccount
                        {
                            accnum = jObjectAccKHR.SelectToken("Acc").ToString(),
                            accname = "FAST KHR Account Branch " + jObjectAccKHR.SelectToken("Nb").ToString(),
                            balance = decimal.Parse(jObjectAccKHR.SelectToken("Bal").ToString()),
                            currency = "KHR",
                            opendate = DateTime.Now,
                            branchnum = jObjectAccKHR.SelectToken("Nb").ToString(),
                            status = "A",
                            type = "FAST"
                        };
                        fastAccounts.Add(accKHR);
                    }
                }
                else if (curAccKHR is JObject)
                {
                    SFastaccount accKHR = new SFastaccount
                    {
                        accnum = curAccKHR.SelectToken("Acc").ToString(),
                        accname = "FAST KHR Account Branch " + curAccKHR.SelectToken("Nb").ToString(),
                        balance = decimal.Parse(curAccKHR.SelectToken("Bal").ToString()),
                        currency = "KHR",
                        opendate = DateTime.Now,
                        branchnum = curAccKHR.SelectToken("Nb").ToString(),
                        status = "A",
                        type = "FAST"
                    };
                    fastAccounts.Add(accKHR);
                }
                dbUtils.AddOrReplaceListFASTAccount(fastAccounts);
                reponsetrans.SetResult(new JObject { { "recordschange: ", fastAccounts.Count } });
                reponsetrans.SetCode(Codetypes.Code_Success_Trans);
                return reponsetrans;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new TransactionReponse(new CodeDescription(9997, ex.Message));
            }
        }

        public async Task<TransactionReponse> MakeFullFundTransfer(JObject model)
        {
            TransactionReponse reponsetrans = new TransactionReponse();
            try
            {
                string ipctranid = Utility.GetValueJObjectByKey(model, "ipctransid");
                if (string.IsNullOrEmpty(ipctranid))
                {
                    reponsetrans.SetCode(Codetypes.Err_Requireipctranid);
                    return reponsetrans;
                }
                DateTime startTime = DateTime.Now;
                Ipclogtran ipclogtran = _context.Ipclogtrans.SingleOrDefault(x => x.ipctransid == long.Parse(ipctranid) && x.num19 == 0);
                if (ipclogtran == null)
                {
                    reponsetrans.SetCode(Codetypes.Err_IPCLogTranNotExist);
                    return reponsetrans;
                }
                JObject isoMessage = dbUtils.MappingJSONObject(GlobalVariable.FASTMAKEFULLFUN, ipclogtran);
                FASTMakeFundTransferRequest soaprequest = new FASTMakeFundTransferRequest()
                {
                    cm_password = GlobalVariable.CMPassword,
                    cm_user_name = GlobalVariable.CMUsername,
                    iso_message = isoMessage
                };
                string res = dbUtils.CreateMessageXML(GlobalVariable.FASTMAKEFULLFUN, JObject.FromObject(soaprequest));
                string responseContent = await FASTUtils.CallSOAPServiceAsync(res);
                JObject jObjectReponse = FASTUtils.AnalyzeMessageFAST(responseContent, true);
                string fastStatus = jObjectReponse.SelectToken("CstmrPmtStsRpt.OrgnlPmtInfAndSts.TxInfAndSts.TxSts")?.ToString();
                string addtlrmtinf = jObjectReponse.SelectToken("CstmrPmtStsRpt.OrgnlPmtInfAndSts.TxInfAndSts.StsRsnInf.AddtlInf")?.ToString();
                if (fastStatus == FASTStatus.ReceivedAtACH)
                {
                    ipclogtran.status = string.IsNullOrEmpty(fastStatus) ? ipclogtran.status : fastStatus;
                    ipclogtran.char30 = string.IsNullOrEmpty(fastStatus) ? ipclogtran.status : fastStatus;
                    ipclogtran.char46 = string.IsNullOrEmpty(addtlrmtinf) ? ipclogtran.char46 : addtlrmtinf;
                    ipclogtran.num26 = (ipclogtran.num26 == null) ? ipclogtran.num26 = 1 : ipclogtran.num26 += 1;
                    ipclogtran.num19 = 1;
                    ipclogtran.num18 = 1;
                    ipclogtran.errordesc = "Make full fund tranfer successful";
                    _context.Ipclogtrans.Update(ipclogtran);
                    _context.SaveChanges();
                    FASTUtils.InsertLogSubTran(ipclogtran.tranref, ipclogtran.ipctransid, ipclogtran.sourceid, GlobalVariable.FASTMAKEFULLFUN,
                                                                   startTime, "S", ipclogtran.userid, "0");
                    reponsetrans.SetResult(new JObject { { "status", ipclogtran.status } });
                    reponsetrans.SetCode(Codetypes.Code_Success_Trans);
                    return reponsetrans;
                }
                else
                {
                    ipclogtran.status = (ipclogtran.num26 >= GlobalVariable.ReplyTimes) ? ipclogtran.status : fastStatus;
                    ipclogtran.char46 = string.IsNullOrEmpty(addtlrmtinf) ? ipclogtran.char46 : addtlrmtinf;
                    ipclogtran.num26 = (ipclogtran.num26 == null) ? ipclogtran.num26 = 1 : ipclogtran.num26 += 1;
                    ipclogtran.char30 = (ipclogtran.num26 >= GlobalVariable.ReplyTimes) ? ipclogtran.status : ipclogtran.char30;
                    ipclogtran.errordesc = "Make full fund tranfer fail";
                    _context.Ipclogtrans.Update(ipclogtran);
                    _context.SaveChanges();
                    FASTUtils.InsertLogSubTran(ipclogtran.tranref, ipclogtran.ipctransid, ipclogtran.sourceid, GlobalVariable.FASTMAKEFULLFUN,
                                                                   startTime, "F", ipclogtran.userid, "1", ipclogtran.char46);
                    reponsetrans.SetResult(new JObject { { "status", ipclogtran.status } });
                    reponsetrans.SetCode(new CodeDescription(9997, ipclogtran.char46));
                    return reponsetrans;
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new TransactionReponse(new CodeDescription(9997, ex.Message));
            }
        }

        public async Task<TransactionReponse> FASTGetOutgoingTransactionByPmtInfId(GetOutgoingTransactionByPmtInfIdRequest model)
        {
            TransactionReponse reponsetrans = new TransactionReponse();
            try
            {
                Ipclogtran ipclogtran = _context.Ipclogtrans.SingleOrDefault(x => x.ipctransid == model.ipctransid && x.ipctrancode == GlobalVariable.FAST_OUTGOING);
                if (ipclogtran == null)
                {
                    reponsetrans.SetCode(Codetypes.Err_IPCLogTranNotExist);
                    return reponsetrans;
                }
                DateTime startTime = DateTime.Now;
                FASTGetOutgoingTransactionByPmtInfIdRequest soaprerquest = new FASTGetOutgoingTransactionByPmtInfIdRequest()
                {
                    cm_password = GlobalVariable.CMPassword,
                    cm_user_name = GlobalVariable.CMUsername,
                    payer_participant_code = GlobalVariable.Sender_ParticipantCode,
                    instruction_ref = ipclogtran.tranref
                };
                string res = dbUtils.CreateMessageXML(GlobalVariable.FASTGetOutgoingTranByPmtId, JObject.FromObject(soaprerquest));
                string responseContent = await FASTUtils.CallSOAPServiceAsync(res);
                JObject jObjectReponse = FASTUtils.AnalyzeMessageFAST(responseContent, true);
                string outgoingStatus = jObjectReponse.SelectToken("CstmrCdtTrfInitn.PmtInf.CdtTrfTxInf.RmtInf.Ustrd")?.ToString();
                string outgoingReason = jObjectReponse.SelectToken("CstmrCdtTrfInitn.PmtInf.CdtTrfTxInf.RmtInf.Strd.AddtlRmtInf")?.ToString();
                if (GlobalVariable.FinalFASTStatusFail.Contains(outgoingStatus) || GlobalVariable.FinalFASTStatusSuccessful.Contains(outgoingStatus)) ipclogtran.num21 = 1;
                ipclogtran.char30 = string.IsNullOrEmpty(outgoingStatus) ? ipclogtran.char30 : outgoingStatus;
                ipclogtran.status = string.IsNullOrEmpty(outgoingStatus) ? ipclogtran.status : outgoingStatus;
                ipclogtran.char46 = string.IsNullOrEmpty(outgoingReason) ? ipclogtran.char46 : outgoingReason;
                _context.Update(ipclogtran);
                _context.SaveChanges();
                FASTUtils.InsertLogSubTran(ipclogtran.tranref, ipclogtran.ipctransid, ipclogtran.sourceid, GlobalVariable.FASTGetOutgoingTranByPmtId,
                                                               startTime, "S", ipclogtran.userid, "0", string.IsNullOrEmpty(outgoingReason) ? "" : outgoingReason);
                reponsetrans.SetCode(Codetypes.Code_Success_Trans);
                return reponsetrans;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new TransactionReponse(new CodeDescription(9997, ex.Message));
            }
        }

        public string GetOriginalPaymentInfoId(string tranref, string char40, string char46)
        {
            string reverseInfo = string.IsNullOrEmpty(char40) ? char46 : char40;
            string senderbank = tranref.Split("/")[0];
            string originalInstrId = reverseInfo.Split(" ", StringSplitOptions.RemoveEmptyEntries)[1];
            string returnStr = originalInstrId.Split("/")[0] + "/" + senderbank + "/" + originalInstrId.Split("/")[1];
            return returnStr;
        }

        public async Task<TransactionReponse> FASTGetIncomingTransaction(JObject model)
        {
            TransactionReponse reponsetrans = new TransactionReponse();
            try
            {
                if (GlobalVariable.FASTMode.Equals("AUTO"))
                {
                    return new TransactionReponse(Codetypes.Code_Success);
                }
                string ccy = Utility.GetValueJObjectByKey(model, "ccy");
                FASTGetIncomingTransactionRequest soaprerquest = new FASTGetIncomingTransactionRequest()
                {
                    cm_password = GlobalVariable.CMPassword,
                    cm_user_name = GlobalVariable.CMUsername,
                    payee_participant_code = GlobalVariable.Sender_ParticipantCode,
                    ccy = (string.IsNullOrEmpty(ccy) ? "" : ccy)
                };
                string res = dbUtils.CreateMessageXML(GlobalVariable.FASTGetIncomingTransaction, JObject.FromObject(soaprerquest));
                string responseContent = await FASTUtils.CallSOAPServiceAsync(res);
                JArray arrayReponse = FASTUtils.AnalyzeListMessageFAST(responseContent, true);
                int recordinserted = 0;
                int recordchanged = 0;
                foreach (JObject jObjectReponse in arrayReponse)
                {
                    DateTime startTime = DateTime.Now;
                    string pmtId = jObjectReponse.SelectToken("CstmrCdtTrfInitn.PmtInf.PmtInfId")?.ToString();
                    string purpose = jObjectReponse.SelectToken("CstmrCdtTrfInitn.PmtInf.CdtTrfTxInf.Purp.Cd")?.ToString();
                    if (string.IsNullOrEmpty(pmtId)) continue;                  
                    if (purpose == "REFU")
                    {
                        string char40 = jObjectReponse.SelectToken("CstmrCdtTrfInitn.PmtInf.CdtTrfTxInf.RmtInf.Strd.RfrdDocInf.Nb")?.ToString();
                        string char46 = jObjectReponse.SelectToken("CstmrCdtTrfInitn.PmtInf.CdtTrfTxInf.RmtInf.Strd.AddtlRmtInf")?.ToString();
                        string originalPaymentId = GetOriginalPaymentInfoId(pmtId, char40, char46);
                        var originalLogTran = _context.Ipclogtrans.SingleOrDefault(x => x.tranref == originalPaymentId);
                        if (originalLogTran == null || !string.IsNullOrEmpty(originalLogTran.char49)) continue;
                        JObject jObjectIncomingTran = dbUtils.MappingJSONObject(GlobalVariable.FASTInComingToLog, jObjectReponse);
                        Ipclogtran refundIncomingTran = new Ipclogtran();
                        refundIncomingTran = (Ipclogtran)Utility.SetValueObject(jObjectIncomingTran, refundIncomingTran);
                        FASTUtils.InsertLogSubTran(pmtId, originalLogTran.ipctransid, originalLogTran.sourceid, GlobalVariable.FASTGetIncomingTransaction,
                                                               startTime, "S", originalLogTran.userid, "0");
                        refundIncomingTran.char48 = "ACK" + refundIncomingTran.char44[3..];
                        refundIncomingTran.char31 = FASTStatus.AcknowledgedByFI;
                        JObject isoMessage = dbUtils.MappingJSONObject(GlobalVariable.FASTAcknowledgement, refundIncomingTran);
                        FASTMakeAcknowledgementRequest makeAck = new FASTMakeAcknowledgementRequest()
                        {
                            cm_password = GlobalVariable.CMPassword,
                            cm_user_name = GlobalVariable.CMUsername,
                            content_message = isoMessage
                        };
                        string requestAck = dbUtils.CreateMessageXML(GlobalVariable.FASTAcknowledgement, JObject.FromObject(makeAck));
                        //string responseContentAck = _context.SParams.SingleOrDefault(x => x.parname == "MAKEACKSUCCESS").parval;
                        string responseContentAck = await FASTUtils.CallSOAPServiceAsync(requestAck);
                        JObject jObjectReponseAck = FASTUtils.AnalyzeMessageFAST(responseContentAck, true);
                        string fastStatus = jObjectReponseAck.SelectToken("CstmrPmtStsRpt.OrgnlPmtInfAndSts.TxInfAndSts.TxSts")?.ToString();
                        string addtlrmtinf = jObjectReponseAck.SelectToken("CstmrPmtStsRpt.OrgnlPmtInfAndSts.TxInfAndSts.StsRsnInf.AddtlInf")?.ToString();
                        if (fastStatus == FASTStatus.ReceivedAtACH || fastStatus == FASTStatus.AcknowledgedByFI)
                        {
                            originalLogTran.char46 = string.IsNullOrEmpty(addtlrmtinf) ? originalLogTran.char46 : addtlrmtinf;
                            originalLogTran.errordesc = "Make acknowledgement refund transaction";
                            originalLogTran.status = FASTStatus.RefundByReceiver;
                            originalLogTran.char30 = FASTStatus.RefundByReceiver;
                            originalLogTran.char49 = pmtId;// Save reverse PmtInfId
                            originalLogTran.char50 = refundIncomingTran.char44;// Save reverse MsgId
                            originalLogTran.char51 = refundIncomingTran.char39;// Save reverse CreDtTm
                            originalLogTran.char40 = refundIncomingTran.char40;
                            originalLogTran.char46 = refundIncomingTran.char46;
                            _context.Ipclogtrans.Update(originalLogTran);
                            _context.SaveChanges();
                            FASTUtils.InsertLogSubTran(pmtId, originalLogTran.ipctransid, originalLogTran.sourceid, GlobalVariable.FASTAcknowledgement,
                                                                           startTime, "S", originalLogTran.userid, "0");
                        }
                        else
                        {
                            originalLogTran.char46 = string.IsNullOrEmpty(addtlrmtinf) ? originalLogTran.char46 : addtlrmtinf;
                            originalLogTran.errordesc = "Reject make acknowledgement refund transaction";
                            originalLogTran.status = FASTStatus.RefundByReceiver;
                            originalLogTran.char30 = FASTStatus.RefundByReceiver;
                            originalLogTran.char49 = pmtId;// Save reverse PmtInfId
                            originalLogTran.char50 = refundIncomingTran.char44;// Save reverse MsgId
                            originalLogTran.char51 = refundIncomingTran.char39;// Save reverse CreDtTm
                            originalLogTran.char40 = refundIncomingTran.char40;
                            originalLogTran.char46 = refundIncomingTran.char46;
                            _context.Ipclogtrans.Update(originalLogTran);
                            _context.SaveChanges();
                            FASTUtils.InsertLogSubTran(pmtId, originalLogTran.ipctransid, originalLogTran.sourceid, GlobalVariable.FASTAcknowledgement,
                                                                           startTime, "F", originalLogTran.userid, "1", originalLogTran.char46);
                        }
                        recordchanged++;
                        continue;
                    }
                    var countIPCLogTran = _context.Ipclogtrans.Where(x => x.ipctrancode == GlobalVariable.FAST_INCOMING && x.tranref == pmtId).Count();
                    if (countIPCLogTran != 0) continue;
                    JObject incomingTran = dbUtils.MappingJSONObject(GlobalVariable.FASTInComingToLog, jObjectReponse);
                    Ipclogtran incomingTranLog = new Ipclogtran();
                    incomingTranLog = (Ipclogtran)Utility.SetValueObject(incomingTran, incomingTranLog);
                    incomingTranLog.num26 = 0;
                    incomingTranLog.num30 = 0;
                    incomingTranLog.ipcworkdate = DateTime.Now;
                    incomingTranLog.ipctransdate = DateTime.Now;
                    incomingTranLog.tranref = incomingTranLog.char27;
                    incomingTranLog.ccyid = incomingTranLog.char15;
                    incomingTranLog.sourceid = "FAST";
                    incomingTranLog.destid = "API";
                    incomingTranLog.ipctrancode = GlobalVariable.FAST_INCOMING;
                    incomingTranLog.sourcetranref = "FAST";
                    incomingTranLog.desttranref = incomingTranLog.char27.Split("/")[2];
                    incomingTranLog.userid = "API";
                    incomingTranLog.trandesc = GlobalVariable.FAST_INCOMING;
                    incomingTranLog.status = incomingTranLog.char38;
                    incomingTranLog.char30 = incomingTranLog.char38;
                    incomingTranLog.char31 = incomingTranLog.char38;
                    incomingTranLog.apprsts = 0;
                    incomingTranLog.offlsts = "N";
                    incomingTranLog.deleted = false;
                    incomingTranLog.errorcode = "0";
                    incomingTranLog.errordesc = "Get from FAST successful";
                    incomingTranLog.online = true;
                    incomingTranLog.num27 = 0;
                    incomingTranLog.num28 = 0;
                    _context.Ipclogtrans.Add(incomingTranLog);
                    _context.SaveChanges();
                    recordinserted++;
                    FASTUtils.InsertLogSubTran(incomingTranLog.tranref, incomingTranLog.ipctransid, incomingTranLog.sourceid, GlobalVariable.FASTGetIncomingTransaction,
                                                               startTime, "S", incomingTranLog.userid, "0");
                }
                reponsetrans.SetResult(new JObject { { "recordinserted", recordinserted }, { "recordchanged", recordchanged } });
                reponsetrans.SetCode(Codetypes.Code_Success_Trans);
                return reponsetrans;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new TransactionReponse(new CodeDescription(9997, ex.Message));
            }
        }

        public async Task<TransactionReponse> FASTReverseTransaction(JObject model)
        {
            TransactionReponse reponsetrans = new TransactionReponse();
            try
            {
                string ipctranid = Utility.GetValueJObjectByKey(model, "ipctransid");
                if (string.IsNullOrEmpty(ipctranid))
                {
                    reponsetrans.SetCode(Codetypes.Err_Requireipctranid);
                    return reponsetrans;
                }
                DateTime startTime = DateTime.Now;
                Ipclogtran ipclogtran = _context.Ipclogtrans.SingleOrDefault(x => x.ipctransid == long.Parse(ipctranid) && x.ipctrancode == GlobalVariable.FAST_INCOMING);
                if (ipclogtran == null)
                {
                    reponsetrans.SetCode(Codetypes.Err_IPCLogTranNotExist);
                    return reponsetrans;
                }
                JObject isoMessage = dbUtils.MappingJSONObject(GlobalVariable.FASTReverseTran, ipclogtran);
                FASTReverseTransactionRequest soaprerquest = new FASTReverseTransactionRequest()
                {
                    cm_password = GlobalVariable.CMPassword,
                    cm_user_name = GlobalVariable.CMUsername,
                    content_message = isoMessage
                };
                string res = dbUtils.CreateMessageXML(GlobalVariable.FASTReverseTran, JObject.FromObject(soaprerquest));
                string responseContent = await FASTUtils.CallSOAPServiceAsync(res);
                JObject jObjectReponse = FASTUtils.AnalyzeMessageFAST(responseContent, true);
                string fastStatus = jObjectReponse.SelectToken("CstmrPmtStsRpt.OrgnlPmtInfAndSts.TxInfAndSts.TxSts")?.ToString();
                string addtlrmtinf = jObjectReponse.SelectToken("CstmrPmtStsRpt.OrgnlPmtInfAndSts.TxInfAndSts.StsRsnInf.AddtlInf")?.ToString();
                ipclogtran.char30 = FASTStatus.RefundByReceiver;
                ipclogtran.status = FASTStatus.RefundByReceiver;
                if (fastStatus == FASTStatus.ReceivedAtACH)
                {
                    ipclogtran.num27 = (ipclogtran.num27 == null) ? ipclogtran.num27 = 1 : ipclogtran.num27 += 1;
                    ipclogtran.num19 = 1;
                    ipclogtran.num18 = 1;
                    ipclogtran.char46 = string.IsNullOrEmpty(addtlrmtinf) ? ipclogtran.char46 : addtlrmtinf;
                    ipclogtran.errordesc = "Make reverse successful";
                    _context.Ipclogtrans.Update(ipclogtran);
                    _context.SaveChanges();
                    FASTUtils.InsertLogSubTran(ipclogtran.tranref, ipclogtran.ipctransid, ipclogtran.sourceid, GlobalVariable.FASTReverseTran,
                                                                   startTime, "S", ipclogtran.userid, "0");
                    reponsetrans.SetCode(Codetypes.Code_Success_Trans);
                    return reponsetrans;
                }
                else
                {
                    ipclogtran.num27 = (ipclogtran.num27 == null) ? ipclogtran.num27 = 1 : ipclogtran.num27 += 1;
                    //ipclogtran.num19 = (ipclogtran.num27 >= GlobalVariable.ReplyTimes) ? 1 : 0;
                    ipclogtran.status = (ipclogtran.num27 >= GlobalVariable.ReplyTimes) ? fastStatus : ipclogtran.status;
                    ipclogtran.char30 = (ipclogtran.num27 >= GlobalVariable.ReplyTimes) ? fastStatus : ipclogtran.char30;
                    ipclogtran.char46 = string.IsNullOrEmpty(addtlrmtinf) ? ipclogtran.char46 : addtlrmtinf;
                    ipclogtran.errordesc = "Make reverse fail";
                    _context.Ipclogtrans.Update(ipclogtran);
                    _context.SaveChanges();
                    FASTUtils.InsertLogSubTran(ipclogtran.tranref, ipclogtran.ipctransid, ipclogtran.sourceid, GlobalVariable.FASTReverseTran,
                                                                   startTime, "F", ipclogtran.userid, "1", ipclogtran.char46);
                    reponsetrans.SetCode(new CodeDescription(9997, ipclogtran.char46));
                    return reponsetrans;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new TransactionReponse(new CodeDescription(9997, ex.Message));
            }
        }

        public async Task<TransactionReponse> FASTAcknowledgement(JObject model)
        {
            TransactionReponse reponsetrans = new TransactionReponse();
            try
            {
                string ipctranid = Utility.GetValueJObjectByKey(model, "ipctransid");
                if (string.IsNullOrEmpty(ipctranid))
                {
                    reponsetrans.SetCode(Codetypes.Err_Requireipctranid);
                    return reponsetrans;
                }
                DateTime startTime = DateTime.Now;
                Ipclogtran ipclogtran = _context.Ipclogtrans.SingleOrDefault(x => x.ipctransid == long.Parse(ipctranid) && x.char33 == GlobalVariable.CoreStatusAccept);
                if (ipclogtran == null)
                {
                    reponsetrans.SetCode(Codetypes.Err_IPCLogTranNotExist);
                    return reponsetrans;
                }
                JObject isoMessage = dbUtils.MappingJSONObject(GlobalVariable.FASTAcknowledgement, ipclogtran);
                FASTMakeAcknowledgementRequest soaprerquest = new FASTMakeAcknowledgementRequest()
                {
                    cm_password = GlobalVariable.CMPassword,
                    cm_user_name = GlobalVariable.CMUsername,
                    content_message = isoMessage
                };
                string res = dbUtils.CreateMessageXML(GlobalVariable.FASTAcknowledgement, JObject.FromObject(soaprerquest));
                string responseContent = await FASTUtils.CallSOAPServiceAsync(res);
                JObject jObjectReponse = FASTUtils.AnalyzeMessageFAST(responseContent, true);
                string fastStatus = jObjectReponse.SelectToken("CstmrPmtStsRpt.OrgnlPmtInfAndSts.TxInfAndSts.TxSts")?.ToString();
                string addtlrmtinf = jObjectReponse.SelectToken("CstmrPmtStsRpt.OrgnlPmtInfAndSts.TxInfAndSts.StsRsnInf.AddtlInf")?.ToString();
                ipclogtran.char30 = FASTStatus.AcknowledgedByFI;
                ipclogtran.status = FASTStatus.AcknowledgedByFI;
                if (fastStatus == FASTStatus.ReceivedAtACH || fastStatus == FASTStatus.AcknowledgedByFI)
                {
                    ipclogtran.num28 = (ipclogtran.num28 == null) ? ipclogtran.num28 = 1 : ipclogtran.num28 += 1;
                    ipclogtran.num19 = 1;
                    ipclogtran.num18 = 1;
                    ipclogtran.char46 = string.IsNullOrEmpty(addtlrmtinf) ? ipclogtran.char46 : addtlrmtinf;
                    ipclogtran.errordesc = "Make acknowledgement successful";
                    _context.Ipclogtrans.Update(ipclogtran);
                    _context.SaveChanges();
                    FASTUtils.InsertLogSubTran(ipclogtran.tranref, ipclogtran.ipctransid, ipclogtran.sourceid, GlobalVariable.FASTAcknowledgement,
                                                                   startTime, "S", ipclogtran.userid, "0");
                    reponsetrans.SetCode(Codetypes.Code_Success_Trans);
                    return reponsetrans;
                }
                else
                {
                    ipclogtran.num28 = (ipclogtran.num28 == null) ? ipclogtran.num28 = 1 : ipclogtran.num28 += 1;
                    ipclogtran.status = (ipclogtran.num28 >= GlobalVariable.ReplyTimes) ? fastStatus : ipclogtran.status;
                    ipclogtran.char30 = (ipclogtran.num28 >= GlobalVariable.ReplyTimes) ? fastStatus : ipclogtran.char30;
                    ipclogtran.char46 = string.IsNullOrEmpty(addtlrmtinf) ? ipclogtran.char46 : addtlrmtinf;
                    ipclogtran.errordesc = "Make acknowledgement fail";
                    _context.Ipclogtrans.Update(ipclogtran);
                    _context.SaveChanges();
                    FASTUtils.InsertLogSubTran(ipclogtran.tranref, ipclogtran.ipctransid, ipclogtran.sourceid, GlobalVariable.FASTAcknowledgement,
                                                                   startTime, "F", ipclogtran.userid, "1", ipclogtran.char46);
                    reponsetrans.SetCode(new CodeDescription(9997, ipclogtran.char46));
                    return reponsetrans;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new TransactionReponse(new CodeDescription(9997, ex.Message));
            }
        }
        #endregion

        #region API call to O9Core
        public TransactionReponse SyncFASTOutgoingTransaction(JObject jObject)
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                string sessionid = Utility.GetValueJObjectByKey(jObject, "SESSIONID");
                string mrecord = Utility.GetValueJObjectByKey(jObject, "MRECORD");
                if (string.IsNullOrEmpty(sessionid)) return new TransactionReponse(Codetypes.Err_RequireSessionID);
                PMT_GOL gol = new PMT_GOL
                {
                    MRECORD = (string.IsNullOrEmpty(mrecord)) ? GlobalVariable.MRECORD : int.Parse(mrecord),
                    SESSIONID = sessionid
                };
                var input = _apiService.CallFunction(gol, GlobalVariable.FUNC_PMT_GOL);
                if (input.errorcode != 0) return input;
                DateTime startTime = DateTime.Now;
                string[] pmtref = null;
                string[] sbiccd = null;
                string[] rbiccd = null;
                int rowinsert = 0;
                List<Ipclogtran> ipclogtrans = new List<Ipclogtran>();
                var logdefines = _context.Ipclogdefines.Where(x => x.ipctrancode == "FAST_OUTGOING" && x.logtype == "I").ToList();
                bool isfirstonce = true;
                foreach (KeyValuePair<string, JToken> keyValuePair in JObject.FromObject(input.result))
                {
                    string key = keyValuePair.Key;
                    if (key == "PMTREF") pmtref = keyValuePair.Value.Children().Select(v => v.Value<string>()).ToArray();
                    if (key == "SBICCD") sbiccd = keyValuePair.Value.Children().Select(v => v.Value<string>()).ToArray();
                    if (key == "RBICCD") rbiccd = keyValuePair.Value.Children().Select(v => v.Value<string>()).ToArray();
                    var logdefine = logdefines.SingleOrDefault(x => x.fieldname == key);
                    if (logdefine == null)
                    {
                        continue;
                    }
                    string propertyname = "";
                    if (logdefine.parmtype == "C")
                    {
                        propertyname = "char" + ((logdefine.pos < 10) ? ("0" + logdefine.pos) : logdefine.pos.ToString());
                        var listvalue = keyValuePair.Value.Children().Select(v => v.Value<string>()).ToArray();
                        if (listvalue.Length > 0)
                        {
                            for (int i = 0; i < listvalue.Length; i++)
                            {
                                if (isfirstonce)
                                {
                                    ipclogtrans.Add(new Ipclogtran());
                                }
                                PropertyInfo prop = ipclogtrans[i].GetType().GetProperty(propertyname, BindingFlags.Public | BindingFlags.Instance);
                                if (null != prop && prop.CanWrite)
                                {
                                    prop.SetValue(ipclogtrans[i], listvalue[i], null);
                                }
                            }
                        }
                    }
                    else if (logdefine.parmtype == "N")
                    {
                        propertyname = "num" + ((logdefine.pos < 10) ? ("0" + logdefine.pos) : logdefine.pos.ToString());
                        var listvalue = keyValuePair.Value.Children().Select(v => v.Value<decimal?>()).ToArray();
                        if (listvalue.Length > 0)
                        {
                            for (int i = 0; i < listvalue.Length; i++)
                            {
                                if (isfirstonce)
                                {
                                    ipclogtrans.Add(new Ipclogtran());
                                }
                                PropertyInfo prop = ipclogtrans[i].GetType().GetProperty(propertyname, BindingFlags.Public | BindingFlags.Instance);
                                if (null != prop && prop.CanWrite)
                                {
                                    prop.SetValue(ipclogtrans[i], listvalue[i], null);
                                }
                            }
                        }
                    }
                    isfirstonce = false;
                }
                if (ipclogtrans.Count > 0)
                {
                    for (var i = 0; i < ipclogtrans.Count; i++)
                    {
                        var ipclogtran = ipclogtrans[i];
                        ipclogtran.num19 = 0;
                        ipclogtran.num20 = 0;
                        ipclogtran.num21 = 0;
                        ipclogtran.num18 = 0;
                        ipclogtran.num26 = 0;
                        ipclogtran.ipcworkdate = DateTime.Now;
                        ipclogtran.ipctransdate = DateTime.Now;
                        ipclogtran.tranref = ipclogtran.char27;
                        ipclogtran.senderbank = sbiccd[i];
                        ipclogtran.receiverbank = rbiccd[i];
                        ipclogtran.ccyid = ipclogtran.char25;
                        ipclogtran.sourceid = "O9";
                        ipclogtran.destid = "API";
                        ipclogtran.ipctrancode = GlobalVariable.FAST_OUTGOING;
                        ipclogtran.sourcetranref = "O9CORE";
                        ipclogtran.desttranref = ipclogtran.char27.Split("/")[2];
                        ipclogtran.char40 = pmtref[i];
                        ipclogtran.userid = "API";
                        ipclogtran.trandesc = GlobalVariable.FAST_OUTGOING;
                        ipclogtran.status = ipclogtran.char30;
                        ipclogtran.apprsts = 0;
                        ipclogtran.offlsts = "N";
                        ipclogtran.deleted = false;
                        ipclogtran.errorcode = "0";
                        ipclogtran.errordesc = "Sync From O9Core Successful";
                        ipclogtran.online = true;
                        _context.Ipclogtrans.Add(ipclogtran);
                        _context.SaveChanges();
                        rowinsert++;
                        FASTUtils.InsertLogSubTran(ipclogtran.tranref, ipclogtran.ipctransid, ipclogtran.sourceid, GlobalVariable.GetOutgoingTransFromO9,
                                                                  startTime, "S", ipclogtran.userid, "0");
                    }
                }
                reponse.SetCode(Codetypes.Code_Success);
                reponse.SetResult(new JObject { { "recordinserted", rowinsert } });
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new TransactionReponse(new CodeDescription(9997, ex.Message));
            }
        }

        public TransactionReponse SyncFASTIncomingTransactionToO9Core(JObject jObject)
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                string ipctransid = Utility.GetValueJObjectByKey(jObject, "ipctransid");
                if (string.IsNullOrEmpty(ipctransid))
                {
                    reponse.SetCode(Codetypes.Err_RequireIpctransid);
                    return reponse;
                }
                DateTime startTime = DateTime.Now;
                var ipclogtran = _context.Ipclogtrans.SingleOrDefault(x => x.ipctransid == long.Parse(ipctransid) && x.num21 == 0);
                if (ipclogtran == null)
                {
                    reponse.SetCode(Codetypes.Err_IPCLogTranNotExistOrSynchronized);
                    return reponse;
                }
                JObject fastTranO9 = dbUtils.MappingJSONObject(GlobalVariable.FASTInComingToO9, ipclogtran);
                fastTranO9.Add("SESSIONID", GlobalVariable.O9CoreUser.ssesionid);
                var reponseFromO9 = FASTUtils.CallFunction(fastTranO9, GlobalVariable.PMT_FAST_INCOMING);
                if (reponseFromO9.errorcode != 0)
                {
                    FASTUtils.InsertLogSubTran(ipclogtran.tranref, ipclogtran.ipctransid, ipclogtran.sourceid, GlobalVariable.FASTInComingToO9,
                                                           startTime, "F", ipclogtran.userid, reponseFromO9.errorcode.ToString(), reponseFromO9.messagedetail);
                    return reponseFromO9;
                }
                JObject resultObject = JObject.FromObject(reponseFromO9.result);
                JObject updateFields = (JObject)resultObject.SelectToken("DATA");
                string corests = updateFields.SelectToken("CORESTS").ToString();
                string ramt = updateFields.SelectToken("RAMT").ToString();
                string rfreason = updateFields.SelectToken("RFREASON").ToString();
                string msgcode = updateFields.SelectToken("MSGCODE").ToString();
                string txrefid = updateFields.SelectToken("TXREFID").ToString();
                string rccr = updateFields.SelectToken("RCCR").ToString();
                string msgid = updateFields.SelectToken("MSGID").ToString();
                string fcnsts = updateFields.SelectToken("FCNSTS")?.ToString();
                if (resultObject["ERRORCODE"].ToString() == "0")
                {
                    ipclogtran.num29 = (ipclogtran.num29 == null) ? ipclogtran.num29 = 1 : ipclogtran.num29 += 1;
                    ipclogtran.char48 = msgid;
                    ipclogtran.char47 = string.IsNullOrEmpty(rfreason) ? ipclogtran.char47 : rfreason;
                    ipclogtran.char28 = string.IsNullOrEmpty(txrefid) ? ipclogtran.char28 : txrefid;
                    ipclogtran.char33 = corests;
                    ipclogtran.char34 = corests;
                    ipclogtran.char45 = string.IsNullOrEmpty(msgcode) ? ipclogtran.char45 : msgcode;
                    ipclogtran.char25 = string.IsNullOrEmpty(rccr) ? ipclogtran.char25 : rccr;
                    ipclogtran.num02 = string.IsNullOrEmpty(ramt) ? ipclogtran.num02 : decimal.Parse(ramt);
                    ipclogtran.status = string.IsNullOrEmpty(fcnsts) ? ((corests == GlobalVariable.CoreStatusAccept) ? FASTStatus.AcknowledgedByFI : FASTStatus.RefundedByReceiver) : fcnsts;
                    ipclogtran.char31 = ipclogtran.status;
                    ipclogtran.char30 = ipclogtran.status;
                    ipclogtran.num21 = 1;
                    ipclogtran.num20 = 1;
                    ipclogtran.errordesc = "Sync transaction to O9Core Successful";
                    _context.Ipclogtrans.Update(ipclogtran);
                    _context.SaveChanges();
                    FASTUtils.InsertLogSubTran(ipclogtran.tranref, ipclogtran.ipctransid, ipclogtran.sourceid, GlobalVariable.FASTInComingToO9,
                                                               startTime, "S", ipclogtran.userid, reponseFromO9.errorcode.ToString());
                    reponse.SetCode(Codetypes.Code_Success);
                    return reponse;
                }
                else
                {
                    ipclogtran.num29 = (ipclogtran.num29 == null) ? ipclogtran.num29 = 1 : ipclogtran.num29 += 1;
                    ipclogtran.num21 = 0;
                    ipclogtran.num20 = 0;
                    ipclogtran.errordesc = "O9Core reject transaction";
                    if (ipclogtran.num29 >= GlobalVariable.ReplyTimes)
                    {
                        ipclogtran.char48 = msgid;
                        ipclogtran.char47 = string.IsNullOrEmpty(rfreason) ? ipclogtran.char47 : rfreason;
                        ipclogtran.char28 = string.IsNullOrEmpty(txrefid) ? ipclogtran.char28 : txrefid;
                        ipclogtran.char33 = corests;
                        ipclogtran.char34 = corests;
                        ipclogtran.char45 = string.IsNullOrEmpty(msgcode) ? ipclogtran.char45 : msgcode;
                        ipclogtran.char25 = string.IsNullOrEmpty(rccr) ? ipclogtran.char25 : rccr;
                        ipclogtran.num02 = string.IsNullOrEmpty(ramt) ? ipclogtran.num02 : decimal.Parse(ramt);
                        ipclogtran.status = string.IsNullOrEmpty(fcnsts) ? ((corests == GlobalVariable.CoreStatusAccept) ? FASTStatus.AcknowledgedByFI : FASTStatus.RefundedByReceiver) : fcnsts;
                        ipclogtran.char31 = ipclogtran.status;
                        ipclogtran.char30 = ipclogtran.status;
                        ipclogtran.num21 = 1;
                        ipclogtran.num20 = 1;
                    }
                    _context.Ipclogtrans.Update(ipclogtran);
                    _context.SaveChanges();
                    FASTUtils.InsertLogSubTran(ipclogtran.tranref, ipclogtran.ipctransid, ipclogtran.sourceid, GlobalVariable.FASTInComingToO9,
                                                            startTime, "F", ipclogtran.userid, resultObject["ERRORCODE"].ToString(), resultObject["ERRORDESC"].ToString());
                    reponse.SetCode(new CodeDescription(int.Parse(resultObject["ERRORCODE"].ToString()), resultObject["ERRORDESC"].ToString()));
                    return reponse;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new TransactionReponse(new CodeDescription(9997, ex.Message));
            }
        }

        public TransactionReponse SyncTransactionStatusToO9Core(JObject model)
        {
            TransactionReponse reponsetrans = new TransactionReponse();
            try
            {
                string ipctranid = Utility.GetValueJObjectByKey(model, "ipctransid");
                if (string.IsNullOrEmpty(ipctranid))
                {
                    reponsetrans.SetCode(Codetypes.Err_Requireipctranid);
                    return reponsetrans;
                }
                DateTime startTime = DateTime.Now;
                Ipclogtran ipclogtran = _context.Ipclogtrans.SingleOrDefault(x => x.ipctransid == long.Parse(ipctranid) && (x.num19 == 1 || (x.num19 == 0 && x.num26 > 0)));
                if (ipclogtran == null)
                {
                    reponsetrans.SetCode(Codetypes.Err_IPCLogTranNotExist);
                    return reponsetrans;
                }
                PMT_SYNC_STATUS_FAST pMT_SYNC_STATUS_FAST = new PMT_SYNC_STATUS_FAST
                {
                    MSGCODE = ipclogtran.char45,
                    FCNSTS = ipclogtran.status,
                    SESSIONID = GlobalVariable.O9CoreUser.ssesionid
                };
                if (!string.IsNullOrEmpty(ipclogtran.char46)) pMT_SYNC_STATUS_FAST.RFREASONMSG = ipclogtran.char46;
                var reponseCore = FASTUtils.CallFunction(JObject.FromObject(pMT_SYNC_STATUS_FAST), GlobalVariable.PMT_SYNC_STATUS_FAST);
                ipclogtran.num30 = (ipclogtran.num30 == null) ? 0 : ipclogtran.num30;
                if (reponseCore.errorcode != 0)
                {
                    ipclogtran.num30++;
                    ipclogtran.errordesc = "Sync status to O9Core Fail";
                    _context.Ipclogtrans.Update(ipclogtran);
                    _context.SaveChanges();
                    if (new string[] { FASTStatus.FailedAtFI, FASTStatus.FailedAtACH, FASTStatus.RefundByReceiver, FASTStatus.RefundedByReceiver, FASTStatus.Rejected }.Contains(ipclogtran.status))
                    {
                        FASTUtils.InsertLogSubTran(ipclogtran.tranref, ipclogtran.ipctransid, ipclogtran.sourceid, GlobalVariable.ReverseTransactionToO9,
                                                                   startTime, "F", ipclogtran.userid, reponseCore.errorcode.ToString(), reponseCore.messagedetail);
                    }
                    else
                    {
                        FASTUtils.InsertLogSubTran(ipclogtran.tranref, ipclogtran.ipctransid, ipclogtran.sourceid, GlobalVariable.SyncTransactionStatusToO9,
                                                               startTime, "F", ipclogtran.userid, reponseCore.errorcode.ToString(), reponseCore.messagedetail);
                    }
                    return reponseCore;
                }
                JObject resultObject = JObject.FromObject(reponseCore.result);
                if (resultObject["ERRORCODE"].ToString() == "0")
                {
                    ipclogtran.num30++;
                    ipclogtran.num19 = 1;
                    ipclogtran.char31 = ipclogtran.status;
                    ipclogtran.errordesc = "Sync status to O9Core Successful";
                    _context.Ipclogtrans.Update(ipclogtran);
                    _context.SaveChanges();
                    if (new string[] { FASTStatus.FailedAtFI, FASTStatus.FailedAtACH, FASTStatus.RefundByReceiver, FASTStatus.RefundedByReceiver, FASTStatus.Rejected }.Contains(ipclogtran.status))
                    {
                        FASTUtils.InsertLogSubTran(ipclogtran.tranref, ipclogtran.ipctransid, ipclogtran.sourceid, GlobalVariable.ReverseTransactionToO9,
                                                                   startTime, "S", ipclogtran.userid, "0");
                    }
                    else
                    {
                        FASTUtils.InsertLogSubTran(ipclogtran.tranref, ipclogtran.ipctransid, ipclogtran.sourceid, GlobalVariable.SyncTransactionStatusToO9,
                                                                   startTime, "S", ipclogtran.userid, "0");
                    }
                    reponsetrans.SetCode(Codetypes.Code_Success);
                    return reponsetrans;
                }
                else
                {
                    ipclogtran.num30++;
                    ipclogtran.errordesc = "Sync status to O9Core Fail";
                    _context.Ipclogtrans.Update(ipclogtran);
                    _context.SaveChanges();
                    if (new string[] { FASTStatus.FailedAtFI, FASTStatus.FailedAtACH, FASTStatus.RefundByReceiver, FASTStatus.RefundedByReceiver, FASTStatus.Rejected }.Contains(ipclogtran.status))
                    {
                        FASTUtils.InsertLogSubTran(ipclogtran.tranref, ipclogtran.ipctransid, ipclogtran.sourceid, GlobalVariable.ReverseTransactionToO9,
                                                                   startTime, "F", ipclogtran.userid, resultObject["ERRORCODE"].ToString(), resultObject["ERRORDESC"].ToString());
                    }
                    else
                    {
                        FASTUtils.InsertLogSubTran(ipclogtran.tranref, ipclogtran.ipctransid, ipclogtran.sourceid, GlobalVariable.SyncTransactionStatusToO9,
                                                            startTime, "F", ipclogtran.userid, resultObject["ERRORCODE"].ToString(), resultObject["ERRORDESC"].ToString());
                    }
                    reponsetrans.SetCode(new CodeDescription(int.Parse(resultObject["ERRORCODE"].ToString()), resultObject["ERRORDESC"].ToString()));
                    return reponsetrans;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new TransactionReponse(new CodeDescription(9997, ex.Message));
            }
        }
        #endregion

    }
}

