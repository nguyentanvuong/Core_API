namespace WebApi.Models.FAST
{
    public class PMT_SYNC_STATUS_FAST : TransactionRequest
    {
        public string MSGCODE { get; set; }
        public string FCNSTS { get; set; }
        public string RFREASONMSG { get; set; }
    }
}
