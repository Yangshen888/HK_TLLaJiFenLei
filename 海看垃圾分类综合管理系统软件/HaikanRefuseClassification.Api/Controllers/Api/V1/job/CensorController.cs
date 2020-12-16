using AutoMapper;
using HaikanRefuseClassification.Api.Entities;
using HaikanRefuseClassification.Api.Entities.Enums;
using HaikanRefuseClassification.Api.Extensions;
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

namespace HaikanRefuseClassification.Api.Controllers.Api.V1.job
{
    [Route("api/v1/job/[controller]/[action]")]
    [ApiController]
    public class CensorController : ControllerBase
    {
        private readonly RefuseClassificationContext _dbContext;
        private readonly IMapper _mapper;
        public CensorController(RefuseClassificationContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// 督导员打卡记录查询
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
                            where a.Type == "0"
                            && a.IsDelete != "1"
                            && u.IsDeleted != 1
                            select new
                            {
                                a.SystemUserUuid,
                                a.AttendanceUuid,
                                a.Name,
                                a.AmstartTime,
                                a.AmstartPlace,
                                a.AmendTime,
                                a.AmendPlace,
                                a.PmstartTime,
                                a.PmstartPlace,
                                a.PmendTime,
                                a.PmendPlace,
                                StartState = a.StartState == 2 ? "迟到" : a.StartState == 1 ? "正常" : "未打卡",
                                u.RealName,
                                EndState = a.EndState == 2 ? "迟到" : a.EndState == 1 ? "正常" : "未打卡",
                                a.Type,
                                a.ColckDate
                            };
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.RealName.Contains(payload.Kw));
                }
                //打卡日期筛选
                if (!string.IsNullOrEmpty(payload.kw2[0]) && !string.IsNullOrEmpty(payload.kw2[1]))
                {
                    var date1 = Convert.ToDateTime(payload.kw2[0]).ToString("yyyy-MM-dd HH:mm:ss");
                    var date2 = Convert.ToDateTime(payload.kw2[1]).ToString("yyyy-MM-dd HH:mm:ss");
                    query = query.Where(x => x.ColckDate.CompareTo(date1) >= 0 && x.ColckDate.CompareTo(date2) <= 0);
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
        /// 督导员打卡记录详情
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        [HttpGet("{guid}")]
        //[HttpPost]Attendance model
        //[ProducesResponseType(200)]
        public IActionResult Detail(string guid)
        {
            using (_dbContext)
            {
                var query = (from a in _dbContext.Attendance
                             where a.AttendanceUuid == Guid.Parse(guid)
                             && a.IsDelete != "1"
                             select new
                             {
                                 a.SystemUserUu.RealName,
                                 StartState = a.StartState == 2 ? "迟到" : a.StartState == 1 ? "正常" : "未打卡",
                                 a.AmstartTime,
                                 a.AmstartPlace,
                                 a.AmendTime,
                                 a.AmendPlace,
                                 a.PmstartTime,
                                 a.PmstartPlace,
                                 a.PmendTime,
                                 a.PmendPlace,
                                 EndState = a.EndState == 2 ? "早退" : a.EndState == 1 ? "正常" : "未打卡",
                             });
                var entity = query.FirstOrDefault();
                //var entity = _dbContext.Attendance.FirstOrDefault(x => x.AttendanceUuid == model.AttendanceUuid);
                //var query = _mapper.Map<HaikanRefuseClassification.Api.Entities.Attendance, CensorShowViewModel>(entity);
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(entity);
                return Ok(response);
            }
        }
    }
}