using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Entities
{
    public partial class VMenu
    {
        public string menuid { get; set; }
        public string menupath { get; set; }
        public long? menuorder { get; set; }
        public string menuparent { get; set; }
        public string type { get; set; }
        public string icon { get; set; }
        public string cmdtype { get; set; }
        public string caption { get; set; }
    }
}
