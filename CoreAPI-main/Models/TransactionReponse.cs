using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using WebApi.Helpers.Common;

namespace WebApi.Models
{
    public  class TransactionReponse
    {
        public int errorcode { get; set; }
        public string messagedetail { get; set; }
        public object result { get; set; }

        public TransactionReponse()
        {
        }

        public void SetCode(CodeDescription code)
        {
            errorcode = code.errorcode;
            messagedetail = code.messagedetail;
        }

        public void SetResult(JObject value)
        {
            result = value;
        }

        public TransactionReponse(CodeDescription code, JObject jsresult = null)
        {
            errorcode = code.errorcode;
            messagedetail = code.messagedetail;
            SetResult(jsresult);
        }
    }
}
