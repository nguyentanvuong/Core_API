using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.FASTPublic
{
    public class Status
    {
        public int code { get; set; }
        public int? errorCode { get; set; }
        public string error { get; set; }
        public string warning { get; set; }
    }

    public class FASTReponse
    {
        public Status status { get; set; }
        public JObject data { get; set; } = null;

        public FASTReponse()
        {
            this.status = new Status();
        }

        public void SetData(object jObject)
        {
            this.status.code = 0;
            this.status.errorCode = null;
            this.status.error = null;
            this.status.warning = null;
            this.data = JObject.FromObject(jObject);
        }

        public void SetError(int code, int? errorCode, string error, string warning = "")
        {
            this.status.code = code;
            this.status.errorCode = errorCode;
            this.status.error = error;
            this.status.warning = warning;
        }
    }

}
