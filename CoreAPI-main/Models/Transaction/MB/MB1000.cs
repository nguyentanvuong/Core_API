

namespace WebApi.Models.Transaction.MB
{
    public class MB1000 : TransactionRequest
    {
        [CoreRequired]
        public string CUSCODE { get; set; }
        [CoreRequired]
        public string FILETYPE { get; set; }
        [CoreRequired]
        public string BASE64STRING { get; set; }
        [CoreRequired]
        public string BASE64STRINGSUB { get; set; }
    }
}
