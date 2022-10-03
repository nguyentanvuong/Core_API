

namespace WebApi.Models.Transaction.LRC
{
    public class PMT_OITR : TransactionRequest
    {
        [CoreRequired]
        public double CBAMT { get; set; }
        [CoreRequired]
        public string CCCR { get; set; }
        [CoreRequired]
        public string CNOSTRO { get; set; }
        [CoreRequired]
        public string DTLCHR { get; set; }
        [CoreRequired]
        public string MSGCODE { get; set; }
        [CoreRequired]
        public double MTAG71F { get; set; }
        [CoreRequired]
        public double MTAG71G { get; set; }
        [CoreRequired]
        public double RAMT { get; set; }
        [CoreRequired]
        public string RCCR { get; set; }
        [CoreRequired]
        public double RFEE { get; set; }
        [CoreRequired]
        public string SACNO { get; set; }
        [CoreRequired]
        public double SAMT { get; set; }
    }
}