namespace WebApi.Models.System
{
    public class AddRoleDetailRequest
    {
        [CoreRequired]
        public string usrname { get; set; }
        [CoreRequired]
        public int roleid { get; set; }
        public string description { get; set; }
    }
}
