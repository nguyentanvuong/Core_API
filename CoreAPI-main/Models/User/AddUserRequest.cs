namespace WebApi.Models.User
{
    public class AddUserRequest
    {
        [CoreRequired]
        public string Username { get; set; }
        [CoreRequired]
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int Gender { get; set; }
        public string Address { get; set; }
        [CoreRequired]
        public string Email { get; set; }
        public string Birthday { get; set; }
        public string Phone { get; set; }
        public string Expiretime { get; set; }
        [CoreRequired]
        public int Branchid { get; set; }
        public int Deptid { get; set; }
        public int Userlevel { get; set; }
        public string Productid { get; set; }
        public int Policyid { get; set; }
    }
}
