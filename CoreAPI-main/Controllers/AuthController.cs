using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Services;
using WebApi.Models;
using Microsoft.AspNetCore.Http;
using WebApi.Helpers.Common;

namespace WebApi.Controllers
{
    /// <summary>
    /// API Authenticate
    /// </summary>
    [ApiController]
    [Route("users")]
    public class AuthController : ControllerBase
    {
        private IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Login to O9Core Banking System
        /// </summary>
        [AllowAnonymous]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("login")]
        public IActionResult Authenticate([FromBody] LoginRequest model)
        {
            var response = _userService.Authenticate(model);

            if (response == null) return BadRequest(Codetypes.Err_Authenticate);
            if (response.errorcode == 9999) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Multi Login to O9Core Banking System
        /// </summary>
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("multilogin")]
        public IActionResult MultiAuthenticate([FromBody] MultiLoginRequest model)
        {
            var response = _userService.MultiAuthenticate(model);

            if (response == null) return BadRequest(Codetypes.Err_Authenticate);
            return Ok(response);
        }

        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("checkpool")]
        public IActionResult CheckPool()
        {
            var response = _userService.Checkpool();

            if (response == null)
                return BadRequest(Codetypes.Err_Authenticate);
            return Ok(response);
        }

        private string IpAdress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
