using AutoMapper;
using HaikanRefuseClassification.Api.Entities;
using HaikanRefuseClassification.Api.Entities.Enums;
using HaikanRefuseClassification.Api.Extensions;
using HaikanRefuseClassification.Api.Models.Response;
using HaikanRefuseClassification.Api.RequestPayload.Score;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using HaikanRefuseClassification.Api.Extensions.AuthContext;
using HaikanRefuseClassification.Api.ViewModels.Score;
using Microsoft.Data.SqlClient;

namespace HaikanRefuseClassification.Api.Controllers.Api.V1.score
{
    [Route("api/v1/score/[controller]/[action]")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private readonly RefuseClassificationContext _dbContext;
        private readonly IMapper _mapper;
        public OwnerController(RefuseClassificationContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        /// <summary>
        /// 住户信息管理(模糊查询)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult List(OwnRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = _dbContext.Ownsource.Select(x => new
                {
                    x.HomeAddressUuid,
                    x.Address,
                    x.Ccmmunity,
                    x.Town,
                    x.Resregion,
                    //x.used,//已使用积分
                    all = x.Alls,   //累计积分
                    //x.can, //可用积分
                    x.LastScore,
                    x.NewScore,
                    State = x.State == 0 ? "未兑换" : "已兑换"
                }); ;
                //var query = from cd in _dbContext.HomeAddress
                //            select new
                //            {
                //                cd.HomeAddressUuid,
                //                cd.Address,
                //                cd.Ccmmunity,
                //                cd.Town,
                //                cd.Resregion,
                //                used = GetUsedScore(cd.HomeAddressUuid),//已使用积分
                //                all = GetAllScore(cd.HomeAddressUuid),   //累计积分
                //                can = GetAllScore(cd.HomeAddressUuid) - GetUsedScore(cd.HomeAddressUuid) //可用积分
                //            };
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
                    query = query.Where(x => x.Address.Contains(payload.street));
                }
                //社区筛选
                if (!string.IsNullOrEmpty(payload.ccmmunity))
                {
                    query = query.Where(x => x.Address.Contains(payload.ccmmunity));
                }
                //地址筛选
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.Address.Contains(payload.Kw));
                }
                if (payload.FirstSort != null)
                {
                    query = query.OrderByDescending(x => x.all);
                }
                //if (payload.IsDeleted > CommonEnum.IsDeleted.All)
                //{
                //    query = query.Where(x => (CommonEnum.IsDeleted)int.Parse(x.IsDeleted) == payload.IsDeleted);
                //}
                //分页
                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(list, totalCount);
                return Ok(response);
            }
        }

        /// <summary>
        /// 获取每月未使用积分
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult HomeScore(Guid guid)
        {
            using (_dbContext)
            {
                var response = ResponseModelFactory.CreateInstance;
                var query = _dbContext.HomeDateScore.FirstOrDefault(x => x.HomeAddressUuid == guid);

                var query1 = _dbContext.HomeAddress.FirstOrDefault(x => x.HomeAddressUuid == guid);
                var entity = _dbContext.Village.FirstOrDefault(x => x.IsDelete == "0" && query1.Address.Contains(x.Vname) || query1.Ccmmunity == x.Vname);
                var Exchange = 1;
                var DisNum = 0;
                if (entity != null)
                {
                    Exchange = entity.Exchange;
                    DisNum = entity.DisNum;
                }
                response.SetData(new { query, Exchange, DisNum });
                return Ok(response);
            }
        }

        /// <summary>
        /// 编辑保存每月积分
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult EditHomeScore(dynamic model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            using (_dbContext)
            {
                Guid guid = model.homeAddressUuid;
                int lastDecScore = model.lastDecScore;
                int janScore = model.janScore;
                int febScore = model.febScore;
                int marScore = model.marScore;
                int aprScore = model.aprScore;
                int mayScore = model.mayScore;
                int junScore = model.junScore;
                int julScore = model.julScore;
                int augScore = model.augScore;
                int sepScore = model.sepScore;
                int octScore = model.octScore;
                int novScore = model.novScore;
                int decScore = model.decScore;
                string jifen = model.jifen; 
                var query = _dbContext.HomeAddress.FirstOrDefault(x => x.HomeAddressUuid == guid);
                var entity = _dbContext.Village.FirstOrDefault(x => x.IsDelete == "0" && query.Address.Contains(x.Vname) || query.Ccmmunity == x.Vname);
                if (entity != null)
                {
                    query.LastDec = lastDecScore;
                    query.Jan = janScore;
                    query.Feb = febScore;
                    query.Mar = marScore;
                    query.Apr = aprScore;
                    query.May = mayScore;
                    query.Jun = junScore;
                    query.Jul = julScore;
                    query.Aug = augScore;
                    query.Sep = sepScore;
                    query.Oct = octScore;
                    query.Nov = novScore;
                    query.Dec = decScore;
                    var datescore = _dbContext.DateScore.FirstOrDefault(x => x.HomeAddressUuid == query.HomeAddressUuid && x.AddTime.Contains(DateTime.Now.ToString("yyyy")));
                    if (datescore!=null)
                    {
                        datescore.Jan = janScore;
                        datescore.Feb = febScore;
                        datescore.Mar = marScore;
                        datescore.Apr = aprScore;
                        datescore.May = mayScore;
                        datescore.Jun = junScore;
                        datescore.Jul = julScore;
                        datescore.Aug = augScore;
                        datescore.Sep = sepScore;
                        datescore.Oct = octScore;
                        datescore.Nov = novScore;
                        datescore.Dec = decScore;
                        
                    }
                    else
                    {
                        var entity1 = new DateScore();
                        entity1.DateScoreUuid = Guid.NewGuid();
                        entity1.HomeAddressUuid = query.HomeAddressUuid;
                        entity1.Jan = janScore;
                        entity1.Feb = febScore;
                        entity1.Mar = marScore;
                        entity1.Apr = aprScore;
                        entity1.May = mayScore;
                        entity1.Jun = junScore;
                        entity1.Jul = julScore;
                        entity1.Aug = augScore;
                        entity1.Sep = sepScore;
                        entity1.Oct = octScore;
                        entity1.Nov = novScore;
                        entity1.Dec = decScore;
                        entity1.AddTime = DateTime.Now.ToString("yyyy-MM-dd");
                        _dbContext.Add(entity1);
                    }
                    _dbContext.SaveChanges();
                    GoodsExchange gd = new GoodsExchange();
                    gd.StoreExchangeUuid = Guid.NewGuid();
                    gd.SystemUserUuid = guid;
                    gd.ExchageTime = DateTime.Now.ToString("yyyy-MM-dd");
                    gd.DeduceScore = jifen;
                    gd.IsDelete = "0";
                    _dbContext.Add(gd);
                    _dbContext.SaveChanges();
                }
                else
                {
                    response.SetFailed("该住户未归属社区");
                }
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
        //详情
        [HttpPost]
        public IActionResult GetRecord(DateScoreRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = (from gr in _dbContext.GrabageDisposal
                             where gr.HomeAddressUuid == Guid.Parse(payload.Kw) && gr.IsDelete != "1" && gr.IsScore == "1"
                             select new
                             {
                                 gr.GarbageDisposalUuid,
                                 gr.AddTime,
                                 grID = gr.GrabageRoom.Ljname,
                                 grtype = gr.GrabType,
                                 gr.ScoreSettingUu.Integral,
                                 gr.ScoreSettingUu.ScoreName,
                                 gr.MarkType,
                                 LoginName = gr.SupervisorUu.LoginName ?? "家庭码",
                                 Phone = gr.SupervisorUu.Phone
                             });

                //年筛选
                if (!string.IsNullOrEmpty(payload.kw1))
                {
                    query = query.Where(x => x.AddTime.Contains(Convert.ToDateTime(payload.kw1).ToString("yyyy")));
                }
                //月筛选
                if (!string.IsNullOrEmpty(payload.kw2))
                {
                    query = query.Where(x => x.AddTime.Contains(Convert.ToDateTime(payload.kw2).ToString("yyyy-MM")));
                }
                var response = ResponseModelFactory.CreateResultInstance;
                var list = query.OrderByDescending(x => x.AddTime).Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();
                response.SetData(list, totalCount);
                return Ok(response);
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
        #region 私有

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
                var sql = string.Format("UPDATE GrabageDisposal SET IsDelete=@IsDeleted WHERE GarbageDisposalUUID IN ({0})", parameterNames);
                parameters.Add(new SqlParameter("@IsDeleted", (int)isDeleted));
                _dbContext.Database.ExecuteSqlRaw(sql, parameters);
                var response = ResponseModelFactory.CreateInstance;
                return response;
            }
        }
        private double GetUsedScore(Guid ownid)
        {
            using (RefuseClassificationContext cc = new RefuseClassificationContext())
            {
                var query = (from a in cc.GoodsExchange
                             where a.SystemUserUuid == ownid
                             select a).ToList();
                double s = 0;
                foreach (var item in query)
                {
                    if (!string.IsNullOrEmpty(item.DeduceScore))
                    {
                        s += double.Parse(item.DeduceScore);
                    }
                }
                return s;
            }

        }
        private double GetAllScore(Guid ownid)
        {
            using (RefuseClassificationContext cc = new RefuseClassificationContext())
            {
                var query = from a in cc.GrabageDisposal
                            where a.HomeAddressUuid == ownid && a.IsScore=="1"
                            select new
                            {
                                Score= a.ScoreSettingUu.Integral
                            };
                double s = 0;
                foreach (var item in query)
                {
                    if (!string.IsNullOrEmpty(item.Score.ToString()))
                    {
                        s += (double)item.Score;
                    }
                }

                return s;
            }
        }
        #endregion




        /// <summary>
        /// 评价积分列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult ScoreList(OwnRequestPayload payload)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var query = _dbContext.ScoreSetting.AsQueryable();
                //姓名筛选
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.ScoreName.Contains(payload.Kw));
                }
                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();

                
                response.SetData(list, totalCount);
                return Ok(response);
            }
        }

        /// <summary>
        /// 添加赋分评价
        /// </summary>
        /// <param name="model">申请视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Createscore(ScoreSetting model)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var ofName=_dbContext.ScoreSetting.FirstOrDefault(x => x.ScoreName.Trim() == model.ScoreName.Trim());
                if(ofName!=null)
                {
                    response.SetFailed("已存在此评价名称");
                    return Ok(response);
                }
                ScoreSetting entity = new ScoreSetting();
                entity.ScoreUuid = Guid.NewGuid();
                entity.ScoreName = model.ScoreName;
                entity.Integral = model.Integral;
                entity.Addpeople = AuthContextService.CurrentUser.RoleName;
                entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd");

                _dbContext.ScoreSetting.Add(entity);
                _dbContext.SaveChanges();
                response.SetSuccess();
                return Ok(response);
            }
        }
        /// <summary>
        /// 编辑(保存)
        /// </summary>
        /// <param name="guid">申请惟一编码</param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        public IActionResult Edit(Guid guid)
        {
            using (_dbContext)
            {
                var entity = _dbContext.ScoreSetting.FirstOrDefault(x => x.ScoreUuid == guid);
                var query = _mapper.Map<HaikanRefuseClassification.Api.Entities.ScoreSetting, ScoreSettingEditView>(entity);
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(query);
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
        public IActionResult Edit(ScoreSettingViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            using (_dbContext)
            {
                var entity = _dbContext.ScoreSetting.FirstOrDefault(x => x.ScoreUuid == model.ScoreUuid);
                entity.ScoreName = model.ScoreName;
                entity.Integral = model.Integral;
                _dbContext.SaveChanges();
                return Ok(response);
            }
        }
        /// <summary>
        /// 每天最多赋分次数（家庭组）
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        [HttpGet("{score}")]
        [ProducesResponseType(200)]
        public IActionResult DivisionTimes(int score)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                if(String.IsNullOrEmpty(score.ToString()))
                {
                    response.SetFailed();
                    return Ok(response);
                }
                var entity = _dbContext.Overallsituation.ToList();
                if(entity.Count>0)
                {
                    entity[0].SetNumber = score;
                }

                _dbContext.SaveChanges();
                response.SetSuccess("设置成功");
                return Ok(response);
            }
        }

        /// <summary>
        /// 每小时对应的积分
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        [HttpGet("{score}")]
        [ProducesResponseType(200)]
        public IActionResult Hourintegral(int score)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                if (String.IsNullOrEmpty(score.ToString()))
                {
                    response.SetFailed();
                    return Ok(response);
                }
                var entity = _dbContext.Overallsituation.ToList();
                if (entity.Count > 0)
                {
                    entity[0].HourScore = score;
                }

                _dbContext.SaveChanges();
                response.SetSuccess("设置成功");
                return Ok(response);
            }
        }
        /// <summary>
        /// 获取赋分次数与对应积分
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult getSetNumber( )
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var entity = _dbContext.Overallsituation.ToList();

                response.SetData(entity);
                response.SetSuccess();
                return Ok(response);
            }
        }


        /// <summary>
        /// 删除积分评价
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        public IActionResult DeleteScoreinfo(string id)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                if (String.IsNullOrEmpty(id))
                {
                    response.SetFailed();
                    return Ok(response);
                }
                var entity = _dbContext.ScoreSetting.FirstOrDefault(x=>x.ScoreUuid.ToString()==id);
                if (entity==null)
                {
                    response.SetFailed();
                    return Ok(response);
                    
                }
                _dbContext.ScoreSetting.Remove(entity);
                _dbContext.SaveChanges();
                response.SetSuccess("删除成功");
                return Ok(response);
            }
        }

        /// <summary>
        /// 积分兑换记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult ChangesList(OwnRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = from cd in _dbContext.GoodsExchange
                            join h in _dbContext.HomeAddress
                            on cd.SystemUserUuid equals h.HomeAddressUuid
                            where cd.IsDelete=="0" 
                            select new
                            {
                                h.Address,
                                h.Score,
                                cd.StoreExchangeUuid,
                                cd.Shop.ShopName,
                                LoginName=h.Address,
                                cd.ExchageTime,
                                cd.DeduceScore,
                                cd.Goods.Gname,
                                cd.Id,
                                cd.SystemUserUuid
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
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.LoginName.Contains(payload.Kw));
                }
                //商品名称
                if (!string.IsNullOrEmpty(payload.kw1))
                {
                    query = query.Where(x => x.ShopName.Contains(payload.kw1));
                }
                //时间筛选
                if (!string.IsNullOrEmpty(payload.kw2[0]) && !string.IsNullOrEmpty(payload.kw2[1]))
                {
                    var date1 = Convert.ToDateTime(payload.kw2[0]).ToString("yyyy-MM-dd HH:mm:ss");
                    var date2 = Convert.ToDateTime(payload.kw2[1]).ToString("yyyy-MM-dd HH:mm:ss");
                    query = query.Where(x => x.ExchageTime.CompareTo(date1) >= 0 && x.ExchageTime.CompareTo(date2) <= 0);
                }
                if (payload.FirstSort != null)
                {
                    query = query.OrderByDescending(x => x.ExchageTime);
                }
                var entity = query.ToList().GroupBy(x => x.Address).Select(g => g.First());
                //分页
                var list = entity.AsQueryable().Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = entity.Count();
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(list, totalCount);
                return Ok(response);
            }
        }

        /// <summary>
        /// 积分兑换记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult ChangesList1(OwnRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = from cd in _dbContext.GoodsExchange
                            join h in _dbContext.HomeAddress
                            on cd.SystemUserUuid equals h.HomeAddressUuid
                            where cd.IsDelete == "0"
                            select new
                            {
                                h.Address,
                                h.Score,
                                cd.StoreExchangeUuid,
                                cd.Shop.ShopName,
                                LoginName=h.Address,
                                cd.ExchageTime,
                                cd.DeduceScore,
                                cd.Goods.Gname,
                                cd.Id,
                                cd.SystemUserUuid
                            };
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.SystemUserUuid==Guid.Parse(payload.Kw));
                }
                if (payload.FirstSort != null)
                {
                    query = query.OrderByDescending(x => x.ExchageTime);
                }
                //分页
                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(list, totalCount);
                return Ok(response);
            }
        }



    }
}