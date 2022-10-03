using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Entities
{
    public partial class SUserrole
    {
        public int roleid { get; set; }
        public string rolename { get; set; }
        public string roledescription { get; set; }
        public string usertype { get; set; }
        public string contractno { get; set; }
        public string usercreated { get; set; }
        public DateTime? datecreated { get; set; }
        public string usermodified { get; set; }
        public DateTime? datemodified { get; set; }
        public string serviceid { get; set; }
        public string status { get; set; }
        public string isshow { get; set; }
    }
}
