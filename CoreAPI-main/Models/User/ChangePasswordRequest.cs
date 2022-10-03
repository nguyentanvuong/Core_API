namespace WebApi.Models.User
{
    public class ChangePasswordRequest
    {
        /// <summary>
        /// username of user to change password
        /// </summary>
        /// <example>ac01</example>
        [CoreRequired]
        public string username { get; set; }
        /// <summary>
        /// Current password of user
        /// </summary>
        /// <example>123456</example>
        [CoreRequired]
        public string oldpass { get; set; }
        /// <summary>
        /// New password of user
        /// </summary>
        /// <example>456789</example>
        [CoreRequired]
        public string newpass { get; set; }
        /// <summary>
        /// Password is encrypt or not. If you send plain password => set this to "false" 
        /// </summary>
        /// <example>ac01</example>
        public bool encrypt { get; set; } = true;
    }
}
