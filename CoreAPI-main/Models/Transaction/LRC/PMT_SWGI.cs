

namespace WebApi.Models.Transaction.LRC
{
    public class PMT_SWGI : TransactionRequest
    {
        [CoreRequired]
        public string PDACC { get; set; }

    }
}