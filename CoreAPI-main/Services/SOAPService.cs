using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using SoapCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Xml;
using WebApi.Entities;
using WebApi.Helpers.Common;
using WebApi.Helpers.DatabaseUtils;
using WebApi.Helpers.Utils;
using WebApi.Models.FAST;
using WebApi.Models.FASTPublic;

namespace WebApi.Services
{
    [ServiceContract(Namespace = "http://fastwebservice.nbc.org.kh/")]
    public interface ISOAPService
    {
        [CustomOperationContract(ResponsePrefix = "ns3", IsLoadParentNS = false)]
        XmlDocument makeAcknowledgment(string cm_user_name, string cm_password, string content_message, string import_file_name);

        [CustomOperationContract(ResponsePrefix = "ns3", IsLoadParentNS = false)]
        XmlDocument makeFullFundTransfer(string cm_user_name, string cm_password, string iso_message, string import_file_name);
    }

    public class SOAPService : ISOAPService
    {
        public readonly ILogger<SOAPService> _logger;
        private readonly DataContext _context;
        private readonly DbUtils dbUtils;
        private const string ErrorAuth = "INVALID AUTHENTICATION!";
        private const string WrongFormatXML = "ISO Message is wrong format xml.";
        private const string ISOMessageIsEmpty = "ISO Message is empty.";
        public SOAPService(ILogger<SOAPService> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
            dbUtils = new DbUtils(_context);
        }

        public bool CheckUserFAST(string username, string password)
        {
            var user = _context.SUserpublics.SingleOrDefault(x => x.username == username && x.password == O9Encrypt.MD5Encrypt(password));
            if (user == null) return false;
            else return true;
        }

