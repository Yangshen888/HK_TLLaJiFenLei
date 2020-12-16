using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaikanRefuseClassification.Api.Entities;
using HaikanRefuseClassification.Api.Extensions;
using HaikanRefuseClassification.Api.Extensions.AuthContext;
using HaikanRefuseClassification.Api.RequestPayload.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HaikanRefuseClassification.Api.Controllers.Api.V1.AttendanceManagement
{
    [Route("api/v1/base/[controller]/[action]")]
    [ApiController]
    public class WingWarningController : ControllerBase
    {
        private readonly RefuseClassificationContext _dbContext;
        private readonly IMapper _mapper;
        public WingWarningController(RefuseClassificationContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        [HttpPost]
        public IActionResult List(WingWarnRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = _dbContext.WingNoRecord.AsQueryable();
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.Ljname.Contains(payload.Kw));
                }
                if (!string.IsNullOrEmpty(payload.Kw1))
                {
                    query = query.Where(x => x.Vname.Contains(payload.Kw));
                }
                //打卡日期筛选
                if (!string.IsNullOrEmpty(payload.Kw2[0]) && !string.IsNullOrEmpty(payload.Kw2[1]))
                {
                    var date1 = Convert.ToDateTime(payload.Kw2[0]).ToString("yyyy-MM-dd HH:mm:ss");
                    var date2 = Convert.ToDateTime(payload.Kw2[1]).ToString("yyyy-MM-dd HH:mm:ss");
                    query = query.Where(x => x.Times.CompareTo(date1) >= 0 && x.Times.CompareTo(date2) <= 0);
                }
                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(list, totalCount);
                return Ok(response);
            }
        }

        [HttpPost]
        public IActionResult Warning()
        {
            
            using (_dbContext)
            {
                var response = ResponseModelFactory.CreateResultInstance;
                if (AuthContextService.CurrentUser.RoleName == "督导员"|| AuthContextService.CurrentUser.RoleName == "超级管理员")
                {
                    string date = DateTime.Now.ToString("yyyy-MM-dd");
                    var query = _dbContext.WingNoRecord.Where(x => x.Times == date);
                    var list = query.ToList();

                    response.SetData(list, list.Count);
                    return Ok(response);
                }
                else
                {
                    response.SetData(null, 0);
                    return Ok(response);
                }


            }
        }
    }
}