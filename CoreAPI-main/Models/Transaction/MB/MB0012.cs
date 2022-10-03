

namespace WebApi.Models.Transaction.MB
{
    public class MB0012 : TransactionRequest
    {
        public string CUSNAME { get; set; }
        [CoreRequired]
        public int? BRANID { get; set; }
        [CoreRequired]
        public string NATIONTYPE { get; set; }
        [CoreRequired]
        public string NATIONNUM { get; set; }
        [CoreRequired]
        public string DOB { get; set; }
        [CoreRequired]
        public string TITLE { get; set; }
        public string ADDRESS { get; set; }
        [CoreRequired]
        public string EMAIL { get; set; }
        public string HOMENUM { get; set; } = "";
        [CoreRequired]
        public string MOBILENUM { get; set; }
        [CoreRequired]
        public string GENDER { get; set; }
        [CoreRequired]
        public string CTYPE { get; set; }
        public string PROVINCE { get; set; }
        public string DISTRICT { get; set; }
        public string COMMUNE { get; set; }
        public string VILLAGE { get; set; }
        public string GRPNO { get; set; }
        public string STRNO { get; set; }
        public string HOUSENO { get; set; }
        [CoreRequired]
        public string KYC { get; set; }
        [CoreRequired]
        public string ID { get; set; }

    }
}