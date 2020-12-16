using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaikanRefuseClassification.Api.Entities;
using HaikanRefuseClassification.Api.Entities.Enums;
using HaikanRefuseClassification.Api.Extensions;
using HaikanRefuseClassification.Api.Extensions.AuthContext;
using HaikanRefuseClassification.Api.Extensions.CustomException;
using HaikanRefuseClassification.Api.RequestPayload.AttendanceManagement.AttendanceList;
using HaikanRefuseClassification.Api.ViewModels.AttendanceManagement.AttendanceList;
using HaikanRefuseClassification.Api.Extensions.CustomException;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HaikanRefuseClassification.Api.Controllers.Api.V1.AttendanceManagement
{
    [Route("api/v1/attendancemanagement/[controller]/[action]")]
    [ApiController]
    [CustomAuthorize]
    public class AttendanceListController : ControllerBase
    {
        private readonly RefuseClassificationContext _dbContext;
        private readonly IMapper _mapper;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        public AttendanceListController(RefuseClassificationContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult List(AttendanceListRequestPayload payload)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var query = from a in _dbContext.Attendance
                            join u in _dbContext.SystemUser
                            on a.SystemUserUuid equals u.SystemUserUuid
                            join d in _dbContext.Department
                            on u.DepartmentUuid equals d.DepartmentUuid
                            select new {
                                AttendanceUuid = a.AttendanceUuid,
                                UserName = u.RealName,
                                DepartmentName = d.DepartmentName,
                                DepartmentUuid = d.DepartmentUuid,
                                ColckDate = a.ColckDate,
                                StartTime = a.AmstartTime,
                                EndTime = a.AmendTime,
                                Id = a.Id
                            };
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.UserName.Contains(payload.Kw.Trim()));
                }
                if (payload.time != null)
                {
                    if (!string.IsNullOrEmpty(payload.time[0]) && !string.IsNullOrEmpty(payload.time[1]))
                    {
                        var date1 = Convert.ToDateTime(payload.time[0]).ToString("yyyy-MM-dd HH:mm:ss");
                        var date2 = Convert.ToDateTime(payload.time[1]).ToString("yyyy-MM-dd HH:mm:ss");
                        query = query.Where(x => x.ColckDate.CompareTo(date1) >= 0 && x.ColckDate.CompareTo(date2) <= 0);
                    }
                        
                }
                if (!string.IsNullOrEmpty(payload.DepartmentUuid))
                {
                    query = query.Where(x => x.DepartmentUuid.ToString()==payload.DepartmentUuid);
                }
                
                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();
                

                response.SetData(list, totalCount);
                return Ok(response);
            }

        }
        /// <summary>
        /// 详情_查看
        /// </summary>
        /// <param name="guid">申请惟一编码</param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        public IActionResult Detail(Guid guid)
        {
            using (_dbContext)
            {
                var entity = from a in _dbContext.Attendance
                             join u in _dbContext.SystemUser
                             on a.SystemUserUuid equals u.SystemUserUuid
                             join d in _dbContext.Department
                             on u.DepartmentUuid equals d.DepartmentUuid
                             where a.AttendanceUuid == guid
                             select new
                             {
                                 UserName = u.RealName,
                                 DepartmentName = d.DepartmentName,
                                 ColckDate = a.ColckDate,
                                 StartTime = a.AmstartTime,
                                 StartPlace=a.AmstartPlace,
                                 StartState=a.StartState,
                                 EndTime = a.AmendTime,
                                 EndPlace = a.AmendPlace,
                                 EndState = a.EndState,
                             };
                var query = entity.FirstOrDefault();
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(query);
                return Ok(response);
            }
        }

        /// <summary>
        /// 上班签到
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult ClockIn(ClockInViewModel model)
        {
            using (_dbContext)
            {
                var response = ResponseModelFactory.CreateInstance;
                string today = DateTime.Now.ToString("yyyy-MM-dd");
                //var worktime = _dbContext.WorkTime.FirstOrDefault();
                //if (worktime == null)
                //{
                //    response.SetFailed("未获取到上下班时间，请通知管理员设置。");
                //    return Ok(response);
                //}
                var query = _dbContext.Attendance.FirstOrDefault(x => x.SystemUserUuid == model.UserUUID && x.ColckDate == today);
                if (query == null)
                {
                    var entity = new Attendance();
                    entity.AttendanceUuid = Guid.NewGuid();
                    entity.SystemUserUuid = model.UserUUID;
                    entity.ColckDate = today;
                    entity.StartState = 0;
                    entity.EndState = 0;
                    entity.Type = model.Type;
                    entity.Name = model.UserName;
                    entity.GarbageRoomUuid = model.GarbageRoomUuid;
                    //上午
                    if (DateTime.Now.Hour < 12)
                    {
                        if (!string.IsNullOrEmpty(model.AmstartTime))
                        {
                            entity.AmstartTime = model.AmstartTime;
                            entity.AmstartPlace = model.AmstartPlace;
                            entity.StartState = 1;
                            //if (Convert.ToDateTime(model.AmstartTime) < Convert.ToDateTime(worktime.StartTime))
                            //{
                            //    entity.StartState = 1;
                            //}
                            //else
                            //{
                            //    entity.StartState = 2;
                            //}
                        }
                        else
                        {
                            response.SetFailed("未获取到上班打卡时间，请重新打卡。");
                            return Ok(response);
                        }
                    }
                    //下午
                    else
                    {
                        if (!string.IsNullOrEmpty(model.PmstartTime))
                        {
                            entity.PmstartTime = model.PmstartTime;
                            entity.PmstartPlace = model.PmstartPlace;
                            entity.StartState = 1;
                            //if (Convert.ToDateTime(model.PmstartTime) < Convert.ToDateTime(worktime.StartTime))
                            //{
                            //    entity.StartState = 1;
                            //}
                            //else
                            //{
                            //    entity.StartState = 2;
                            //}
                        }
                        else
                        {
                            response.SetFailed("未获取到上班打卡时间，请重新打卡。");
                            return Ok(response);
                        }
                    }
                    _dbContext.Attendance.Add(entity);
                    _dbContext.SaveChanges();
                }
                else
                {
                    //上午
                    if (DateTime.Now.Hour < 12)
                    {
                        if (string.IsNullOrEmpty(query.AmstartTime))
                        {
                            if (!string.IsNullOrEmpty(model.AmstartTime))
                            {
                                query.StartState = 1;
                                //if (Convert.ToDateTime(model.AmstartTime) < Convert.ToDateTime(worktime.StartTime))
                                //{
                                //    query.StartState = 1;
                                //}
                                //else
                                //{
                                //    query.StartState = 2;
                                //}
                                query.AmstartTime = model.AmstartTime;
                                query.AmstartPlace = model.AmstartPlace;

                            }
                            else
                            {
                                response.SetFailed("未获取到上班打卡时间，请重新打卡。");
                                return Ok(response);
                            }
                        }
                        else
                        {
                            response.SetFailed("上班已打卡，无需重复打卡。");
                            return Ok(response);
                        }
                    }
                    //下午
                    else
                    {
                        if (string.IsNullOrEmpty(query.PmstartTime))
                        {
                            if (!string.IsNullOrEmpty(model.PmstartTime))
                            {
                                query.StartState = 1;
                                //if (Convert.ToDateTime(model.PmstartTime) < Convert.ToDateTime(worktime.StartTime))
                                //{
                                //    query.StartState = 1;
                                //}
                                //else
                                //{
                                //    query.StartState = 2;
                                //}
                                query.PmstartTime = model.PmstartTime;
                                query.PmstartPlace = model.PmstartPlace;
                            }
                            else
                            {
                                response.SetFailed("未获取到上班打卡时间，请重新打卡。");
                                return Ok(response);
                            }
                        }
                        else
                        {
                            response.SetFailed("上班已打卡，无需重复打卡。");
                            return Ok(response);
                        }
                    }
                    _dbContext.SaveChanges();
                }
                response.SetSuccess("打卡成功");
                response.SetData(query);
                return Ok(response);
            }
        }

        /// <summary>
        /// 消息弹窗
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpPost("{guid}")]
        public IActionResult Message(string guid)
        {
            using (_dbContext)
            {
                string data = DateTime.Now.ToString("yyyy-MM-dd");
                var query = _dbContext.SupervisorVolunteer.Where(x => x.SystemUserUuid == Guid.Parse(guid) && x.ColckDate == data);
                var list = query.Select(x => new { x.RealName, x.Ap }).ToList();
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < list.Count; i++)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(",");
                    }
                    sb.Append(list[i].RealName);
                }
                sb.Append("志愿者今天与您一起值班");
                var message = new Message();
                message.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                message.Message1 = sb.ToString();
                message.MessageUuid = Guid.NewGuid();
                message.SystemUserUuid = Guid.Parse(guid);
                _dbContext.Message.Add(message);
                _dbContext.SaveChanges();
                var response = ResponseModelFactory.CreateInstance;
                //response.SetData(list);
                return Ok(response);
            }
        }

        /// <summary>
        /// 下班签退
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult ClockBack(ClockInViewModel model)
        {
            using (_dbContext)
            {
                var response = ResponseModelFactory.CreateInstance;
                string today = DateTime.Now.ToString("yyyy-MM-dd");
                //var worktime = _dbContext.WorkTime.FirstOrDefault();
                //if (worktime == null)
                //{
                //    response.SetFailed("未获取到上下班时间，请通知管理员设置。");
                //    return Ok(response);
                //}
                var query = _dbContext.Attendance.FirstOrDefault(x => x.SystemUserUuid == model.UserUUID && x.ColckDate == today);
                if (query == null)
                {
                    response.SetFailed("当前人员信息不存在，打卡失败。");
                    return Ok(response);
                }
                else
                {
                    if(model.TimeState=="am")
                    {
                        if (string.IsNullOrEmpty(query.AmstartTime))
                        {
                            response.SetFailed("未获取到上班打卡时间，请重新打卡。");
                            return Ok(response);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(query.AmendTime))
                            {
                                if (!string.IsNullOrEmpty(model.AmendTime))
                                {
                                    query.AmendTime = model.AmendTime;
                                    query.AmendPlace = model.AmendPlace;
                                    query.EndState = 1;
                                    //if (Convert.ToDateTime(model.AmendTime) > Convert.ToDateTime(worktime.EndTime))
                                    //{
                                    //    query.EndState = 1;
                                    //}
                                    //else
                                    //{
                                    //    query.EndState = 2;
                                    //}
                                }
                                else
                                {
                                    response.SetFailed("未获取到下班打卡时间，请重新打卡。");
                                    return Ok(response);
                                }
                            }
                            else
                            {
                                response.SetFailed("下班已打卡，无需重复打卡。");
                                return Ok(response);
                            }
                            _dbContext.SaveChanges();
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(query.PmstartTime))
                        {
                            response.SetFailed("未获取到上班打卡时间，请重新打卡。");
                            return Ok(response);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(query.PmendTime))
                            {
                                if (!string.IsNullOrEmpty(model.PmendTime))
                                {
                                    query.PmendTime = model.PmendTime;
                                    query.PmendPlace = model.PmendPlace;
                                    query.EndState = 1;
                                    //if (Convert.ToDateTime(model.PmendTime) > Convert.ToDateTime(worktime.EndTime))
                                    //{
                                    //    query.EndState = 1;
                                    //}
                                    //else
                                    //{
                                    //    query.EndState = 2;
                                    //}
                                }
                                else
                                {
                                    response.SetFailed("未获取到下班打卡时间，请重新打卡。");
                                    return Ok(response);
                                }
                            }
                            else
                            {
                                response.SetFailed("下班已打卡，无需重复打卡。");
                                return Ok(response);
                            }
                            _dbContext.SaveChanges();
                        }
                    }
                    

                }
                response.SetSuccess("打卡成功");
                response.SetData(query);
                return Ok(response);
            }

        }

    }
}