using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Helpers.Common;

namespace WebApi.Models
{
    public class PagingRequest
    {
        public int page { get; set; } = GlobalVariable.pageStart;
        public int numberperpage { get; set; } = GlobalVariable.numberPerPageStart;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public JObject search { get; set; }
        public string searchfunc { get; set; }
    }
}
