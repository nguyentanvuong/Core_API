using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Entities
{
    public partial class Ipcoutputdefinexml
    {
        public string sourceid { get; set; }
        public string destid { get; set; }
        public string ipctrancode { get; set; }
        public bool online { get; set; }
        public int fieldno { get; set; }
        public string fielddesc { get; set; }
        public string fieldstyle { get; set; }
        public string fieldname { get; set; }
        public string valuestyle { get; set; }
        public string valuename { get; set; }
        public string valueobject { get; set; }
        public string formattype { get; set; }
        public string formatobject { get; set; }
        public string formatfunction { get; set; }
        public string formatparm { get; set; }
        public int arraylevel { get; set; }
        public int sublevel { get; set; }
        public string arraynode { get; set; }
        public string defaultvalue { get; set; }
    }
}
