


namespace WebApi.Models.Transaction.LRC
{
    public class PMT_IITK : TransactionRequest
    {
        [CoreRequired]
        public string ADDBANK { get; set; }
        [CoreRequired]
        public string BICCD { get; set; }
        [CoreRequired]
        public string BNKCD { get; set; }
        [CoreRequired]
        public string BNKNAME { get; set; }
        [CoreRequired]
        public string CCOUNTRY { get; set; }
        [CoreRequired]
        public string CCCR { get; set; }
        [CoreRequired]
        public string CCTMA { get; set; }
        [CoreRequired]
        public string CNCAT { get; set; }
        [CoreRequired]
        public string CNOSTRO { get; set; }
        [CoreRequired]
        public string CRADDR { get; set; }
        [CoreRequired]
        public string CRBANK { get; set; }
        [CoreRequired]
        public string CRID { get; set; }
        [CoreRequired]
        public string CRNAME { get; set; }
        [CoreRequired]
        public string CRTYPE { get; set; }
        [CoreRequired]
        public string CSPUR { get; set; }
        [CoreRequired]
        public string CTMTYPE { get; set; }
        [CoreRequired]
        public string CTXG1 { get; set; }
        [CoreRequired]
        public string C_LICENSE { get; set; }
        [CoreRequired]
        public string DTLCHR { get; set; }
        [CoreRequired]
        public string H3_121 { get; set; }
        [CoreRequired]
        public string MSGCODE { get; set; }
        [CoreRequired]
        public string MTAG70 { get; set; }
        [CoreRequired]
        public string PMREF { get; set; }
        [CoreRequired]
        public string RACNO { get; set; }
        [CoreRequired]
        public string RCCR { get; set; }
        [CoreRequired]
        public string SACNM { get; set; }
        [CoreRequired]
        public string SACNO { get; set; }
        [CoreRequired]
        public double CBAMT { get; set; }
        [CoreRequired]
        public double CBRCD { get; set; }
        [CoreRequired]
        public double CCRRATE { get; set; }
        [CoreRequired]
        public double CTXEXR { get; set; }
        [CoreRequired]
        public double PGEXR { get; set; }
        [CoreRequired]
        public double RAMT { get; set; }
        [CoreRequired]
        public double SAMT { get; set; }
        [CoreRequired]
        public double TFDPT { get; set; }
    }
}
