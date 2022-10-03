using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using WebApi.Helpers.Common;
using WebApi.Services;

namespace WebApi.Controllers
{
    /// <summary>
    /// API for IPC service
    /// </summary>
    [ApiController]
    [Route("ipc")]
    public class IPCController : ControllerBase
    {
        private readonly IIPCService _IPCService;

        public IPCController(IIPCService IPCService)
        {
            _IPCService = IPCService;
        }

        /// <summary>
        /// Get list schedule information
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpGet("getlistschedule")]
        public IActionResult ListSchedule()
        {
            var response = _IPCService.GetListSchedule();
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            return Ok(response);
        }

        /// <summary>
        /// View schedule information
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("viewschedule")]
        public IActionResult ViewSchedule([FromBody] JObject model)
        {
            var response = _IPCService.ViewSchedule(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode == 2000) return NoContent();
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Modify schedule
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("modifyschedule")]
        public IActionResult ModifySchedule([FromBody] JObject model)
        {
            var response = _IPCService.ModifySchedule(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode == 2000) return NoContent();
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Add schedule
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("addschedule")]
        public IActionResult AddSchedule([FromBody] JObject model)
        {
            var response = _IPCService.AddSchedule(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Delete schedule
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("deleteschedule")]
        public IActionResult FastBankDelete([FromBody] JObject model)
        {
            var response = _IPCService.DeleteSchedule(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode == 2000) return NoContent();
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }
    }
}
