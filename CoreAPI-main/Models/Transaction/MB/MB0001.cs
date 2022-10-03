

namespace WebApi.Models.Transaction.MB
{
    public class MB0001 : TransactionRequest
    {
        [CoreRequired]
        public string CUSCODE { get; set; }
        [CoreRequired]
        public string DPTTYPE { get; set; }
        [CoreRequired]
        public string ID { get; set; }
    }
}