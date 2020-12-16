using AutoMapper;
using HaikanRefuseClassification.Api.Entities;
using HaikanRefuseClassification.Api.Entities.Enums;
using HaikanRefuseClassification.Api.Extensions;
using HaikanRefuseClassification.Api.Extensions.AuthContext;
using HaikanRefuseClassification.Api.Models.Response;
using HaikanRefuseClassification.Api.RequestPayload.Person;
using HaikanRefuseClassification.Api.ViewModels.Person;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using HaikanRefuseClassification.Api.Extensions;

namespace HaikanRefuseClassification.Api.Controllers.Api.V1.Person
{
    [Route("api/v1/Person/[controller]/[action]")]
    [ApiController]
    public class ResponseController : ControllerBase
    {
        private readonly RefuseClassificationContext _dbContext;
        private readonly IMapper _mapper;

        public ResponseController(RefuseClassificationContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// 问题反馈(模糊查询)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult List(ResponseRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = from cd in _dbContext.SystemUser
                            join q in _dbContext.QuestionPerson
                            on cd.SystemUserUuid equals q.HomeUserUuid
                            where cd.SystemRoleUuid.Contains(AuthContextService.CurrentUser.YH)
                            && cd.IsDeleted == 0 
                            select new
                            {
                               cd.SystemUserUuid,
                               cd.LoginName,
                               cd.RealName,
                               cd.HomeAddressUu.Address,
                               q.Id,
                               q.AddTime,
                               q.ProblemContent,
                               q.ProblemType,
                                cd.HomeAddressUu.Town,
                                cd.HomeAddressUu.Ccmmunity,
                                cd.HomeAddressUu.Resregion,
                                q.QuestionPersonUuid,
                               q.Remarks
                            };
                if (AuthContextService.CurrentUser.Streets == "" && AuthContextService.CurrentUser.Community == "" && AuthContextService.CurrentUser.Biotope != "")
                {
                    var Biotope = AuthContextService.CurrentUser.Biotope.Split(',');
                    query = query.Where(x => Biotope.Contains(x.Resregion));
                }
                else if (AuthContextService.CurrentUser.Streets == "" && AuthContextService.CurrentUser.Community != "" && AuthContextService.CurrentUser.Biotope == "")
                {
                    var Community = AuthContextService.CurrentUser.Community.Split(',');
                    query = query.Where(x => Community.Contains(x.Ccmmunity));
                }
                else if (AuthContextService.CurrentUser.Streets != "" && AuthContextService.CurrentUser.Community == "" && AuthContextService.CurrentUser.Biotope == "")
                {
                    var Streets = AuthContextService.CurrentUser.Streets.Split(',');
                    query = query.Where(x => Streets.Contains(x.Town));
                }
                else if (AuthContextService.CurrentUser.Streets != "" && AuthContextService.CurrentUser.Community != "" && AuthContextService.CurrentUser.Biotope == "")
                {
                    var Streets = AuthContextService.CurrentUser.Streets.Split(',');
                    var Community = AuthContextService.CurrentUser.Community.Split(',');
                    query = query.Where(x => Streets.Contains(x.Town) || Community.Contains(x.Ccmmunity));
                }
                else if (AuthContextService.CurrentUser.Streets != "" && AuthContextService.CurrentUser.Community == "" && AuthContextService.CurrentUser.Biotope != "")
                {
                    var Streets = AuthContextService.CurrentUser.Streets.Split(',');
                    var Biotope = AuthContextService.CurrentUser.Biotope.Split(',');
                    query = query.Where(x => Streets.Contains(x.Town) || Biotope.Contains(x.Resregion));
                }
                else if (AuthContextService.CurrentUser.Streets == "" && AuthContextService.CurrentUser.Community != "" && AuthContextService.CurrentUser.Biotope != "")
                {
                    var Community = AuthContextService.CurrentUser.Community.Split(',');
                    var Biotope = AuthContextService.CurrentUser.Biotope.Split(',');
                    query = query.Where(x => Community.Contains(x.Ccmmunity) || Biotope.Contains(x.Resregion));
                }
                else if (AuthContextService.CurrentUser.Streets != "" && AuthContextService.CurrentUser.Community != "" && AuthContextService.CurrentUser.Biotope != "")
                {
                    var Streets = AuthContextService.CurrentUser.Streets.Split(',');
                    var Community = AuthContextService.CurrentUser.Community.Split(',');
                    var Biotope = AuthContextService.CurrentUser.Biotope.Split(',');
                    query = query.Where(x => Streets.Contains(x.Town) || Community.Contains(x.Ccmmunity) || Biotope.Contains(x.Resregion));
                }
                //街道筛选
                if (!string.IsNullOrEmpty(payload.street))
                {
                    query = query.Where(x => x.Address.Contains( payload.street));
                }
                //所在社区筛选
                if (!string.IsNullOrEmpty(payload.ccmmunity))
                {
                    query = query.Where(x => x.Address.Contains(payload.ccmmunity));
                }
                //姓名筛选
                if (!string.IsNullOrEmpty(payload.kw))
                {
                    query = query.Where(x => x.LoginName.Contains(payload.kw));
                }
                if (payload.FirstSort != null)
                {
                    query = query.OrderByDescending(x => x.Id);
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
        /// App问题反馈(模糊查询)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AppList(AppResponseRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = from cd in _dbContext.SystemUser
                            where cd.SystemRoleUuid.Contains(AuthContextService.CurrentUser.YH)
                            where cd.IsDeleted == 0
                            where cd.ProblemContent.Length > 0
                            select new
                            {
                                cd.SystemUserUuid,
                                cd.RealName,
                                cd.ProblemContent,
                                cd.AddPeople,
                                cd.AddTime,
                                cd.Id,
                            };
                //姓名筛选
                if (!string.IsNullOrEmpty(payload.kw))
                {
                    query = query.Where(x => x.RealName.Contains(payload.kw));
                }
                //排序
                if (payload.FirstSort != null)
                {
                    query = query.OrderByDescending(x => x.Id);
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
        /// 添加(保存)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Create(ResponseViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;

            using (_dbContext)
            {
                var entitys = _dbContext.SystemUser.FirstOrDefault(x => x.SystemUserUuid == model.SystemUserUuid);
                if (entitys == null)
                {
                    response.SetFailed("没有此人");
                    return Ok(response);
                }
                var entity = new HaikanRefuseClassification.Api.Entities.QuestionPerson();
                entity.QuestionPersonUuid = Guid.NewGuid();
                entity.HomeUserUuid = model.SystemUserUuid;
                entity.ProblemContent = model.ProblemContent;
                entity.ProblemType = model.ProblemType;
                entity.Remarks = model.Remarks;
                entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                _dbContext.QuestionPerson.Add(entity);
                _dbContext.SaveChanges();

                
                //    var entity1 = _dbContext.SystemUser.FirstOrDefault(x => x.RealName == model.RealName && x.SystemRoleUuid == "C6BDB5B3-990B-4943-B2A1-1492044E38B8");
                //    if (entity1 == null)
                //    {
                //        response.SetFailed("此人不是用户");
                //        return Ok(response);
                //    }
                //    entity.RealName = model.RealName;
                //    entity.ProblemContent = model.ProblemContent;
                //    entity.ProblemType = model.ProblemType;
                //    entity.Remarks = model.Remarks;
                //    _dbContext.SaveChanges();
                
                //var entity = new HaikanRefuseClassification.Api.Entities.SystemUser();
                //entity.SystemUserUuid = Guid.NewGuid();
                //entity.RealName = model.RealName;
                //entity.ProblemContent = model.ProblemContent;
                //entity.ProblemType = model.ProblemType;
                //entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd");
                //entity.IsDeleted = 0;
                //entity.SystemRoleUuid = "C6BDB5B3-990B-4943-B2A1-1492044E38B8";
                //_dbContext.SystemUser.Add(entity);
                _dbContext.SaveChanges();
                response.SetSuccess("成功");
                return Ok(response);
            }
        }
       
        /// <summary>
        /// 保存编辑后的(保存)
        /// </summary>
        /// <param name="model">申请视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Edit(dynamic model)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                string guid = model.guid;
                var entity = _dbContext.QuestionPerson.FirstOrDefault(x => x.QuestionPersonUuid == Guid.Parse(guid));
                entity.Estimate = model.estimate;
                entity.Esttime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                entity.Estpeople = AuthContextService.CurrentUser.DisplayName;
                _dbContext.SaveChanges();
                response.SetSuccess("处理成功");
                return Ok(response);
            }
        }
        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="guid">申请惟一编码</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult Detail(Guid guid)
        {
            using (_dbContext)
            {
                var entity = from cd in _dbContext.SystemUser
                             join q in _dbContext.QuestionPerson
                             on cd.SystemUserUuid equals q.HomeUserUuid
                             where cd.SystemRoleUuid.Contains(AuthContextService.CurrentUser.YH)
                             && cd.IsDeleted == 0 
                             select new
                             {
                                 cd.SystemUserUuid,
                                 cd.LoginName,
                                 cd.RealName,
                                 cd.HomeAddressUu.Address,
                                 q.Id,
                                 q.AddTime,
                                 q.ProblemContent,
                                 q.ProblemType,
                                 q.QuestionPersonUuid,
                                 q.Estimate,
                                 q.Estpeople,
                                 q.Esttime,
                                 q.Remarks
                             };
                var query = entity.FirstOrDefault(x=>x.QuestionPersonUuid == guid);
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(query);
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
                var sql = string.Format("UPDATE SystemUser SET IsDeleted=@IsDeleted WHERE SystemUserUuid IN ({0})", parameterNames);
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
