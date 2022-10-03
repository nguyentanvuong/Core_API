using Newtonsoft.Json.Linq;

namespace WebApi.Models.FAST
{
    public class FASTMakeAcknowledgementRequest
    {
        public string cm_user_name { get; set; }
        public string cm_password { get; set; }
        //pain.002.001.06
        public JObject content_message { get; set; }
    }
}
