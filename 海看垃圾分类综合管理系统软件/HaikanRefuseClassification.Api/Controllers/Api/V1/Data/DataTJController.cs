using AutoMapper;
using HaikanRefuseClassification.Api.Entities;
using HaikanRefuseClassification.Api.Extensions;
using HaikanRefuseClassification.Api.RequestPayload.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.Controllers.Api.V1.Data
{
    [Route("api/v1/Data/[controller]/[action]")]
    [ApiController]
    public class DataTJController : ControllerBase
    {
        private readonly RefuseClassificationContext _dbContext;
        private readonly IMapper _mapper;

        public DataTJController(RefuseClassificationContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// 数据统计
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult List(DataTJRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = from r in _dbContext.RubbishSyrecord
                            join v in _dbContext.Village
                            on r.VillageUuid equals v.VillageUuid
                            where v.IsDelete == "0"
                            select new
                            {
                                r.GarbageSyuuid,
                                r.Sytime,
                                v.VillageUuid,
                                v.Vname,
                                v.Towns,
                                Bl = GetRubbishRecord(),//平均每月易腐垃圾比例
                                month = GetRubbishBL(v.VillageUuid, 1),//单月易腐烂垃圾比例
                                state = Getstate(),  //状态
                            };
                //日期筛选
                if (!string.IsNullOrEmpty(payload.kw2[0]) && !string.IsNullOrEmpty(payload.kw2[1]))
                {
                    var date1 = Convert.ToDateTime(payload.kw2[0]).ToString("yyyy-MM-dd HH:mm:ss");
                    var date2 = Convert.ToDateTime(payload.kw2[1]).ToString("yyyy-MM-dd HH:mm:ss");
                    query = query.Where(x => x.Sytime.CompareTo(date1) >= 0 && x.Sytime.CompareTo(date2) <= 0);
                }
               // query = query.Where(x => x.IsDelete == 0);
                //分页
                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(list, totalCount);
                return Ok(response);
            }
        }
        public string Date = "";//绑定所在乡镇
        private string GetYearRecordCount()
        {
            string das = "";
            string sq ="";
            //string sq = "<td>社区名称</td>";
            for (int i = 1; i <= 12; i++)
            {
                sq += "<td>" + i + "月</td>";
                das += "'" + i + "月',";
            }
            //sq += "<td>平均每月易腐垃圾比例</td>";
            Date = das.TrimEnd(',');
            return sq;
        }
        private string Binddate()
        {
            string dt = "";
            Date = "'一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'";


            var query = _dbContext.Village.Where(x => x.IsDelete == "0");
            return dt;
        }
        //状态
        private string Getstate()
        {
            string s = "";
            var query = _dbContext.RubbishSyrecord.Where(x => x.IsDelete == "0");
            return s;
        }
        //单月易腐烂垃圾比例
        private string GetRubbishBL(Guid id, int type)
        {
            string date = "0";
            double s = 0;
            int month = 0;
            using (RefuseClassificationContext cc = new RefuseClassificationContext())
            {
                if (month == 0)
                {
                    //（当月易腐烂垃圾的重量）
                    //var qu = cc.GrabageWeighting.Where(x => x.GrabageRoom.VillageId == id && x.IsDelete != "1");
                    //if (type == 1)
                    //    qu = qu.Where(x => x.Type.Contains("易腐"));
                    //foreach (var item in qu)
                    //{
                    //    s += double.Parse(item.Weight);
                    //}
                }
                else if (month == 13)
                {
                    for (int i = 1; i <= 13; i++)
                    {
                        if (i != 13)
                        {
                            var qu = cc.GrabageWeighting.Where(x => x.GrabageRoom.VillageId == id && x.IsDelete != "1" && x.AddTime.Contains(date));
                        }
                        else
                        {

                        }
                    }
                }
                else
                {

                }
                return s.ToString();
            }
            }
        //平均每月易腐垃圾比例
        private string GetRubbishRecord()
        {
            string s = "";
            var query = _dbContext.RubbishSyrecord.Where(x => x.IsDelete == "0");

            //var q = from p in _dbContext.RubbishSyrecord
            //        group p by p.GarbageSyuuid into g
            //        select new
            //        {
            //            //g.Key,
            //            CorruptRubbishPercent = g.Average(p => p.CorruptRubbishPercent)
            //        };
            return s;
        }
    }
}
