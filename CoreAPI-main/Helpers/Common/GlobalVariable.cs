using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Mail;
using WebApi.Entities;
using WebApi.Models;
using WebApi.Models.FAST;
using WebApi.Models.Util;

namespace WebApi.Helpers.Common
{
    public class GlobalVariable
    {
        // Key of JITS
        public static string myPrivateKey = Startup.path + "private_key_JITS.pem";
        public static string myPublicKey = Startup.path + "public_key_JITS.pem";

        public static string mySignPrivateKey = Startup.path + "private_sign_key_JITS.pem";
        public static string mySignPublicKey = Startup.path + "public_sign_key_JITS.pem";

        // Key of AYA
        public static string yourPrivateKey = Startup.path + "private_key_AYA.pem";
        public static string yourPublicKey = Startup.path + "public_key_AYA.pem";

        public static string yourSignPrivateKey = Startup.path + "private_sign_key_AYA.pem";
        public static string yourSignPublicKey = Startup.path + "public_sign_key_AYA.pem";

        // global variable for all setting
        public static string O9_GLOBAL_TXDT = "";
        public static string O9_GLOBAL_LANG = "en";
        public static string COMCODE = "O9";
        public static bool IsUsingJWT = false;
        public static bool IsUsingLicense = false;
        public static bool IsUsingEncrypt = false;
        public static string AMLAuthUser = "AMLSVC";
        public static string AMLAuthPswd = "AMLSVC";
        public static string Sender_SWIFTCode = "";
        public static string Sender_ParticipantCode = "";
        public static string Sender_BICFI = "";
        public static LoginRequest O9CoreLoginRequest = null;
        public static Users O9CoreUser = null;

        // App config
        public static int TIME_UPDATE_TXDT = 30; // 30S

        // format
        public static string FORMAT_SHORT_DATE = "dd/MM/yyyy";
        public static string FORMAT_LONG_DATE = "dd/MM/yyyy hh:mm:ss";

        // call function
        public static string UTIL_CALL_PROC = "UTIL_CALL_PROC";
        public static string UTIL_CALL_FUNC = "UTIL_CALL_FUNC";
        public static string UTIL_GET_BUSDATE = "UTIL_GET_BUSDATE";
        public static string UMG_LOGIN = "UMG_LOGIN";

        //function to call
        public const string FUNC_PMT_ISWK = "O9SYS.FUNC_PMT_ISWK";
        public const string FUNC_PMT_GOL = "O9SYS.FUNC_FAST_GOL";
        public const string FUNC_PMT_GIL = "O9SYS.FUNC_FAST_GIL";
        public const string FUNC_PMT_USTF = "O9SYS.FUNC_PMT_USTF";
        public const string FUNC_PMT_RFT = "O9SYS.FUNC_PMT_RFT";
        public const string FUNC_FAST_ACNO = "O9SYS.FUNC_FAST_ACNO";
        public const string PMT_FAST_INCOMING = "O9SYS.FUNC_FAST_INCOMING";
        public const string PMT_SYNC_STATUS_FAST = "O9SYS.FUNC_FAST_SYNC_STATUS";

        // transaction LRC
        public static string PMT_ISWK = "PMT_ISWK";
        public static string PMT_SCS = "PMT_SCS";
        public static string PMT_OITR = "PMT_OITR";
        public static string PMT_IITK = "PMT_IITK";
        public static string PMT_GSWK = "PMT_GSWK";
        public static string PMT_SWGI = "PMT_SWGI";
        public static string PMT_IIKR = "PMT_IIKR";

        // transaction PMT
        public static string PMT_GRMB = "PMT_GRMB";

        // transaction MB
        public static string MB1000 = "MB1000";
        public static string MB0020 = "MB0020";
        public static string MB0019 = "MB0019";
        public static string MB0018 = "MB0018";
        public static string MB0017 = "MB0017";
        public static string MB0016 = "MB0016";
        public static string MB0015 = "MB0015";
        public static string MB0014 = "MB0014";
        public static string MB0013 = "MB0013";
        public static string MB0012 = "MB0012";
        public static string MB0011 = "MB0011";
        public static string MB0010 = "MB0010";
        public static string MB0009 = "MB0009";
        public static string MB0008 = "MB0008";
        public static string MB0007 = "MB0007";
        public static string MB0006 = "MB0006";
        public static string MB0005 = "MB0005";
        public static string MB0004 = "MB0004";
        public static string MB0003 = "MB0003";
        public static string MB0002 = "MB0002";
        public static string MB0001 = "MB0001";
        public static string MB0000 = "MB0000";

        // transaction ATM
        public static string ATM_ACNM = "ATM_ACNM";
        public static string ATM_CCDP = "ATM_CCDP";
        public static string ATM_IFTF = "ATM_IFTF";
        public static string ATM_FTF = "ATM_FTF";
        public static string ATM_MSTM = "ATM_MSTM";
        public static string ATM_BINQ = "ATM_BINQ";
        public static string ATM_PFEE = "ATM_PFEE";
        public static string ATM_CDP = "ATM_CDP";
        public static string ATM_TCDP = "ATM_TCDP";
        public static string ATM_CP2P = "ATM_CP2P";
        public static string ATM_BPM = "ATM_BPM";
        public static string ATM_CWR = "ATM_CWR";
        public static string ATM_TCWR = "ATM_TCWR";
        public static string ATM_DP2P = "ATM_DP2P";
        public static string ATM_RV2 = "ATM_RV2";
        public static string ATM_REV = "ATM_REV";
        public static string ATM_DTGL = "ATM_DTGL";

