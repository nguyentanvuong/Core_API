using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Entities
{
    public partial class SDepartment
    {
        public int deprtid { get; set; }
        public string deprtcd { get; set; }
        public int branchid { get; set; }
        public string deprname { get; set; }
    }
}
