using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.IPC
{
    public class GetScheduleReponse
    {
        public string scheduleid { get; set; }
        public string scheduletype { get; set; }
        public string scheduletime { get; set; }
        public string schedulename { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public string usercreate { get; set; }
        public string userapproved { get; set; }
        public string isapproved { get; set; }
        public string serviceid { get; set; }
        public string actiontype { get; set; }
        public string actionid { get; set; }
        public string nextexecute { get; set; }
        public string createdate { get; set; }
        public string enddate { get; set; }
        
    }
}
