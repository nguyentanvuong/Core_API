

namespace WebApi.Models.Transaction.AML
{
    public class OnlineStatusResponse
    {
        public string rtnResult {get; set;}
        public OnlineStatusResponse()
        {

        }
        public OnlineStatusResponse(string rtnResult)
        {
            this.rtnResult = rtnResult;
        }
    }
}
