using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Entities
{
    public partial class SUsrac
    {
        public int usrid { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public int? gender { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public DateTime? birthday { get; set; }
        public string phone { get; set; }
        public DateTime? lastlogintime { get; set; }
        public string status { get; set; }
        public string usercreated { get; set; }
        public DateTime? datecreated { get; set; }
        public string usermodified { get; set; }
        public DateTime? datemodified { get; set; }
        public bool? islogin { get; set; }
        public DateTime? expiretime { get; set; }
        public int branchid { get; set; }
        public int? deptid { get; set; }
        public int? userlevel { get; set; }
        public string coreuserid { get; set; }
        public string corelgname { get; set; }
        public string productid { get; set; }
        public bool? isshow { get; set; }
        public int? policyid { get; set; }
        public int? failnumber { get; set; }
    }
}
