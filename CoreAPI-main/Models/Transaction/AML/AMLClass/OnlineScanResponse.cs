namespace WebApi.Models.Transaction.AML
{
    public class OnlineScanResponse
    {
        public string RtnHit { get; set; }
        public string RtnScanID { get; set; }
        public string RtnEnCryptScanID { get; set; }
        public string RtnHitPEP { get; set; }
        public string RtnHitSanction { get; set; }
        public string RtnHitBlacklist { get; set; }
        public OnlineScanResponse()
        {

        }
        public OnlineScanResponse(string rtnHit, string rtnScanID, string rtnEnCryptScanID)
        {
            RtnHit = rtnHit;
            RtnScanID = rtnScanID;
            RtnEnCryptScanID = rtnEnCryptScanID;
        }

        public OnlineScanResponse(string rtnHit, string rtnScanID, string rtnEnCryptScanID, string rtnHitPEP, string rtnHitSanction, string rtnHitBlacklist) : this(rtnHit, rtnScanID, rtnEnCryptScanID)
        {
            RtnHitPEP = rtnHitPEP;
            RtnHitSanction = rtnHitSanction;
            RtnHitBlacklist = rtnHitBlacklist;
        }
    }
}
