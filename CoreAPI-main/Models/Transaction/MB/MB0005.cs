

namespace WebApi.Models.Transaction.MB
{
    public class MB0005 : TransactionRequest
    {
        [CoreRequired]
        public string ID { get; set; }
        [CoreRequired]
        public string DPTTYPE { get; set; }
    }
}
