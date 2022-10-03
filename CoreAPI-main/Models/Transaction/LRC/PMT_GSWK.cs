
namespace WebApi.Models.Transaction.LRC
{
    public class PMT_GSWK : TransactionRequest
    {
        [CoreRequired]
        public string BICCD { get; set; }
        [CoreRequired]
        public string BNKCD { get; set; }
        public string CCCR { get; set; }
        [CoreRequired]
        public string CCOUNTRY { get; set; }
        public string CCTMA { get; set; }
        public string CRADDR { get; set; }
        [CoreRequired]
        public string CRNAME { get; set; }
        [CoreRequired]
        public string DTLCHR { get; set; }
        public string FNAME { get; set; }
        public string H3_121 { get; set; }
        [CoreRequired]
        public long? IPCCD { get; set; }
        public string MTAG70 { get; set; }
        [CoreRequired]
        public string RACNO { get; set; }
        [CoreRequired]
        public string SACNM { get; set; }
        [CoreRequired]
        public string SACNO { get; set; }
        [CoreRequired]
        public double? SAMT { get; set; }
        [CoreRequired]
        public string TRANREF { get; set; }
    }
}