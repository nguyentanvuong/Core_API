namespace WebApi.Models.Transaction.ATM
{
    public class ATM_REV : TransactionRequest
    {
        [CoreRequired]
        public string ID { get; set; }
        [CoreRequired]
        public string TXREFCD { get; set; }
    }
}