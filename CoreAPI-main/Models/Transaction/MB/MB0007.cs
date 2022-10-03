

namespace WebApi.Models.Transaction.MB
{
    public class MB0007 : TransactionRequest
    {
        [CoreRequired]
        public string ACCNUM { get; set; }
        [CoreRequired]
        public string FROMDT { get; set; }
        [CoreRequired]
        public string TODT { get; set; }
        [CoreRequired]
        public int? SRECORD { get; set; }
        [CoreRequired]
        public int? MRECORD { get; set; }
        [CoreRequired]
        public string ID { get; set; }
    }
}
