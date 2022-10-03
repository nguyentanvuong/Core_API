

namespace WebApi.Models.Transaction.MB
{
    public class MB0004 : TransactionRequest
    {
        [CoreRequired]
        public string CBSID { get; set; }
        [CoreRequired]
        public string ID { get; set; }
    }
}
