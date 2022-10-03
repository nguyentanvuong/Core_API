namespace WebApi.Models.Transaction.FAST
{
    public class PMT_RFT : TransactionRequest
    {
        [CoreRequired]
        public string MSGID { get; set; }
        [CoreRequired]
        public string SBNAME { get; set; }
        [CoreRequired]
        public string SBSTREET { get; set; }
        [CoreRequired]
        public string SBBDMB { get; set; }
        [CoreRequired]
        public string SBPCODE { get; set; }
        [CoreRequired]
        public string SBTOWN { get; set; }
        [CoreRequired]
        public string SBCOUNTRY { get; set; }
        [CoreRequired]
        public string PMTINFID { get; set; }
        [CoreRequired]
        public string PMTMTD { get; set; }
        [CoreRequired]
        public string SDNAME { get; set; }
        [CoreRequired]
        public string SDSTREET { get; set; }
        [CoreRequired]
        public string SDBDMB { get; set; }
        [CoreRequired]
        public string SDPCODE { get; set; }
        [CoreRequired]
        public string SDTOWN { get; set; }
        [CoreRequired]
        public string SDCOUNTRY { get; set; }
        [CoreRequired]
        public string SACCNO { get; set; }
        [CoreRequired]
        public string SCCR { get; set; }
        [CoreRequired]
        public string SBANK { get; set; }
        [CoreRequired]
        public string INSTRID { get; set; }
        [CoreRequired]
        public string CCCR { get; set; }
        [CoreRequired]
        public double? SAMT { get; set; }
        [CoreRequired]
        public string CHRGBR { get; set; }
        [CoreRequired]
        public string RBANK { get; set; }
        [CoreRequired]
        public string RCNAME { get; set; }
        [CoreRequired]
        public string RCSTREET { get; set; }
        [CoreRequired]
        public string RCBDMB { get; set; }
        [CoreRequired]
        public string RCTOWN { get; set; }
        [CoreRequired]
        public string RCCOUNTRY { get; set; }
        [CoreRequired]
        public string RACCNO { get; set; }
        [CoreRequired]
        public string PURPOSE { get; set; }
        [CoreRequired]
        public string REFCD { get; set; }
        [CoreRequired]
        public string REFNB { get; set; }
        [CoreRequired]
        public string STRINFOR { get; set; }
    }
}