namespace WebApi.Models.Transaction.AML
{
    public class Fault
    {
        public string faultcode { get; set; }
        public string faultstring { get; set; }
        public Fault()
        {

        }
        public Fault(string faultcode, string faultstring)
        {
            this.faultcode = faultcode;
            this.faultstring = faultstring;
        }
    }
}
