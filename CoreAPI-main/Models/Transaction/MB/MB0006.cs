

namespace WebApi.Models.Transaction.MB
{
    public class MB0006 : TransactionRequest
    {
        [CoreRequired]
        public string IDCARDNUM { get; set; }
        [CoreRequired]
        public string IDCARDTYPE { get; set; }
        [CoreRequired]
        public string ID { get; set; }
    }
}
