namespace WebApi.Models.Transaction.ATM
{
    public class ATM_RV2 : TransactionRequest
    {
        [CoreRequired]
        public string ID { get; set; }
        [CoreRequired]
        public string TXREFCD { get; set; }
    }
}
