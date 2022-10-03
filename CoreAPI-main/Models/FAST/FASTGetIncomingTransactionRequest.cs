using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.FAST
{
    public class FASTGetIncomingTransactionRequest
    {
        public string cm_user_name { get; set; }
        public string cm_password { get; set; }
        public string payee_participant_code { get; set; }
        public string ccy { get; set; }
    }
}
