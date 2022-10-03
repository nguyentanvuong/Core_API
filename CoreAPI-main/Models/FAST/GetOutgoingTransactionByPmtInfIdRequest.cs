namespace WebApi.Models.FAST
{
    public class GetOutgoingTransactionByPmtInfIdRequest
    {
        [CoreRequired]
        public long ipctransid { get; set; }
    }
}
