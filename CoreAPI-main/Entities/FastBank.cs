using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Entities
{
    public partial class FastBank
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
