using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Entities
{
    public partial class IpcEmailtemplate
    {
        public int bankid { get; set; }
        public string channelid { get; set; }
        public string trancode { get; set; }
        public string from { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string attachment { get; set; }
        public string sendattachment { get; set; }
        public string lang { get; set; }
        public string rowdetailtable { get; set; }
        public string rowdetailtableatachment { get; set; }
        public string attachmentpassword { get; set; }
        public string contentpassword { get; set; }
        public string titlepassword { get; set; }
        public string serviceid { get; set; }
    }
}
