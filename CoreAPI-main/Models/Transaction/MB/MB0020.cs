

namespace WebApi.Models.Transaction.MB
{
    public class MB0020 : TransactionRequest
    {
        [CoreRequired]
        public string ACCNUM { get; set; }
        [CoreRequired]
        public string ID { get; set; }
    }
}
