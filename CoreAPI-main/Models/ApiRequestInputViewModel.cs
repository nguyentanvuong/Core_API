using Microsoft.AspNetCore.Http;

namespace WebApi.Helpers.Services
{
    internal class ApiRequestInputViewModel
    {
        public string HttpType { get; set; }
        public string Query { get; set; }
        public PathString RequestUrl { get; set; }
        public string RequestName { get; set; }
        public string RequestIP { get; set; }
        public string Body { get; internal set; }
        public string ResponseBody { get; internal set; }
        public long ElapsedTime { get; internal set; }
    }
}