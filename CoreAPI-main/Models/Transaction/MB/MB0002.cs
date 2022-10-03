

namespace WebApi.Models.Transaction.MB
{
    public class MB0002 : TransactionRequest
    {
        [CoreRequired]
        public string PHONENO { get; set; }
        [CoreRequired]
        public string DPTTYPE { get; set; }
        [CoreRequired]
        public string ID { get; set; }

    }
}
