using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace WebApi.Entities
{
    public class Users
    {
        [Key]
        [JsonIgnore]
        public int usrid { get; set; }

        public string usrbranch { get; set; }

        public int usrbranchid { get; set; }

        public string usrname { get; set; }

        public string lang { get; set; }
        public string txdt { get; set; }
        public string ssesionid { get; set; }

        [JsonIgnore]
        public string usrpass { get; set; }

        public Users()
        {

        }

        public Users(int Usrid, int UsrBranchid, string UsrBranch, string Usrname, string Usrpass, string Sessionid, string Lang, string Txdt)
        {
            usrid = Usrid;
            usrbranch = UsrBranch;
            usrbranchid = UsrBranchid;
            usrname = Usrname;
            usrpass = Usrpass;
            ssesionid = Sessionid;
            lang = Lang;
            txdt = Txdt;
        }
    }
}