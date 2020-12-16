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
using HaikanRefuseClassification.Api.ViewModels.Grab;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace HaikanRefuseClassification.Api.Controllers.Api.V1.grab
{
    [Route("api/v1/Grab/[controller]/[action]")]
    [ApiController]
    public class GrabRecordController : ControllerBase
    {
        private readonly RefuseClassificationContext _dbContext;
        private readonly IMapper _mapper;

        public GrabRecordController(RefuseClassificationContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        /// <summary>
        /// 垃圾箱房收运记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AllList(GrabRecordRequestPaload payload)
        {
            using (_dbContext)
            {
                var query = from gw in _dbContext.GrabageWeightSon      //称重表
                            join gr in _dbContext.GrabageRoom          //箱房表
                            on gw.GrabageRoomId equals gr.GarbageRoomUuid
                            join v in _dbContext.Village      //社区表
                            on gr.VillageId equals v.VillageUuid
                            select new
                            {
                                gw.GrabageWeighingRecordUuid,
                                gw.GrabageRoomId,
                                gw.AddTime,           //收运时间
                                gw.Type, 
                                State = gr.State == "0" ? "使用中" : gr.State == "1" ? "暂停使用" : "",
                                v.Vname,             //社区名字
                                v.Towns,             //街道名称
                                gr.Ljname,            //垃圾箱房名字
                                gw.Weight,            //称重重量
                                v.VillageUuid

                            };
                //街道筛选
                if (!string.IsNullOrEmpty(payload.street))
                {
                    query = query.Where(x => x.Towns==payload.street);
                }
                //社区筛选
                if (!string.IsNullOrEmpty(payload.ccmmunity))
                {
                    query = query.Where(x => x.Vname==payload.ccmmunity);
                }
                //社区管理员筛选
                if (!string.IsNullOrEmpty(payload.vuuid))
                {
                    query = query.Where(x => x.VillageUuid == Guid.Parse(payload.vuuid));
                }
                //箱房名字筛选
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.Ljname.ToString().Contains(payload.Kw));
                }
                //所在社区筛选
                if (!string.IsNullOrEmpty(payload.Kw1))
                {
                    query = query.Where(x => x.Vname.ToString().Contains(payload.Kw1));
                }
                //收运日期筛选
                if (!string.IsNullOrEmpty(payload.time[0]))
                {
                    DateTime d1 = DateTime.Parse(payload.time[0]);
                    DateTime d2 = DateTime.Parse(payload.time[1]);
                    d2 = d2.AddDays(1);
                    query = query.Where(x => DateTime.Parse(x.AddTime) >= d1 && DateTime.Parse(x.AddTime) <= d2);
                }
                if (payload.FirstSort != null)
                {
                    query = query.OrderByDescending(x => x.AddTime);
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
        /// 垃圾箱房收运记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult List(GrabRecordRequestPaload payload)
        {

            using (_dbContext)
            {
                var query = from gw in _dbContext.GrabageWeightSon      //称重表
                            join gr in _dbContext.GrabageRoom          //箱房表
                            on gw.GrabageRoomId equals gr.GarbageRoomUuid
                            join v in _dbContext.Village      //社区表
                            on gr.VillageId equals v.VillageUuid
                            select new
                            {
                                gw.GrabageWeighingRecordUuid,
                                gw.GrabageRoomId,
                                gw.AddTime,           //收运时间
                                gw.Type,
                                State = gr.State == "0" ? "使用中" : gr.State == "1" ? "暂停使用" : "",
                                v.Vname,             //社区名字
                                v.Towns,             //街道名称
                                gr.Ljname,            //垃圾箱房名字
                                gw.Weight,            //称重重量
                                v.VillageUuid

                            };
                //街道筛选
                if (!string.IsNullOrEmpty(payload.street))
                {
                    query = query.Where(x => x.Towns==payload.street);
                }
                //社区筛选
                if (!string.IsNullOrEmpty(payload.ccmmunity))
                {
                    query = query.Where(x => x.Vname==payload.ccmmunity);
                }
                //社区管理员筛选
                if (!string.IsNullOrEmpty(payload.vuuid))
                {
                    query = query.Where(x => x.VillageUuid == Guid.Parse(payload.vuuid));
                }
                //箱房名字筛选
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.Ljname.ToString().Contains(payload.Kw));
                }
                //所在社区筛选
                if (!string.IsNullOrEmpty(payload.Kw1))
                {
                    query = query.Where(x => x.Vname.ToString().Contains(payload.Kw1));
                }
                //收运日期筛选
                if (!string.IsNullOrEmpty(payload.time[0]))
                {
                    DateTime d1 = DateTime.Parse(payload.time[0]);
                    DateTime d2 = DateTime.Parse(payload.time[1]);
                    d2 = d2.AddDays(1);
                    query = query.Where(x => DateTime.Parse(x.AddTime) >= d1 && DateTime.Parse(x.AddTime) <= d2);
                }
                if (payload.FirstSort != null)
                {
                    query = query.OrderByDescending(x => x.AddTime);
                }
                //分页
                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(list, totalCount);
                return Ok(response);
            }
        }


        [HttpPost]
        public IActionResult RecordList(GrabRecordRequestPaload payload)
        {

            using (_dbContext)
            {

                var query = from dow in _dbContext.DayOfWeight
                            join gr in _dbContext.GrabageRoom
                            on dow.GrabageRoomId equals gr.GarbageRoomUuid
                            join v in _dbContext.Village
                            on gr.VillageId equals v.VillageUuid
                            where dow.GrabageRoomId != null
                            select new
                            {
                                GrabageRoomID=dow.GrabageRoomId,
                                WTime=dow.Wtime,
                                Proportion = dow.Proportion,
                                WingID=gr.WingId,
                                State = gr.State == "0" ? "使用中" : gr.State == "1" ? "暂停使用" : "",
                                v.Vname,
                                v.Towns,
                                gr.Ljname,
                                v.VillageUuid
                            };

                //街道筛选
                if (!string.IsNullOrEmpty(payload.street))
                {
                    query = query.Where(x => x.Towns == payload.street);
                }
                //社区筛选
                if (!string.IsNullOrEmpty(payload.ccmmunity))
                {
                    query = query.Where(x => x.Vname == payload.ccmmunity);
                }
                //社区管理员筛选
                if (!string.IsNullOrEmpty(payload.vuuid))
                {
                    query = query.Where(x => x.VillageUuid == Guid.Parse(payload.vuuid));
                }
                //箱房名字筛选
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.Ljname.ToString().Contains(payload.Kw));
                }
                //所在社区筛选
                if (!string.IsNullOrEmpty(payload.Kw1))
                {
                    query = query.Where(x => x.Vname.ToString().Contains(payload.Kw1));
                }
                //收运日期筛选
                if (payload.time!=null&&!string.IsNullOrEmpty(payload.time[0]))
                {
                    DateTime d1 = DateTime.Parse(payload.time[0]);
                    DateTime d2 = DateTime.Parse(payload.time[1]);
                    d2 = d2.AddDays(1); 
                    var date1 = Convert.ToDateTime(d1).ToString("yyyy-MM-dd HH:mm:ss");
                    var date2 = Convert.ToDateTime(d2).ToString("yyyy-MM-dd HH:mm:ss");
                    query = query.Where(x => x.WTime.CompareTo(date1) >= 0 && x.WTime.CompareTo(date2) <= 0);
                }
                if (payload.FirstSort != null)
                {
                    query = query.OrderByDescending(x => x.WTime);
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
        /// 称重记录详情
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Listcz(string guid, string time)
        {

            using (_dbContext)
            {
                var query = from gws in _dbContext.GrabageWeightSon      //称重表
                            join gr in _dbContext.GrabageRoom          //箱房表
                            on gws.GrabageRoomId equals gr.GarbageRoomUuid
                            join v in _dbContext.Village      //社区表
                            on gr.VillageId equals v.VillageUuid
                            where gws.GrabageRoomId == Guid.Parse(guid) && gws.WeightTime.Substring(0, 10) == time && gws.IsDelete == "0" && gws.IsCheck == 1
                            orderby gws.WeightTime descending
                            select new
                            {
                                gws.GrabageWeighingRecordUuid,
                                gws.AddTime,           //收运时间
                                Type = gws.Type == "0" ? "其他垃圾" : gws.Type == "1" ? "易腐垃圾" : gws.Type == "2" ? "有害垃圾" : gws.Type == "3" ? "可回收垃圾" : "",
                                v.Vname,             //社区名字
                                v.Towns,             //街道名称
                                gr.Ljname,            //垃圾箱房名字
                                gws.Weight,            //称重重量
                                gws.RecordType,
                                gws.CarNumber,
                                State = gr.State == "0" ? "使用中" : gr.State == "1" ? "暂停使用" : "",
                            };
                var list = query.ToList();
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(list);
                return Ok(response);
            }
        }

        /// <summary>
        /// 获取单条称重记录
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        public IActionResult Edit(Guid guid)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            var query = from gws in _dbContext.GrabageWeightSon      //称重表
                        join gr in _dbContext.GrabageRoom          //箱房表
                        on gws.GrabageRoomId equals gr.GarbageRoomUuid
                        join v in _dbContext.Village      //社区表
                        on gr.VillageId equals v.VillageUuid
                        where gws.GrabageWeighingRecordUuid == guid
                        select new
                        {
                            gws.GrabageWeighingRecordUuid,
                            gws.AddTime,           //收运时间
                            gws.Type,
                            v.Vname,             //社区名字
                            v.Towns,             //街道名称
                            gr.Ljname,            //垃圾箱房名字
                            Weight=double.Parse(gws.Weight),            //称重重量
                            gws.RecordType,
                            gws.CarNumber,
                             gr.State,
                        };
            var entity = query.FirstOrDefault();
            response.SetData(entity);
            return Ok(response);
        }

        /// <summary>
        /// 编辑保存单条称重记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Edit(GWeightSonModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            using (_dbContext)
            {
                var entity = _dbContext.GrabageWeightSon.FirstOrDefault(x => x.GrabageWeighingRecordUuid == model.GrabageWeighingRecordUuid);
                if (entity == null)
                {
                    response.SetFailed("数据不存在");
                    return Ok(response);
                }
                
                entity.Type = model.Type;
                entity.Weight = model.Weight.ToString();
                _dbContext.SaveChanges();
                
                
                response = ResponseModelFactory.CreateInstance;
                return Ok(response);
            }
        }



        /// <summary>
        /// 删除单个角色
        /// </summary>
        /// <param name="ids">角色ID,多个以逗号分隔</param>
        /// <returns></returns>
        [HttpGet("{ids}")]
        [ProducesResponseType(200)]
        public IActionResult Delete(string ids)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            response = UpdateIsDelete(CommonEnum.IsDeleted.Yes, ids);
            return Ok(response);
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="isDeleted"></param>
        /// <param name="ids">角色ID字符串,多个以逗号隔开</param>
        /// <returns></returns>
        private ResponseModel UpdateIsDelete(CommonEnum.IsDeleted isDeleted, string ids)
        {
            using (_dbContext)
            {
                var parameters = ids.Split(",").Select((id, index) => new SqlParameter(string.Format("@p{0}", index), id)).ToList();
                var parameterNames = string.Join(", ", parameters.Select(p => p.ParameterName));
                var sql = string.Format("UPDATE RubbishSyrecord SET IsDelete=@IsDelete WHERE GarbageSyuuid IN ({0})", parameterNames);
                parameters.Add(new SqlParameter("@IsDelete", (int)isDeleted));
                _dbContext.Database.ExecuteSqlRaw(sql, parameters);
                var response = ResponseModelFactory.CreateInstance;
                return response;
            }
        }
        /// <summary>
        /// 删除多条批量操作
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ids">角色ID,多个以逗号分隔</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult Batch(string command, string ids)
        {
            var response = ResponseModelFactory.CreateInstance;
            switch (command)
            {
                case "delete":
                    if (ConfigurationManager.AppSettings.IsTrialVersion)
                    {
                        response.SetIsTrial();
                        return Ok(response);
                    }
                    response = UpdateIsDelete(CommonEnum.IsDeleted.Yes, ids);
                    break;
                case "recover":
                    response = UpdateIsDelete(CommonEnum.IsDeleted.No, ids);
                    break;
                default:
                    break;
            }
            return Ok(response);
        }


    }
}