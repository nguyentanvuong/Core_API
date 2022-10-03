using Newtonsoft.Json.Linq;

namespace WebApi.Models.FAST
{
    public class FASTReverseTransactionRequest
    {
        public string cm_user_name { get; set; }
        public string cm_password { get; set; }
        //pain.007.001.05
        public JObject content_message { get; set; }
    }
}
