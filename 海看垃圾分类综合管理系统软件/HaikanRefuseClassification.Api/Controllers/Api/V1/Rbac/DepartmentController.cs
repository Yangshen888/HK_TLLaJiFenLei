using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaikanRefuseClassification.Api.Entities;
using HaikanRefuseClassification.Api.Entities.Enums;
using HaikanRefuseClassification.Api.Extensions;
using HaikanRefuseClassification.Api.Extensions.AuthContext;
using HaikanRefuseClassification.Api.Extensions.CustomException;
using HaikanRefuseClassification.Api.Models.Response;
using HaikanRefuseClassification.Api.RequestPayload.Rbac.Department;
using HaikanRefuseClassification.Api.ViewModels.Rbac.Department;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace HaikanRefuseClassification.Api.Controllers.Api.V1.Rbac
{
    [Route("api/v1/rbac/[controller]/[action]")]
    [ApiController]
    [CustomAuthorize]
    public class DepartmentController : ControllerBase
    {
        private readonly RefuseClassificationContext _dbContext;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        public DepartmentController(RefuseClassificationContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult List(DepartmentRequestPaload payload)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var query = _dbContext.Department.AsQueryable();
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.DepartmentName.Contains(payload.Kw.Trim()));
                }
                if (payload.IsDeleted > CommonEnum.IsDeleted.All)
                {
                    query = query.Where(x => (CommonEnum.IsDeleted)x.IsDeleted == payload.IsDeleted);
                }

                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();
                var data = list.Select(_mapper.Map<Department, DepartmentJsonViewModel>);

                response.SetData(data, totalCount);
                return Ok(response);
            }
        }
        /// <summary>
        /// 创建科室
        /// </summary>
        /// <param name="model">科室视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Create(DepartmentCreateViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (model.DepartmentName.Trim().Length <= 0)
            {
                response.SetFailed("请输入科室名称");
                return Ok(response);
            }
            using (_dbContext)
            {
                if (_dbContext.Department.Count(x => x.DepartmentName == model.DepartmentName) > 0)
                {
                    response.SetFailed("科室名已存在");
                    return Ok(response);
                }
                var entity = new Department();
                entity.DepartmentUuid = Guid.NewGuid();
                entity.DepartmentName = model.DepartmentName;
                entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd");
                entity.AddPeople = AuthContextService.CurrentUser.DisplayName;
                entity.IsDeleted = 0;
                _dbContext.Department.Add(entity);
                _dbContext.SaveChanges();

                response.SetSuccess();
                return Ok(response);
            }
        }

        /// <summary>
        /// 编辑科室
        /// </summary>
        /// <param name="guid">角色惟一编码</param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        public IActionResult Edit(Guid guid)
        {
            using (_dbContext)
            {
                var entity = _dbContext.Department.FirstOrDefault(x => x.DepartmentUuid == guid);
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(_mapper.Map<Department, DepartmentCreateViewModel>(entity));
                return Ok(response);
            }
        }

        /// <summary>
        /// 保存编辑后的科室信息
        /// </summary>
        /// <param name="model">科室视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Edit(DepartmentCreateViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            using (_dbContext)
            {
                if (_dbContext.Department.Count(x => x.DepartmentName == model.DepartmentName && x.DepartmentUuid != model.DepartmentUuid) > 0)
                {
                    response.SetFailed("科室已存在");
                    return Ok(response);
                }

                var entity = _dbContext.Department.FirstOrDefault(x => x.DepartmentUuid == model.DepartmentUuid);
                entity.DepartmentName = model.DepartmentName;

                _dbContext.SaveChanges();
                return Ok(response);
            }
        }
        /// <summary>
        /// 获取科室列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult DepartmentList()
        {
            using (_dbContext)
            {
                var entity = _dbContext.Department.Where(x => x.IsDeleted == 0).ToList();
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(entity);
                return Ok(response);
            }
        }

        /// <summary>
        /// 删除科室
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
        /// 恢复科室
        /// </summary>
        /// <param name="ids">角色ID,多个以逗号分隔</param>
        /// <returns></returns>
        [HttpGet("{ids}")]
        [ProducesResponseType(200)]
        public IActionResult Recover(string ids)
        {
            var response = UpdateIsDelete(CommonEnum.IsDeleted.No, ids);
            return Ok(response);
        }

        /// <summary>
        /// 批量操作
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ids">科室ID,多个以逗号分隔</param>
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
                //case "forbidden":
                //    if (ConfigurationManager.AppSettings.IsTrialVersion)
                //    {
                //        response.SetIsTrial();
                //        return Ok(response);
                //    }
                //    response = UpdateStatus(UserStatus.Forbidden, ids);
                //    break;
                //case "normal":
                //    response = UpdateStatus(UserStatus.Normal, ids);
                //    break;
                default:
                    break;
            }
            return Ok(response);
        }
        #region 私有方法

        /// <summary>
        /// 删除科室
        /// </summary>
        /// <param name="isDeleted"></param>
        /// <param name="ids">科室ID字符串,多个以逗号隔开</param>
        /// <returns></returns>
        private ResponseModel UpdateIsDelete(CommonEnum.IsDeleted isDeleted, string ids)
        {
            using (_dbContext)
            {
                var parameters = ids.Split(",").Select((id, index) => new SqlParameter(string.Format("@p{0}", index), id)).ToList();
                var parameterNames = string.Join(", ", parameters.Select(p => p.ParameterName));
                var sql = string.Format("UPDATE Department SET IsDeleted=@IsDeleted WHERE DepartmentUUID IN ({0})", parameterNames);
                parameters.Add(new SqlParameter("@IsDeleted", (int)isDeleted));
                _dbContext.Database.ExecuteSqlRaw(sql, parameters);
                var response = ResponseModelFactory.CreateInstance;
                return response;
            }
        }
        #endregion
    }
}