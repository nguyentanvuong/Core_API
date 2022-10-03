using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.System
{
    public class ListRoleInvoke
    {
        [JsonProperty("data")]
        public List<RoleInvoke> data { get; set; }
    }
}
