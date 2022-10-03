using System;

namespace WebApi.Helpers.Common
{
    public class JsonHeader
    {
        public string TXCODE { get; set; }
        public string TXDT { get; set; }
        public string TXREFID { get; set; }
        public string VALUEDT { get; set; }
        public int BRANCHID { get; set; }
        public int USRID { get; set; }
        public string LANG { get; set; }
        public string USRWS { get; set; }
        public Object APUSER { get; set; }
        public string APUSRIP { get; set; }
        public string APUSRWS { get; set; }
        public string APDT { get; set; }
        public string STATUS { get; set; }
        public string ISREVERSE { get; set; }
        public int? HBRANCHID { get; set; }
        public int? RBRANCHID { get; set; }
        public string APREASON { get; set; }
        public string PRN { get; set; }
        public string ID { get; set; }
    }


    public class JsonHeaderMapping
    {
        public string A { get; set; }
        public string B { get; set; }
        public string C { get; set; }
        public string D { get; set; }
        public int E { get; set; }
        public int F { get; set; }
        public string G { get; set; }
        public string H { get; set; }
        public Object I { get; set; }
        public string J { get; set; }
        public string K { get; set; }
        public string L { get; set; }
        public string M { get; set; }
        public string N { get; set; }
        public int? O { get; set; }
        public int? P { get; set; }
        public string Q { get; set; }
        public string R { get; set; }
        public string ID { get; set; }
    }
}
