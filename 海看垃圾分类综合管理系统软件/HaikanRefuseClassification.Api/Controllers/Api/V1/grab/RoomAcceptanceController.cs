using System;
using System.Collections.Generic;
using System.Data;

using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaikanRefuseClassification.Api.Configurations;
using HaikanRefuseClassification.Api.Entities;
using HaikanRefuseClassification.Api.Entities.Enums;
using HaikanRefuseClassification.Api.Extensions;
using HaikanRefuseClassification.Api.Extensions.AuthContext;
using HaikanRefuseClassification.Api.Models.Response;
using HaikanRefuseClassification.Api.RequestPayload.Grab;
using HaikanRefuseClassification.Api.ViewModels.Base;
using HaikanRefuseClassification.Api.ViewModels.Grab;
using HaikanRefuseClassification.Api.ViewModels.Person;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace HaikanRefuseClassification.Api.Controllers.Api.V1.grab
{
    [Route("api/v1/Grab/[controller]/[action]")]
    [ApiController]
    public class RoomAcceptanceController : ControllerBase
    {
        private readonly RefuseClassificationContext _dbContext;
        private readonly IMapper _mapper;

        public RoomAcceptanceController(RefuseClassificationContext dbContext, IMapper mapper )
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        /// <summary>
        /// 垃圾箱房收运记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult List(GrabRoomAcceptanceRequstPaload payload)
        {

            using (_dbContext)
            {
                var query = from g in _dbContext.GrabageRoom      //垃圾箱房
                            join v in _dbContext.Village          //小区
                            on g.VillageId equals v.VillageUuid
                            join u in _dbContext.GrabageWeighting      //称重
                            on g.GarbageRoomUuid equals u.GrabageRoomId
                            where u.IsDelete == "0"
                            select new
                            {
                                u.GrabageWeighingRecordUuid,
                                g.Ljname,   //箱房名称
                                g.GarbageRoomUuid,  //垃圾箱房编号
                                v.Vname,        //社区
                                v.Towns,           //街道
                                State = g.State == "1" ? "使用中" : g.State == "0" ? "暂停使用" : "",    
                                g.RottenRubbishPercent,    //易腐垃圾比例
                                u.AddTime,                //收运时间

                            };
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
                if (!string.IsNullOrEmpty(payload.Kw2[0]) && !string.IsNullOrEmpty(payload.Kw2[1]))
                {
                    var date1 = Convert.ToDateTime(payload.Kw2[0]).ToString("yyyy-MM-dd HH:mm:ss");
                    var date2 = Convert.ToDateTime(payload.Kw2[1]).ToString("yyyy-MM-dd HH:mm:ss");
                    query = query.Where(x => x.AddTime.CompareTo(date1) >= 0 && x.AddTime.CompareTo(date2) <= 0);
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
        /// 称重记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ListCZ(string guid , string time)
        {

            using (_dbContext)
            {
                var query = from g in _dbContext.GrabageWeighting      //称重记录
                            join v in _dbContext.GrabageRoom          //垃圾箱房箱房
                            on g.GrabageRoomId equals v.GarbageRoomUuid
                            join u in _dbContext.Village      //称重
                            on v.VillageId equals u.VillageUuid
                            where g.GrabageRoomId ==Guid.Parse(guid) && g.AddTime == time
                            select new
                            {
                                g.GrabageWeighingRecordUuid,
                                v.Ljname,         // 垃圾箱房名称
                                v.GarbageRoomUuid,  //垃圾箱房编号
                                u.Vname,        //社区名字
                                g.CarNumber,       //车牌
                                g.Type,            //垃圾类型
                                g.Weight,          //重量
                                g.AddTime,          //时间
                                v.IsDelete
                            };
                var entrty = query.FirstOrDefault();
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(entrty);
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
                var sql = string.Format("UPDATE GrabageWeighting SET IsDelete=@IsDelete WHERE GrabageWeighingRecordUuid IN ({0})", parameterNames);
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