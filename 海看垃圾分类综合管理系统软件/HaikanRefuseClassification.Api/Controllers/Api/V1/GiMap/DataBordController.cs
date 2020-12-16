using AutoMapper;
using HaikanRefuseClassification.Api.Configurations;
using HaikanRefuseClassification.Api.Entities;
using HaikanRefuseClassification.Api.Extensions;
using HaikanRefuseClassification.Api.Extensions.AuthContext;
using HaikanRefuseClassification.Api.Extensions.CustomException;
using HaikanRefuseClassification.Api.ViewModels.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using NPOI.SS.Formula.Functions;

namespace HaikanRefuseClassification.Api.Controllers.Api.V1.GiMap
{
    [Route("api/v1/GiMap/[controller]/[action]")]
    [ApiController]
    [CustomAuthorize]
    public class DataBordController : ControllerBase
    {
        private readonly RefuseClassificationContext _dbContext;
        private readonly IMapper _mapper;
        private readonly MdDesEncrypt MdDesEncrypt;
        //用来获取路径相关
        private IHostingEnvironment _hostingEnvironment;
        public DataBordController(RefuseClassificationContext dbContext, IMapper mapper, IOptions<MdDesEncrypt> mdDesEncrypt, IHostingEnvironment hostingEnvironment)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            MdDesEncrypt = mdDesEncrypt.Value;
            _hostingEnvironment = hostingEnvironment;
        }
        /// <summary>
        /// 分类概况（小区数 厢房数 用户数 全部已赚取积分 商店数 已使用积分）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetData1()
        {
            using (_dbContext)
            {
                int c1, c2, c3, c4, c5,cc = 0;
                double c6 = 0.0;


                if (AuthContextService.CurrentUser.Streets == "" && AuthContextService.CurrentUser.Community != "")
                {
                    var Community = AuthContextService.CurrentUser.Community.Split(',');
                    c1 = _dbContext.Village.Where(x => x.IsDelete != "1" && Community.Contains(x.Vname)).Count();//社区数
                    c2 = _dbContext.GrabageRoom.Where(x => x.IsDelete != "1" && Community.Contains(x.Village.Vname)).Count();//厢房数
                    c3 = _dbContext.SystemUser.Where(x => x.IsDeleted != 1 && x.SystemRoleUuid.Contains(AuthContextService.CurrentUser.YH) && Community.Contains(x.HomeAddressUu.Ccmmunity)).Count();//用户数
                    c4 = 0;
                    cc = 1;
                    c5 = 0;
                    var query2 = _dbContext.GrabageDisposal.Where(x => x.IsDelete != "1" && Community.Contains(x.HomeAddressUu.Ccmmunity)).Select(x => new { x.ScoreSettingUu.Integral });
                    foreach (var item in query2)
                    {
                        if (!string.IsNullOrEmpty(item.Integral.ToString()))
                            c4 += (int)item.Integral;//全部已赚取积分
                    }

                    var query21 = from ge in _dbContext.GoodsExchange
                                  join ha in _dbContext.HomeAddress on ge.SystemUserUuid equals ha.HomeAddressUuid 
                                  where ge.IsDelete != "1" &&
                                        Community.Contains(ha.Ccmmunity)
                                  select new
                                  {
                                      ge.DeduceScore
                                  };
                             //var query21 = _dbContext.GoodsExchange.Where(x => x.IsDelete != "1" && Community.Contains(x.SystemUserUu.Ccmmunity));
                    foreach (var item in query21)
                    {
                        c6 += Convert.ToDouble(item.DeduceScore);//已使用积分
                    }
                }
                else if (AuthContextService.CurrentUser.Streets != "" && AuthContextService.CurrentUser.Community == "")
                {
                    var Streets = AuthContextService.CurrentUser.Streets.Split(',');
                    c1 = _dbContext.Village.Where(x => x.IsDelete != "1" && Streets.Contains(x.Towns)).Count();//社区数
                    c2 = _dbContext.GrabageRoom.Where(x => x.IsDelete != "1" && Streets.Contains(x.Village.Towns)).Count();//厢房数
                    c3 = _dbContext.SystemUser.Where(x => x.IsDeleted != 1 && x.SystemRoleUuid.Contains(AuthContextService.CurrentUser.YH) && Streets.Contains(x.HomeAddressUu.Town)).Count();//用户数
                    c4 = 0;
                    cc = 1;
                    c5 = 0;
                    var query2 = _dbContext.GrabageDisposal.Where(x => x.IsDelete != "1" && Streets.Contains(x.HomeAddressUu.Town)).Select(x => new { x.ScoreSettingUu.Integral });
                    foreach (var item in query2)
                    {
                        if (!string.IsNullOrEmpty(item.Integral.ToString()))
                            c4 += (int)item.Integral;//全部已赚取积分
                    }
                    var query21 = from ge in _dbContext.GoodsExchange
                        join ha in _dbContext.HomeAddress on ge.SystemUserUuid equals ha.HomeAddressUuid
                        where ge.IsDelete != "1" &&
                              Streets.Contains(ha.Town)
                                  select new
                        {
                            ge.DeduceScore
                        };
                    //var query21 = _dbContext.GoodsExchange.Where(x => x.IsDelete != "1" && Streets.Contains(x.SystemUserUu.Town));
                    foreach (var item in query21)
                    {
                        c6 += Convert.ToDouble(item.DeduceScore);//已使用积分
                    }
                }
                else if (AuthContextService.CurrentUser.Streets != null && AuthContextService.CurrentUser.Streets != "" && AuthContextService.CurrentUser.Community != "")
                {
                    var Streets = AuthContextService.CurrentUser.Streets.Split(',');
                    var Community = AuthContextService.CurrentUser.Community.Split(',');
                    c1 = _dbContext.Village.Where(x => x.IsDelete != "1" && Streets.Contains(x.Towns) || Community.Contains(x.Vname)).Count();//社区数
                    c2 = _dbContext.GrabageRoom.Where(x => x.IsDelete != "1" && Streets.Contains(x.Village.Towns) || Community.Contains(x.Village.Vname)).Count();//厢房数
                    c3 = _dbContext.SystemUser.Where(x => x.IsDeleted != 1 && x.SystemRoleUuid.Contains(AuthContextService.CurrentUser.YH) && Streets.Contains(x.HomeAddressUu.Town) || Community.Contains(x.HomeAddressUu.Ccmmunity)).Count();//用户数
                    c4 = 0;
                    cc = 1;
                    c5 = 0;
                    var query2 = _dbContext.GrabageDisposal.Where(x => x.IsDelete != "1" && Streets.Contains(x.HomeAddressUu.Town) || Community.Contains(x.HomeAddressUu.Ccmmunity)).Select(x => new { x.ScoreSettingUu.Integral });
                    foreach (var item in query2)
                    {
                        if (!string.IsNullOrEmpty(item.Integral.ToString()))
                            c4 += (int)item.Integral;//全部已赚取积分
                    }
                    var query21 = from ge in _dbContext.GoodsExchange
                        join ha in _dbContext.HomeAddress on ge.SystemUserUuid equals ha.HomeAddressUuid
                        where ge.IsDelete != "1" &&
                              Streets.Contains(ha.Town) || Community.Contains(ha.Ccmmunity)
                                  select new
                        {
                            ge.DeduceScore
                        };
                    //var query21 = _dbContext.GoodsExchange.Where(x => x.IsDelete != "1" && Streets.Contains(x.SystemUserUu.Town) || Community.Contains(x.SystemUserUu.Ccmmunity));
                    foreach (var item in query21)
                    {
                        c6 += Convert.ToDouble(item.DeduceScore);//已使用积分
                    }
                }
                else
                {

                    c1 = _dbContext.Village.Where(x => x.IsDelete != "1").Count();//社区数
                    c2 = _dbContext.GrabageRoom.Where(x => x.IsDelete != "1").Count();//厢房数
                    c3 = _dbContext.SystemUser.Where(x => x.IsDeleted != 1 && x.SystemRoleUuid.Contains(AuthContextService.CurrentUser.YH)).Count();//用户数
                    c4 = 0;
                    cc = 0;
                    c5 = _dbContext.Shop.Where(x => x.IsDelete != "1").Count();//商店数
                    var query = _dbContext.GrabageDisposal.Where(x => x.IsDelete != "1").Select(x => new { x.ScoreSettingUu.Integral });
                    foreach (var item in query)
                    {
                        if (!string.IsNullOrEmpty(item.Integral.ToString()))
                            c4 += (int)item.Integral;//全部已赚取积分
                    }

                    var query1 = from ge in _dbContext.GoodsExchange
                        join ha in _dbContext.HomeAddress on ge.SystemUserUuid equals ha.HomeAddressUuid
                        where ge.IsDelete != "1" 
                        select new
                        {
                            ge.DeduceScore,
                            ha.HomeAddressUuid
                        };
                    //var query1 = _dbContext.GoodsExchange.Include(x => x.SystemUserUu).Where(x => x.IsDelete != "1").Select(x => new { x.SystemUserUu.HomeAddressUuid, x.DeduceScore });
                    foreach (var item in query1)
                    {
                        c6 += Convert.ToDouble(item.DeduceScore);//已使用积分
                    }
                }

                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(new List<double>() { c1, c2, c3, c4, c5, c6,cc });
                return Ok(response);
            }
        }
        //各类占比（其他 易腐 可回收）
        [HttpGet]
        public IActionResult GetData2()
        {
            using (_dbContext)
            {
                List<dynamic> data = new List<dynamic>();
                string year = DateTime.Now.Year.ToString();
                List<GrabageWeightSon> a = new List<GrabageWeightSon>();
                List<GrabageWeightSon> b = new List<GrabageWeightSon>();
                List<GrabageWeightSon> c = new List<GrabageWeightSon>();
                List<GrabageWeightSon> d = new List<GrabageWeightSon>();
                if (AuthContextService.CurrentUser.Streets == "" && AuthContextService.CurrentUser.Community != "")
                {
                    var Community = AuthContextService.CurrentUser.Community.Split(',');
                    a = _dbContext.GrabageWeightSon.Where(x => x.Type.Contains("1") && x.IsDelete != "1" && Community.Contains(x.GrabageRoom.Village.Vname) && x.GrabageRoomId!=null).ToList();
                    b = _dbContext.GrabageWeightSon.Where(x => x.Type.Contains("3") && x.IsDelete != "1" && Community.Contains(x.GrabageRoom.Village.Vname) && x.GrabageRoomId != null).ToList();
                    c = _dbContext.GrabageWeightSon.Where(x => x.Type.Contains("2") && x.IsDelete != "1" && Community.Contains(x.GrabageRoom.Village.Vname) && x.GrabageRoomId != null).ToList();
                    d = _dbContext.GrabageWeightSon.Where(x => x.Type.Contains("0") && x.IsDelete != "1" && Community.Contains(x.GrabageRoom.Village.Vname) && x.GrabageRoomId != null).ToList();
                }
                else if (AuthContextService.CurrentUser.Streets != "" && AuthContextService.CurrentUser.Community == "")
                {
                    var Streets = AuthContextService.CurrentUser.Streets.Split(',');
                    a = _dbContext.GrabageWeightSon.Where(x => x.Type.Contains("1") && x.IsDelete != "1" && Streets.Contains(x.GrabageRoom.Village.Towns) && x.GrabageRoomId != null).ToList();
                    b = _dbContext.GrabageWeightSon.Where(x => x.Type.Contains("3") && x.IsDelete != "1" && Streets.Contains(x.GrabageRoom.Village.Towns) && x.GrabageRoomId != null).ToList();
                    c = _dbContext.GrabageWeightSon.Where(x => x.Type.Contains("2") && x.IsDelete != "1" && Streets.Contains(x.GrabageRoom.Village.Towns) && x.GrabageRoomId != null).ToList();
                    d = _dbContext.GrabageWeightSon.Where(x => x.Type.Contains("0") && x.IsDelete != "1" && Streets.Contains(x.GrabageRoom.Village.Towns) && x.GrabageRoomId != null).ToList();
                }
                else if (AuthContextService.CurrentUser.Streets != null && AuthContextService.CurrentUser.Streets != "" && AuthContextService.CurrentUser.Community != "")
                {
                    var Streets = AuthContextService.CurrentUser.Streets.Split(',');
                    var Community = AuthContextService.CurrentUser.Community.Split(',');
                    a = _dbContext.GrabageWeightSon.Where(x => x.Type.Contains("1") && x.IsDelete != "1" && Community.Contains(x.GrabageRoom.Village.Vname) || Streets.Contains(x.GrabageRoom.Village.Towns) && x.GrabageRoomId != null).ToList();
                    b = _dbContext.GrabageWeightSon.Where(x => x.Type.Contains("3") && x.IsDelete != "1" && Community.Contains(x.GrabageRoom.Village.Vname) || Streets.Contains(x.GrabageRoom.Village.Towns) && x.GrabageRoomId != null).ToList();
                    c = _dbContext.GrabageWeightSon.Where(x => x.Type.Contains("2") && x.IsDelete != "1" && Community.Contains(x.GrabageRoom.Village.Vname) || Streets.Contains(x.GrabageRoom.Village.Towns) && x.GrabageRoomId != null).ToList();
                    d = _dbContext.GrabageWeightSon.Where(x => x.Type.Contains("0") && x.IsDelete != "1" && Community.Contains(x.GrabageRoom.Village.Vname) || Streets.Contains(x.GrabageRoom.Village.Towns) && x.GrabageRoomId != null).ToList();
                }
                else
                {
                    a = _dbContext.GrabageWeightSon.Where(x => x.Type.Contains("1") && x.IsDelete != "1" && x.GrabageRoomId != null).ToList();
                    b = _dbContext.GrabageWeightSon.Where(x => x.Type.Contains("3") && x.IsDelete != "1" && x.GrabageRoomId != null).ToList();
                    c = _dbContext.GrabageWeightSon.Where(x => x.Type.Contains("2") && x.IsDelete != "1" && x.GrabageRoomId != null).ToList();
                    d = _dbContext.GrabageWeightSon.Where(x => x.Type.Contains("0") && x.IsDelete != "1" && x.GrabageRoomId != null).ToList();
                }
                double aa = 0;
                double bb = 0;
                double cc = 0;
                double dd = 0;
                foreach (var item in a)
                {
                    if (!string.IsNullOrEmpty(item.Weight))
                        aa += Convert.ToDouble(item.Weight);
                }
                foreach (var item in b)
                {
                    if (!string.IsNullOrEmpty(item.Weight))
                        bb += Convert.ToDouble(item.Weight);
                }
                foreach (var item in c)
                {
                    if (!string.IsNullOrEmpty(item.Weight))
                        cc += Convert.ToDouble(item.Weight);
                }
                foreach (var item in d)
                {
                    if (!string.IsNullOrEmpty(item.Weight))
                        dd += Convert.ToDouble(item.Weight);
                }
                aa = double.Parse((aa / 1000).ToString("0.000"));
                bb = double.Parse((bb / 1000).ToString("0.000"));
                cc = double.Parse((cc / 1000).ToString("0.000"));
                dd = double.Parse((dd / 1000).ToString("0.000"));
                data.Add(new { aa, bb, cc, dd });

                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(data);
                return Ok(response);


            }
        }
        //年垃圾分类（总量，易腐比例）
        [HttpGet]
        public IActionResult Getdata3()
        {
            using (_dbContext)
            {
                List<double> num = new List<double>();
                List<double> percent = new List<double>();
                string year = DateTime.Now.Year.ToString();
                var Community = AuthContextService.CurrentUser.Community.Split(',');
                var Streets = AuthContextService.CurrentUser.Streets.Split(',');
                
                for (int i = 0; i < 12; i++)
                {
                    double la = 0.0;
                    double a = 0.0;
                    double per = 0.0;
                    string mouth = i + 1 + "";
                    if (i + 1 < 10)
                        mouth = "0" + (i + 1);
                    if (AuthContextService.CurrentUser.Streets == "" && AuthContextService.CurrentUser.Community != "")
                    {
                        la = _dbContext.GrabageWeightSon.Where(x => x.AddTime.Contains(year + "-" + mouth) && Community.Contains(x.GrabageRoom.Village.Vname) && x.GrabageRoomId!=null).Sum(x=> Convert.ToDouble(x.Weight));
                        a = _dbContext.GrabageWeightSon.Where(x => x.Type.Contains("1") && x.IsDelete != "1" && x.AddTime.Contains(year + "-" + mouth) && Community.Contains(x.GrabageRoom.Village.Vname) && x.GrabageRoomId != null).Sum(x => Convert.ToDouble(x.Weight));
                    }
                    else if (AuthContextService.CurrentUser.Streets != "" && AuthContextService.CurrentUser.Community == "")
                    {
                        la = _dbContext.GrabageWeightSon.Where(x => x.AddTime.Contains(year + "-" + mouth) && Streets.Contains(x.GrabageRoom.Village.Towns) && x.GrabageRoomId != null).Sum(x => Convert.ToDouble(x.Weight));
                        a = _dbContext.GrabageWeightSon.Where(x => x.Type.Contains("1") && x.IsDelete != "1" && x.AddTime.Contains(year + "-" + mouth) && Streets.Contains(x.GrabageRoom.Village.Towns) && x.GrabageRoomId != null).Sum(x => Convert.ToDouble(x.Weight));
                    }
                    else if (!string.IsNullOrEmpty(AuthContextService.CurrentUser.Streets) && AuthContextService.CurrentUser.Community != "")
                    {
                        la = _dbContext.GrabageWeightSon.Where(x => x.AddTime.Contains(year + "-" + mouth) && Community.Contains(x.GrabageRoom.Village.Vname) || Streets.Contains(x.GrabageRoom.Village.Towns) && x.GrabageRoomId != null).Sum(x => Convert.ToDouble(x.Weight));
                        a = _dbContext.GrabageWeightSon.Where(x => x.Type.Contains("1") && x.IsDelete != "1" && x.AddTime.Contains(year + "-" + mouth) && Community.Contains(x.GrabageRoom.Village.Vname) || Streets.Contains(x.GrabageRoom.Village.Towns) && x.GrabageRoomId != null).Sum(x => Convert.ToDouble(x.Weight));
                    }
                    else
                    {
                        la = _dbContext.GrabageWeightSon.Where(x => x.AddTime.Contains(year + "-" + mouth) && x.GrabageRoomId != null).Sum(x => Convert.ToDouble(x.Weight));
                        a = _dbContext.GrabageWeightSon.Where(x => x.Type.Contains("1") && x.IsDelete != "1" && x.AddTime.Contains(year + "-" + mouth) && x.GrabageRoomId != null).Sum(x => Convert.ToDouble(x.Weight));
                    }
                    per = a / la;
                    if (per.ToString() == "NaN")
                        per = 0;
                    num.Add(double.Parse((la / 1000).ToString("0.000")));
                    percent.Add(double.Parse((per * 100).ToString("0.00")));
                }
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(new { num, percent });
                return Ok(response);

            }
        }
        // 居民参与率
        [HttpGet]
        public IActionResult Getdata4()
        {
            using (_dbContext)
            {
                List<double> num = new List<double>();
                List<double> pnum = new List<double>();
                string year = DateTime.Now.Year.ToString();
                double allUser = 0;
                if (AuthContextService.CurrentUser.Streets == "" && AuthContextService.CurrentUser.Community != "")
                {
                    var Community = AuthContextService.CurrentUser.Community.Split(',');
                    allUser = _dbContext.SystemUser.Where(x => x.IsDeleted != 1 && x.SystemRoleUuid.Contains(AuthContextService.CurrentUser.YH) && x.HomeAddressUuid != null && Community.Contains(x.HomeAddressUu.Ccmmunity)).GroupBy(x => x.HomeAddressUuid).Count();
                }
                else if (AuthContextService.CurrentUser.Streets != "" && AuthContextService.CurrentUser.Community == "")
                {
                    var Streets = AuthContextService.CurrentUser.Streets.Split(',');
                    allUser = _dbContext.SystemUser.Where(x => x.IsDeleted != 1 && x.SystemRoleUuid.Contains(AuthContextService.CurrentUser.YH) && x.HomeAddressUuid != null && Streets.Contains(x.HomeAddressUu.Town)).GroupBy(x => x.HomeAddressUuid).Count();
                }
                else if (!string.IsNullOrEmpty(AuthContextService.CurrentUser.Streets) && AuthContextService.CurrentUser.Community != "")
                {
                    var Streets = AuthContextService.CurrentUser.Streets.Split(',');
                    var Community = AuthContextService.CurrentUser.Community.Split(',');
                    allUser = _dbContext.SystemUser.Where(x => x.IsDeleted != 1 && x.SystemRoleUuid.Contains(AuthContextService.CurrentUser.YH) && x.HomeAddressUuid != null && Streets.Contains(x.HomeAddressUu.Town) || Community.Contains(x.HomeAddressUu.Ccmmunity)).GroupBy(x => x.HomeAddressUuid).Count();
                }
                else
                {
                    allUser = _dbContext.SystemUser.Where(x => x.IsDeleted != 1 && x.SystemRoleUuid.Contains(AuthContextService.CurrentUser.YH) && x.HomeAddressUuid != null).GroupBy(x => x.HomeAddressUuid ).Select(x=>x.Key).Count();
                }
                for (int i = 0; i < 12; i++)
                {
                    using (RefuseClassificationContext cc = new RefuseClassificationContext())
                    {
                        string mouth = i + 1 + "";
                        if (i + 1 < 10)
                            mouth = "0" + (i + 1);
                        double temp = 0;
                        if (AuthContextService.CurrentUser.Streets == "" && AuthContextService.CurrentUser.Community != "")
                        {
                            var Community = AuthContextService.CurrentUser.Community.Split(',');
                            temp = cc.GrabageDisposal.Where(x => x.AddTime.Contains(year + "-" + mouth) && x.IsDelete != "1" && x.HomeAddressUuid != null && Community.Contains(x.HomeAddressUu.Ccmmunity) && x.IsScore == "1").GroupBy(x => x.HomeAddressUuid).Count();
                        }
                        else if (AuthContextService.CurrentUser.Streets != "" && AuthContextService.CurrentUser.Community == "")
                        {
                            var Streets = AuthContextService.CurrentUser.Streets.Split(',');
                            temp = cc.GrabageDisposal.Where(x => x.AddTime.Contains(year + "-" + mouth) && x.IsDelete != "1" && x.HomeAddressUuid != null && Streets.Contains(x.HomeAddressUu.Town) && x.IsScore == "1").GroupBy(x => x.HomeAddressUuid).Count();
                        }
                        else if (AuthContextService.CurrentUser.Streets != null && AuthContextService.CurrentUser.Streets != "" && AuthContextService.CurrentUser.Community != "")
                        {
                            var Streets = AuthContextService.CurrentUser.Streets.Split(',');
                            var Community = AuthContextService.CurrentUser.Community.Split(',');
                            temp = cc.GrabageDisposal.Where(x => x.AddTime.Contains(year + "-" + mouth) && x.IsDelete != "1" && x.HomeAddressUuid != null && Streets.Contains(x.HomeAddressUu.Town) || Community.Contains(x.HomeAddressUu.Ccmmunity) && x.IsScore == "1").GroupBy(x => x.HomeAddressUuid).Count();
                        }
                        else
                        {
                            temp = cc.GrabageDisposal.Where(x => x.AddTime.Contains(year + "-" + mouth) && x.IsDelete != "1" && x.HomeAddressUuid != null && x.IsScore=="1").Select(x=>x.HomeAddressUuid).GroupBy(x => x.Value).Count();
                        }

                        if (mouth=="08")
                        {
                            var ss = temp;
                            var aa = double.Parse((temp / allUser * 100).ToString("0.00"));
                         }
                        pnum.Add(temp);
                        num.Add(double.Parse((temp / allUser * 100).ToString("0.00")));
                    }
                }
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(new { num, pnum });
                return Ok(response);

            }
        }
        //新用户注册（注册数 新增积分数） 
        [HttpGet]
        public IActionResult Getdata5()
        {
            using (_dbContext)
            {

                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(null);
                return Ok(response);
            }
        }
        //社区垃圾分类排行榜 
        [HttpGet]
        public IActionResult Getdata6()
        {
            using (_dbContext)
            {
                var query1 = _dbContext.WasteRatio.Select(x=>new
                {
                    x.Vname,
                    c1 = x.Eflj,
                    c2 = x.Qtlj,
                    percent = x.Per
                }).ToList();
                var query = from wr in _dbContext.WasteRatio
                    select new
                    {
                        wr.Vname,
                        c1=wr.Eflj,
                        c2=wr.Qtlj,
                        percent=wr.Per+"%"
                    };
                //query = query.OrderByDescending(x => x.percent);
                var response = ResponseModelFactory.CreateResultInstance;
                var list = query1;
                response.SetData(list);
                return Ok(response);
            }
        }

