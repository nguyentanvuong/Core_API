using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Entities
{
    public partial class SForm
    {
        public string formname { get; set; }
        public string formtag { get; set; }
        public string formuid { get; set; }
        public string caption { get; set; }
        public string type { get; set; }
        public long? colunm { get; set; }
        public int? beforespan { get; set; }
        public int? afterspan { get; set; }
        public bool? line { get; set; }
        public int? order { get; set; }
        public bool? isvisibleonadd { get; set; }
        public bool? isvisible { get; set; }
        public bool? isreadonly { get; set; }
        public bool? isrequired { get; set; }
        public bool? isadd { get; set; }
        public bool? ismodify { get; set; }
        public string defaultvalue { get; set; }
        public string format { get; set; }
        public string validation { get; set; }
        public string controlaction { get; set; }
        public string prefield { get; set; }
        public string ctltype { get; set; }
    }
}
