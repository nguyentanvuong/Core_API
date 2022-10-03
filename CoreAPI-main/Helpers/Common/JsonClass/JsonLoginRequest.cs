namespace WebApi.Helpers.Common
{
    public class JsonLoginRequest
    {
        public string L { get; set; }
        public string P { get; set; }
        public bool A { get; set; }
    }

    public class JsonLoginResponse :JsonErrorName
    {
        public string UUID { get; set; }
        public int USRID { get; set; }
        public string USRNAME { get; set; }
        public string STATUS { get; set; }
        public string LANG { get; set; }
        public int BRANCHID { get; set; }
        public string BRANCHCD { get; set; }
        public string BRNAME { get; set; }
        public int PWDCNT { get; set; }
        public string ISONLINE { get; set; }
        public string BUSDATE { get; set; }
        public int DEPRTID { get; set; }
        public string DEPRTCD { get; set; }
        public string LUSRCD { get; set; }
        public string COMCODE { get; set; }
        public string COMTYPE { get; set; }
        public string PWDRESET { get; set; }
        public string WSIP { get; set; }
        public string WSNAME { get; set; }
        public string BANKACTIVE { get; set; }

    //Private m_MENU As JObject
    //Private m_POSITION As JObject
    //Private m_MENUARC As JArray
    //Private m_USRAC As Usrac
    //Private m_TXDEF As JArray
    //Private m_MINPWDLEN As Integer
    //Private m_PWDAGEMAX As Integer
    //Private m_PWDAGEMIN As Integer
    //Private m_LASTDT As String
    //Private m_ISVALIDATEPOLICY As Boolean = False
    //Private m_POLICYID As Integer
    //Private m_UPWDDFLT As String
    //Private m_PWDDFLT As String
    //Private m_PWDDFLTEXPIRE As Integer
    //Private m_BLKAFT As Integer
    //Private m_ISVALIDLICENCE As Boolean
    //Private m_IDLTIME As Integer
    }
}


