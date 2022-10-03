

namespace WebApi.Helpers.Common
{
    public class AMLConfig
    {
        public string BASE_URL { get; set; }
        public string AMLUser { get; set; }
        public string AMLPass { get; set; }

        public void Init()
        {
            this.BASE_URL = BASE_URL.EndsWith("/") ? BASE_URL : BASE_URL + "/";
        }
        public string GetScanURL()
        {
            return BASE_URL + "AMLWS/AMLWS.svc";
        }
        public string GetCheckURL()
        {
            return BASE_URL + "AMLStatus/AMLStatus.svc";
        }
    }
}
