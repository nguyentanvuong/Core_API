

namespace WebApi.Models.Transaction.LRC
{
    public class PMT_SCS : TransactionRequest
    {
        [CoreRequired]
        public string MSGCODE { get; set; }
        [CoreRequired]
        public string CONTENT { get; set; }
        [CoreRequired]
        public string SWSSTS { get; set; }
    }
}







