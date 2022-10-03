namespace WebApi.Models.Transaction.AML
{
    public class ScanRequest
    {
        [CoreRequired]
        public string SearchCountry { get; set; }
        [CoreRequired]
        public string SearchDOB { get; set; }
        [CoreRequired]
        public string SysName { get; set; }
        [CoreRequired]
        public string PassportVerify { get; set; }
        [CoreRequired]
        public string TransRefid { get; set; }
        [CoreRequired]
        public string SearchName { get; set; }
        [CoreRequired]
        public string CHANNELID { get; set; }
        [CoreRequired]
        public string ScanOption { get; set; }
        [CoreRequired]
        public string TransType { get; set; }
        [CoreRequired]
        public string SearchID { get; set; }
        [CoreRequired]
        public string PassExpiryDate { get; set; }
        [CoreRequired]
        public string Username { get; set; }
        [CoreRequired]
        public string PassExpiryDtVerify { get; set; }
        [CoreRequired]
        public string RiskFactors { get; set; }
        [CoreRequired]
        public string Content { get; set; }
        [CoreRequired]
        public string Customercode { get; set; }
        [CoreRequired]
        public string FromDate { get; set; }
        [CoreRequired]
        public string ToDate { get; set; }
        [CoreRequired]
        public string BrCode { get; set; }
        [CoreRequired]
        public string SecurityNo1 { get; set; }
        [CoreRequired]
        public string SecurityNo2 { get; set; }
    }
}