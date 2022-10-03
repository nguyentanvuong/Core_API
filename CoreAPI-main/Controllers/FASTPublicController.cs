using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Helpers.Common;
using WebApi.Models.FASTPublic;
using WebApi.Services;

namespace WebApi.Controllers
{

    /// <summary>
    /// List API public for NBC
    /// </summary>
    [ApiController]
    [Route("api/v1")]
    public class FASTPublicController : ControllerBase
    {
        private readonly IFASTPublicService _FASTPublicService;

        public FASTPublicController(IFASTPublicService FASTPublicService)
        {
            _FASTPublicService = FASTPublicService;
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [AllowAnonymous]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("auth")]
        public IActionResult FASTLogin([FromBody] PublicLoginRequest model)
        {
            var response = _FASTPublicService.FASTLogin(model);
            if (response.status.code != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Customer Account Inquiry 
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("account")]
        public IActionResult FASTAccountInquiry([FromBody] AccountInquiryRequest model)
        {
            var response = _FASTPublicService.AccountInquiry(model);
            if (response.status.code != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }
    }
}
