using Newtonsoft.Json.Linq;

namespace WebApi.Helpers.Common
{
    public class JsonPosting
    {
        public JArray ACNO { get; set; }
        public JArray BACNO { get; set; }
        public JArray CCRCD { get; set; }
        public JArray ACTION { get; set; }
        public JArray AMT { get; set; }
        public JArray ACGRP { get; set; }
        public JArray ACIDX { get; set; }
        public JArray PRN { get; set; }
        public JArray ACNAME { get; set; }
        public JArray BACNO2 { get; set; }
        public JArray BACNAME2 { get; set; }

        public JsonPosting()
        {

        }

        public JsonPosting(JsonPostingMapping jsonPostingMapping)
        {
            if (jsonPostingMapping != null)
            {
                ACNO = jsonPostingMapping.A;
                BACNO = jsonPostingMapping.B;
                CCRCD = jsonPostingMapping.C;
                ACTION = jsonPostingMapping.D;
                AMT = jsonPostingMapping.E;
                ACGRP = jsonPostingMapping.F;
                ACIDX = jsonPostingMapping.G;
                PRN = jsonPostingMapping.H;
                ACNAME = jsonPostingMapping.I;
                BACNO2 = jsonPostingMapping.J;
                BACNAME2 = jsonPostingMapping.K;
            }
        }
    }


    public class JsonPostingMapping
    {
        public JArray A { get; set; }
        public JArray B { get; set; }
        public JArray C { get; set; }
        public JArray D { get; set; }
        public JArray E { get; set; }
        public JArray F { get; set; }
        public JArray G { get; set; }
        public JArray H { get; set; }
        public JArray I { get; set; }
        public JArray J { get; set; }
        public JArray K { get; set; }


        public JsonPostingMapping()
        {

        }

        public JsonPostingMapping(JsonPosting jsonPosting)
        {
            if (jsonPosting != null)
            {
                A = jsonPosting.ACNO;
                B = jsonPosting.BACNO;
                C = jsonPosting.CCRCD;
                D = jsonPosting.ACTION;
                E = jsonPosting.AMT;
                F = jsonPosting.ACGRP;
                G = jsonPosting.ACIDX;
                H = jsonPosting.PRN;
                I = jsonPosting.ACNAME;
                J = jsonPosting.BACNO2;
                K = jsonPosting.BACNAME2;
            }
        }
    }


}
