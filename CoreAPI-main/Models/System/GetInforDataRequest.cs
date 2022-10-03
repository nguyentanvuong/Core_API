using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.System
{
    public class GetInforDataRequest
    {
        [CoreRequired]
        public string inforid { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public JObject param { get; set; } = new JObject();
    }
}
