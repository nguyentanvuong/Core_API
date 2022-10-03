using System;

namespace WebApi.Models.Util
{
    public class AuthenCodeInfo
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }
        public DateTime Expirytime { get; set; }
    }
}
