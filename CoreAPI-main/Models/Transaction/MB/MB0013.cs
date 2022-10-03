

namespace WebApi.Models.Transaction.MB
{
    public class MB0013 : TransactionRequest
    {
        [CoreRequired]
        public string ACCNAME { get; set; }
        [CoreRequired]
        public int? BRANID { get; set; }
        [CoreRequired]
        public string CUSCODE { get; set; }
        [CoreRequired]
        public string CATCODE { get; set; }
        [CoreRequired]
        public string ACCTYPE { get; set; }
        [CoreRequired]
        public string ISPKACNO { get; set; }
        [CoreRequired]
        public string DEBITACC { get; set; }
        [CoreRequired]
        public string FEEACC { get; set; }
        [CoreRequired]
        public double? AMT { get; set; }
        [CoreRequired]
        public double? FEEAMT { get; set; }
        [CoreRequired]
        public string ID { get; set; }
        [CoreRequired]
        public string RLLOVER { get; set; }

    }
}
