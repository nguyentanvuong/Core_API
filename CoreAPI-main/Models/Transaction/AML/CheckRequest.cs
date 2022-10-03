namespace WebApi.Models.Transaction.AML
{
    public class CheckRequest
    {
        [CoreRequired]
        public string Username { set; get; }
        [CoreRequired]
        public string BrCode { set; get; }
        [CoreRequired]
        public string SysName { set; get; }
        [CoreRequired]
        public string ScanOption { set; get; }
        [CoreRequired]
        public string ScanID { set; get; }
    }
}
