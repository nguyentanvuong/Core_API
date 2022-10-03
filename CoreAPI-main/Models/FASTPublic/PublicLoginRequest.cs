namespace WebApi.Models.FASTPublic
{
    public class PublicLoginRequest
    {
        [CoreRequired]
        public string username { get; set; }
        [CoreRequired]
        public string password { get; set; }
        public bool rememberMe { get; set; } = true;
    }
}
