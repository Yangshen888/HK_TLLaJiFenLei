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
using HaikanRefuseClassification.Api.RequestPayload.AttendanceManagement.WorkTime;
using HaikanRefuseClassification.Api.ViewModels.AttendanceManagement.WorkTime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace HaikanRefuseClassification.Api.Controllers.Api.V1.AttendanceManagement
{
    [Route("api/v1/attendancemanagement/[controller]/[action]")]
    [ApiController]
    [CustomAuthorize]
    public class WorkTimeController : ControllerBase
    {
        private readonly RefuseClassificationContext _dbContext;
        private readonly IMapper _mapper;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        public WorkTimeController(RefuseClassificationContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult List(WorkTimeRequestPayload payload)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var query = _dbContext.WorkTime.AsQueryable();
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.StartTime.Contains(payload.Kw.Trim())|| x.EndTime.Contains(payload.Kw.Trim()));
                }
                if (payload.IsDeleted > CommonEnum.IsDeleted.All)
                {
                    query = query.Where(x => (CommonEnum.IsDeleted)x.IsDeleted == payload.IsDeleted);
                }

                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();
                var data = list.Select(_mapper.Map<WorkTime, WorkTimeJsonViewModel>);

                response.SetData(data, totalCount);
                return Ok(response);
            }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <param name="model">时间视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Create(WorkTimeCreateViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (model.StartTime.Trim().Length <= 0|| model.EndTime.Trim().Length <= 0)
            {
                response.SetFailed("请输入上下班时间");
                return Ok(response);
            }
            using (_dbContext)
            {
                if (_dbContext.WorkTime.Count() > 0)
                {
                    response.SetFailed("上下班时间已设置");
                    return Ok(response);
                }
                var entity = new WorkTime();
                entity.WorkTimeUuid = Guid.NewGuid();
                entity.StartTime = model.StartTime;
                entity.EndTime = model.EndTime;
                entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd");
                entity.AddPeople = AuthContextService.CurrentUser.DisplayName;
                entity.IsDeleted = 0;
                _dbContext.WorkTime.Add(entity);
                _dbContext.SaveChanges();

                response.SetSuccess();
                return Ok(response);
            }
        }

        /// <summary>
        /// 编辑时间
        /// </summary>
        /// <param name="guid">时间惟一编码</param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        public IActionResult Edit(Guid guid)
        {
            using (_dbContext)
            {
                var entity = _dbContext.WorkTime.FirstOrDefault(x => x.WorkTimeUuid == guid);
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(_mapper.Map<WorkTime, WorkTimeCreateViewModel>(entity));
                return Ok(response);
            }
        }

        /// <summary>
        /// 保存编辑后的上下班信息
        /// </summary>
        /// <param name="model">时间视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Edit(WorkTimeCreateViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            using (_dbContext)
            {
                if (_dbContext.WorkTime.Count(x=>x.WorkTimeUuid != model.WorkTimeUuid) > 0)
                {
                    response.SetFailed("时间已存在");
                    return Ok(response);
                }

                var entity = _dbContext.WorkTime.FirstOrDefault(x => x.WorkTimeUuid == model.WorkTimeUuid);
                entity.StartTime = model.StartTime;
                entity.EndTime = model.EndTime;
                _dbContext.SaveChanges();
                return Ok(response);
            }
        }
        /// <summary>
        /// 删除时间
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
        /// 恢复角色
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
                var sql = string.Format("UPDATE WorkTime SET IsDeleted=@IsDeleted WHERE WorkTimeUUID IN ({0})", parameterNames);
                parameters.Add(new SqlParameter("@IsDeleted", (int)isDeleted));
                _dbContext.Database.ExecuteSqlRaw(sql, parameters);
                var response = ResponseModelFactory.CreateInstance;
                return response;
            }
        }
        #endregion
    }
}