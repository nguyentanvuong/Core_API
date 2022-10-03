using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Transaction.FAST
{
    public class PMT_USTF : TransactionRequest
    {
        [CoreRequired]
        public string MSGCODE { get; set; }
        [CoreRequired]
        public string PMTREF { get; set; }
        [CoreRequired]
        public string FCNSTS { get; set; }
    }
}
