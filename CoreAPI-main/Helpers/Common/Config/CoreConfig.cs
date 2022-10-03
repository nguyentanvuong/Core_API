 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Helpers.Common
{
    public class CoreConfig
    {
        public int WKDTimes { get; set; } = 30;
        public string CoreName { get; set; } = "OracleLinux";
        public string CoreIP { get; set; } = "127.0.0.1";
        public string CoreMac { get; set; } = "JITS";
        public string Serial { get; set; } = "Serial1";

        public string GetReplyString(string replyname, string seperator, int queues)
        {
            return replyname + Serial + seperator + CoreName + seperator + CoreMac + seperator + CoreIP + seperator + queues;
        }
    }
}
