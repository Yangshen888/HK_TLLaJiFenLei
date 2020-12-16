using HaikanRefuseClassification.Api.Extensions.AuthContext;
using HaikanRefuseClassification.Api.Models.Response;
using HaikanRefuseClassification.Api.RequestPayload.Base;
using HaikanRefuseClassification.Api.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HaikanRefuseClassification.Api.RequestPayload.job;
using HaikanRefuseClassification.Api.ViewModels.job;
using HaikanRefuseClassification.Api.Entities;
using AutoMapper;
using HaikanRefuseClassification.Api.Extensions;

namespace HaikanRefuseClassification.Api.Controllers.Api.V1.job
{
    [Route("api/v1/job/[controller]/[action]")]
    [ApiController]
    public class PostulantController : ControllerBase
    {
        private readonly RefuseClassificationContext _dbContext;
        private readonly IMapper _mapper;
        public PostulantController(RefuseClassificationContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// 志愿者打卡记录查询
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult List(CensorRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = from a in _dbContext.Attendance
                            join u in _dbContext.SystemUser
                            on a.SystemUserUuid equals u.SystemUserUuid
                            where a.Type == "1"
                            && a.IsDelete != "1"
                            && u.IsDeleted!=1
                            select new
                            {
                                a.SystemUserUuid,
                                a.AttendanceUuid,
                                a.AmstartTime,
                                a.AmstartPlace,
                                a.AmendTime,
                                a.AmendPlace,
                                StartState = a.AmendTime == null && a.AmstartTime == null ? "未预约" : a.AmendTime == null || a.AmstartTime == null ? "未打卡" : "正常",
                                a.PmstartTime,
                                a.PmstartPlace,
                                a.PmendTime,
                                a.PmendPlace,
                                u.LoginName,
                                EndState = a.PmendTime == null && a.PmstartTime == null ? "未预约" : a.PmendTime == null || a.PmstartTime == null ? "未打卡" : "正常",
                                a.Type,
                                a.ColckDate
                            };
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.LoginName.Contains(payload.Kw));
                }
                //打卡日期筛选
                if (!string.IsNullOrEmpty(payload.kw2[0]) && !string.IsNullOrEmpty(payload.kw2[1]))
                {
                    var date1 = Convert.ToDateTime(payload.kw2[0]).ToString("yyyy-MM-dd HH:mm:ss");
                    var date2 = Convert.ToDateTime(payload.kw2[1]).ToString("yyyy-MM-dd HH:mm:ss");
                    query = query.Where(x =>  x.ColckDate.CompareTo(date1) >= 0 && x.ColckDate.CompareTo(date2) <= 0);
                }
                //分页
                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(list, totalCount);
                return Ok(response);
            }
        }
        /// <summary>
        /// 志愿者打卡记录详情
        /// </summary>
        /// <returns></returns>
        [HttpGet("{guid}")]
        //[HttpGet("{guid}")]
        //[ProducesResponseType(200)]
        public IActionResult Detail(string guid)
        {
            using (_dbContext)
            {
                var query = (from a in _dbContext.Attendance
                             join u in _dbContext.SystemUser
                             on a.SystemUserUuid equals u.SystemUserUuid
                             where a.AttendanceUuid == Guid.Parse(guid)
                             && a.IsDelete != "1"
                             select new
                             {
                                 a.SystemUserUu.RealName,
                                 StartState = a.AmendTime == null && a.AmstartTime == null ? "未预约" : a.AmendTime == null || a.AmstartTime == null ? "未打卡" : "正常",
                                 a.AmstartTime,
                                 a.AmstartPlace,
                                 a.AmendTime,
                                 a.AmendPlace,
                                 EndState = a.PmendTime == null && a.PmstartTime == null ? "未预约" : a.PmendTime == null || a.PmstartTime == null ? "未打卡" : "正常",
                                 a.PmstartTime,
                                 a.PmstartPlace,
                                 a.PmendTime,
                                 u.LoginName,
                                 a.PmendPlace,
                                 a.AttendanceUuid
                             });
                var entity = query.FirstOrDefault();
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(entity);
                return Ok(response);
            }
        }

        [HttpPost]
        public IActionResult EditAttend(CensorShowViewModel model)
        {
            using (_dbContext)
            {
                var response = ResponseModelFactory.CreateInstance;
                var entity = _dbContext.Attendance.FirstOrDefault(x=>x.AttendanceUuid==model.AttendanceUuid);
                if (model.StartState=="未打卡" && model.satend=="0")
                {
                    if (DateTime.Parse(model.AMEndTime)> DateTime.Parse(model.AMStartTime))
                    {
                        if (entity.AmstartTime == null && model.AMStartTime.Length == 5)
                        {
                            entity.AmstartTime = DateTime.Now.ToString("yyyy-MM-dd") + " " + model.AMStartTime + ":00";
                        }
                        entity.AmstartPlace = model.AMStartPlace;
                        if (entity.AmstartTime == null && model.AMEndTime.Length == 5)
                        {
                            entity.AmendTime = DateTime.Now.ToString("yyyy-MM-dd") + " " + model.AMEndTime + ":00";
                        }
                        entity.AmendPlace = model.AMEndPlace;
                        _dbContext.SaveChanges();
                        response.SetSuccess("补卡成功");
                    }
                    else
                    {
                        response.SetFailed("下班时间不能低于上班时间");
                    }
                }
                else if(model.EndState == "未打卡" && model.satend == "1")
                {
                    if (DateTime.Parse(model.PMEndTime) > DateTime.Parse(model.PMStartTime))
                    {
                        if (entity.PmstartTime == null && model.PMStartTime.Length == 5)
                        {
                            entity.PmstartTime = DateTime.Now.ToString("yyyy-MM-dd") + " " + model.PMStartTime + ":00";
                        }
                        entity.PmstartPlace = model.PMStartPlace;
                        if (entity.PmendTime == null && model.PMEndTime.Length==5)
                        {
                            entity.PmendTime = DateTime.Now.ToString("yyyy-MM-dd") + " " + model.PMEndTime + ":00";
                        }
                        entity.PmendPlace = model.PMEndPlace;
                        _dbContext.SaveChanges();
                        response.SetSuccess("补卡成功");
                    }
                    else
                    {
                        response.SetFailed("下班时间不能低于上班时间");
                    }
                }
                else
                {
                    response.SetFailed("补卡失败");
                }
                return Ok(response);
            }
        }
    }
}