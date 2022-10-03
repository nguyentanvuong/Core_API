using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Entities
{
    public partial class SLoopkup
    {
        public string lookupid { get; set; }
        public string tablename { get; set; }
        public string key { get; set; }
        public string colunmname { get; set; }
        public string query { get; set; }
    }
}
