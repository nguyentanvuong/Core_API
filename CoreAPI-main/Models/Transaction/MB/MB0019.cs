

namespace WebApi.Models.Transaction.MB
{
    public class MB0019 : TransactionRequest
    {
        [CoreRequired]
        public string CUSCODE { get; set; }
        [CoreRequired]
        public string CATCODELST { get; set; }
        [CoreRequired]
        public string ID { get; set; }
    }
}
