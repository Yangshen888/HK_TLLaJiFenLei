
using AutoMapper;
using HaikanRefuseClassification.Api.Entities;
using HaikanRefuseClassification.Api.Extensions;
using HaikanRefuseClassification.Api.Extensions.AuthContext;
using HaikanRefuseClassification.Api.Models.Response;
using HaikanRefuseClassification.Api.RequestPayload.Grab;
using HaikanRefuseClassification.Api.Configurations;
using HaikanRefuseClassification.Api.Entities.Enums;
using HaikanRefuseClassification.Api.Extensions.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace HaikanRefuseClassification.Api.Controllers.Api.V1.grab
{
    [Route("api/v1/grab/[controller]/[action]")]
    [ApiController]
    public class GrabageDisposalController : ControllerBase
    {
        private readonly RefuseClassificationContext _dbContext;
        private readonly IMapper _mapper;
        private readonly MdDesEncrypt MdDesEncrypt;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        public GrabageDisposalController(RefuseClassificationContext dbContext, IMapper mapper, IOptions<MdDesEncrypt> mdDesEncrypt)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            MdDesEncrypt = mdDesEncrypt.Value;
        }

        /// <summary>
        /// 各社区赋分
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult VillageList(GrabRequestPaload payload)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var query = _dbContext.HomeAddress.Select(x => new {
                    x.Ccmmunity,
                    x.Address,
                    x.Town,
                    x.Resregion
                });
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
                var entity = query.GroupBy(x => x.Ccmmunity).Select(x => new
                {
                    Score = score(x.Key),
                    Ccmmunity = x.Key
                });
                var list = entity.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = entity.ToList().Count();
                response.SetData(list, totalCount);
                return Ok(response);
            }
        }

        private static int score(string name)
        {
            using (RefuseClassificationContext cc = new RefuseClassificationContext())
            {
                var num = (from g in cc.GrabageDisposal
                            join h in cc.HomeAddress
                            on g.HomeAddressUuid equals h.HomeAddressUuid
                            join s in cc.ScoreSetting
                            on g.ScoreSettingUuid equals s.ScoreUuid
                            where h.Ccmmunity == name && g.IsScore == "1"
                            && g.IsDelete != "1"
                            select new
                            {
                                s.Integral
                            }).Sum(x=>x.Integral).Value;
                return num;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult List(GrabRequestPaload payload)
        {
            using (_dbContext)
            {
                var query = from gd in _dbContext.GrabageDisposal 
                             join gr in _dbContext.GrabageRoom
                            on gd.GrabageRoomId equals gr.GarbageRoomUuid
                            //join o in _dbContext.SystemUser
                            //on gd.SystemUserUuid equals o.SystemUserUuid
                            join ha in _dbContext.HomeAddress
                            on gd.HomeAddressUuid equals ha.HomeAddressUuid
                            select new
                            {
                                GarbageDisposalUuid = gd.GarbageDisposalUuid,
                                TrueName = _dbContext.SystemUser.FirstOrDefault(x => x.SystemUserUuid == gd.SystemUserUuid).LoginName,
                                Phone = _dbContext.SystemUser.FirstOrDefault(x=>x.SystemUserUuid==gd.SystemUserUuid).Phone,
                                RoomID = gd.GrabageRoom.Ljname,
                                HomeAddressUuid = gd.HomeAddressUuid,
                                Address = ha.Address,
                                //o.OldCard,
                                //o.Wechat,
                                //SupervisorName = s.Sname,
                                AddTime = gd.AddTime,//投放时间
                                ScoreAddtime = gd.ScoreAddtime, 
                                Score = gd.ScoreSettingUu.Integral,
                                IsDeleted = gd.IsDelete,
                                Id = gd.Id,
                                gr.GarbageRoomUuid,
                                gd.MarkType,
                                ha.Ccmmunity,
                                ha.Town,
                                ha.Resregion,
                                IsScore=gd.IsScore=="0"?"已投放":"已赋分"
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
                //查看督导员所属的厢房赋分记录
                if (payload.Gruuid != null)
                {
                    query = query.Where(x => x.GarbageRoomUuid == payload.Gruuid);
                }
                if (!string.IsNullOrEmpty(payload.Kw1))
                {
                    query = query.Where(x => x.Address.ToString().Contains(payload.Kw1));
                }
                //街道筛选
                if (!string.IsNullOrEmpty(payload.street))
                {
                    query = query.Where(x => x.Address.Contains(payload.street));
                }
                //所在社区筛选
                if (!string.IsNullOrEmpty(payload.ccmmunity))
                {
                    query = query.Where(x => x.Address.Contains(payload.ccmmunity));
                }
                //if (!string.IsNullOrEmpty(payload.Kw))
                //{
                //    query = query.Where(x => x.RoomID.ToString().Contains(payload.Kw.Trim()) || x.TrueName.Contains(payload.Kw.Trim()));
                //}
                if (payload.MarkType!="全部")
                {
                    query = query.Where(x => x.MarkType.Contains(payload.MarkType));
                }
                if (payload.isScore!= "全部")
                {
                    query = query.Where(x => x.IsScore == payload.isScore);
                }
                //if (!string.IsNullOrEmpty(payload.time[0]))
                //{
                //    DateTime d1 = DateTime.Parse(payload.time[0]);
                //    DateTime d2 = DateTime.Parse(payload.time[1]);
                //    d2 = d2.AddDays(1);
                //    query = query.Where(x => DateTime.Parse(x.AddTime) >= d1 && DateTime.Parse(x.AddTime) <= d2);
                //}
                if (!string.IsNullOrEmpty(payload.time[0]))
                {
                    DateTime d1 = DateTime.Parse(payload.time[0]);
                    DateTime d2 = DateTime.Parse(payload.time[1]);
                    d2 = d2.AddDays(1);
                    var date1 = d1.ToString("yyyy-MM-dd HH:mm:ss");
                    var date2 = d2.ToString("yyyy-MM-dd HH:mm:ss");
                    query = query.Where(x => x.AddTime.CompareTo(date1) >= 0 && x.AddTime.CompareTo(date2) <= 0);
                }
                if (payload.FirstSort != null)
                {
                    query = query.OrderBy(payload.FirstSort.Field, payload.FirstSort.Direct == "DESC");
                }
                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();

                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(list, totalCount);
                return Ok(response);
            }
        }

        [HttpGet]
        public IActionResult disUpdate(string guid)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var entity = _dbContext.GrabageDisposal.FirstOrDefault(x=>x.GarbageDisposalUuid==Guid.Parse(guid));
                //当天赋分次数
                var disposal = _dbContext.GrabageDisposal.Count(x => x.HomeAddressUuid == entity.HomeAddressUuid && x.AddTime.Substring(0, 10)== entity.AddTime.Substring(0, 10) && x.IsScore=="1");
                //获取设置的评分次数
                var num = _dbContext.Overallsituation.First();
                if (disposal >= num.SetNumber)
                {
                    response.SetFailed("此日该家庭赋分超过" + num.SetNumber + "次");
                    return Ok(response);
                }
                entity.IsScore = "1";
                entity.ScoreAddtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                _dbContext.SaveChanges();
                return Ok(response);
            }
        }

        /// <summary>
        /// 取消赋分
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult disUpdateNo(string guid)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var entity = _dbContext.GrabageDisposal.FirstOrDefault(x => x.GarbageDisposalUuid == Guid.Parse(guid));
                entity.IsScore = "0";
                entity.ScoreAddtime ="";
                _dbContext.SaveChanges();
                return Ok(response);
            }
        }
        /// <summary>
        /// 删除垃圾收运记录
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
        [HttpPost]
        public IActionResult Batch(dynamic command)
        {
            if (command.command.ToString() == "delete")
                return Delete(command.ids.ToString());
            else if (command.command.ToString() == "recover")
                return Recover(command.ids.ToString());
            else
                return null;
        }
        /// <summary>
        /// 恢复垃圾收运记录
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpGet("{ids}")]
        [ProducesResponseType(200)]
        public IActionResult Recover(string ids)
        {
            var response = UpdateIsDelete(CommonEnum.IsDeleted.No, ids);
            return Ok(response);
        }
        #region 私有方法

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="isDeleted"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        private ResponseModel UpdateIsDelete(CommonEnum.IsDeleted isDeleted, string ids)
        {
            using (_dbContext)
            {
                var parameters = ids.Split(",").Select((id, index) => new SqlParameter(string.Format("@p{0}", index), id)).ToList();
                var parameterNames = string.Join(", ", parameters.Select(p => p.ParameterName));
                var sql = string.Format("UPDATE GrabageDisposal SET IsDelete=@IsDeleted WHERE GarbageDisposalUUID IN ({0})", parameterNames);
                parameters.Add(new SqlParameter("@IsDeleted", (int)isDeleted));
                _dbContext.Database.ExecuteSqlRaw(sql, parameters);
                var response = ResponseModelFactory.CreateInstance;
                return response;
            }
        }
        #endregion
    }
}