using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.FAST
{
    public class GetListFastBankReponse
    {
        public string bankid { get; set; }
        public string bankname { get; set; }
        public string bankstreet { get; set; }
        public string bankbuilding { get; set; }
        public string bankpostalcode { get; set; }
        public string banktown { get; set; }
        public string bankcountry { get; set; }
        public string participatecode { get; set; }
        public string bicfi { get; set; }
        
    }
}
