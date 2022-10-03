


namespace WebApi.Models.Transaction.LRC
{
    public class PMT_IIKR : TransactionRequest
    {
        [CoreRequired]
        public long? IPCCD { get; set; }
    }
}
