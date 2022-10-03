

namespace WebApi.Models.Transaction.MB
{
    public class MB0009 :TransactionRequest
    {
        [CoreRequired]
        public string ACCNUM { get; set; }
        [CoreRequired]
        public string ID { get; set; }
    }
}
