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
using HaikanRefuseClassification.Api.RequestPayload.Score;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace HaikanRefuseClassification.Api.Controllers.Api.V1.score
{
    [Route("api/v1/score/[controller]/[action]")]
    [ApiController]
    public class VoluController : ControllerBase
    {
        private readonly RefuseClassificationContext _dbContext;
        private readonly IMapper _mapper;
        public VoluController(RefuseClassificationContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        /// <summary>
        /// 志愿者信息(模糊查询)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult List(VoluRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = from cd in _dbContext.SystemUser
                            where cd.SystemRoleUuid.Contains(AuthContextService.CurrentUser.ZYZ) && cd.IsDeleted != 1
                            select new
                            {
                                cd.SystemUserUuid,
                                cd.HomeAddressUuid,
                                cd.LoginName,//昵称
                                cd.Phone,
                                cd.RealName,
                                cd.AddTime,
                                cd.IsDeleted,
                                cd.HomeAddressUu.Town,
                                cd.HomeAddressUu.Ccmmunity,
                                cd.HomeAddressUu.Resregion,
                                count = GetAllCount(cd.SystemUserUuid),//累积服务次数
                                alltime = GetServiceTime(cd.SystemUserUuid) + "小时",//累积时长
                                score = GetAllScore(cd.SystemUserUuid)//服务积分
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
                //姓名筛选
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.LoginName.Contains(payload.Kw));
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
        //详情
        [HttpGet]
        public IActionResult GetRecord(Guid id)
        {
            using (_dbContext)
            {
                //var query = (from gr in _dbContext.VolunteerService
                //             where gr.SystemUserUuid == Guid.Parse(id) && gr.IsDelete != "1"
                //             select new
                //             {
                //                 gr.VolunteerServiceUuid,
                //                 name = gr.SystemUserUu.RealName,
                //                 grID = gr.GrabageRoom.Ljname,
                //                 vilage = gr.Village.Vname,
                //                 gr.Intime,
                //                 gr.Outtime,
                //                 fw = Math.Floor((DateTime.Parse(gr.Outtime) - DateTime.Parse(gr.Intime)).TotalHours).ToString() + "小时",
                //                 Score = GetSingleScore(gr.VolunteerServiceUuid),
                //             }).ToList();
                var query = from a in _dbContext.Attendance
                            where a.SystemUserUuid == id && a.IsDelete != "1" && a.Type == "1"
                            select new
                            {
                                a.SystemUserUu.LoginName,
                                
                                a.GarbageRoomUu.Ljname,
                                a.AmstartTime,
                                a.AmendTime,
                                a.PmstartTime,
                                a.PmendTime,
                                a.GarbageRoomUu.Village.Vname
                            };
                //重组数据，分上下午
                List<dynamic> data = new List<dynamic>();
                foreach (var item in query)
                {
                    //上/下午有一次打卡记录就加1，一天最多2次
                    if (!string.IsNullOrEmpty(item.AmstartTime) || !string.IsNullOrEmpty(item.AmendTime))
                    {
                        data.Add(new
                        {
                            name = item.LoginName,
                            grID = item.Ljname,
                            vilage = item.Vname,
                            Intime = item.AmstartTime,
                            Outtime = item.AmendTime,
                            fw = "上午"
                        });
                    }
                    if (!string.IsNullOrEmpty(item.PmstartTime) || !string.IsNullOrEmpty(item.PmendTime))
                    {
                        data.Add(new
                        {
                            name = item.LoginName,
                            grID = item.Ljname,
                            vilage = item.Vname,
                            Intime = item.PmstartTime,
                            Outtime = item.PmendTime,
                            fw = "下午"
                        });
                    }
                }
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(data);
                return Ok(response);
            }
        }
        /// <summary>
        /// 删除单个
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
        #region 方法
        /// <summary>
        /// 删除、恢复
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
                var sql = string.Format("UPDATE VolunteerService SET IsDelete=@IsDeleted WHERE VolunteerServiceUUID IN ({0})", parameterNames);
                parameters.Add(new SqlParameter("@IsDeleted", (int)isDeleted));
                _dbContext.Database.ExecuteSqlRaw(sql, parameters);
                var response = ResponseModelFactory.CreateInstance;
                return response;
            }
        }
        /// <summary>
        /// 获取总服务时长
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private static string GetServiceTime(Guid id)
        {
            using (RefuseClassificationContext cc = new RefuseClassificationContext())
            {
                var query = (from a in cc.Attendance
                             where a.SystemUserUuid == id && a.IsDelete != "1" && a.Type == "1"
                             select a).ToList();
                TimeSpan s = new TimeSpan();
                foreach (var item in query)
                {

                    //上午
                    if (!string.IsNullOrEmpty(item.AmstartTime) || !string.IsNullOrEmpty(item.AmendTime))
                    {
                        //当天日期

                        //没有签退时间就默认为12:00
                        if (!string.IsNullOrEmpty(item.AmendTime))
                            s += (DateTime.Parse(item.AmendTime) - DateTime.Parse(item.AmstartTime));
                        else
                        {
                            string date = item.AmstartTime.Trim().Split(" ")[0];
                            s += (DateTime.Parse(date + " 12:00:00") - DateTime.Parse(item.AmstartTime));
                        }

                    }
                    //下午
                    if (!string.IsNullOrEmpty(item.PmstartTime) || !string.IsNullOrEmpty(item.PmendTime))
                    {
                        //没有签退时间就默认为24:00
                        if (!string.IsNullOrEmpty(item.PmendTime))
                            s += (DateTime.Parse(item.PmendTime) - DateTime.Parse(item.PmstartTime));
                        else
                        {
                            string date = item.PmstartTime.Trim().Split(" ")[0];
                            s += (DateTime.Parse(date + " 23:59:59") - DateTime.Parse(item.PmstartTime));
                        }

                    }
                    //if (!string.IsNullOrEmpty(item.Intime) && !string.IsNullOrEmpty(item.Outtime))
                    //{
                    //    s += (DateTime.Parse(item.Outtime) - DateTime.Parse(item.Intime));
                    //}
                }
                return Math.Floor(s.TotalHours).ToString();
            }

        }
        /// <summary>
        /// 获取总服务次数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private static int GetAllCount(Guid id)
        {
            using (RefuseClassificationContext cc = new RefuseClassificationContext())
            {
                //var query = (from a in cc.VolunteerService
                //             where a.SystemUserUuid == id && a.IsDelete != "1"
                //             select a).ToList();
                //return query.Count().ToString();

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
        /// <summary>
        /// 获取全部分数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private static int GetAllScore(Guid id)
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
        /// 获取单个分数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private static int GetSingleScore(Guid id)
        {
            using (RefuseClassificationContext cc = new RefuseClassificationContext())
            {
                int hs = cc.Overallsituation.FirstOrDefault().HourScore;
                int s = 0;
                string t = "";
                TimeSpan ts = new TimeSpan();
                var query = cc.VolunteerService.FirstOrDefault(x => x.VolunteerServiceUuid == id);
                if (!string.IsNullOrEmpty(query.Intime) && !string.IsNullOrEmpty(query.Outtime))
                {
                    ts = (DateTime.Parse(query.Outtime) - DateTime.Parse(query.Intime));
                }
                t = Math.Floor(ts.TotalHours).ToString();
                s = hs * int.Parse(t);
                return s;
            }
        }

        //时间格式化
        private static string formatDateTime(string secondTime)
        {
            if (string.IsNullOrEmpty(secondTime))
            {
                return "";
            }
            else
            {
                long mss = long.Parse(secondTime);

                string DateTimes = null;
                long days = mss / (60 * 60 * 24);
                long hours = (mss % (60 * 60 * 24)) / (60 * 60);
                long minutes = (mss % (60 * 60)) / 60;
                long seconds = mss % 60;
                if (days > 0)
                {
                    DateTimes = days + "天" + hours + "小时" + minutes + "分钟"
                     + seconds + "秒";
                }
                else if (hours > 0)
                {
                    DateTimes = hours + "小时" + minutes + "分钟"
                     + seconds + "秒";
                }
                else if (minutes > 0)
                {
                    DateTimes = minutes + "分钟"
                     + seconds + "秒";
                }
                else
                {
                    DateTimes = seconds + "秒";
                }

                return DateTimes;
            }

        }
        #endregion
    }
}