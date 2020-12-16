using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaikanRefuseClassification.Api.Entities;
using HaikanRefuseClassification.Api.Entities.Enums;
using HaikanRefuseClassification.Api.Extensions;
using HaikanRefuseClassification.Api.Models.Response;
using HaikanRefuseClassification.Api.RequestPayload.Grab;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HaikanRefuseClassification.Api.Controllers.Api.V1.grab
{
    [Route("api/v1/grab/[controller]/[action]")]
    [ApiController]
    public class WeighController : ControllerBase
    {
        private readonly RefuseClassificationContext _dbContext;
        private readonly IMapper _mapper;
        public WeighController(RefuseClassificationContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult List(WeighRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = from a in _dbContext.GrabageWeighting
                            //join u in _dbContext.GrabageRoom
                            //on a.GrabageRoomId equals u.GarbageRoomUuid
                            where  a.RecordType == "车辆称重"
                            && a.IsDelete != "1"
                            && a.Weight!="0"
                            //&& u.IsDelete != "1"
                            select new
                            {
                                a.GrabageWeighingRecordUuid,
                                a.CarNumber,
                                //u.Ljname,
                                a.Weight,
                                a.AddTime,
                                a.Type,
                                GrabType=a.CarUu.GrabType=="0"?"其他垃圾": a.CarUu.GrabType == "1"?"易腐垃圾": a.CarUu.GrabType == "2" ? "有害垃圾" : a.CarUu.GrabType == "3" ? "可回收垃圾" : "",
                                WingID=a.GrabageRoom.WingId,
                                //u.VillageId,
                                a.CarUu.Company,
                                a.WeightTime,
                                a.CarUu.CarType
                            };
                //车牌号筛选
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.CarNumber.Contains(payload.Kw));
                }
                //注册时间筛选
                if (!string.IsNullOrEmpty(payload.kw2[0]) && !string.IsNullOrEmpty(payload.kw2[1]))
                {
                    var date1 = Convert.ToDateTime(payload.kw2[0]).ToString("yyyy-MM-dd HH:mm:ss");
                    var date2 = Convert.ToDateTime(payload.kw2[1]).ToString("yyyy-MM-dd HH:mm:ss");
                    query = query.Where(x => x.AddTime.CompareTo(date1) >= 0 && x.AddTime.CompareTo(date2) <= 0);
                }
                if (payload.FirstSort != null)
                {
                    query = query.OrderByDescending(x => x.WeightTime);
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