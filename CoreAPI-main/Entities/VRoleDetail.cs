using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Entities
{
    public partial class VRoleDetail
    {
        public string menuid { get; set; }
        public int? roleid { get; set; }
        public bool? invoke { get; set; }
    }
}
