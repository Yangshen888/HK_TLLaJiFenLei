using AutoMapper;
using HaikanRefuseClassification.Api.Entities;
using HaikanRefuseClassification.Api.Entities.Enums;
using HaikanRefuseClassification.Api.Extensions;
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
using HaikanRefuseClassification.Api.Extensions.AuthContext;
using HaikanRefuseClassification.Api.Extensions.CustomException;
using Microsoft.Data.SqlClient;

namespace HaikanRefuseClassification.Api.Controllers.Api.V1.Base
{
    [Route("api/v1/Base/[controller]/[action]")]
    [ApiController]
    public class CollegeController : ControllerBase
    {
        private readonly RefuseClassificationContext _dbContext;
        private readonly IMapper _mapper;

        public CollegeController(RefuseClassificationContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// 社区管理(模糊查询)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult List(CollegeRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = from v in _dbContext.Village
                            where v.IsDelete == "0"
                            select new
                            {
                                v.VillageUuid,
                                v.Vname,  //社区名称
                                v.Towns,//乡镇
                                //v.Address,   //所在街道
                                v.Id,
                                ljname = Getlj(v.VillageUuid),
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
        private static string Getlj(Guid ljid)
        {
            using (RefuseClassificationContext cc = new RefuseClassificationContext())
            {
                var query = (from a in cc.GrabageRoom
                             where a.VillageId == ljid && a.IsDelete =="0"
                             select a).ToList();
                string s = "";
                foreach (var item in query)
                {
                    if (!string.IsNullOrEmpty(item.Ljname))
                    {
                        s += item.Ljname+"、";
                    }
                }
                return s.TrimEnd('、'); 
            }
        }
        /// <summary>
        /// 添加社区管理(保存)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize]
        [ProducesResponseType(200)]
        public IActionResult Create(CollegeViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;

            using (_dbContext)
            {
                var entity = new HaikanRefuseClassification.Api.Entities.Village();
                entity.VillageUuid = Guid.NewGuid();
                if (_dbContext.Village.Where(x => x.IsDelete == "0").Count(x => x.Vname == model.Vname) > 0)
                {
                    response.SetFailed("社区名已存在");
                    return Ok(response);
                }
                entity.Vname = model.Vname;//社区
                entity.Towns = model.Towns;//乡镇
                entity.Exchange = model.Exchange;
                entity.DisNum = model.DisNum;
                //entity.Address = model.Address;//街道
                entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd");
                entity.Addpeople = AuthContextService.CurrentUser.DisplayName;
                entity.IsDelete = "0"; //默认未删除
                _dbContext.Village.Add(entity);
                _dbContext.SaveChanges();
                response.SetSuccess("添加成功");
                return Ok(response);
            }
        }
        /// <summary>
        /// 编辑社区管理(保存)
        /// </summary>
        /// <param name="guid">申请惟一编码</param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [CustomAuthorize]
        [ProducesResponseType(200)]
        public IActionResult Edit(Guid guid)
        {
            using (_dbContext)
            {
                var entity = _dbContext.Village.FirstOrDefault(x => x.VillageUuid == guid);
                var query = _mapper.Map<HaikanRefuseClassification.Api.Entities.Village, CollegeEditView>(entity);
                //query.Sname = _dbContext.Supervisor.FirstOrDefault(x => x.VillageId == guid).Sname;//督导员
                //query.GarbageRoomUuid = _dbContext.GrabageRoom.FirstOrDefault(x => x.VillageId == guid).GarbageRoomUuid; //垃圾箱房
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(query);
                return Ok(response);
            }
        }
        ////获取设区下拉框
        //[HttpPost]
        //public IActionResult Huoqushequ()
        //{
        //    var response = ResponseModelFactory.CreateInstance;
        //    using (_dbContext)
        //    {
        //        var query = _dbContext.Village.Where(x => x.IsDelete == "0").ToList();
        //        response.SetData(query);
        //        return Ok(response);
        //    }
        //}
        //获取乡镇下拉框①
        [HttpPost]
        public IActionResult HuoquTowns()
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var query = _dbContext.Village.Where(x => x.IsDelete == "0"&&x.Towns!=null).Select(x=>x.Towns).Distinct().ToList();
                response.SetData(query);
                return Ok(response);
            }
        }