        public XmlDocument makeAcknowledgment(string cm_user_name, string cm_password, string content_message, string import_file_name)
        {
            try
            {
                DateTime startTime = DateTime.Now;
                string errorDesc = "";
                bool checkUser = CheckUserFAST(cm_user_name, cm_password);
                if (!checkUser)
                {
                    return SOAPReponse.GetError(ErrorAuth);
                }
                if (string.IsNullOrEmpty(content_message))
                {
                    return SOAPReponse.GetError(ISOMessageIsEmpty);
                }
                if (!XmlUtils.IsValidXml(content_message.Trim()))
                {
                    return SOAPReponse.GetError(WrongFormatXML);
                }
                XmlDocument docRequest = new XmlDocument();
                docRequest.LoadXml(content_message.Trim());
                docRequest = XmlUtils.RemoveXmlDeclaration(docRequest);
                JObject isoMessage = XmlUtils.ConvertXMLToJSON(docRequest, true);
                string pmtId = isoMessage.SelectToken("CstmrPmtStsRpt.OrgnlPmtInfAndSts.OrgnlPmtInfId")?.ToString();
                if (string.IsNullOrEmpty(pmtId))
                {
                    errorDesc = "REJECTED AT CORE DUE TO: [ISO message contained wrong format.]";
                }
                JObject tranInfor = dbUtils.MappingJSONObject(GlobalVariable.ReceiveMakeAcknowledgment, isoMessage);
                string messageid = tranInfor.SelectToken("tranref").ToString();
                string senderbank = tranInfor.SelectToken("senderbank").ToString();
                string receiverbank = tranInfor.SelectToken("receiverbank").ToString();
                string paymentid = tranInfor.SelectToken("char27").ToString();
                string endtoendid = tranInfor.SelectToken("char43").ToString();
                string status = tranInfor.SelectToken("status").ToString();
                string amount = tranInfor.SelectToken("num01").ToString();
                string currency = tranInfor.SelectToken("char15").ToString();
                string sendername = tranInfor.SelectToken("char02").ToString();
                string receivername = tranInfor.SelectToken("char17").ToString();
                string originmessageid = tranInfor.SelectToken("char44").ToString();
                string origindate = tranInfor.SelectToken("char42").ToString();
                string messageType = originmessageid[..3];               
                Ipclogtran incomingTranLog = _context.Ipclogtrans.SingleOrDefault(x => x.tranref == pmtId && x.ipctrancode == GlobalVariable.FAST_OUTGOING && x.status == FASTStatus.ReceivedAtACH);
                if (incomingTranLog == null)
                {
                    if (messageType == "REF")
                    {
                        FASTGetOutgoingTransactionByPmtInfIdRequest soaprerquest = new FASTGetOutgoingTransactionByPmtInfIdRequest()
                        {
                            cm_password = GlobalVariable.CMPassword,
                            cm_user_name = GlobalVariable.CMUsername,
                            payer_participant_code = GlobalVariable.Sender_ParticipantCode,
                            instruction_ref = pmtId
                        };
                        string res = dbUtils.CreateMessageXML(GlobalVariable.FASTGetOutgoingTranByPmtId, JObject.FromObject(soaprerquest));
                        try
                        {
                            string responseContent = FASTUtils.CallSOAPServiceAsync(res).Result;
                            JObject jObjectReponse = FASTUtils.AnalyzeMessageFAST(responseContent, true);
                            string purpose = jObjectReponse.SelectToken("CstmrCdtTrfInitn.PmtInf.CdtTrfTxInf.Purp.Cd")?.ToString();
                            if (purpose == "REFU")
                            {
                                string outgoingStatus = jObjectReponse.SelectToken("CstmrCdtTrfInitn.PmtInf.CdtTrfTxInf.RmtInf.Ustrd")?.ToString();
                                string outgoingReason = jObjectReponse.SelectToken("CstmrCdtTrfInitn.PmtInf.CdtTrfTxInf.RmtInf.Strd.AddtlRmtInf")?.ToString();
                                string refNb = jObjectReponse.SelectToken("CstmrCdtTrfInitn.PmtInf.CdtTrfTxInf.RmtInf.Strd.RfrdDocInf.Nb")?.ToString();
                                string originalDate = Utility.ConvertDateTimeToStringDatetime(DateTime.Parse(jObjectReponse.SelectToken("CstmrCdtTrfInitn.PmtInf.ReqdExctnDt").ToString()));
                                string originalPmtInfId = GetOriginalPaymentInfoId(pmtId, refNb, outgoingReason);
                                Ipclogtran originalLogTran = _context.Ipclogtrans.SingleOrDefault(x => x.tranref == originalPmtInfId && x.ipctrancode == GlobalVariable.FAST_INCOMING);
                                originalLogTran.errordesc = "Received from NBC SOAP make acknowledgment";
                                originalLogTran.status = FASTStatus.RefundByReceiver;
                                originalLogTran.char30 = FASTStatus.RefundByReceiver;
                                originalLogTran.char49 = pmtId;// Save reverse transaction PmtInfId
                                originalLogTran.char50 = originmessageid;// Save reverse transaction MsgId
                                originalLogTran.char51 = originalDate;// Save reverse transaction CreDtTm
                                originalLogTran.char52 = outgoingStatus;// Save reverse transaction status
                                originalLogTran.char40 = refNb;
                                originalLogTran.char46 = outgoingReason;
                                _context.Ipclogtrans.Update(originalLogTran);
                                _context.SaveChanges();
                                FASTUtils.InsertLogSubTran(pmtId, originalLogTran.ipctransid, "NBC", GlobalVariable.ReceiveMakeAcknowledgment,
                                                                   startTime, "S", originalLogTran.userid, "0");
                                tranInfor["tranref"] = "REP" + messageid[3..];
                                string xmlReponse = dbUtils.CreateMessageXML(GlobalVariable.ReplyMessageSuccessful, tranInfor);
                                XmlDocument doc = new XmlDocument();
                                doc.LoadXml(xmlReponse);
                                return SOAPReponse.GetResult(doc.Beautify());
                            }
                            else
                            {
                                errorDesc = string.Format("REJECTED AT CORE DUE TO: [This original payment info id [{0}] not found.]", pmtId);
                            }
                        }
                        catch(Exception ex)
                        {
                            if (ex is FormatException || ex is ArgumentNullException)
                            {
                                errorDesc = ex.Message;
                            }
                            else
                            {
                                errorDesc = string.Format("REJECTED AT CORE DUE TO: [This original payment info id [{0}] not found.]", pmtId);
                            }
                        }
                    }
                    else
                    {
                        errorDesc = string.Format("REJECTED AT CORE DUE TO: [This original payment info id [{0}] not found.]", pmtId);
                    }
                }
                tranInfor.Add("char46", errorDesc);
                tranInfor["tranref"] = "REP" + messageid[3..];
                if (string.IsNullOrEmpty(errorDesc))
                {
                    incomingTranLog.status = status;
                    incomingTranLog.errordesc = "Received from NBC SOAP make acknowledgment";
                    _context.Ipclogtrans.Update(incomingTranLog);
                    _context.SaveChanges();
                    FASTUtils.InsertLogSubTran(incomingTranLog.tranref, incomingTranLog.ipctransid, "NBC", GlobalVariable.ReceiveMakeAcknowledgment,
                                                               startTime, "S", incomingTranLog.userid, "0");
                    string xmlReponse = dbUtils.CreateMessageXML(GlobalVariable.ReplyMessageSuccessful, tranInfor);
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlReponse);
                    return SOAPReponse.GetResult(doc.Beautify());
                }
                else
                {
                    if (incomingTranLog != null)
                    {
                        //không update status
                        incomingTranLog.errordesc = "Reject make acknowledgment NBC SOAP";
                        _context.Ipclogtrans.Update(incomingTranLog);
                        _context.SaveChanges();
                        FASTUtils.InsertLogSubTran(incomingTranLog.tranref, incomingTranLog.ipctransid, "NBC", GlobalVariable.ReceiveMakeAcknowledgment,
                                                                   startTime, "F", incomingTranLog.userid, "0", errorDesc);
                    }
                    string xmlReponse = dbUtils.CreateMessageXML(GlobalVariable.ReplyMessageFail, tranInfor);
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlReponse);
                    return SOAPReponse.GetResult(doc.Beautify());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return SOAPReponse.GetError(ex.Message);
            }
        }

