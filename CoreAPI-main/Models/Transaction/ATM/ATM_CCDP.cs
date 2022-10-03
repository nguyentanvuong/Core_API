namespace WebApi.Models.Transaction.ATM
{
    public class ATM_CCDP : TransactionRequest
    {
        [CoreRequired]
        public string CAC { get; set; }
        [CoreRequired]
        public double? CAMT { get; set; }
        [CoreRequired]
        public string CCRCD { get; set; }
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
        [CoreRequired]
        public string PGACC { get; set; }
    }
}
