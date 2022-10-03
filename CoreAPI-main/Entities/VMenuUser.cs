using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Entities
{
    public partial class VMenuUser
    {
        public string menuid { get; set; }
        public string menupath { get; set; }
        public long? menuorder { get; set; }
        public string menuparent { get; set; }
        public string type { get; set; }
        public string pageid { get; set; }
        public string actionid { get; set; }
        public string icon { get; set; }
        public string cmdtype { get; set; }
        public string actionsetting { get; set; }
        public string searchfunc { get; set; }
        public string caption { get; set; }
        public string username { get; set; }
    }
}
