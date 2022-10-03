using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Entities;
using WebApi.Helpers.Common;
using WebApi.Helpers.DatabaseUtils;
using WebApi.Helpers.Utils;
using WebApi.Models;
using WebApi.Models.IPC;

namespace WebApi.Services
{
    public interface IIPCService
    {
        public TransactionReponse GetListSchedule();
        public TransactionReponse ViewSchedule(JObject model);
        public TransactionReponse ModifySchedule(JObject model);
        public TransactionReponse AddSchedule(JObject model);
        public TransactionReponse DeleteSchedule(JObject model);

    }
    public class IPCService : IIPCService
    {
        private DataContext _context;
        public readonly ILogger<IPCService> _logger;

        public IPCService(DataContext context, ILogger<IPCService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public TransactionReponse GetListSchedule()
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                List<IpcSchedule> schedules = _context.IpcSchedules.ToList();
                List<GetScheduleReponse> scheduleList = new List<GetScheduleReponse>();
                foreach (IpcSchedule schedule in schedules)
                {
                    GetScheduleReponse scheduleResponse = new GetScheduleReponse
                    {
                        actionid = schedule.actionid,
                        createdate = Utility.ConvertDateTimeToStringDatetime(schedule.createdate),
                        actiontype = schedule.actiontype,
                        description = schedule.description,
                        enddate = Utility.ConvertDateTimeToStringDatetime(schedule.enddate),
                        isapproved = schedule.isapproved,
                        nextexecute = Utility.ConvertDateTimeToStringDatetime(schedule.nextexecute),
                        scheduleid = schedule.scheduleid,
                        schedulename = schedule.schedulename,
                        scheduletime = Utility.ConvertDateTimeToStringDatetime(schedule.scheduletime),
                        scheduletype = schedule.scheduletype,
                        serviceid = schedule.serviceid,
                        status = schedule.status,
                        userapproved = schedule.userapproved,
                        usercreate = schedule.usercreate
                    };

                    scheduleList.Add(scheduleResponse);
                }
                JArray array = JArray.FromObject(scheduleList);
                JObject _schedules = new JObject
                {
                    { "data", array }
                };
                reponse.SetCode(Codetypes.Code_Success);
                reponse.SetResult(_schedules);
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                reponse.SetCode(new CodeDescription(9997, ex.Message));
                return reponse;
            }
        }

        public TransactionReponse ViewSchedule(JObject model)
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                string scheduleid = Utility.GetValueJObjectByKey(model,"scheduleid");
                if (string.IsNullOrEmpty(scheduleid))
                {
                    reponse.SetCode(Codetypes.Err_Requirescheduleid);
                    return reponse;
                }
                var schedule = _context.IpcSchedules.SingleOrDefault(x => x.scheduleid == scheduleid);
                if(schedule == null)
                {
                    reponse.SetCode(Codetypes.Err_DataNotFound);
                    return reponse;
                }
                JObject obj = Utility.FromatJObject(schedule);
                if (obj == null)
                {
                    return null;
                }
                reponse.SetCode(Codetypes.Code_Success);
                reponse.SetResult(obj);
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                reponse.SetCode(new CodeDescription(9997, ex.Message));
                return reponse;
            }
        }

        public TransactionReponse ModifySchedule(JObject model)
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                string scheduleid = Utility.GetValueJObjectByKey(model, "scheduleid");
                if (string.IsNullOrEmpty(scheduleid))
                {
                    reponse.SetCode(Codetypes.Err_Requirescheduleid);
                    return reponse;
                }
                var schedule = _context.IpcSchedules.SingleOrDefault(x => x.scheduleid == scheduleid);
                if (schedule == null)
                {
                    reponse.SetCode(Codetypes.Err_DataNotFound);
                    return reponse;
                }
                schedule = (IpcSchedule)Utility.SetValueObject(model, schedule);
                if (schedule == null)
                {
                    return null;
                }
                _context.SaveChanges();
                JObject obj = Utility.FromatJObject(schedule);
                if (obj == null)
                {
                    return null;
                }
                reponse.SetCode(Codetypes.Code_Success);
                reponse.SetResult(obj);
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                reponse.SetCode(new CodeDescription(9997, ex.Message));
                return reponse;
            }
        }

        public TransactionReponse AddSchedule(JObject model)
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                string scheduleid = Utility.GetValueJObjectByKey(model, "scheduleid");
                if (string.IsNullOrEmpty(scheduleid))
                {
                    reponse.SetCode(Codetypes.Err_Requirescheduleid);
                    return reponse;
                }
                int count = _context.IpcSchedules.Where(x => x.scheduleid == scheduleid).Count();
                if (count > 0)
                {
                    reponse.SetCode(Codetypes.Err_ScheduleExist);
                    return reponse;
                }
                IpcSchedule schedule = new IpcSchedule();
                schedule = (IpcSchedule)Utility.SetValueObject(model, schedule);
                schedule.createdate = DateTime.Now;
                if (schedule == null)
                {
                    return null;
                }
                _context.IpcSchedules.Add(schedule);
                _context.SaveChanges();
                JObject obj = Utility.FromatJObject(schedule);
                if (obj == null)
                {
                    return null;
                }
                reponse.SetCode(Codetypes.Code_Success);
                reponse.SetResult(obj);
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                reponse.SetCode(new CodeDescription(9997, ex.Message));
                return reponse;
            }
        }

        public TransactionReponse DeleteSchedule(JObject model)
        {
            TransactionReponse reponse = new TransactionReponse();
            try
            {
                string scheduleid = Utility.GetValueJObjectByKey(model, "scheduleid");
                if (string.IsNullOrEmpty(scheduleid))
                {
                    reponse.SetCode(Codetypes.Err_Requirescheduleid);
                    return reponse;
                }
                IpcSchedule schedule = _context.IpcSchedules.SingleOrDefault(x => x.scheduleid == scheduleid);
                if (schedule == null)
                {
                    reponse.SetCode(Codetypes.Err_DataNotFound);
                    return reponse;
                }
                _context.IpcSchedules.Remove(schedule);
                _context.SaveChanges();
                reponse.SetCode(Codetypes.Code_Success);
                return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                reponse.SetCode(new CodeDescription(9997, ex.Message));
                return reponse;
            }
        }
    }
}
