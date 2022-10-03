namespace WebApi.Models.FAST
{
    public class FASTGetOutgoingTransactionRequest
    {
        public string cm_user_name { get; set; }
        public string cm_password { get; set; }
        public string payer_participant_code { get; set; }
    }
}
