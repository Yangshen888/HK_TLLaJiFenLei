using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using HaikanRefuseClassification.Api.Entities;
using HaikanRefuseClassification.Api.Extensions;
using HaikanRefuseClassification.Api.RequestPayload.Grab;
using Microsoft.AspNetCore.Mvc;

namespace HaikanRefuseClassification.Api.Controllers.Api.V1.grab
{
    /// <summary>
    /// 统计收运信息
    /// </summary>
    [Route("api/v1/grab/[controller]/[action]")]
    [ApiController]
    public class CarWeightController : ControllerBase
    {
        private readonly RefuseClassificationContext _dbContext;
        private readonly IMapper _mapper;
        public CarWeightController(RefuseClassificationContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult List(WeighRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = from view in _dbContext.CarWeightView 
                    select new
                    {
                        view.CarNumber,
                        view.CarUuid,
                        GrabType = view.GrabType == "0" ? "其他垃圾" : view.GrabType == "1" ? "易腐垃圾" : view.GrabType == "2" ? "有害垃圾" : view.GrabType == "3" ? "可回收垃圾" : "",
                        view.Street,
                        view.Weight,
                        view.Wtime
                    };
                //车牌号筛选
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.CarNumber.Contains(payload.Kw));
                }
                //类型筛选
                if (!string.IsNullOrEmpty(payload.kw1))
                {
                    query = query.Where(x => x.GrabType.ToString()==payload.kw1);
                }
                //街道筛选
                if (!string.IsNullOrEmpty(payload.kw3))
                {
                    query = query.Where(x => x.Street == payload.kw3);
                }
                if (payload.FirstSort != null)
                {
                    query = query.OrderByDescending(x => x.Wtime);
                }
                //分页
                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(list, totalCount);
                return Ok(response);
            }
        }
    }
}
