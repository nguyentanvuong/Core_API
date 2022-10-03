using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using WebApi.Helpers.Common;
using WebApi.Models.System;
using WebApi.Services;

namespace WebApi.Controllers
{
    /// <summary>
    /// API for system management
    /// </summary>
    [ApiController]
    [Route("system")]
    public class SystemController: ControllerBase
    {
        private readonly ISystemService _systemService;

        public SystemController(ISystemService systemService)
        {
            _systemService = systemService;
        }

        /// <summary>
        /// Get list system code
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpGet("systemcode")]
        public IActionResult SystemCode()
        {
            var response = _systemService.GetListSystemCode();
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            if (response.errorcode == 9999) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Get list search function
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpGet("searchfunc")]
        public IActionResult SearchFunction()
        {
            var response = _systemService.GetListSearchFuntion();
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            if (response.errorcode == 9999) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Get list form
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpGet("form")]
        public IActionResult GetForm()
        {
            var response = _systemService.GetListForm();
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            if (response.errorcode == 9999) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Get list menu
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpGet("menu/{username}")]
        public IActionResult GetMenu(string username)
        {
            var response = _systemService.GetListMenu(username);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            if (response.errorcode == 9999) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Get List System bank
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpGet("getlistbank")]
        public IActionResult ListBank()
        {
            var response = _systemService.GetListBank();
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            if (response.errorcode == 9999) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Add System bank
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("bank/add")]
        public IActionResult BankAdd([FromBody] JObject model)
        {
            var response = _systemService.AddSystemBank(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode == 9999) return BadRequest(response);
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Modify System bank
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("bank/modify")]
        public IActionResult BankModify([FromBody] JObject model)
        {
            var response = _systemService.ModifySystemBank(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode == 2000) return NoContent();
            else if (response.errorcode == 9999) return BadRequest(response);
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// View System bank
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("bank/view")]
        public IActionResult ViewBank([FromBody] JObject model)
        {
            var response = _systemService.ViewBank(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode == 2000) return NoContent();
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Delete System bank
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("bank/delete")]
        public IActionResult DeleteBank([FromBody] JObject model)
        {
            var response = _systemService.DeleteBank(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode == 2000) return NoContent();
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Look up data
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("lookup")]
        public IActionResult LookupData([FromBody] LookupDataRequest model)
        {
            var response = _systemService.LookupData(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode == 2000) return NoContent();
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Get infor data
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("getinfor")]
        public IActionResult GetInforData([FromBody] GetInforDataRequest model)
        {
            var response = _systemService.MultiGetInfor(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode == 2000) return NoContent();
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Get Menu Invoke By Roleid
        /// </summary>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("getmenuinvoke")]
        public IActionResult GetMenuInvokeByRoleid([FromBody] JObject model)
        {
            var response = _systemService.GetMenuInvokeByRoleid(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Update List Menu Invoke
        /// </summary>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("updatemenuinvoke")]
        public IActionResult UpdateMenuInvoke([FromBody] ListRoleInvoke model)
        {
            var response = _systemService.UpdateMenuInvoke(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Get Report For Dashboard Page
        /// </summary>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpGet("getreportdashboard")]
        public IActionResult GetReportForDashboardPage()
        {
            var response = _systemService.GetReportForDashboardPage();
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Get List Role Detail
        /// </summary>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpGet("getlistroledetail")]
        public IActionResult GetListRoleDetail()
        {
            var response = _systemService.GetListRoleDetail();
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Get List Role Detail
        /// </summary>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("addroledetail")]
        public IActionResult RoleDetailRequest([FromBody] AddRoleDetailRequest model)
        {
            var response = _systemService.AddRoleDetail(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Get Menu Invoke By Roleid
        /// </summary>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("getmenupermission")]
        public IActionResult GetMenuInvoke()
        {
            var response = _systemService.GetMenuInvoke();
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Switch Mode FAST
        /// </summary>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("switchmodefast")]
        public IActionResult SwitchModeFAST()
        {
            var response = _systemService.SwitchModeFAST();
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Switch Mode FAST
        /// </summary>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("getalluserbyroleid")]
        public IActionResult GetAllUserByRoleID([FromBody] JObject model)
        {
            var response = _systemService.GetUserListByRoleID(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Switch Mode FAST
        /// </summary>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("deleteroledetail")]
        public IActionResult DeleteRoleDetail([FromBody] AddRoleDetailRequest model)
        {
            var response = _systemService.DeleteRoleDetail(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Get List Parameter
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpGet("parameterlist")]
        public IActionResult ListParameter()
        {
            var response = _systemService.GetListParameter();
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode == 9999) return BadRequest(response);
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Add System parameter
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("addparameter")]
        public IActionResult AddParameter([FromBody] AddParam model)
        {
            var response = _systemService.AddSystemParameter(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode == 9999) return BadRequest(response);
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Modify System Parameter
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("modifyparameter")]
        public IActionResult ModifyParameter([FromBody] AddParam model)
        {
            var response = _systemService.ModifySystemParameter(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode == 2000) return NoContent();
            else if (response.errorcode == 9999) return BadRequest(response);
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// View System parameter
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("viewparameter")]
        public IActionResult ViewParameter([FromBody] ViewParam model)
        {
            var response = _systemService.ViewParameter(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode == 2000) return NoContent();
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Delete System parameter
        /// </summary>
        /// <response code = "400" > Bad Request</response>
        /// <response code = "401" > Unauthorized </response >
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("deleteparameter")]
        public IActionResult DeleteParameter([FromBody] ViewParam model)
        {
            var response = _systemService.DeleteParameter(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode == 2000) return NoContent();
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Get List System code
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpGet("systemcodelist")]
        public IActionResult ListSystemCode()
        {
            var response = _systemService.GetListSystemCode1();
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode == 9999) return BadRequest(response);
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// View System system code
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("viewsystemcode")]
        public IActionResult ViewSystemCode([FromBody] ViewSystemCode model)
        {
            var response = _systemService.ViewSystemCode(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode == 2000) return NoContent();
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Add System system code
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("addsystemcode")]
        public IActionResult AddSystemCode([FromBody] AddSystemCode model)
        {
            var response = _systemService.AddSystemCode(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode == 2000) return NoContent();
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Modify System system code
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("modifysystemcode")]
        public IActionResult ModifySystemCode([FromBody] AddSystemCode model)
        {
            var response = _systemService.ModifySystemCode(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode == 2000) return NoContent();
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Delete System system code
        /// </summary>
        /// <response code = "400" > Bad Request</response>
        /// <response code = "401" > Unauthorized </response >
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("deletesystemcode")]
        public IActionResult DeleteSystemCode([FromBody] ViewSystemCode model)
        {
            var response = _systemService.DeleteSystemCode(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode == 2000) return NoContent();
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }


    }
}
