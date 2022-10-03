using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.System
{
    public class RoleInvoke
    {
        [JsonProperty("roleid")]
        public int roleid { get; set; }
        [JsonProperty("menuid")]
        public string menuid { get; set; }
        [JsonProperty("invoke")]
        public bool invoke { get; set; }
    }
}
