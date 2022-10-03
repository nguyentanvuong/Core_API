using System.Collections.Generic;

namespace WebApi.Models.Transaction.PMT
{
    public class SDRFile : TransactionRequest
    {
        [CoreRequired]
        public List<Payment> Payment { get; set; }
    }
}

