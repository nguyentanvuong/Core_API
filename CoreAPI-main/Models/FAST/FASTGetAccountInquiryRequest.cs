using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.FAST
{
    public class FASTGetAccountInquiryRequest
    {
        public string cm_user_name { get; set; }
        public string cm_password { get; set; }
        public string participant_code { get; set; }
    }
}
