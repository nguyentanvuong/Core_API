using Newtonsoft.Json.Linq;

namespace WebApi.Models.FAST
{
    public class FASTMakeFundTransferRequest
    {
        public string cm_user_name { get; set; }
        public string cm_password { get; set; }
        public JObject iso_message { get; set; }
        public string import_file_name { get; set; }
    }
}
