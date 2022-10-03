
namespace WebApi.Models.Transaction.LRC
{
    public class PMT_ISWK : TransactionRequest
    {
        [CoreRequired]
        public long? LTXREFID { get; set; }
        [CoreRequired]
        public long? MRECORD { get; set; }
    }
}
