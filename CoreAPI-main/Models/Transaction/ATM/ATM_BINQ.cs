namespace WebApi.Models.Transaction.ATM
{
    public class ATM_BINQ : TransactionRequest
    {
        [CoreRequired]
        public string ACNO { get; set; }
        [CoreRequired]
        public string F54 { get; set; }
        [CoreRequired]
        public string FEEACC { get; set; }
        [CoreRequired]
        public double? FEEAMT { get; set; }
        [CoreRequired]
        public string FEECCRCD { get; set; }
        [CoreRequired]
        public string ID { get; set; }
    }
}