

namespace WebApi.Helpers.Common
{
    public class CodeDescription
    {
        public int errorcode { get; set; }
        public string messagedetail { get; set; }
        public CodeDescription()
        {

        }

        public CodeDescription (int errorCode, string messgaeDetail)
        {
            errorcode = errorCode;
            messagedetail = messgaeDetail;
        }

    }
}
