namespace WebApi.Helpers.Common
{
    public static class Codetypes
    {
        public static CodeDescription FormatCodeDescription(CodeDescription codeDescription, string[] formatstring)
        {
            return new CodeDescription(codeDescription.errorcode, string.Format(codeDescription.messagedetail, formatstring));
        }

        public static CodeDescription Err_Not_Started = new CodeDescription(-1, "System not yet started. Please wait abit");
        public static CodeDescription Code_Success_Login = new CodeDescription(0, "Login successful");
        public static CodeDescription Code_Success_Trans = new CodeDescription(0, "Transaction successful");
        public static CodeDescription Err_Unauthorized = new CodeDescription(1, "Unauthorized");
        public static CodeDescription Err_ExpireDate = new CodeDescription(2, "Token expired date");
        public static CodeDescription Err_InvalidToken = new CodeDescription(3, "Invalid token");
        public static CodeDescription Err_Authenticate = new CodeDescription(10, "Username or password is not correct");
        public static CodeDescription Err_Duplicate = new CodeDescription(5, "Message code is duplicated");
        public static CodeDescription Err_AccessFolder = new CodeDescription(6, "Can't submit this file. Please contact SBILH to checking this case");


        public static CodeDescription SYS_INVALID_SESSION = new CodeDescription(7, "Invalid session by ");
        public static CodeDescription UMG_INVALID_LOGIN_TIME = new CodeDescription(8, "Do not allow login in this time");
        public static CodeDescription UMG_INVALID_EXP_POLICY = new CodeDescription(9, "The policy for user is expired!");
        public static CodeDescription SYS_LOGIN_FALSE = new CodeDescription(10, "The username or password you entered is incorrect!");
        public static CodeDescription SYS_LOGIN_BLOCK = new CodeDescription(11, "System auto block after atemp fail login");
        public static CodeDescription UMG_INVALID_STATUS = new CodeDescription(12, "User invalid status");
        public static CodeDescription UMG_INVALID_EXPDT = new CodeDescription(13, "User's expiry date already");

        public static CodeDescription Err_Image_Extensions = new CodeDescription(14, "Extensions is not support. Extensions must be in (PNG|JPG)");
        public static CodeDescription Err_Folder_Config = new CodeDescription(15, "Config not correct. Please contact SBILH to checking this case");
        public static CodeDescription Err_Image_Duplicate = new CodeDescription(16, "Duplicate file name image");
        public static CodeDescription Err_MT103_Duplicate = new CodeDescription(17, "Duplicate file MT103");
        public static CodeDescription Err_MT103_Extensions = new CodeDescription(18, "Extensions is not support. Extensions must be in (mt103|MT103)");

        //Other
        public static CodeDescription Err_AML_Exception = new CodeDescription(19, "Exception when sending message to AML");
        public static CodeDescription Err_FAST_Exception = new CodeDescription(19, "Exception when sending message to FAST");
        public static CodeDescription Err_ChangePassword = new CodeDescription(22, "New password must be different from old password");
        public static CodeDescription Err_Unknown = new CodeDescription(9999, "Error message from server. Please contact SBILH to more information");
        public static CodeDescription Err_InputFormat = new CodeDescription(9997, "Input field is incorrect format. Please check input field again");
        public static CodeDescription Err_CanNotDeletePolicy = new CodeDescription(9997, "Can't delete default policy");
        public static CodeDescription Err_DatabaseError = new CodeDescription(9997, "Error in Database");
        public static CodeDescription Err_DeleteRoleDetail = new CodeDescription(9997, "Can not delete this role detail. One user must have at least one role detail.");

        //Incorrect
        public static CodeDescription Err_IncorrectPassword = new CodeDescription(2001, "Current password is incorrect");
        public static CodeDescription Err_IncorrectAuthenCode = new CodeDescription(2002, "Verification code is incorrect");
        public static CodeDescription Err_DataNotFound = new CodeDescription(2000, "Data not found");

        //Exists
        public static CodeDescription Err_UserExists = new CodeDescription(3001, "This user already exists");
        public static CodeDescription Err_UserNotExists = new CodeDescription(3002, "This user doesn't exist");
        public static CodeDescription Err_UserEmailExists = new CodeDescription(3003, "This user email already exists");
        public static CodeDescription Err_BankIdNotExists = new CodeDescription(3004, "BankId doesn't exist");
        public static CodeDescription Err_BankCodeExists = new CodeDescription(3005, "BankCode already exists");

