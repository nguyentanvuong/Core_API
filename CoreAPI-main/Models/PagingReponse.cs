using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class PagingReponse
    {
        public int currentpage { get; set; }
        public int maxpage { get; set; }
        public int total { get; set; }
        public JArray data { get; set; }

        public PagingReponse()
        {

        }
        
    }
}
