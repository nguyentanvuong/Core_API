using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Entities
{
    public partial class VFastIncoming
    {
        public long ipctransid { get; set; }
        public string tranref { get; set; }
        public string ipctransdate { get; set; }
        public string saccno { get; set; }
        public string sdname { get; set; }
        public string sdcountry { get; set; }
        public string sdtown { get; set; }
        public string sdpcode { get; set; }
        public string sdstreet { get; set; }
        public string sdbdmb { get; set; }
        public string sbank { get; set; }
        public string sbname { get; set; }
        public string sbcountry { get; set; }
        public string sbtown { get; set; }
        public string sbpcode { get; set; }
        public string sbstreet { get; set; }
        public string sbbdmb { get; set; }
        public string sccr { get; set; }
        public string raccno { get; set; }
        public string rcname { get; set; }
        public string rccountry { get; set; }
        public string rctown { get; set; }
        public string rcstreet { get; set; }
        public string rcbdmb { get; set; }
        public string rbank { get; set; }
        public string rbname { get; set; }
        public string rbtown { get; set; }
        public string rccr { get; set; }
        public string pmtmtd { get; set; }
        public string pmtinfid { get; set; }
        public string txrefid { get; set; }
        public string fcntxrefid { get; set; }
        public string fcnsts { get; set; }
        public string fcnstsud { get; set; }
        public string instrid { get; set; }
        public string corests { get; set; }
        public string corestsud { get; set; }
        public string chrgbr { get; set; }
        public string bacthbookg { get; set; }
        public string purpose { get; set; }
        public string strinfor { get; set; }
        public string createdt { get; set; }
        public string refnb { get; set; }
        public string refcd { get; set; }
        public string requestdt { get; set; }
        public string endtoendid { get; set; }
        public string msgid { get; set; }
        public decimal? samt { get; set; }
        public decimal? ramt { get; set; }
        public string status { get; set; }
    }
}
