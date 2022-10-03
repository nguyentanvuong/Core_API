using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class RestHeaderRequest
    {
        public string Authentication { get; set; }
        public string ContenType { get; set; } = "*/*";
    }
}
