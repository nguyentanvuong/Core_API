using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Entities
{
    public partial class IpcSchedule
    {
        public string scheduleid { get; set; }
        public string scheduletype { get; set; }
        public DateTime scheduletime { get; set; }
        public string schedulename { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public string usercreate { get; set; }
        public string userapproved { get; set; }
        public string isapproved { get; set; }
        public string serviceid { get; set; }
        public string actiontype { get; set; }
        public string actionid { get; set; }
        public DateTime nextexecute { get; set; }
        public DateTime createdate { get; set; }
        public DateTime enddate { get; set; }
        public int? ord { get; set; }
    }
}
