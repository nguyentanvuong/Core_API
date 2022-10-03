
namespace WebApi.Models.Transaction.MB
{
    public class MB0017 : TransactionRequest
    {
        [CoreRequired]
        public string CUSTOMERCD { get; set; }
        [CoreRequired]
        public string ID { get; set; }
    }
}
