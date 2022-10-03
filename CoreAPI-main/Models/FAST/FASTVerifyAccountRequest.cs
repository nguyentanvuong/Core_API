using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.FAST
{
    public class FASTVerifyAccountRequest
    {
        [CoreRequired]
        public string BICCD { get; set; }
        [CoreRequired]
        public string RACNO { get; set; }
    }
}
