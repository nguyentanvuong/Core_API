using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using WebApi.Helpers.Common;
using WebApi.Models;
using WebApi.Models.FAST;
using WebApi.Models.Transaction.FAST;
using WebApi.Services;

namespace WebApi.Controllers
{
    /// <summary>
    /// List API for FAST
    /// </summary>
    [Route("fast")]
    [ApiController]
    public class FASTController : ControllerBase
    {
        private readonly IApiService _apiService;
        private readonly IFASTService _FASTService;
        public FASTController(IApiService apiService, IFASTService FASTService)
        {
            _apiService = apiService;
            _FASTService = FASTService;
        }

        /// <summary>
        /// Get list of bank join into FAST NBC
        /// </summary>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpGet("fastbank")]
        public IActionResult FastBank()
        {
            var response = _FASTService.GetListFASTBank();
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            if (response.errorcode == 9999) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Get list FAST outgoing transaction
        /// </summary>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("transaction/outgoing")]
        public IActionResult TransactionOutgoing([FromBody] PagingRequest model)
        {
            var response = _FASTService.GetTransactionOutgoing(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            if (response.errorcode == 9999) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Get list FAST incoming transaction
        /// </summary>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("transaction/incoming")]
        public IActionResult TransactionIncoming([FromBody] PagingRequest model)
        {
            var response = _FASTService.GetTransactionIncoming(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            if (response.errorcode == 9999) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Get FAST transaction by id
        /// </summary>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("transaction/view")]
        public IActionResult GetTransactionById([FromBody] JObject model)
        {
            var response = _FASTService.GetTransactionById(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            if (response.errorcode == 9999) return BadRequest(response);
            return Ok(response);
        }

        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("PMT_USTF")]
        public IActionResult PMT_USTF([FromBody] PMT_USTF model)
        {
            var response = _apiService.CallFunction(model, GlobalVariable.FUNC_PMT_USTF);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            if (response.errorcode == 9999) return BadRequest(response);
            if (response.errorcode == 1) return Unauthorized(response);
            return Ok(response);
        }

        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("PMT_RFT")]
        public IActionResult PMT_RFT([FromBody] PMT_RFT model)
        {
            var response = _apiService.CallFunction(model, GlobalVariable.FUNC_PMT_RFT);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            if (response.errorcode == 9999) return BadRequest(response);
            if (response.errorcode == 1) return Unauthorized(response);
            return Ok(response);
        }

        /// <summary>
        /// Get transaction details by transid
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("getsubtrans")]
        public IActionResult GetLogsubtransByIpctransid([FromBody] JObject model)
        {
            var response = _FASTService.GetLogsubtransByIpctransid(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Get fast bank by bankid
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("viewfastbank")]
        public IActionResult FastBankView([FromBody] JObject model)
        {
            var response = _FASTService.ViewFastBank(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode == 2000) return NoContent();
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Modify FAST bank
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("modifyfastbank")]
        public IActionResult FaskBankModify([FromBody] JObject model)
        {
            var response = _FASTService.ModifyFastBank(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode == 2000) return NoContent();
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Add FAST Bank
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("addfastbank")]
        public IActionResult FaskBankAdd([FromBody] JObject model)
        {
            var response = _FASTService.AddFastBank(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Delete FAST bank by bankid
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("deletefastbank")]
        public IActionResult FastBankDelete([FromBody] JObject model)
        {
            var response = _FASTService.DeleteFastBank(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode == 2000) return NoContent();
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// FAST Verify Account
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("getreceiveracno")]
        public IActionResult VerifyAccount([FromBody] FASTVerifyAccountRequest model)
        {
            var response = _FASTService.FASTVerifyAccount(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// FAST make Full Fund Transfer
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("makefullfundtransfer")]
        public IActionResult MakeFullFundTransfer([FromBody] JObject model)
        {
            var response = _FASTService.MakeFullFundTransfer(model).Result;
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// FAST Account Inquiry
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("accountinquiry")]
        public IActionResult FASTAccountInquiry()
        {
            var response = _FASTService.FASTAccountInquiry().Result;
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Pull Incoming Transaction from FAST
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("incomingtransaction")]
        public IActionResult FASTIncomingTransaction([FromBody] JObject model)
        {
            var response = _FASTService.FASTGetIncomingTransaction(model).Result;
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// FAST get Outgoing Transaction by PmtInfId
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("outgoingtransactionbypmtinfid")]
        public IActionResult FASTOutgingTransactionByPmtInfId(GetOutgoingTransactionByPmtInfIdRequest model)
        {
            var response = _FASTService.FASTGetOutgoingTransactionByPmtInfId(model).Result;
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// FAST Reverse Transaction
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("reversetransaction")]
        public IActionResult ReverseTransaction([FromBody] JObject model)
        {
            var response = _FASTService.FASTReverseTransaction(model).Result;
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// FAST Make Acknowledgement
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("makeacknowledgement")]
        public IActionResult FASTAcknowledgement([FromBody] JObject model)
        {
            var response = _FASTService.FASTAcknowledgement(model).Result;
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            else if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Sync FAST Outgoing transaction from O9Core
        /// </summary>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("syncoutgoingtran")]
        public IActionResult SyncOutgoingTran([FromBody] JObject model)
        {
            var response = _FASTService.SyncFASTOutgoingTransaction(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            if (response.errorcode == 9999) return BadRequest(response);
            if (response.errorcode == 21) return BadRequest(response);
            if (response.errorcode == 1) return Unauthorized(response);
            if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Get List FAST Account from NBC
        /// </summary>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpGet("getfastaccount")]
        public IActionResult GetFastAccount()
        {
            var response = _FASTService.GetListFASTAccount();
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            if (response.errorcode == 9999) return BadRequest(response);
            if (response.errorcode == 2000) return NoContent();
            if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Sync FAST incoming transaction from O9Core
        /// </summary>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("syncincomingtrantoo9")]
        public IActionResult SyncIncomingTranToO9([FromBody] JObject model)
        {
            var response = _FASTService.SyncFASTIncomingTransactionToO9Core(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            if (response.errorcode == 1) return Unauthorized(response);
            if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }

        ///// <summary>
        ///// Get incoming transaction status from O9Core
        ///// </summary>
        //[Authorize]
        //[Consumes("application/json", "application/xml")]
        //[Produces("application/json", "application/xml")]
        //[HttpPost("getincomingstatuso9")]
        //public IActionResult PMT_GIL([FromBody] PMT_GIL model)
        //{
        //    var response = _FASTService.GetIncomingTransactionStatusFromO9Core(model);
        //    if (response == null) return BadRequest(Codetypes.Err_Unknown);
        //    if (response.errorcode == 21) return BadRequest(response);
        //    if (response.errorcode == 9999) return BadRequest(response);
        //    if (response.errorcode == 1) return Unauthorized(response);
        //    if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
        //    return Ok(response);
        //}

        /// <summary>
        /// Sync transaction status to O9Core
        /// </summary>
        [Authorize]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("synctransactionstatustoo9")]
        public IActionResult SyncTransStatusToO9([FromBody] JObject model)
        {
            var response = _FASTService.SyncTransactionStatusToO9Core(model);
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            if (response.errorcode == 21) return BadRequest(response);
            if (response.errorcode == 9999) return BadRequest(response);
            if (response.errorcode == 1) return Unauthorized(response);
            if (response.errorcode != GlobalVariable.SuccessCode) return BadRequest(response);
            return Ok(response);
        }
    }
}