        public static CodeDescription Err_ParameterNameExists = new CodeDescription(3005, "ParameterName already exists");

        public static CodeDescription Err_FastBankNotExist = new CodeDescription(3005, "Fast bank doesn't exist");
        public static CodeDescription Err_FastBankExist = new CodeDescription(3005, "Fast bank already exists");
        public static CodeDescription Err_PolicyExist = new CodeDescription(3005, "User policy already exists");
        public static CodeDescription Err_RoleExist = new CodeDescription(3005, "User role already exists");
        public static CodeDescription Err_RoleNotExist = new CodeDescription(3005, "User role doesn't exists");
        public static CodeDescription Err_RoleDetailExist = new CodeDescription(3005, "User role detail already exists");
        public static CodeDescription Err_ScheduleExist = new CodeDescription(3005, "Schedule already exists");
        public static CodeDescription Err_IPCLogTranNotExist = new CodeDescription(3006, "Log Tran is not exist or is completed");
        public static CodeDescription Err_IPCLogTranNotExistOrSynchronized = new CodeDescription(3006, "Log Tran is not exist or is synchronized");
        public static CodeDescription Err_ReportNotExist = new CodeDescription(3007, "Report doesn't exist");

        //Invalid
        public static CodeDescription Err_PolicyIdInvalid = new CodeDescription(4001, "Policy is invalid");
        public static CodeDescription Err_InvalidLicense = new CodeDescription(4002, "Invalid license");
        public static CodeDescription Err_UserStatus = new CodeDescription(4003, "Invalid user status");
        public static CodeDescription Err_InvalidBranchId = new CodeDescription(4004, "Invalid branch");
        public static CodeDescription Err_InvalidDeptId = new CodeDescription(4005, "Invalid department");
        public static CodeDescription Err_AuthenCodeExpried = new CodeDescription(4006, "Verification has expired");
        public static CodeDescription Err_LookupConfigInvalid = new CodeDescription(4007, "Lookup config invalid {0} {1}. Please check config again");
        public static CodeDescription Err_GetInforConfigInvalid = new CodeDescription(4008, "Get infor config invalid. Please check config again");
        public static CodeDescription Err_LtxrefidConfigInvalid = new CodeDescription(4009, "Invalid last txrefid. Please check config again");
        public static CodeDescription Err_InvalidBankId = new CodeDescription(4005, "Invalid Bank");

        //Exception
        public static CodeDescription Err_SendMailFail = new CodeDescription(5001, "Send email fail. Please check email config again.");

        //Success
        public static CodeDescription Code_Success = new CodeDescription(0, "Successful");

        //Require
        public static CodeDescription Err_RequireSessionID = new CodeDescription(21, "Field SESSIONID is required");
        public static CodeDescription Err_RequireBankID = new CodeDescription(6001, "Field BankID is required");
        public static CodeDescription Err_RequireParameter = new CodeDescription(6001, "Field Parametername is required");

        public static CodeDescription Err_RequireBICFI = new CodeDescription(6001, "Field BICFI is required");
        public static CodeDescription Err_RequireBankCode = new CodeDescription(6002, "Field BankCode is required");
        public static CodeDescription Err_RequireParameterName = new CodeDescription(6002, "Field ParameterName is required");
        public static CodeDescription Err_RequireUsername = new CodeDescription(6003, "Field Username is required");
        public static CodeDescription Err_RequirePolicyID = new CodeDescription(6004, "Field PolicyID is required");
        public static CodeDescription Err_RequireRoleID = new CodeDescription(6004, "Field RoleID is required");
        public static CodeDescription Err_Requireipctranid = new CodeDescription(6005, "Field ipctranid is required");
        public static CodeDescription Err_Requirescheduleid = new CodeDescription(6005, "Field scheduleid is required");
        public static CodeDescription Err_RequireCcy = new CodeDescription(6005, "Field ccy is required");
        public static CodeDescription Err_RequireTranref = new CodeDescription(6006, "Field tranref is required");
        public static CodeDescription Err_RequireIpctransid = new CodeDescription(6006, "Field ipctransid is required");
        public static CodeDescription Err_RequireReportName = new CodeDescription(6007, "Field ReportName is required");

    }
}
