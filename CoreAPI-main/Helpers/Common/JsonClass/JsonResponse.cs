using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Helpers.Common
{
    public class JsonResponse
    {
        public int R { get; set; }
        public Object M { get; set; }


        public bool IsERROR()
        {
            return (R == (int)EnmJsonResponse.E) ? true : false;
        }

        public bool IsOK()
        {
            return (R == (int)EnmJsonResponse.O) ? true : false;
        }

        public bool IsWARN()
        {
            return (R == (int)EnmJsonResponse.W) ? true : false;
        }

        public string GetMessage()
        {
            if (M != null) return M.ToString();
            return String.Empty;
        }
    }
}
