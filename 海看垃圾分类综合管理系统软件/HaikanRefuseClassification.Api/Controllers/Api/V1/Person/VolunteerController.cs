using AutoMapper;
using HaikanRefuseClassification.Api.Entities;
using HaikanRefuseClassification.Api.Entities.Enums;
using HaikanRefuseClassification.Api.Extensions;
using HaikanRefuseClassification.Api.Extensions.AuthContext;
using HaikanRefuseClassification.Api.Models.Response;
using HaikanRefuseClassification.Api.RequestPayload.Person;
using HaikanRefuseClassification.Api.ViewModels.Person;
using HaikanRefuseClassification.Api.ViewModels.Rbac.SystemUser;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace HaikanRefuseClassification.Api.Controllers.Api.V1.Person
{
    [Route("api/v1/Person/[controller]/[action]")]
    [ApiController]
    public class VolunteerController: ControllerBase 
    {
        private readonly RefuseClassificationContext _dbContext;
        private readonly IMapper _mapper;

        public VolunteerController(RefuseClassificationContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        /// <summary>
        /// 志愿者管理(模糊查询)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult List(VolunteerRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = _dbContext.SystemUser.Where(x=>x.UserType == 5 && x.SystemRoleUuid.Contains(AuthContextService.CurrentUser.ZYZ)).AsQueryable();
                //街道筛选
                if (!string.IsNullOrEmpty(payload.street))
                {
                    query = query.Where(x => x.HomeAddressUu.Address.Contains(payload.street));
                }
                //社区筛选
                if (!string.IsNullOrEmpty(payload.ccmmunity))
                {
                    query = query.Where(x => x.HomeAddressUu.Address.Contains(payload.ccmmunity));
                }
                //姓名筛选
                if (!string.IsNullOrEmpty(payload.kw))
                {
                    query = query.Where(x => x.LoginName.Contains(payload.kw));
                }
                //联系方式筛选
                if (!string.IsNullOrEmpty(payload.kw1))
                {
                    query = query.Where(x => x.Phone.Contains(payload.kw1));
                }
                //注册时间筛选
                if (!string.IsNullOrEmpty(payload.kw2[0]) && !string.IsNullOrEmpty(payload.kw2[1]))
                {
                    var date1 = Convert.ToDateTime(payload.kw2[0]).ToString("yyyy-MM-dd HH:mm:ss");
                    var date2 = Convert.ToDateTime(payload.kw2[1]).ToString("yyyy-MM-dd HH:mm:ss");
                    query = query.Where(x => x.AddTime.CompareTo(date1) >= 0 && x.AddTime.CompareTo(date2) <= 0);
                }
                query = query.Where(x => x.IsDeleted == 0);
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
        /// 添加志愿者管理(保存)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Create(VolunteerViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;

            using (_dbContext)
            {
                //是用户就添加志愿者身份
                var entity = _dbContext.SystemUser.FirstOrDefault(x => x.Phone == model.Phone && x.LoginName == model.LoginName && x.IsDeleted == 0);
                if (entity == null)
                {
                    response.SetFailed("姓名和手机号码不匹配");
                    return Ok(response);
                }
                entity.SystemRoleUuid = entity.SystemRoleUuid.TrimEnd(',') + "," + AuthContextService.CurrentUser.ZYZ;
                _dbContext.SaveChanges();
                //var entity = new HaikanRefuseClassification.Api.Entities.SystemUser();
                //entity.SystemUserUuid = Guid.NewGuid();
                //entity.RealName = model.RealName;
                //entity.Phone = model.Phone;
                //if (!string.IsNullOrEmpty(entity.AddTime))
                //{
                //    entity.AddTime = DateTime.Parse(model.AddTime).ToString("yyyy-MM-dd");
                //}
                //else
                //{
                //    entity.AddTime = model.AddTime;
                //}
                //entity.SystemRoleUuid = "964E6977-5867-4680-A258-B358D5FA1A4B";
                //entity.UserType = 2;
                //entity.IsDeleted = 0;
                //_dbContext.SystemUser.Add(entity);
                _dbContext.SaveChanges();
                response.SetSuccess();
                return Ok(response);
            }
        }
        /// <summary>
        /// 编辑志愿者管理(保存)
        /// </summary>
        /// <param name="guid">申请惟一编码</param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        public IActionResult Edit(Guid guid)
        {
            using (_dbContext)
            {
                var entity = _dbContext.SystemUser.FirstOrDefault(x => x.SystemUserUuid == guid);
                var query = _mapper.Map<HaikanRefuseClassification.Api.Entities.SystemUser, VolunteerEditView>(entity);
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(query);
                return Ok(response);
            }
        }
        /// <summary>
        /// 保存编辑后的志愿者管理(保存)
        /// </summary>
        /// <param name="model">申请视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Edit(VolunteerViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            using (_dbContext)
            {
                var entity = _dbContext.SystemUser.FirstOrDefault(x => x.SystemUserUuid == model.SystemUserUuid);
                entity.RealName = model.RealName;
                entity.Phone = model.Phone;
                entity.AddTime = model.AddTime;
                if (!string.IsNullOrEmpty(entity.AddTime))
                {
                    entity.AddTime = DateTime.Parse(model.AddTime).ToString("yyyy-MM-dd");
                }
                else
                {
                    entity.AddTime = model.AddTime;
                }
                _dbContext.SaveChanges();
                return Ok(response);
            }
        }
        /// <summary>
        /// 志愿者管理详情
        /// </summary>
        /// <param name="guid">申请惟一编码</param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        public IActionResult Detail(Guid guid)
        {
            using (_dbContext)
            {
                var entity = _dbContext.SystemUser.FirstOrDefault(x => x.SystemUserUuid == guid);
                var query = _mapper.Map<HaikanRefuseClassification.Api.Entities.SystemUser, VolunteerCreateViewModel>(entity);
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
