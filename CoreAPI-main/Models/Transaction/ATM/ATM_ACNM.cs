namespace WebApi.Models.Transaction.ATM
{
    public class ATM_ACNM : TransactionRequest
    {
        [CoreRequired]
        public string ACNO { get; set; }
        [CoreRequired]
        public string F48 { get; set; }
        [CoreRequired]
        public string ID { get; set; }
    }
}