        [HttpGet]
        public IActionResult HuoquTownss(string vuuid)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var query = _dbContext.Village.Where(x => x.IsDelete == "0" && x.Towns != null && x.VillageUuid==Guid.Parse(vuuid)).Select(x => x.Towns).Distinct().ToList();
                response.SetData(query);
                return Ok(response);
            }
        }

        //根据乡镇(街道)获取社区
        [HttpGet("{towns}")]
        public IActionResult Huoqushequ(string towns)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var query = _dbContext.Village.Where(x => x.IsDelete == "0");
                if (towns != "" && towns != null)
                {
                    query = query.Where(x => x.Towns == towns);
                }
                var entiy = query.ToList();
                response.SetData(entiy);
                return Ok(response);
            }
        }

        //根据乡镇获取街道下拉框②
        //[HttpGet("{towns}")]
        //public IActionResult HuoquAddress(string towns)
        //{
        //    var response = ResponseModelFactory.CreateInstance;
        //    using (_dbContext)
        //    {
        //        var query = _dbContext.Village.Where(x => x.IsDelete == "0");
        //        if (towns != "" && towns != null)
        //        {
        //             query = query.Where(x => x.Towns == towns);
        //        }
        //        var entiy=query.Select(x => x.Address).Distinct().ToList();
        //        response.SetData(entiy);
        //        return Ok(response);
        //    }
        //}
        //再根据街道获取设区下拉框③
        //[HttpGet("{address}")]
        //public IActionResult Huoqushequ(string address)
        //{
        //    var response = ResponseModelFactory.CreateInstance;
        //    using (_dbContext)
        //    {
        //        var query = _dbContext.Village.Where(x => x.IsDelete == "0");
        //        if (address != "" && address != null)
        //        {
        //            query = query.Where(x => x.Address == address);
        //        }
        //        var entiy = query.ToList();
        //        response.SetData(entiy);
        //        return Ok(response);
        //    }
        //}
        /// <summary>
        /// 保存编辑后的社区管理(保存)
        /// </summary>
        /// <param name="model">申请视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize]
        [ProducesResponseType(200)]
        public IActionResult Edit(CollegeViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            using (_dbContext)
            {
                var entity = _dbContext.Village.FirstOrDefault(x => x.VillageUuid == model.VillageUuid);
                entity.Towns = model.Towns;
                entity.Vname = model.Vname;
                entity.Exchange = model.Exchange;
                entity.DisNum = model.DisNum;
                //entity.Address = model.Address;
                //垃圾箱房
               // var rubbish = _dbContext.GrabageRoom.FirstOrDefault(x => x.VillageId == entity.VillageUuid).Ljname;
                //督导员
                //var dudao = _dbContext.Supervisor.FirstOrDefault(x => x.VillageId == entity.VillageUuid).Sname;
                _dbContext.SaveChanges();
                return Ok(response);
            }
        }
        /// <summary>
        /// 社区管理详情
        /// </summary>
        /// <param name="model">申请惟一编码</param>
        /// <returns></returns>
        //[HttpGet("{guid}")]
        [HttpPost]
        [CustomAuthorize]
        [ProducesResponseType(200)]
        public IActionResult Detail(VillageParms model)
        {
            using (_dbContext)
            {
                var entity = _dbContext.Village.FirstOrDefault(x => x.VillageUuid == model.guid);
                var query = _mapper.Map<HaikanRefuseClassification.Api.Entities.Village, CollegeShowViewModel>(entity);
                //垃圾箱房
                query.Ljname = model.trashName;
                //督导员
                //query.Sname = _dbContext.Supervisor.FirstOrDefault(x => x.VillageId == entity.VillageUuid).Sname;
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
                var sql = string.Format("UPDATE Village SET IsDelete=@IsDeleted WHERE VillageUuid IN ({0})", parameterNames);
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
    }    
}
