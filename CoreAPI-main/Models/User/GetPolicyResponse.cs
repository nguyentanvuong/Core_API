using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.User
{
    public class GetPolicyResponse
    {
        public int policyid { get; set; }
        public string descr { get; set; }
        public bool? isdefault { get; set; }
        public string effrom { get; set; }
        public string efto { get; set; }
        public int pwdhis { get; set; }
        public int pwdagemax { get; set; }
        public int minpwdlen { get; set; }
        public bool pwdcplx { get; set; }
        public bool pwdcplxlc { get; set; }
        public bool pwdcplxuc { get; set; }
        public bool pwdcplxsc { get; set; }
        public bool pwccplxsn { get; set; }
        public bool timelginrequire { get; set; }
        public string lginfr { get; set; }
        public string lginto { get; set; }
        public string llkoutthrs { get; set; }
        public string resetlkout { get; set; }
        public string usercreate { get; set; }
        public string datecreate { get; set; }
        public string usermodify { get; set; }
        public string datemodify { get; set; }
        public string contractid { get; set; }
        public bool? isbankedit { get; set; }
        public bool? iscorpedit { get; set; }
        public int? baseonpolicy { get; set; }
        public string pwdft { get; set; }   
       
    }
}
