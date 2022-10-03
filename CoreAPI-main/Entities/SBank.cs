using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Entities
{
    public partial class SBank
    {
        public int bankid { get; set; }
        public string bankname { get; set; }
        public string bankcode { get; set; }
        public string serviceid { get; set; }
        public string uri { get; set; }
        public string mt103acceptpath { get; set; }
        public string mt103rejectpath { get; set; }
        public string banksts { get; set; }
        public string usercreated { get; set; }
        public DateTime? datecreated { get; set; }
        public string usermodified { get; set; }
        public DateTime? datemodified { get; set; }
        public string issender { get; set; }
        public string countrycode { get; set; }
        public string autogetquote { get; set; }
        public string autoacceptquote { get; set; }
    }
}
