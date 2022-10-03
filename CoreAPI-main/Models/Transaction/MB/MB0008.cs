

namespace WebApi.Models.Transaction.MB
{
    public class MB0008 :TransactionRequest
    {
        [CoreRequired]
        public string CUSCODE { get; set; }
        [CoreRequired]
        public string ID { get; set; }
    }
}
