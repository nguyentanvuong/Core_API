namespace WebApi.Models.User
{
    public class UserLoginRequest
    {
        [CoreRequired]
        public string username { get; set; }
        /// <summary>
        /// Password of user to login
        /// </summary>
        /// <example>123456</example>
        [CoreRequired]
        public string password { get; set; }
        /// <summary>
        /// Password is encrypt or not. If you send plain password => set this to "false" 
        /// </summary>
        /// <example>ac01</example>
        public bool encrypt { get; set; } = true;
        public UserLoginRequest()
        {
        }
    }
}
