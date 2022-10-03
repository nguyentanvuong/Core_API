using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Entities
{
    public partial class Ipcmappingmsg
    {
        public int fieldno { get; set; }
        public string ipctrancode { get; set; }
        public string sourcefield { get; set; }
        public string destfield { get; set; }
        public string defaultvalue { get; set; }
        public string fieldtype { get; set; }
        public string fieldformat { get; set; }
    }
}
