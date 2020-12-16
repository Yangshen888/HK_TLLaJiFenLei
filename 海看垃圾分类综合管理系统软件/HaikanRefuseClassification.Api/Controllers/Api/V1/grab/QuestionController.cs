using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaikanRefuseClassification.Api.Entities;
using HaikanRefuseClassification.Api.Entities.Enums;
using HaikanRefuseClassification.Api.Extensions;
using HaikanRefuseClassification.Api.Extensions.AuthContext;
using HaikanRefuseClassification.Api.Models.Response;
using HaikanRefuseClassification.Api.RequestPayload.Grab;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace HaikanRefuseClassification.Api.Controllers.Api.V1.grab
{
    [Route("api/v1/grab/[controller]/[action]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly RefuseClassificationContext _dbContext;
        private readonly IMapper _mapper;
        public QuestionController(RefuseClassificationContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult List(QuestionRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = from a in _dbContext.Question
                            join g in _dbContext.GrabageRoom
                            on a.QueRoomId equals g.GarbageRoomUuid
                            join c in _dbContext.Car
                            on a.CarUuid equals c.CarUuid
                            where  a.IsDelete != "1"
                            && g.IsDelete != "1"
                            && c.IsDelete !=1
                            select new
                            {
                                g.GarbageRoomUuid,
                                a.QuestionUuid,
                                a.QueType,
                                a.QueRoomId,
                                a.AddPeople,
                                a.AddTime,
                                c.CarNum,
                                g.Ljname,
                                g.VillageId,
                                g.Village.Towns,
                                g.Village.Vname,
                                a.Remarks,
                                a.Estimate
                            };
                if (AuthContextService.CurrentUser.Streets == "" && AuthContextService.CurrentUser.Community != "")
                {
                    var Community = AuthContextService.CurrentUser.Community.Split(',');
                    query = query.Where(x => Community.Contains(x.Vname));
                }
                else if (AuthContextService.CurrentUser.Streets != "" && AuthContextService.CurrentUser.Community == "")
                {
                    var Streets = AuthContextService.CurrentUser.Streets.Split(',');
                    query = query.Where(x => Streets.Contains(x.Towns));
                }
                else if (AuthContextService.CurrentUser.Streets != "" && AuthContextService.CurrentUser.Community != "")
                {
                    var Streets = AuthContextService.CurrentUser.Streets.Split(',');
                    var Community = AuthContextService.CurrentUser.Community.Split(',');
                    query = query.Where(x => Streets.Contains(x.Towns) || Community.Contains(x.Vname));
                }
                //街道筛选
                if (!string.IsNullOrEmpty(payload.street))
                {
                    query = query.Where(x => x.Towns == payload.street);
                }
                //所在社区筛选
                if (!string.IsNullOrEmpty(payload.ccmmunity))
                {
                    query = query.Where(x => x.Vname == payload.ccmmunity);
                }
                //社区管理员筛选
                if (!string.IsNullOrEmpty(payload.vuuid))
                {
                    query = query.Where(x => x.VillageId == Guid.Parse(payload.vuuid));
                }
                //问题类型筛选
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.QueType.Contains(payload.Kw));
                }
                //问题厢房筛选
                if (!string.IsNullOrEmpty(payload.kw1))
                {
                    query = query.Where(x => x.GarbageRoomUuid.ToString() == payload.kw1);
                }
                //分页
                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(list, totalCount);
                return Ok(response);
            }
        }

        //获取垃圾箱下拉框
        [HttpPost]
        public IActionResult Huoqu()
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var query = _dbContext.GrabageRoom.Where(x => x.IsDelete == "0").ToList();
                response.SetData(query);
                return Ok(response);
            }
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ShowQuestion(string guid)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var query = _dbContext.Question.FirstOrDefault(x => x.QuestionUuid==Guid.Parse(guid));
                response.SetData(query);
                return Ok(response);
            }
        }

        [HttpPost]
        public IActionResult EditQuestion(dynamic model)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                string guid = model.guid;
                var query = _dbContext.Question.FirstOrDefault(x => x.QuestionUuid == Guid.Parse(guid));
                query.Estimate = model.estimate;
                query.Esttime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                query.Estpeople = AuthContextService.CurrentUser.DisplayName;
                _dbContext.SaveChanges();
                response.SetSuccess("处理成功");
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
                var sql = string.Format("UPDATE Question SET IsDelete=@IsDeleted WHERE QuestionUUID IN ({0})", parameterNames);
                parameters.Add(new SqlParameter("@IsDeleted", (int)isDeleted));
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