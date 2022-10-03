namespace WebApi.Models.Transaction.ATM
{
    public class ATM_DP2P : TransactionRequest
    {
        [CoreRequired]
        public string DAC { get; set; }
        [CoreRequired]
        public double? DAMT { get; set; }
        [CoreRequired]
        public string ID { get; set; }
        [CoreRequired]
        public string F54 { get; set; }
        [CoreRequired]
        public string CCRCD { get; set; }
        [CoreRequired]
        public string PGACC { get; set; }
        [CoreRequired]
        public string FEEACC { get; set; }
        [CoreRequired]
        public double? FEEAMT { get; set; }
        [CoreRequired]
        public string FEECCRCD { get; set; }

    }
}
