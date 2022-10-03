using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using WebApi.Helpers.Common;
using WebApi.Models.User;
using WebApi.Services;

namespace WebApi.Controllers
{
    /// <summary>
    /// API for user management
    /// </summary>
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Login User Web
        /// </summary>
        /// <response code="400">Bad Request</response>
        [AllowAnonymous]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginRequest model)
        {
            var response = _userService.LoginUser(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode == 9999) return BadRequest(response);
            else if(response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Get List User Web
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpGet("userlist")]
        public IActionResult ListUser()
        {
            var response = _userService.GetListUserWeb();
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            return Ok(response);
        }

        /// <summary>
        /// Get List Policy
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpGet("userpolicy")]
        public IActionResult ListPolicy()
        {
            var response = _userService.GetListPolicy();
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            return Ok(response);
        }

        /// <summary>
        /// Get List User Role
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpGet("userrole")]
        public IActionResult ListUserRole()
        {
            var response = _userService.GetListUserRole();
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            return Ok(response);
        }

        /// <summary>
        /// Change password user Web
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("changepwd")]
        public IActionResult ChangePassword([FromBody] ChangePasswordRequest model)
        {
            var response = _userService.ChangePasswordUser(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode == 9999) return BadRequest(response);
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Add new user Web
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("adduser")]
        public IActionResult AddUser([FromBody] AddUserRequest model)
        {
            var response = _userService.AddUser(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Reset password user Web
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [AllowAnonymous]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("resetpassword")]
        public IActionResult ResetPassword([FromBody] ResetPasswordRequest model)
        {
            var response = _userService.ResetPassword(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode == 9999) return BadRequest(response);
            else if (response.errorcode != GlobalVariable.SuccessCode) return Ok(response);
            return Ok(response);
        }

        /// <summary>
        /// Modify user Web
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("modifyuser")]
        public IActionResult ModifyUser([FromBody] JObject model)
        {
            var response = _userService.ModifyUser(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode == 2000) return NoContent();
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// View user Web
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("viewuser")]
        public IActionResult ViewUser([FromBody] JObject model)
        {
            var response = _userService.ViewUser(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode == 2000) return NoContent();
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Delete user Web
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("deleteuser")]
        public IActionResult DeleteUser([FromBody] JObject model)
        {
            var response = _userService.DeleteUser(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode == 2000) return NoContent();
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// View user policy
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("viewpolicy")]
        public IActionResult ViewPolicy([FromBody] JObject model)
        {
            var response = _userService.ViewPolicy(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode == 2000) return NoContent();
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Insert user policy
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("addpolicy")]
        public IActionResult InsertPolicy([FromBody] JObject model)
        {
            var response = _userService.InsertPolicy(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Modify user policy
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("modifypolicy")]
        public IActionResult ModifyPolicy([FromBody] JObject model)
        {
            var response = _userService.UpdatePolicy(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode == 2000) return NoContent();
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Delete user policy
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("deletepolicy")]
        public IActionResult DeletePolicy([FromBody] JObject model)
        {
            var response = _userService.DeletePolicy(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode == 2000) return NoContent();
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// View User Role
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("viewuserrole")]
        public IActionResult ViewUserRole([FromBody] JObject model)
        {
            var response = _userService.ViewUserRole(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode == 2000) return NoContent();
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Insert User Role
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("adduserrole")]
        public IActionResult InsertUserRole([FromBody] JObject model)
        {
            var response = _userService.AddUserRole(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Modify User Role
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("modifyuserrole")]
        public IActionResult ModifyUserRole([FromBody] JObject model)
        {
            var response = _userService.ModifyUserRole(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode == 2000) return NoContent();
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Delete User Role
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("deleteuserrole")]
        public IActionResult DeleteUserRole([FromBody] JObject model)
        {
            var response = _userService.DeleteUserRole(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode == 2000) return NoContent();
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }
    }
}
