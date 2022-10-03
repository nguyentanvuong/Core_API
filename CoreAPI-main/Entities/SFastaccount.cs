using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Entities
{
    public partial class SFastaccount
    {
        public string accnum { get; set; }
        public string accname { get; set; }
        public string type { get; set; }
        public DateTime? opendate { get; set; }
        public decimal balance { get; set; }
        public string currency { get; set; }
        public string status { get; set; }
        public string branchnum { get; set; }
    }
}
