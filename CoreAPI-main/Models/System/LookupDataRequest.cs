using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.System
{
    public class LookupDataRequest
    {
        [CoreRequired]
        public string lookupid { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public JObject param { get; set; }
    }
}
