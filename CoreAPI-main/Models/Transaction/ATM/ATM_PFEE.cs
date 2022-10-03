namespace WebApi.Models.Transaction.ATM
{
    public class ATM_PFEE : TransactionRequest
    {
        [CoreRequired]
        public string DAC { get; set; }
        [CoreRequired]
        public double? DAMT { get; set; }
        [CoreRequired]
        public string ID { get; set; }
        [CoreRequired]
        public string CCRCD { get; set; }
        [CoreRequired]
        public string PGACC { get; set; }
    }
}
