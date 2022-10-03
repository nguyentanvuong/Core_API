using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using WebApi.Helpers.Common;
using WebApi.Services;

namespace WebApi.Controllers
{
    /// <summary>
    /// API for report service
    /// </summary>
    [ApiController]
    [Route("report")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        /// <summary>
        /// Get list report
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpGet("reportlist")]
        public IActionResult GetListReport()
        {
            var response = _reportService.GetListReport();
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            if (response.errorcode == 9999) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Get report token
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("getreporttoken")]
        public IActionResult GetReportToken([FromBody] JObject reportname)
        {
            var response = _reportService.GetReportToken(reportname);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            if (response.errorcode == 9999) return BadRequest(response);
            return Ok(response);
        }
    }
}
