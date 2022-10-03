using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Entities
{
    public partial class SToken
    {
        public string bankid { get; set; }
        public string varname { get; set; }
        public string varext { get; set; }
        public string varvalue { get; set; }
        public DateTime? vardate { get; set; }
        public string description { get; set; }
    }
}
