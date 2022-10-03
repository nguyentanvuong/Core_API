namespace WebApi.Helpers.Common
{
    public class FASTStatus
    {
        //FAST status
        public const string Pending = "PDNG";
        public const string Rejected = "RJCT";
        public const string Received = "RCVD";
        public const string FailedAtACH = "FACH";
        public const string ReceivedAtACH = "ACSP";
        public const string AcknowledgedByFI = "ACSC";
        public const string Settled = "STTL";
        public const string AcknowledgedandSettled = "ACST";
        public const string RefundedByReceiver = "RCFI";
        public const string RefundByReceiver = "RFRC";
        public const string AuthorizedAndSent = "SENT";
        public const string FailedAtFI = "FLFI";
    }
}