        public XmlDocument makeFullFundTransfer(string cm_user_name, string cm_password, string iso_message, string import_file_name)
        {
            try
            {
                bool checkUser = CheckUserFAST(cm_user_name, cm_password);
                if (!checkUser)
                {
                    return SOAPReponse.GetError(ErrorAuth);
                }
                if (string.IsNullOrEmpty(iso_message))
                {
                    return SOAPReponse.GetError(ISOMessageIsEmpty);
                }
                if (!XmlUtils.IsValidXml(iso_message.Trim()))
                {
                    return SOAPReponse.GetError(WrongFormatXML);
                }
                XmlDocument docRequest = new XmlDocument();
                docRequest.LoadXml(iso_message.Trim());
                docRequest = XmlUtils.RemoveXmlDeclaration(docRequest);
                JObject isoMessage = XmlUtils.ConvertXMLToJSON(docRequest, true);
                string purpose = isoMessage.SelectToken("CstmrCdtTrfInitn.PmtInf.CdtTrfTxInf.Purp.Cd")?.ToString();
                if (purpose == "REFU")
                {
                    XmlDocument reponseXml = MakeReverseProcess(isoMessage);
                    return reponseXml;
                }
                else
                {
                    XmlDocument reponseXml = MakeFullFundProcess(isoMessage);
                    return reponseXml;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return SOAPReponse.GetError(ex.Message);
            }
        }

        public XmlDocument MakeFullFundProcess(JObject isoMessage)
        {
            DateTime startTime = DateTime.Now;
            string errorDesc = "";
            string pmtId = isoMessage.SelectToken("CstmrCdtTrfInitn.PmtInf.PmtInfId")?.ToString();
            if (string.IsNullOrEmpty(pmtId))
            {
                errorDesc = "REJECTED AT CORE DUE TO: [ISO message contained wrong format.]";
            }
            JObject incomingTran = dbUtils.MappingJSONObject(GlobalVariable.ReceiveMakeFullFund, isoMessage);
            Ipclogtran incomingTranLog = new Ipclogtran();
            incomingTranLog = (Ipclogtran)Utility.SetValueObject(incomingTran, incomingTranLog);
            incomingTranLog.char30 = FASTStatus.Received;
            incomingTranLog.char31 = FASTStatus.Received;
            incomingTranLog.status = FASTStatus.Received;
            incomingTranLog.num27 = 0;
            incomingTranLog.num28 = 0;
            incomingTranLog.num18 = 0;
            incomingTranLog.num19 = 0;
            incomingTranLog.num20 = 0;
            incomingTranLog.num21 = 0;
            incomingTranLog.ipcworkdate = DateTime.Now;
            incomingTranLog.ipctransdate = DateTime.Now;
            incomingTranLog.tranref = incomingTranLog.char27;
            incomingTranLog.ccyid = incomingTranLog.char15;
            incomingTranLog.sourceid = "NBC";
            incomingTranLog.destid = "API";
            incomingTranLog.ipctrancode = GlobalVariable.FAST_INCOMING;
            incomingTranLog.sourcetranref = "NBC";
            incomingTranLog.desttranref = incomingTranLog.char27.Split("/")[2];
            incomingTranLog.userid = "API";
            incomingTranLog.trandesc = GlobalVariable.FAST_INCOMING;
            incomingTranLog.apprsts = 0;
            incomingTranLog.offlsts = "N";
            incomingTranLog.deleted = false;
            incomingTranLog.errorcode = "0";
            incomingTranLog.errordesc = "Received from NBC SOAP make full fund";
            incomingTranLog.online = true;
            Dictionary<string, object> paramCheckMessage = new Dictionary<string, object>
                {
                    { "PAYMENTID", pmtId },
                    { "SENDERBANK", incomingTranLog.senderbank },
                    { "CURRENCY", incomingTranLog.char15 }
                };
            string checkMessage = DbFactory.GetVariableFromStoredProcedure("CheckMessageMakeFullFund", paramCheckMessage);
            switch (checkMessage)
            {
                case "1":
                    errorDesc = string.Format("REJECTED AT CORE DUE TO: [This original payment info id [{0}] is already existed.]", pmtId);
                    break;
                case "2":
                    errorDesc = string.Format("REJECTED AT CORE DUE TO: [Sender bank [{0}] not found.]", incomingTranLog.senderbank);
                    break;
                case "3":
                    errorDesc = string.Format("REJECTED AT CORE DUE TO: [Invalid currency [{0}]]", incomingTranLog.char15);
                    break;
                default:
                    break;
            }
            if (string.IsNullOrEmpty(errorDesc))
            {
                string CtrlSum = isoMessage.SelectToken("CstmrCdtTrfInitn.GrpHdr.CtrlSum").ToString();
                if (decimal.Parse(CtrlSum) != incomingTranLog.num01)
                    errorDesc = string.Format("REJECTED AT CORE DUE TO: [Invalid CtrlSum [{0}]]", CtrlSum);
                if (incomingTranLog.char15 == "KHR" && (incomingTranLog.num01 % 1) != 0)
                    errorDesc = "REJECTED AT CORE DUE TO: [Invalid amount.]";
            }
            if (string.IsNullOrEmpty(errorDesc))
            {
                _context.Ipclogtrans.Add(incomingTranLog);
                _context.SaveChanges();
                FASTUtils.InsertLogSubTran(incomingTranLog.tranref, incomingTranLog.ipctransid, "NBC", GlobalVariable.ReceiveMakeFullFund,
                                                           startTime, "S", incomingTranLog.userid, "0");
                incomingTranLog.tranref = "REP" + incomingTranLog.char44[3..];
                incomingTranLog.senderbank = DbFactory.GetVariableFromSQL("SELECT bicfi FROM FAST_BANK WHERE BANKID= '" + incomingTranLog.senderbank + "'");
                incomingTranLog.receiverbank = DbFactory.GetVariableFromSQL("SELECT bicfi FROM FAST_BANK WHERE BANKID= '" + incomingTranLog.receiverbank + "'");
                string xmlReponse = dbUtils.CreateMessageXML(GlobalVariable.ReplyMessageSuccessful, JObject.FromObject(incomingTranLog));
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlReponse);
                return SOAPReponse.GetResult(doc.Beautify());
            }
            else
            {
                incomingTranLog.tranref = "REP" + incomingTranLog.char44[3..];
                incomingTranLog.char46 = errorDesc;
                incomingTranLog.senderbank = DbFactory.GetVariableFromSQL("SELECT bicfi FROM FAST_BANK WHERE BANKID= '" + incomingTranLog.senderbank + "'");
                incomingTranLog.receiverbank = DbFactory.GetVariableFromSQL("SELECT bicfi FROM FAST_BANK WHERE BANKID= '" + incomingTranLog.receiverbank + "'");
                string xmlReponse = dbUtils.CreateMessageXML(GlobalVariable.ReplyMessageFail, JObject.FromObject(incomingTranLog));
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlReponse);
                return SOAPReponse.GetResult(doc.Beautify());
            }
        }

        public XmlDocument MakeReverseProcess(JObject isoMessage)
        {
            DateTime startTime = DateTime.Now;
            string errorDesc = "";
            string pmtId = isoMessage.SelectToken("CstmrCdtTrfInitn.PmtInf.PmtInfId")?.ToString();
            if (string.IsNullOrEmpty(pmtId))
            {
                errorDesc = "REJECTED AT CORE DUE TO: [ISO message contained wrong format.]";
            }
            JObject incomingTran = dbUtils.MappingJSONObject(GlobalVariable.MakeReverseIncoming, isoMessage);
            Ipclogtran incomingTranLog = new Ipclogtran();
            incomingTranLog = (Ipclogtran)Utility.SetValueObject(incomingTran, incomingTranLog);
            incomingTranLog.senderbank = DbFactory.GetVariableFromSQL("SELECT bicfi FROM FAST_BANK WHERE participatecode= '" + incomingTranLog.char27.Split("/")[0] + "'");
            incomingTranLog.receiverbank = DbFactory.GetVariableFromSQL("SELECT bicfi FROM FAST_BANK WHERE participatecode= '" + incomingTranLog.char27.Split("/")[1] + "'");
            incomingTranLog.ipcworkdate = DateTime.Now;
            incomingTranLog.ipctransdate = DateTime.Now;
            incomingTranLog.tranref = incomingTranLog.char27;
            incomingTranLog.ipctrancode = GlobalVariable.FAST_INCOMING;
            incomingTranLog.desttranref = incomingTranLog.char27.Split("/")[2];
            incomingTranLog.trandesc = GlobalVariable.FAST_INCOMING;
            string originalPmtInfId = GetOriginalPaymentInfoId(incomingTranLog.tranref, incomingTranLog.char40, incomingTranLog.char46);
            Ipclogtran originalLogTran = _context.Ipclogtrans.SingleOrDefault(x => x.tranref == originalPmtInfId && x.ipctrancode == GlobalVariable.FAST_OUTGOING);
            if (originalLogTran == null)
            {
                errorDesc = string.Format("REJECTED AT CORE DUE TO: [This original payment info id [{0}] is already existed.]", originalPmtInfId);
            }
            if (string.IsNullOrEmpty(errorDesc))
            {
                string CtrlSum = isoMessage.SelectToken("CstmrCdtTrfInitn.GrpHdr.CtrlSum").ToString();
                if (decimal.Parse(CtrlSum) != incomingTranLog.num01)
                    errorDesc = string.Format("REJECTED AT CORE DUE TO: [Invalid CtrlSum [{0}]]", CtrlSum);
                if (incomingTranLog.char15 == "KHR" && (incomingTranLog.num01 % 1) != 0)
                    errorDesc = "REJECTED AT CORE DUE TO: [Invalid amount.]";
                if (originalLogTran.char01 != incomingTranLog.char16)
                    errorDesc = string.Format("REJECTED AT CORE DUE TO: [Wrong original sender account [{0}]. Original sender account must be [{1}]]", incomingTranLog.char16, originalLogTran.char01);           
                if (originalLogTran.char16 != incomingTranLog.char01)
                    errorDesc = string.Format("REJECTED AT CORE DUE TO: [Wrong original receiver account [{0}]. Original receiver account must be [{1}]]", incomingTranLog.char01, originalLogTran.char16);
            }
            string corests = "", rfreasonmsg = "";
            if (string.IsNullOrEmpty(errorDesc))
            {
                //Call to O9Core
                JObject fastTranO9 = dbUtils.MappingJSONObject(GlobalVariable.FASTInComingToO9, incomingTranLog);
                fastTranO9.Add("SESSIONID", GlobalVariable.O9CoreUser.ssesionid);
                var reponseFromO9 = FASTUtils.CallFunction(fastTranO9, GlobalVariable.PMT_FAST_INCOMING);
                if (reponseFromO9.errorcode != 0)
                {
                    FASTUtils.InsertLogSubTran(incomingTranLog.tranref, originalLogTran.ipctransid, originalLogTran.sourceid, GlobalVariable.FASTInComingToO9,
                                                           startTime, "F", originalLogTran.userid, reponseFromO9.errorcode.ToString(), reponseFromO9.messagedetail);
                    errorDesc = reponseFromO9.messagedetail;
                }
                else
                {
                    FASTUtils.InsertLogSubTran(incomingTranLog.tranref, originalLogTran.ipctransid, originalLogTran.sourceid, GlobalVariable.FASTInComingToO9,
                                                           startTime, "S", originalLogTran.userid, "0");
                    JObject resultObject = JObject.FromObject(reponseFromO9.result);
                    JObject updateFields = (JObject)resultObject.SelectToken("DATA");
                    corests = updateFields.SelectToken("CORESTS").ToString();
                    rfreasonmsg = updateFields.SelectToken("RFREASONMSG").ToString();
                }
            }
            if (string.IsNullOrEmpty(errorDesc) && corests == GlobalVariable.CoreStatusAccept)
            {
                originalLogTran.num20 = 1;
                originalLogTran.status = FASTStatus.RefundByReceiver;
                originalLogTran.char30 = FASTStatus.RefundByReceiver;
                originalLogTran.char49 = incomingTranLog.tranref;// Save reverse PmtInfId
                originalLogTran.char50 = incomingTranLog.char44;// Save reverse MsgId
                originalLogTran.char51 = incomingTranLog.char51;// Save reverse CreDtTm
                originalLogTran.char40 = incomingTranLog.char40;
                originalLogTran.char46 = incomingTranLog.char46;
                originalLogTran.errordesc = "Received from NBC SOAP make reverse transaction";
                _context.Ipclogtrans.Update(originalLogTran);
                _context.SaveChanges();
                FASTUtils.InsertLogSubTran(incomingTranLog.tranref, originalLogTran.ipctransid, "NBC", GlobalVariable.MakeReverseIncoming,
                                                           startTime, "S", "API", "0");
                //Call make ACK for new transaction
                incomingTranLog.char48 = "ACK" + incomingTranLog.char44[3..];
                JObject isoMessageMakeACK = dbUtils.MappingJSONObject(GlobalVariable.FASTAcknowledgement, incomingTranLog);
                FASTMakeAcknowledgementRequest soaprerquest = new FASTMakeAcknowledgementRequest()
                {
                    cm_password = GlobalVariable.CMPassword,
                    cm_user_name = GlobalVariable.CMUsername,
                    content_message = isoMessageMakeACK
                };
                string res = dbUtils.CreateMessageXML(GlobalVariable.FASTAcknowledgement, JObject.FromObject(soaprerquest));
                string responseContent = FASTUtils.CallSOAPServiceAsync(res).Result;
                JObject jObjectReponse = FASTUtils.AnalyzeMessageFAST(responseContent, true);
                string fastStatus = jObjectReponse.SelectToken("CstmrPmtStsRpt.OrgnlPmtInfAndSts.TxInfAndSts.TxSts")?.ToString();
                string addtlrmtinf = jObjectReponse.SelectToken("CstmrPmtStsRpt.OrgnlPmtInfAndSts.TxInfAndSts.StsRsnInf.AddtlInf")?.ToString();
                if (fastStatus == FASTStatus.ReceivedAtACH || fastStatus == FASTStatus.AcknowledgedByFI)
                {
                    FASTUtils.InsertLogSubTran(incomingTranLog.tranref, originalLogTran.ipctransid, "NBC", GlobalVariable.FASTAcknowledgement,
                                                               startTime, "S", "API", "0");
                }
                else
                {
                    FASTUtils.InsertLogSubTran(incomingTranLog.tranref, originalLogTran.ipctransid, "NBC", GlobalVariable.FASTAcknowledgement,
                                                              startTime, "F", "API", "0", addtlrmtinf);
                }
                //Reponse message SOAP
                incomingTranLog.tranref = "REP" + incomingTranLog.char44[3..];
                string xmlReponse = dbUtils.CreateMessageXML(GlobalVariable.ReplyMessageSuccessful, JObject.FromObject(incomingTranLog));
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlReponse);
                return SOAPReponse.GetResult(doc.Beautify());
            }
            else
            {
                if (originalLogTran != null)
                {
                    //không update status
                    originalLogTran.num20 = 1;
                    originalLogTran.char49 = incomingTranLog.tranref;// Save reverse PmtInfId
                    originalLogTran.char50 = incomingTranLog.char44;// Save reverse MsgId
                    originalLogTran.char51 = incomingTranLog.char51;// Save reverse CreDtTm
                    originalLogTran.char40 = incomingTranLog.char40;
                    originalLogTran.char46 = string.IsNullOrEmpty(errorDesc)? rfreasonmsg : errorDesc;
                    originalLogTran.errordesc = "Reject make reverse NBC SOAP";
                    _context.Ipclogtrans.Update(originalLogTran);
                    _context.SaveChanges();
                    FASTUtils.InsertLogSubTran(incomingTranLog.tranref, originalLogTran.ipctransid, "NBC", GlobalVariable.MakeReverseIncoming,
                                                               startTime, "F", "API", "0", errorDesc);
                }
                incomingTranLog.tranref = "REP" + incomingTranLog.char44[3..];
                incomingTranLog.char46 = string.IsNullOrEmpty(errorDesc) ? rfreasonmsg : errorDesc;
                string xmlReponse = dbUtils.CreateMessageXML(GlobalVariable.ReplyMessageFail, JObject.FromObject(incomingTranLog));
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlReponse);
                return SOAPReponse.GetResult(doc.Beautify());
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
    }
}