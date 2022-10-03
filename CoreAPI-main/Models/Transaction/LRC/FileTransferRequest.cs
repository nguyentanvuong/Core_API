namespace WebApi.Models.Transaction.LRC
{
    public class FileTransferRequest
    {
        [CoreRequired]
        public string MSGCODE { get; set; }
        [CoreRequired]
        public string CONTENT { get; set; }
    }
}
