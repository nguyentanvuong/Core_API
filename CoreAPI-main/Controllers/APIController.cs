using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using WebApi.Helpers.Common;
using WebApi.Helpers.Utils;
using WebApi.Models;
using WebApi.Models.Transaction.PMT;
using WebApi.Models.Util;
using WebApi.Services;

namespace WebApi.Controllers
{
    /// <summary>
    /// List API that using General (Using for all module)
    /// </summary>
    [ApiController]
    [Route("api")]
    public class APIController : ControllerBase
    {
        private IApiService _apiService;

        public APIController(IApiService apiService)
        {
            _apiService = apiService;
        }     

        /// <summary>
        /// Get Working date of O9System
        /// </summary>
        [AllowAnonymous]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpGet("getworkingdate")]
        public IActionResult GetWorkingDate()
        {
            var response = _apiService.GetWorkingDate();
            if (response == null) return BadRequest(Codetypes.Err_Unknown);
            if (response.errorcode == 9999) return BadRequest(response);
            return Ok(response);
        }

        //get license for test
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Authorize]
        [HttpGet("getlicense")]
        public IActionResult GetLicense([FromBody] LicenseInfo model)
        {
            string licenseString = O9License.GenLicsence(model);
            if (licenseString == null) return BadRequest(Codetypes.Err_Unknown);
            var license = new JObject
            {
                { "license", licenseString }
            };
            TransactionReponse reponse = new TransactionReponse(Codetypes.Code_Success_Trans, license);
            return Ok(reponse);
        }

        #region Testing
        /// <summary>
        /// Testing API. Response all that this API Receive
        /// </summary>
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("ripple")]
        public IActionResult Ripple([FromBody] JObject json)
        {
            return Ok(json);
        }

        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [HttpPost("opencredit")]
        public IActionResult TestCredit([FromBody] TransactionRequest model)
        {
            RandomGenerator random = new RandomGenerator();
            string num = random.RandomPassword();
            string name = "Tester Credit account " + num;
            string request = "{\"CRDPRP\":\"100X\",\"CCVT\":0,\"PFSTDT\":\"30\\/05\\/2021\",\"CACCCR\":\"USD\",\"FFI\":0,\"CBKEXR\":1,\"CTMTYPE\":\"C\",\"CRLIMIT\":1000,\"ANIN\":0,\"TODT\":\"30\\/04\\/2022\",\"CRDCLS\":\"01\",\"ORATE\":0,\"CCATNM\":\"Agriculture ST Loan-Investment-Fixed ( USD )\",\"MARGIN\":12,\"PCSAMT\":999999999999999,\"CCTMCD\":\"99991000298\",\"GRAFFI\":0,\"DESCS\":\"5500: Open new credit account\",\"IFSTDT\":\"30\\/05\\/2021\",\"BULAMT\":0,\"CNCAT\":\"52113\",\"USRCD\":\"9999\",\"ACAMT\":1000}";
            JObject json = JObject.Parse(request);
            json.Add("CACNM", name);
            json.Add("FROMDT", GlobalVariable.O9_GLOBAL_TXDT);
            var response = _apiService.CallFrontOffice(json, model.SESSIONID, "CRD_OPN", true);
            if (response != null && response.result != null)
            {
                JObject result = (JObject)response.result;

                string pcacc = result.GetValue("PCACC").ToString();
                string request2 = "{\"CNSEQ\":null,\"CCVT\":1000,\"CACCCR\":\"USD\",\"PDACC\":null,\"CBKEXR\":1,\"TXAMT\":1000,\"CCCR\":\"USD\",\"CCATNM\":\"Agriculture ST Loan-Investment-Fixed ( USD )\",\"CCTMT\":\"C\",\"DESCS\":\"5550: Approve credit account\",\"CNCAT\":\"52113\",\"TELLER\":\"anhben\",\"USRCD\":\"9999\",\"ACAMT\":1000,\"STR1\":\"21050252113000013\",\"CCRNAME\":\"Dollar\",\"CCRUNIT\":\"Cent\"}";
                JObject json2 = JObject.Parse(request2);
                json2.Add("CACNM", name);
                json2.Add("ACNM", name);
                json2.Add("PCACC", pcacc);
                var response2 = _apiService.CallFrontOffice(json2, model.SESSIONID, "CRD_APR", true);
                if (response2 != null)
                {
                    string request3 = "{\"CCVT\":1000,\"MDESC\":{\"O1\":\"\",\"H1\":\"\"},\"PCSEXR\":1,\"CCSCVT\":1000,\"CCTMA\":\"\",\"PCSCCR\":\"USD\",\"CBKEXR\":1,\"CCRRATE\":1,\"CCCR\":\"USD\",\"CREPIDTYPE\":\"N\",\"PCSAMT\":1000,\"CCTMCD\":\"99991000298\",\"PCSCVT\":1000,\"DESCS\":\"5520: Cash disbursement\",\"TELLER\":\"anhben\",\"USRCD\":\"9999\",\"ACAMT\":1000,\"TPAYABLE\":1000}";
                    JObject json3 = JObject.Parse(request3);
                    json3.Add("CACNM", name);
                    json3.Add("ACNM", name);
                    json3.Add("PCACC", pcacc);
                    json.Add("CVLDT", GlobalVariable.O9_GLOBAL_TXDT);
                    var response3 = _apiService.CallFrontOffice(json3, model.SESSIONID, "CRD_CDR", true);
                    if (response3 == null) return BadRequest(Codetypes.Err_Unknown);
                    if (response3.errorcode == 9999) return BadRequest(response);
                    if (response3.errorcode == 1) return Unauthorized(response);

                    JObject account = new JObject();
                    account.Add("PDACC", pcacc);
                    account.Add("ACNAME", name);
                    response3.SetResult(account);
                    return Ok(response3);
                }  
            }
            return BadRequest(Codetypes.Err_Unknown);
        }
        #endregion

        /// <summary>
        /// Generate remittance via mobile banking
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Authorize]
        [HttpPost("SDRFile")]
        public IActionResult PMT_GRMB([FromBody] SDRFile model)
        {
            return Ok(model);
        }

    }
}
