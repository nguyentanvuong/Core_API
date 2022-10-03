namespace WebApi.Models.Transaction.PMT
{
    public class PMT_GRMB : TransactionRequest
    {
        [CoreRequired]
        [CoreMaxLength(25)]
        public string BICCD { get; set; }
        [CoreRequired]
        [CoreMaxLength(9)]
        public string SACNO { get; set; }
        [CoreRequired]
        [CoreMaxLength(50)]
        public string RACNO { get; set; }
        [CoreRequired]
        [CoreMaxLength(3)]
        public string RCCR { get; set; }
        [CoreRequired]
        [CoreMaxLength(100)]
        public string CRNAME { get; set; }
        [CoreRequired]
        public double? RAMT { get; set; }
        [CoreRequired]
        public double? TFDPT { get; set; }
        [CoreRequired]
        [CoreMaxLength(2)]
        public string CSPUR { get; set; }
        public string TRANREF { get; set; }
    }
}