        private double GetYF(Guid id, int type)
        {
            double s = 0;
            string mouth = DateTime.Now.Month + "";
            if (DateTime.Now.Month + 1 < 10)
                mouth = "0" + DateTime.Now.Month;
            string date = DateTime.Now.Year + "-" + mouth;//本月

            using (RefuseClassificationContext cc = new RefuseClassificationContext())
            {
                var qu = cc.GrabageWeightSon.Where(x => x.GrabageRoom.VillageId == id && x.IsDelete != "1" && x.AddTime.Contains(date) && x.GrabageRoomId != null);
                if (type == 1)
                    qu = qu.Where(x => x.Type.Contains("1"));
                else if (type == 2)
                    qu = qu.Where(x => !x.Type.Contains("1"));
                foreach (var item in qu.ToList())
                {
                    s += double.Parse(item.Weight);
                }
                return s;
            }
        }

        //志愿者积分排行榜 
        [HttpGet]
        public IActionResult Getdata7()
        {
            using (_dbContext)
            {

                var Streets = AuthContextService.CurrentUser.Streets.Split(',');
                var Community = AuthContextService.CurrentUser.Community.Split(',');
                var query = from s in _dbContext.Leaderboard
                    select new
                    {
                        s.LoginName,
                        HomeAddressUUID=s.HomeAddressUuid,
                        zyzsc=s.Zyzsc,
                        town=s.Town,
                        ccmmunity=s.Ccmmunity
                    };
                if (AuthContextService.CurrentUser.Streets != null && AuthContextService.CurrentUser.Streets == "" && AuthContextService.CurrentUser.Community != "")
                {
                    query = query.Where(x => Community.Contains(x.ccmmunity));
                }
                else if (!string.IsNullOrEmpty(AuthContextService.CurrentUser.Streets) && AuthContextService.CurrentUser.Community == "")
                {
                    query = query.Where(x => Streets.Contains(x.town));
                }
                else if (!string.IsNullOrEmpty(AuthContextService.CurrentUser.Streets) && AuthContextService.CurrentUser.Community != "")
                {
                    query = query.Where(x => Streets.Contains(x.town) || Community.Contains(x.ccmmunity));
                }
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(query.OrderByDescending(x => x.zyzsc).Take(30).ToList());
                return Ok(response);
            }
        }

