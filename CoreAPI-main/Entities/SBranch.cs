using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Entities
{
    public partial class SBranch
    {
        public int branchid { get; set; }
        public string branchcd { get; set; }
        public string refid { get; set; }
        public string brname { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string mphone { get; set; }
        public string taxcode { get; set; }
        public string bcycid { get; set; }
        public string bcynm { get; set; }
        public string lcycid { get; set; }
        public string lcynm { get; set; }
        public string refcode { get; set; }
        public string country { get; set; }
        public string lang { get; set; }
        public int? timezn { get; set; }
        public string numfmtt { get; set; }
        public string numfmtd { get; set; }
        public string datefmt { get; set; }
        public string ldatefmt { get; set; }
        public string timefmt { get; set; }
        public string isonline { get; set; }
        public string udfield1 { get; set; }
    }
}
