using AutoMapper;
using HaikanRefuseClassification.Api.Entities;
using HaikanRefuseClassification.Api.Entities.Enums;
using HaikanRefuseClassification.Api.Extensions.AuthContext;
using HaikanRefuseClassification.Api.Models.Response;
using HaikanRefuseClassification.Api.RequestPayload.Base;
using HaikanRefuseClassification.Api.ViewModels.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;
using HaikanRefuseClassification.Api.Extensions;
using HaikanRefuseClassification.Api.Extensions.CustomException;
using Microsoft.Data.SqlClient;

namespace HaikanRefuseClassification.Api.Controllers.Api.V1.Base
{
    [Route("api/v1/Base/[controller]/[action]")]
    [ApiController]
    public class HouseholdController : ControllerBase
    {
        private readonly RefuseClassificationContext _dbContext;
        private readonly IMapper _mapper;

        public HouseholdController(RefuseClassificationContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        /// <summary>
        /// 住户信息管理(模糊查询)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize]
        public IActionResult List(HouseholdRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = from cd in _dbContext.SystemUser
                            where cd.SystemRoleUuid.Contains(AuthContextService.CurrentUser.YH)
                            where cd.IsDeleted == 0
                            where cd.UserType == 5
                            select new
                            {
                                cd.SystemUserUuid,
                                cd.RealName,//姓名
                                cd.Phone,
                                cd.OldCard,
                                cd.Wechat,
                                cd.AddTime,
                                //h.Address,
                                cd.LoginName,
                                cd.HomeAddressUu.Address,
                                all= GetAllTFScore(cd.SystemUserUuid),//投放积分
                                score = GetAllzyzScore(cd.SystemUserUuid)//服务积分

                            };
                //街道筛选
                if (!string.IsNullOrEmpty(payload.street))
                {
                    query = query.Where(x => x.Address.Contains(payload.street));
                }
                //社区筛选
                if (!string.IsNullOrEmpty(payload.ccmmunity))
                {
                    query = query.Where(x => x.Address.Contains(payload.ccmmunity));
                }
                //姓名筛选
                if (!string.IsNullOrEmpty(payload.kw))
                {
                    query = query.Where(x => x.LoginName.Contains(payload.kw));
                }
                //社区管理员筛选
                if (!string.IsNullOrEmpty(payload.vuuid))
                {
                    var ventity = _dbContext.Village.FirstOrDefault(x=>x.VillageUuid==Guid.Parse( payload.vuuid));

                    query = query.Where(x => x.Address.Contains(ventity.Vname));
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
                    query = query.Where(x =>  x.AddTime.CompareTo(date1) >=0 && x.AddTime.CompareTo(date2) <=0);
                }
                //排序
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
        //投放积分
        private static int GetAllTFScore(Guid ownid)
        {
            using (RefuseClassificationContext cc = new RefuseClassificationContext())
            {
                var query = from a in cc.GrabageDisposal
                            where a.SystemUserUuid == ownid && a.IsDelete != "1"
                            select new
                            {
                                Score=a.ScoreSettingUu.Integral
                            };
                int s = 0;
                foreach (var item in query)
                {
                    if (!string.IsNullOrEmpty(item.Score.ToString()))
                    {
                        s += (int)item.Score;
                    }
                }

                return s;
            }
        }

        private static int GetAllzyzScore(Guid id)
        {
            int s = 0;
            using (RefuseClassificationContext cc = new RefuseClassificationContext())
            {
                var query = cc.Overallsituation.FirstOrDefault();
                //string t = GetServiceTime(id);
                s = query.HourScore * GetAllCount(id);
                return s;
            }
        }
        /// <summary>
        /// 获取打卡次数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private static int GetAllCount(Guid id)
        {
            using (RefuseClassificationContext cc = new RefuseClassificationContext())
            {
                int count = 0;//打卡次数
                //搜索志愿者打卡次数
                var a = cc.Attendance.Where(x => x.Type == "1" && x.IsDelete != "1" && x.SystemUserUuid == id).ToList();
                foreach (var item in a)
                {
                    //上/下午有一次打卡记录就加1，一天最多2次
                    if (!string.IsNullOrEmpty(item.AmstartTime) || !string.IsNullOrEmpty(item.AmendTime))
                        count++;
                    if (!string.IsNullOrEmpty(item.PmstartTime) || !string.IsNullOrEmpty(item.PmendTime))
                        count++;
                }
                return count;
            }
        }
        //服务积分
        private static int GetAllFWScore(Guid id)
        {
            using (RefuseClassificationContext cc = new RefuseClassificationContext())
            {
                var query = (from a in cc.VolunteerService
                             where a.SystemUserUuid == id && a.IsDelete != "1"
                             select a).ToList();
                int s = 0;
                foreach (var item in query)
                {
                    if (!string.IsNullOrEmpty(item.Score))
                    {
                        s += int.Parse(item.Score);
                    }
                }

                return s;
            }
        }
        /// <summary>
        /// 添加住户信息管理(保存)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize]
        [ProducesResponseType(200)]
        public IActionResult Create(HouseholdViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;

            using (_dbContext)
            {
                var entity = new HaikanRefuseClassification.Api.Entities.SystemUser();
                entity.SystemUserUuid = Guid.NewGuid();
                entity.RealName = model.RealName;
                entity.Phone = model.Phone;
                entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd");
                entity.SystemRoleUuid = "C6BDB5B3-990B-4943-B2A1-1492044E38B8";
                entity.IsDeleted = 0;
                _dbContext.SystemUser.Add(entity);
                _dbContext.SaveChanges();
                response.SetSuccess();
                return Ok(response);
            }
        }
        /// <summary>
        /// 编辑住户信息管理(保存)
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
                var query = _mapper.Map<HaikanRefuseClassification.Api.Entities.SystemUser, HouseholdEditView>(entity);
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(query);
                return Ok(response);
            }
        }
        /// <summary>
        /// 保存编辑后的住户信息管理(保存)
        /// </summary>
        /// <param name="model">申请视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize]
        [ProducesResponseType(200)]
        public IActionResult Edit(HouseholdViewModel model)
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
                entity.Wechat = model.Wechat;
                _dbContext.SaveChanges();
                return Ok(response);
            }
        }
        /// <summary>
        /// 住户信息管理详情
        /// </summary>
        /// <param name="guid">申请惟一编码</param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [CustomAuthorize]
        [ProducesResponseType(200)]
        public IActionResult Detail(Guid guid)
        {
            using (_dbContext)
            {
                var entity = _dbContext.SystemUser.FirstOrDefault(x => x.SystemUserUuid == guid);
                var query = _mapper.Map<HaikanRefuseClassification.Api.Entities.SystemUser, HouseholdCreateViewModel>(entity);
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
        [CustomAuthorize]
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
        [CustomAuthorize]
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

        //根据乡镇(街道)获取社区
        [HttpGet]
        public IActionResult Huoqushequ(string guid)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {

                var query = from v in _dbContext.Village
                            select new
                            {
                                v.Vname,
                                v.Towns,
                                v.VillageUuid,
                                v.Address
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
                var entiy = query.ToList();
                response.SetData(entiy);
                return Ok(response);
            }
        }
    }
}