        // String Config
        public const string Oracle = "Oracle";
        public const string MySql = "MySql";
        public const string SqlLite = "SqlLite";
        public const string MSSQL = "MSSQL";
        public const string PostgreSQL = "PostgreSQL";
        public const string MemoryDB = "MemoryDB";

        // Manual string
        public static string WorkingDate = "workingdate";
        public static string LoginName = "username";
        public static string Session = "sessionid";
        public static string BranchCode = "branchcode";
        public static string Token = "token";
        public const string Identity = "id";
        public const string UserType = "type";
        public const string FromCore = "core";
        public const string FromWeb = "web";
        public const string UserPublic = "public";
        public static string FASTMode = "AUTO";
        public const string DatetimeFormatWithMilisecond = "dd/MM/yyyy HH:mm:ss.fff";
        public static string[] DatetimeFormat = { "dd/MM/yyyy", "dd/MM/yyyy HH:mm:ss", "dd/MM/yyyy HH:mm", "MM/dd/yyyy","yyyy/MM/dd","yyyy-MM-dd'T'",
                                                "dd-MM-yyyy", "MM-dd-yyyy", "yyyy-MM-dd", "yyyy-MM-dd'Z'",
                                                "dd/MM/yyyy hh:mm:ss tt", "yyyy-MM-dd'T'HH:mm:ss.ff", "yyyy-MM-dd'T'HH:mm:ss",
                                                "yyyy-MM-dd'T'HH:mm:ss.fff'Z'", "yyyy-MM-dd'T'HH:mm:ss", "d/M/YYYY H:m:s tt"};

        //Constant in Database
        public static string UserStatus = "N";
        public static string TypeSearchNumber = "NUM";
        public static long LAST_TXREF_OUTGOING = 0;
        public static long LAST_TXREF_INCOMING = 0;
        public static long MRECORD = 100;
        public static string DbInUse = "";
        public static string DbConnectionString = "";

        // Global Object
        public static string[] FinalFASTStatusSuccessful = { "STTL", "RFST", "ACST" };
        public static string[] FinalFASTStatusFail = { "RJCT", "FLFI", "FACH", "STLF" };
        public const string CoreStatusReject = "IF";
        public const string CoreStatusAccept = "IS";
        public const int TimeoutCallFASTService = 20;
        public const int ReplyTimes = 3;
        public static JObject CDLIST = null;
        public static JObject SearchFunction = null;
        public static JObject SForm = null;
        public static SmtpClient SmtpClient = null;
        public static string EmailDelivery = "";
        public static string EmailFrom = "";
        public static List<AuthenCodeInfo> AuthenCodeList = new List<AuthenCodeInfo>();
        public static FASTGetTokenRequest FASTGetTokenRequest = null;
        public static SToken CurrentFastToken = null;

        // Paging config
        public static int pageStart = 1;
        public static int numberPerPageStart = 10;

        //Reponse Code
        public static int SuccessCode = 0;

        //Stored Procedure name
        public static string SYS_CHECK_SUSER_ADD = "SYS_CHECK_SUSER_ADD";
        public static string SYS_CHECK_SUSER_MODIFY = "SYS_CHECK_SUSER_MODIFY";

        //API FAST variable
        public static string FASTRestfulURL = "";
        public static string FASTSOAPURL = "";
        public static string URLGetToken = "/api/v1/auth";
        public static string URLVerifyAccount = "/api/v1/account-inquiry";
        public static string CMPassword = "";
        public static string CMUsername = "";
        public const string FASTGetIncomingSplitSeperater = "</Document>";
        public const string PathSOAPPublic = "/Service.asmx";

        //IPCTRANCODE
        public const string FAST_INCOMING = "FAST_INCOMING";
        public const string FAST_OUTGOING = "FAST_OUTGOING";
        public const string FASTAccountInquiry = "FASTAccountInquiry";
        public const string FASTGetIncomingTransaction = "FASTIncomingTran";
        public const string FASTInComingToLog = "FASTInComingToLog";
        public const string FASTInComingToO9 = "FASTInComingToO9";
        public const string FASTMAKEFULLFUN = "FASTMAKEFULLFUN";
        public const string GetIncomingStatusFromO9 = "GetIncomingStatusFromO9";
        public const string FASTAcknowledgement = "FASTAcknowledgement";
        public const string FASTReverseTran = "FASTReverseTran";
        public const string GetOutgoingTransFromO9 = "GetOutgoingTransFromO9";
        public const string SyncTransactionStatusToO9 = "SyncTransactionStatusToO9";
        public const string FASTGetOutgoingTranByPmtId = "FASTOutTranByPmtId";
        public const string ReverseTransactionToO9 = "ReverseTransactionToO9";

        //SOAP
        public const string ReceiveMakeFullFund = "MAKEFULLFUNDIN";
        public const string ReceiveMakeAcknowledgment = "MAKEACKIN";
        public const string ReceiveMakeReverse = "MAKEREVERSEIN";
        public const string MakeReverseIncoming = "MAKEREVERSEINCOMING";
        public const string ReplyMessageSuccessful = "pain.002.001.06_S";
        public const string ReplyMessageFail = "pain.002.001.06_F";

    }
}
