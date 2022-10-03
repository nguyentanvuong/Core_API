

namespace WebApi.Models.Transaction.MB
{
    public class MB0014 : TransactionRequest
    {
        [CoreRequired]
        public string RTYPE { get; set; }
        [CoreRequired]
        public string ID { get; set; }
    }
}
