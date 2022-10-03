namespace WebApi.Models.Transaction.PMT
{
    public class Payment : TransactionRequest
    {
        [CoreRequired]
        public string PaymentTypeCode { get; set; }
        public string SerialNo { get; set; }
        public string PaymentDate { get; set; }
        public string PayerBank { get; set; }
        public int PayerCheckDigit { get; set; }
        public string PayerAccountNo { get; set; }
        public string CurrencyCode { get; set; }
        public int Amount { get; set; }
        public string SecurityCode { get; set; }
        public string PayerName { get; set; }
        public string PayerRef { get; set; }
        public string PayeeBank { get; set; }
        public int PayeeCheckDigit { get; set; }
        public string PayeeAccountNo { get; set; }
        public string PayeeName { get; set; }
        public string PayeeRef { get; set; }
        public string ReturnCode { get; set; }
        public string InputDate { get; set; }
        public string ProcessingDate { get; set; }
        public string InputBatchNo { get; set; }
        public string OutputBatchNo { get; set; }

    }
}

