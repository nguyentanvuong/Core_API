using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Entities
{
    public partial class SReportdata
    {
        public string reportcode { get; set; }
        public string spname { get; set; }
        public string sourcetype { get; set; }
        public string outputtype { get; set; }
        public string description { get; set; }
    }
}
