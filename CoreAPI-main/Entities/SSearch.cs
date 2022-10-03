using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Entities
{
    public partial class SSearch
    {
        public string searchfunc { get; set; }
        public string searchname { get; set; }
        public string ftag { get; set; }
        public string caption { get; set; }
        public string uid { get; set; }
        public decimal? width { get; set; }
        public bool? isadvsearch { get; set; }
        public string type { get; set; }
        public string soption { get; set; }
        public bool? ispk { get; set; }
        public bool? isorder { get; set; }
        public long? order { get; set; }
        public bool? isvisible { get; set; }
    }
}
