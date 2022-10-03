namespace WebApi.Models.Transaction.MB
{
    public class MB0000 : TransactionRequest
    {
        [CoreRequired]
        public string TXREFCD { get; set; }
        [CoreRequired]
        public string ID { get; set; }
    }
}
