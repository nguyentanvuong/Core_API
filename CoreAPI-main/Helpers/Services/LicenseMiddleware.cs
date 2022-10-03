using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Helpers.Common;

namespace WebApi.Helpers.Services
{
    public class LicenseMiddleware
    {
        private readonly RequestDelegate _next;

        public LicenseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string license = context.Request.Headers["License"].ToString();
            bool result = false;
            if (license != null && license != "")
            {
                result = Utils.O9License.CheckLicense(license);
            }
            context.Items["License"] = result;
            await _next(context);
        }
    }
}
