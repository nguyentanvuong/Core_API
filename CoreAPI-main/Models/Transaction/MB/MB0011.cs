

namespace WebApi.Models.Transaction.MB
{
    public class MB0011 : TransactionRequest
    {
        [CoreRequired]
        public string CATCODE { get; set; }
        [CoreRequired]
        public string ID { get; set; }
    }
}
