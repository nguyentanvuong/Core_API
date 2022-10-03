using Apache.NMS;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using WebApi.Entities;
using WebApi.Helpers.Common;
using WebApi.Helpers.DatabaseUtils;
using WebApi.Helpers.Utils;

namespace WebApi.Services
{
    public class DerivedBackgroundPrinter : BackgroundService
    {
        private readonly IWorkerService worker;

        public DerivedBackgroundPrinter(IWorkerService worker)
        {
            this.worker = worker;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await worker.DoWork(stoppingToken);
        }
    }

    public interface IWorkerService
    {
        Task DoWork(CancellationToken cancellationToken);
    }

    public class WorkerService : IWorkerService
    {
        public readonly ILogger<WorkerService> _logger;
        public WorkerService(ILogger<WorkerService> logger)
        {
            _logger = logger;
        }

        public async Task DoWork(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (O9Client.isInit)
                {
                    string strResult = O9Utils.GenJsonBodyRequest(null, GlobalVariable.UTIL_GET_BUSDATE, "");
                    if (!string.IsNullOrEmpty(strResult))
                    {
                        string workingDate = O9Utils.ConvertTimeStampToShortString(long.Parse(strResult));
                        O9Utils.UpdateWorkingDate(workingDate);
                    }
                    GetO9CoreUser(_logger);
                }
                try
                {
                    GlobalVariable.AuthenCodeList.RemoveAll(x => x.Expirytime <= DateTime.Now);
                    FASTUtils.RefreshFASTToken();
                }
                catch(Exception ex)
                {
                    _logger.LogError(string.Format("{0}.{1}: \n\t{2}", MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, ex.ToString()));
                }
                await Task.Delay(1000 * GlobalVariable.TIME_UPDATE_TXDT, cancellationToken);
            }
        }

        private static void GetO9CoreUser(ILogger logger)
        {
            if (!string.IsNullOrEmpty(GlobalVariable.O9_GLOBAL_TXDT) && GlobalVariable.O9CoreLoginRequest != null)
            {
                JsonLoginRequest loginRequest = new JsonLoginRequest
                {
                    L = GlobalVariable.O9CoreLoginRequest.Username,
                    P = GlobalVariable.O9CoreLoginRequest.encrypt ? GlobalVariable.O9CoreLoginRequest.Password : O9Encrypt.MD5Encrypt(GlobalVariable.O9CoreLoginRequest.Password),
                    A = false
                };

                string strResult = O9Utils.GenJsonBodyRequest(loginRequest, GlobalVariable.UMG_LOGIN, "", EnmCacheAction.NoCached, EnmSendTypeOption.Synchronize, "-1", MsgPriority.Normal);
                JsonLoginResponse clsJsonLoginResponse = JsonConvert.DeserializeObject<JsonLoginResponse>(strResult);
                if (clsJsonLoginResponse != null)
                {
                    if (string.IsNullOrEmpty(clsJsonLoginResponse.E))
                    {
                        Users users = new Users(clsJsonLoginResponse.USRID, clsJsonLoginResponse.BRANCHID, clsJsonLoginResponse.BRANCHCD, GlobalVariable.O9CoreLoginRequest.Username, loginRequest.P,
                                                clsJsonLoginResponse.UUID, clsJsonLoginResponse.LANG, clsJsonLoginResponse.BUSDATE);
                        GlobalVariable.O9CoreUser = users;
                    }
                    else
                    {
                        logger.LogError(string.Format("{0}.{1}: \n\t{2}", MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, clsJsonLoginResponse.E));
                    }
                }
            }
            else
            {
                logger.LogError(string.Format("{0}.{1}: \n\t{2}", MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, "System not yet started. Please wait abit"));
            }
        }
    }
}
