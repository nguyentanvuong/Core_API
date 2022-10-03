namespace WebApi.Models
{
    public class LoginRequest
    {
        /// <summary>
        /// Username to login O9Core
        /// </summary>
        /// <example>ac01</example>
        [CoreRequired]
        public string Username { get; set; }
        /// <summary>
        /// Password of user to login
        /// </summary>
        /// <example>123456</example>
        [CoreRequired]
        public string Password { get; set; }
        /// <summary>
        /// Password is encrypt or not. If you send plain password => set this to "false" 
        /// </summary>
        /// <example>ac01</example>
        public bool encrypt { get; set; } = true;

        public LoginRequest()
        {

        }
    }
}