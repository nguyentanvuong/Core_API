
namespace WebApi.Models
{
    public class TransactionRequest
    {
        /// <summary>
        /// Session ID when user login to O9Core System. User must login to have this session ID
        /// </summary>
        /// <example>00000178-6dd0-7723-0000-01786dd0772c</example>
        public string SESSIONID { get; set; }
        /// <summary>
        /// Description of transaction
        /// </summary>
        /// <example>Description Follow Trans</example>
        public string DESCS { get; set; }
    }
}
