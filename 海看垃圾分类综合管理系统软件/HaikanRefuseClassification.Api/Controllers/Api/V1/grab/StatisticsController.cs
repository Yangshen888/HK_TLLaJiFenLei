using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaikanRefuseClassification.Api.Entities;
using HaikanRefuseClassification.Api.Entities.Enums;
using HaikanRefuseClassification.Api.Extensions;
using HaikanRefuseClassification.Api.Models.Response;
using HaikanRefuseClassification.Api.RequestPayload.Grab;
using HaikanRefuseClassification.Api.ViewModels.Grab;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HaikanRefuseClassification.Api.Controllers.Api.V1.grab
{
    [Route("api/v1/grab/[controller]/[action]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly RefuseClassificationContext _dbContext;
        private readonly IMapper _mapper;

        public StatisticsController(RefuseClassificationContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult List(StatisticsRequestPaload payload)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var query = _dbContext.Village.Where(x => x.IsDelete != "1");
                //社区管理员筛选
                if (!string.IsNullOrEmpty(payload.vuuid))
                {
                    query = query.Where(x => x.VillageUuid == Guid.Parse(payload.vuuid)).OrderBy(x => x.Vname);
                }
                else
                {
                    query = query.OrderBy(x => x.Vname);
                }
                var query1 = _dbContext.RubbishAll.Where(x => x.Ye == payload.Time);
                var query2 = _dbContext.PerishableRubbish.Where(x => x.Ye == payload.Time);
                List<Village> villages = query.ToList();
                List<RubbishAll> alls = query1.ToList();
                List<PerishableRubbish> perishables = query2.ToList();
                List<StatisticsViewModel> models = new List<StatisticsViewModel>();
                for (int i = 0; i < villages.Count; i++)
                {
                    var model = new StatisticsViewModel();
                    //var xxx = perishables.Find(y => y.Mon == 1)?.Num ?? 0;
                    //var yyyy = alls.Find(y => y.Mon == 1)?.Num ?? 1;
                    //var zz = xxx / yyyy;
                    var xx = perishables.Find(x => x.VillageUuid == villages[i].VillageUuid && x.Mon == 4);
                    model.Vname = villages[i].Vname;
                    model.Towns = villages[i].Towns;
                    var one = ((perishables.Find(x => x.VillageUuid == villages[i].VillageUuid && x.Mon == 1)?.Num ?? 0) / (alls.Find(x => x.VillageUuid == villages[i].VillageUuid && x.Mon == 1)?.Num ?? 1));
                    var two = ((perishables.Find(x => x.VillageUuid == villages[i].VillageUuid && x.Mon == 2)?.Num ?? 0) / (alls.Find(x => x.VillageUuid == villages[i].VillageUuid && x.Mon == 2)?.Num ?? 1));
                    var three = ((perishables.Find(x => x.VillageUuid == villages[i].VillageUuid && x.Mon == 3)?.Num ?? 0) / (alls.Find(x => x.VillageUuid == villages[i].VillageUuid && x.Mon == 3)?.Num ?? 1));
                    var four = ((perishables.Find(x => x.VillageUuid == villages[i].VillageUuid && x.Mon == 4)?.Num ?? 0) / (alls.Find(x => x.VillageUuid == villages[i].VillageUuid && x.Mon == 4)?.Num ?? 1));
                    var five = ((perishables.Find(x => x.VillageUuid == villages[i].VillageUuid && x.Mon == 5)?.Num ?? 0) / (alls.Find(x => x.VillageUuid == villages[i].VillageUuid && x.Mon == 5)?.Num ?? 1));
                    var six = ((perishables.Find(x => x.VillageUuid == villages[i].VillageUuid && x.Mon == 6)?.Num ?? 0) / (alls.Find(x => x.VillageUuid == villages[i].VillageUuid && x.Mon == 6)?.Num ?? 1));
                    var seven = ((perishables.Find(x => x.VillageUuid == villages[i].VillageUuid && x.Mon == 7)?.Num ?? 0) / (alls.Find(x => x.VillageUuid == villages[i].VillageUuid && x.Mon == 7)?.Num ?? 1));
                    var eight = ((perishables.Find(x => x.VillageUuid == villages[i].VillageUuid && x.Mon == 8)?.Num ?? 0) / (alls.Find(x => x.VillageUuid == villages[i].VillageUuid && x.Mon == 8)?.Num ?? 1));
                    var ning = ((perishables.Find(x => x.VillageUuid == villages[i].VillageUuid && x.Mon == 9)?.Num ?? 0) / (alls.Find(x => x.VillageUuid == villages[i].VillageUuid && x.Mon == 9)?.Num ?? 1));
                    var ten = ((perishables.Find(x => x.VillageUuid == villages[i].VillageUuid && x.Mon == 10)?.Num ?? 0) / (alls.Find(x => x.VillageUuid == villages[i].VillageUuid && x.Mon == 10)?.Num ?? 1));
                    var eleven = ((perishables.Find(x => x.VillageUuid == villages[i].VillageUuid && x.Mon == 11)?.Num ?? 0) / (alls.Find(x => x.VillageUuid == villages[i].VillageUuid && x.Mon == 11)?.Num ?? 1));
                    var twelve = ((perishables.Find(x => x.VillageUuid == villages[i].VillageUuid && x.Mon == 12)?.Num ?? 0) / (alls.Find(x => x.VillageUuid == villages[i].VillageUuid && x.Mon == 12)?.Num ?? 1));
                    model.One = one.ToString("P");
                    model.Two = two.ToString("P");
                    model.Three = three.ToString("P");
                    model.Four = four.ToString("P");
                    model.Five = five.ToString("P");
                    model.Six = six.ToString("P");
                    model.Seven = seven.ToString("P");
                    model.Eight = eight.ToString("P");
                    model.Ning = ning.ToString("P");
                    model.Ten = ten.ToString("P");
                    model.Eleven = eleven.ToString("P");
                    model.Twelve = twelve.ToString("P");
                    model.Ave = ((one + two + three + four + five + six + seven + eight + ning + ten + eleven + twelve) / 12).ToString("P");
                    models.Add(model);
                }
                var list2 = models.AsQueryable().Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = models.Count;
                response.SetData(list2, totalCount);
                return Ok(response);
            }

        }


        //public IActionResult List()
        //{

        //    using (_dbContext)
        //    {
        //        var query = from g in _dbContext.Village
        //                    where g.IsDelete != "1"
        //                    select new
        //                    {
        //                        g.Vname,
        //                        g.Towns,
        //                        c1 = GetYF(g.VillageUuid, 1),
        //                        c2 = GetYF(g.VillageUuid, 2),
        //                        percent = (GetYF(g.VillageUuid, 1) / GetYF(g.VillageUuid, 0)).ToString("p")
        //                    };
        //        query = query.OrderBy(x => x.percent);
        //        var response = ResponseModelFactory.CreateResultInstance;
        //        response.SetData(query.ToList());
        //        return Ok(response);
        //    }
        //}

        //private double GetYF(Guid id, int type)
        //{
        //    double s = 0;
        //    for (int i = 0; i < 12; i++)
        //    {
        //        string mouth = i + 1 + "";
        //        if (i + 1 < 10)
        //            mouth = "0" + (i + 1);
        //        string date = DateTime.Now.Year + "-" + mouth;//本月

        //        using (RefuseClassificationContext cc = new RefuseClassificationContext())
        //        {
        //            var qu = cc.GrabageWeighting.Where(x => x.GrabageRoom.VillageId == id && x.IsDelete != "1" && x.AddTime.Contains(date));
        //            if (type == 1)
        //                qu = qu.Where(x => x.Type.Contains("易腐"));
        //            else if (type == 2)
        //                qu = qu.Where(x => !x.Type.Contains("易腐"));
        //            foreach (var item in qu.ToList())
        //            {
        //                s += double.Parse(item.Weight);
        //            }
        //            return s;
        //        }
        //    }
        //    return s;
        //    //string mouth = DateTime.Now.Month + "";
        //    //if (DateTime.Now.Month + 1 < 10)
        //    //    mouth = "0" + DateTime.Now.Month;

        //}

        /// <summary>
        /// 数据统计
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //StatisticsRequestPaload payload

        public IActionResult List()
        {
            List<StatisticsViewModel> statisticsViewModel = new List<StatisticsViewModel>();
            
            using (_dbContext)
            {
               
                string year = DateTime.Now.Year.ToString();
                var query = from g in _dbContext.Village
                            where g.IsDelete != "1"
                            select new
                            {
                                g.Vname,
                                g.Towns,
                                g.VillageUuid
                            };
                var query1 = query.ToList();
                //if (!string.IsNullOrEmpty(payload.Time))
                //{
                //    var stime =
                //    query = query.Where(x => Convert.ToDateTime(x) >= Convert.ToDateTime(payload.Time));
                //}

                foreach (var item in query.ToList())
                {
                    var model = new StatisticsViewModel();
                    List<double> percent = new List<double>();
                    List<double> asd = new List<double>();

                    //for (int i = 0; i < 12; i++)
                    //{
                    //    percent.Add(GetYF(item.VillageUuid, i));
                    //}
                    //var one = percent.Add(GetYF(item.VillageUuid, 0));
                    
                    model.One = GetYF(item.VillageUuid, 0).ToString() + "%";
                    model.Two = GetYF(item.VillageUuid, 1).ToString() + "%";
                    model.Three = GetYF(item.VillageUuid, 2).ToString() + "%";
                    model.Four = GetYF(item.VillageUuid, 3).ToString() + "%";
                    model.Five = GetYF(item.VillageUuid, 4).ToString() + "%";
                    model.Six = GetYF(item.VillageUuid, 5).ToString() + "%";
                    model.Seven = GetYF(item.VillageUuid, 6).ToString() + "%";
                    model.Eight = GetYF(item.VillageUuid, 7).ToString() + "%";
                    model.Ning = GetYF(item.VillageUuid, 8).ToString() + "%";
                    model.Ten = GetYF(item.VillageUuid, 9).ToString() + "%";
                    model.Eleven = GetYF(item.VillageUuid, 10).ToString() + "%";
                    model.Twelve = GetYF(item.VillageUuid, 11).ToString() + "%";
                    model.Vname = item.Vname;
                    model.Towns = item.Towns;
                    double Ave = (GetYF(item.VillageUuid, 0)
                        + GetYF(item.VillageUuid, 1)
                        + GetYF(item.VillageUuid, 2)
                        + GetYF(item.VillageUuid, 3)
                        + GetYF(item.VillageUuid, 4)
                        + GetYF(item.VillageUuid, 5)
                        + GetYF(item.VillageUuid, 6)
                        + GetYF(item.VillageUuid, 7)
                        + GetYF(item.VillageUuid, 8)
                        + GetYF(item.VillageUuid, 9)
                        + GetYF(item.VillageUuid, 10)
                        + GetYF(item.VillageUuid, 11)) / 12;
                    model.Ave = Ave.ToString("0.00") + "%";


                    statisticsViewModel.Add(model);
                }
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(statisticsViewModel);
                return Ok(response);
            }
        }

        private double GetYF(Guid id,int i)
        {
            double la = 0.0;
            double a = 0.0;
            double per = 0.0;
            string year = DateTime.Now.Year.ToString();
                string mouth = i + 1 + "";
                if (i + 1 < 10)
                    mouth = "0" + (i + 1);
                var temp = _dbContext.GrabageWeightSon.Where(x => x.AddTime.Contains(year + "-" + mouth));
                var aa = _dbContext.GrabageWeightSon.Where(x => x.Type.Contains("1")&& x.GrabageRoom.VillageId == id && x.IsDelete != "1" && x.AddTime.Contains(year + "-" + mouth)).ToList();
                foreach (var item in temp)
                {
                    if (!string.IsNullOrEmpty(item.Weight))
                        la += Convert.ToDouble(item.Weight);
                }
                foreach (var item in aa)
                {
                    if (!string.IsNullOrEmpty(item.Weight))
                        a += Convert.ToDouble(item.Weight);
                }
                per = a / la;
                if (per.ToString() == "NaN")
                    per = 0;
                per = double.Parse((per * 100).ToString("0.00"));

            return per;

        }


        #region 街道社区易腐垃圾比例统计列表
        /// <summary>
        /// 街道社区易腐垃圾比例统计列表
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult StreetList(StatisticsRequestPaload payload)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                // var lists = _dbContext.StreesSatistical_View.Where(x => x.IsDelete!="1").ToList();
                var lists = from i in _dbContext.StreeStatistical
                            select new
                            {
                                i.Vname,
                                i.Towns,
                                i.Times,
                                Weektime= i.Weekstr.Value.ToString("yyyy-MM-dd") + "-"+ i.Weekend.Value.ToString("yyyy-MM-dd"),
                                Weekprop=i.Weekprop!=null ? i.Weekprop.ToString().Substring(0,5)+"%" :i.Weekprop==null ? "0%" :null ,
                                Dayprop =i.Dayprop!=null ? i.Dayprop.ToString().Substring(0, 5) + "%" : i.Dayprop==null ? "0%" :null,
                                Yearprop=i.Yearprop!=null ? i.Yearprop.ToString().Substring(0, 5) + "%" : i.Yearprop==null ? "0%" :null
                            };
                
                var list2 = lists.AsQueryable().Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = lists.ToList().Count;
                response.SetData(list2, totalCount);
                return Ok(response);
            }

        }
        #endregion

    }
}
