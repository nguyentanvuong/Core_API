

namespace WebApi.Models.Transaction.MB
{
    public class MB0018 : TransactionRequest
    {
        [CoreRequired]
        public string NATIONNUM { get; set; }
        [CoreRequired]
        public string NATIONTYPE { get; set; }
        [CoreRequired]
        public string ID { get; set; }
    }
}
