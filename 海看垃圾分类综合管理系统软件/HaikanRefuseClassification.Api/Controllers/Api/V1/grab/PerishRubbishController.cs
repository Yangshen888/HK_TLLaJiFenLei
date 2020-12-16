using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AutoMapper;
using HaikanRefuseClassification.Api.Entities;
using HaikanRefuseClassification.Api.Extensions;
using HaikanRefuseClassification.Api.RequestPayload.Grab;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using NPOI.SS.Formula.Functions;

namespace HaikanRefuseClassification.Api.Controllers.Api.V1.grab
{
    /// <summary>
    /// 易腐垃圾统计信息
    /// </summary>
    [Route("api/v1/Grab/[controller]/[action]")]
    [ApiController]
    public class PerishRubbishController : ControllerBase
    {
        private readonly RefuseClassificationContext _dbContext;
        private readonly IMapper _mapper;

        public PerishRubbishController(RefuseClassificationContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// 易腐垃圾统计信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult PerishRubbishAllList(PerishRubbishRequestPayload payload)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                //var GarbageRoomUuidList = _dbContext.GrabageWeightSon.Where(x => x.GrabageRoomId != null).ToList();
                //var listss = GarbageRoomUuidList.Where((x, i) => GarbageRoomUuidList.FindIndex(z => z.GrabageRoomId == x.GrabageRoomId) == i).ToList();
                var query = from gw in _dbContext.GrabageWeightSon //垃圾称重筛选表
                            join gr in _dbContext.GrabageRoom //箱房表
                                on gw.GrabageRoomId equals gr.GarbageRoomUuid
                            join v in _dbContext.Village //社区表
                                on gr.VillageId equals v.VillageUuid
                            select new
                            {
                                gw.GrabageWeighingRecordUuid,
                                gw.GrabageRoomId,
                                AddTime = DateTime.Now.ToString("yyyy-MM-dd"), //当天记录时间
                                gw.Type, //垃圾类型
                                State = gr.State == "0" ? "使用中" : gr.State == "1" ? "暂停使用" : "", //社区状态
                                v.Vname, //社区名字
                                v.Towns, //街道名称
                                gr.Ljname, //垃圾箱房名字
                                gw.Weight, //称重重量
                                v.VillageUuid
                            };

                #region 查询条件-数据排序
                //街道筛选
                //if (!string.IsNullOrEmpty(payload.street))
                //{
                //    query = query.Where(x => x.Towns == payload.street);
                //}
                ////社区筛选
                //if (!string.IsNullOrEmpty(payload.ccmmunity))
                //{
                //    query = query.Where(x => x.Vname == payload.ccmmunity);
                //}
                ////社区管理员筛选
                //if (!string.IsNullOrEmpty(payload.vuuid))
                //{
                //    query = query.Where(x => x.VillageUuid == Guid.Parse(payload.vuuid));
                //}
                ////箱房名字筛选
                //if (!string.IsNullOrEmpty(payload.Kw))
                //{
                //    query = query.Where(x => x.Ljname.ToString().Contains(payload.Kw));
                //}
                ////所在社区筛选
                //if (!string.IsNullOrEmpty(payload.Kw1))
                //{
                //    query = query.Where(x => x.Vname.ToString().Contains(payload.Kw1));
                //}
                ////记录日期筛选
                //if (!string.IsNullOrEmpty(payload.time[0]))
                //{
                //    DateTime d1 = DateTime.Parse(payload.time[0]);
                //    DateTime d2 = DateTime.Parse(payload.time[1]);
                //    d2 = d2.AddDays(1);
                //    query = query.Where(x => DateTime.Parse(x.AddTime) >= d1 && DateTime.Parse(x.AddTime) <= d2);
                //}
                //if (payload.FirstSort != null)
                //{
                //    query = query.OrderByDescending(x => x.AddTime);
                //}
                #endregion

                ////分页
                //var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList().GroupBy(x => new { x.GrabageRoomId, x.Vname, x.Ljname }).Where(g => g.Count() >= 1).ToList();
                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();
                response.SetData(list, totalCount);
                return Ok(response);
            }
        }


        /// <summary>
        /// 易腐垃圾统计信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult PerishRubbishAllListTwo(PerishRubbishRequestPayload payload)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                //var query = _dbContext.PerishRubbishView.Where(x => x.GrabageRoomId != null);
                var query = from gw in _dbContext.PerishRubbishViewTwo
                            select new
                            {
                                gw.Towns,
                                addtimes=gw.Addtimes,
                                LJName=gw.Ljname,
                                VName=gw.Vname,
                                dataratio = gw.Dataratio != null ? gw.Dataratio.ToString().Substring(0, 5) + "%" : "0%",
                                weekdata=gw.Weekdata,
                                weekratio = gw.Weekratio != null ? gw.Weekratio.ToString().Substring(0, 5) + "%" : "0%",
                                yeardata=gw.Yeardata,
                                yearratio = gw.Yearratio != null ? gw.Yearratio.ToString().Substring(0, 5) + "%" : "0%",
                            };
                #region 查询条件-数据排序
                //街道筛选
                if (!string.IsNullOrEmpty(payload.street))
                {
                    query = query.Where(x => x.Towns == payload.street);
                }
                //社区筛选
                if (!string.IsNullOrEmpty(payload.ccmmunity))
                {
                    query = query.Where(x => x.VName == payload.ccmmunity);
                }
                //箱房名字筛选
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.LJName.Contains(payload.Kw));
                }
                #endregion
                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();
                response.SetData(list, totalCount);
                return Ok(response);
            }
        }

    }
}