        private int GetAllzyzScore(Guid id)
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
        private int GetAllCount(Guid id)
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

        //用户积分排行榜 
        [HttpGet]
        public IActionResult Getdata8()
        {
            try
            {
                using (_dbContext)
                {
                    List<string> Address = new List<string>();
                    List<string> LoginName = new List<string>();
                    List<int?> percent = new List<int?>();
                    var Streets = AuthContextService.CurrentUser.Streets.Split(',');
                    var Community = AuthContextService.CurrentUser.Community.Split(',');
                    var query = from g in _dbContext.HomeAddressView
                                    //join d in _dbContext.GrabageDisposal
                                    //on g.HomeAddressUUID equals d.HomeAddressUuid
                                select new
                                {



                                    g.LoginName,
                                    g.Address,
                                    town = g.Town,
                                    ccmmunity = g.Ccmmunity,
                                    percent = g.Per
                                };
                    if (AuthContextService.CurrentUser.Streets != null && AuthContextService.CurrentUser.Streets == "" && AuthContextService.CurrentUser.Community != "")
                    {
                        query = query.Where(x => Community.Contains(x.ccmmunity));
                    }
                    else if (!string.IsNullOrEmpty(AuthContextService.CurrentUser.Streets) && AuthContextService.CurrentUser.Community == "")
                    {
                        query = query.Where(x => Streets.Contains(x.town));
                    }
                    else if (!string.IsNullOrEmpty(AuthContextService.CurrentUser.Streets) && AuthContextService.CurrentUser.Community != "")
                    {
                        query = query.Where(x => Streets.Contains(x.town) || Community.Contains(x.ccmmunity));
                    }
                    query = query.OrderByDescending(x => x.percent).Take(100);
                    foreach (var item in query.ToList())
                    {
                        if (item.Address.Contains("区"))
                        {
                            var res = item.Address.Split("区")[1];
                            if (res.Contains("室"))
                            {
                                Address.Add(res);
                                LoginName.Add(item.LoginName);
                                percent.Add(item.percent);
                                continue;
                            }
                            else if (res.Contains("层"))
                            {
                                Address.Add(res);
                                LoginName.Add(item.LoginName);
                                percent.Add(item.percent);
                                continue;
                            }
                            else if (res.Contains("元"))
                            {
                                var Addre = res.Substring(0, res.IndexOf("元") + 1);
                                Address.Add(Addre);
                                LoginName.Add(item.LoginName);
                                percent.Add(item.percent);
                                continue;
                            }
                            else if (res.Contains("幢"))
                            {
                                var Addres = res.Substring(0, res.IndexOf("幢") + 1);
                                Address.Add(Addres);
                                LoginName.Add(item.LoginName);
                                percent.Add(item.percent);
                                continue;
                            }
                        }
                        else
                        {
                            Address.Add(item.Address);
                            LoginName.Add(item.LoginName);
                            percent.Add(item.percent);
                        }

                    }

                    var response = ResponseModelFactory.CreateResultInstance;
                    response.SetData(new { LoginName,Address, percent });
                    return Ok(response);

                }
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

        /// <summary>
        /// 首页本月新增数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetHPageData1()
        {
            DateTime date = DateTime.Now;
            var month = date.Month;
            var sdate = Convert.ToDateTime(date.Year + "/" + date.Month + "/1");
            var edate = sdate.AddMonths(1);
            var tosdate = sdate.ToString("yyyy-MM-dd HH:mm:ss");
            var toedate = edate.ToString("yyyy-MM-dd HH:mm:ss");

            var response = ResponseModelFactory.CreateResultInstance;
            int room,user,user2 = 0;
            if (AuthContextService.CurrentUser.Streets != null && AuthContextService.CurrentUser.Streets == "" && AuthContextService.CurrentUser.Community != "")
            {
                var Community = AuthContextService.CurrentUser.Community.Split(',');
                //新增厢房
                room = _dbContext.GrabageRoom.Where(x => x.IsDelete != "1" && x.AddTime.CompareTo(tosdate)>=0 && x.AddTime.CompareTo(toedate) < 0 && Community.Contains(x.Village.Vname)).Count();
                //新增志愿者
                user = _dbContext.SystemUser.Where(x => x.UserType == 5 && x.IsDeleted != 1 && x.SystemRoleUuid.Contains(AuthContextService.CurrentUser.ZYZ) && x.AddTime.CompareTo(tosdate)>=0 && x.AddTime.CompareTo(toedate) < 0 && Community.Contains(x.HomeAddressUu.Ccmmunity)).Count();
                //新增用户
                user2 = _dbContext.SystemUser.Where(x => x.UserType == 5 && x.IsDeleted != 1 && x.AddTime.CompareTo(tosdate)>=0 && x.AddTime.CompareTo(toedate) < 0 && Community.Contains(x.HomeAddressUu.Ccmmunity)).Count();
            }
            else if (AuthContextService.CurrentUser.Streets != null && AuthContextService.CurrentUser.Streets != "" && AuthContextService.CurrentUser.Community == "")
            {
                var Streets = AuthContextService.CurrentUser.Streets.Split(',');
                //新增厢房
                room = _dbContext.GrabageRoom.Where(x => x.IsDelete != "1" && x.AddTime.CompareTo(tosdate)>=0 && x.AddTime.CompareTo(toedate) < 0 && Streets.Contains(x.Village.Towns)).Count();
                //新增志愿者
                user = _dbContext.SystemUser.Where(x => x.UserType == 5 && x.IsDeleted != 1 && x.SystemRoleUuid.Contains(AuthContextService.CurrentUser.ZYZ) && x.AddTime.CompareTo(tosdate)>=0 && x.AddTime.CompareTo(toedate) < 0 && Streets.Contains(x.HomeAddressUu.Town) ).Count();
                //新增用户
                user2 = _dbContext.SystemUser.Where(x => x.UserType == 5 && x.IsDeleted != 1 && x.AddTime.CompareTo(tosdate)>=0 && x.AddTime.CompareTo(toedate) < 0 && Streets.Contains(x.HomeAddressUu.Town) ).Count();
            }
            else if (AuthContextService.CurrentUser.Streets != null && AuthContextService.CurrentUser.Streets != "" && AuthContextService.CurrentUser.Community != "")
            {
                var Streets = AuthContextService.CurrentUser.Streets.Split(',');
                var Community = AuthContextService.CurrentUser.Community.Split(',');
                //新增厢房
                room = _dbContext.GrabageRoom.Where(x => x.IsDelete != "1" && x.AddTime.CompareTo(tosdate)>=0 && x.AddTime.CompareTo(toedate) < 0 && Streets.Contains(x.Village.Towns) || Community.Contains(x.Village.Vname)).Count();
                //新增志愿者
                user = _dbContext.SystemUser.Where(x => x.UserType == 5 && x.IsDeleted != 1 && x.SystemRoleUuid.Contains(AuthContextService.CurrentUser.ZYZ) && x.AddTime.CompareTo(tosdate)>=0 && x.AddTime.CompareTo(toedate) < 0 && Streets.Contains(x.HomeAddressUu.Town) || Community.Contains(x.HomeAddressUu.Ccmmunity)).Count();
                //新增用户
                user2 = _dbContext.SystemUser.Where(x => x.UserType == 5 && x.IsDeleted != 1 && x.AddTime.CompareTo(tosdate) >= 0 && x.AddTime.CompareTo(toedate) < 0 && Streets.Contains(x.HomeAddressUu.Town) || Community.Contains(x.HomeAddressUu.Ccmmunity)).Count();
            }
            else
            {
                //新增厢房
                room = _dbContext.GrabageRoom.Where(x => x.IsDelete != "1" && x.AddTime.CompareTo(tosdate)>=0 && x.AddTime.CompareTo(toedate) < 0).ToList().Count();
                //新增志愿者
                user = _dbContext.SystemUser.Count(x => x.UserType == 5 && x.IsDeleted != 1 && x.SystemRoleUuid.Contains(AuthContextService.CurrentUser.ZYZ) && x.AddTime.CompareTo(tosdate) >= 0 && x.AddTime.CompareTo(toedate) < 0);
                //新增用户
                user2 = _dbContext.SystemUser.Count(x => x.UserType == 5 && x.IsDeleted != 1 && x.AddTime.CompareTo(tosdate) >= 0 && x.AddTime.CompareTo(toedate) < 0);
            }
            response.SetData(new { room, user, user2 });
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetVisitorNumAsync()
        {
            var response = ResponseModelFactory.CreateResultInstance;
            string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=wxda982699a0604145&secret=a070591fca4a02e9502125d8b0a4d87a";
            ViewModels.Data.Token token = await Haikan3.Utils.ToHttp<ViewModels.Data.Token>.ToGet(url);
            List<ActiveData> datas = new List<ActiveData>();
            DateTime date = DateTime.Now;

            if (token != null && !string.IsNullOrEmpty(token.Access_token))
            {
                for (int i = date.Day - 1; i > 0; i--)
                {
                    string month = date.Month.ToString();
                    string day = i.ToString();
                    if (month.Length == 1)
                    {
                        month = "0" + month;
                    }
                    if (i < 10)
                    {
                        day = "0" + day;
                    }
                    string time = date.Year + month + day;
                    string url2 = "https://api.weixin.qq.com/datacube/getweanalysisappiddailyvisittrend?access_token=" + token.Access_token;
                    string pdata = "{\"begin_date\": \"" + time + "\",\"end_date\":\"" + time + "\"}";
                    ActiveData data = await Haikan3.Utils.ToHttp<ActiveData>.ToPost(url2, pdata);
                    if (data != null && data.List != null && data.List.Count > 0)
                    {
                        datas.Add(data);
                    }
                    else
                    {
                        break;
                    }
                }
                int sum = 0;
                if (datas.Count == 0)
                {
                    sum = 0;
                }
                else
                {
                    sum = datas.Sum(x => x.List.Sum(y => y.Visit_uv));
                }

                response.SetSuccess();
                response.SetData(sum);
                return Ok(response);
            }
            else
            {
                response.SetFailed("token为空");
                return Ok(response);
            }
        }

        private int YHjifen(string id)
        {
            string mouth = DateTime.Now.Month + "";
            if (DateTime.Now.Month + 1 < 10)
                mouth = "0" + DateTime.Now.Month;
            string date = DateTime.Now.Year + "-" + mouth;//本月

            using (RefuseClassificationContext cc = new RefuseClassificationContext())
            {
                int qu = cc.GrabageDisposal.Where(x => x.HomeAddressUuid == Guid.Parse(id) && x.IsDelete != "1" && x.AddTime.Contains(date)).Select(x => new { Integral = x.ScoreSettingUu.Integral }).Sum(x => x.Integral).Value;
                return qu;
            }
        }
    }
}