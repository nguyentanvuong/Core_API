using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Entities
{
    public partial class SReportparam
    {
        public string spname { get; set; }
        public string paramname { get; set; }
        public string paramtype { get; set; }
        public string defaultvalue { get; set; }
        public string description { get; set; }
    }
}
