namespace WebApi.Models.User
{
    public class ResetPasswordRequest
    {
        [CoreRequired]
        public string username { get; set; }
        [CoreRequired]
        public string email { get; set; }
        public string code { get; set; }
    }
}
