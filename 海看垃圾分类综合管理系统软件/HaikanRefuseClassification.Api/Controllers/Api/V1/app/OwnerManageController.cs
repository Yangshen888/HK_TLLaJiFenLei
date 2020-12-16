using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AutoMapper;
using HaikanRefuseClassification.Api.Entities;
using HaikanRefuseClassification.Api.Extensions;
using HaikanRefuseClassification.Api.Extensions.AuthContext;
using HaikanRefuseClassification.Api.Extensions.CustomException;
using HaikanRefuseClassification.Api.ViewModels.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using QRCoder;

namespace HaikanRefuseClassification.Api.Controllers.Api.V1.app
{
    [Route("api/v1/app/[controller]/[action]")]
    [ApiController]
    //[CustomAuthorize]
    public class OwnerManageController : ControllerBase
    {
        private readonly RefuseClassificationContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _hostingEnvironment;
        public OwnerManageController(RefuseClassificationContext dbContext, IMapper mapper, IHostingEnvironment hostingEnvironment)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// 获取所有垃圾厢房的数据
        /// </summary>
        /// <param name="idcard"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult getAllGrabroomPoint()
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                using (_dbContext)
                {
                    var query = (from g in _dbContext.GrabageRoom      //垃圾
                                 where g.IsDelete == "0"&&g.State=="0"
                                 select new
                                 {
                                     id = g.Id,
                                     latitude = g.Lat.Trim() != "" ? float.Parse(g.Lat) : 0,
                                     longitude = g.Lon.Trim() != "" ? float.Parse(g.Lon) : 0,
                                     title = g.Ljname
                                 }).ToList();
                    response.SetData(query);
                    return Ok(response);
                }


            }
        }

        /// <summary>
        /// 乡镇/街道/社区  三级联动
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult AllCommunity()
        {
            using (_dbContext)
            {
                var query = _dbContext.Village.Where(x => x.IsDelete == "0").OrderByDescending(x => x.Towns).ThenByDescending(x => x.Address).ThenByDescending(x => x.Vname).Select(x => new
                {
                    x.VillageUuid,
                    x.Vname,
                    x.Address,
                    Towns = x.Towns ?? "",
                });
                var query2 = _dbContext.Village.Where(x => x.IsDelete == "0").OrderByDescending(x=>x.Towns).AsEnumerable().GroupBy(x => x.Towns);
                var list = query.ToList();
                var list2 = new List<string>();
                foreach (var item in query2)
                {
                    if (item.Key == null)
                    {
                        list2.Add("");
                    }
                    else
                    {
                        list2.Add(item.Key);
                    }
                }
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(new { list, list2 }, list.Count);
                return Ok(response);
            }
        }

        /// <summary>
        /// 获取分数
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult getSingleScoreRecord(dynamic model)
        {
            Guid uid = model.uid;
            Guid hid = model.hid;
            DateTime time1 = model.time1;
            DateTime time2 = model.time2 + " 23:59:59";
            var date1 = Convert.ToDateTime(time1).ToString("yyyy-MM-dd HH:mm:ss");
            var date2 = Convert.ToDateTime(time2).ToString("yyyy-MM-dd HH:mm:ss");
            string MarkType = model.markType;

            using (_dbContext)
            {
                //垃圾投放
                var query = (from gr in _dbContext.GrabageDisposal
                             where gr.HomeAddressUuid == hid && gr.IsDelete != "1" && gr.IsScore=="1" && 
                             gr.AddTime.CompareTo(date1) >= 0 && gr.AddTime.CompareTo(date2) <= 0
                             select new
                             {
                                 gr.GarbageDisposalUuid,
                                 gr.AddTime,
                                 ddName = gr.SupervisorUu.RealName,
                                 gr.ScoreSettingUu.Integral,
                                 gr.ScoreSettingUu.ScoreName,
                                 gr.MarkType
                             }).OrderByDescending(x=>x.AddTime).ToList();
                var query1 = (from a in _dbContext.Attendance
                              where a.SystemUserUuid == uid && a.IsDelete != "1" && a.Type == "1" && 
                              a.ColckDate.CompareTo(date1) >= 0 && a.ColckDate.CompareTo(date2) <= 0
                              select new
                              {
                                  a.AmstartTime,
                                  a.AmendTime,
                                  a.PmstartTime,
                                  a.PmendTime,
                              }).ToList() ;
                var h = _dbContext.Overallsituation.FirstOrDefault();
                //重组数据，分上下午
                List<dynamic> temp = new List<dynamic>();
                foreach (var item in query1)
                {
                    //上/下午有一次打卡记录就加1，一天最多2次
                    if (!string.IsNullOrEmpty(item.AmstartTime) || !string.IsNullOrEmpty(item.AmendTime))
                    {
                        temp.Add(new
                        {
                            score=h.HourScore,
                            time = item.AmstartTime
                        });
                    }
                    if (!string.IsNullOrEmpty(item.PmstartTime) || !string.IsNullOrEmpty(item.PmendTime))
                    {
                        temp.Add(new
                        {
                            score = h.HourScore,
                            time = item.PmstartTime
                        });
                    }
                }
                //商品兑换记录
                //var query2 = (from gr in _dbContext.GoodsExchange
                //              where gr.SystemUserUuid == hid && gr.IsDelete != "1" && DateTime.Parse(gr.ExchageTime) <= time2 && DateTime.Parse(gr.ExchageTime) >= time1
                //              select new
                //              {
                //                  gr.StoreExchangeUuid,
                //                  gr.ExchageTime,
                //                  shop = gr.Shop.ShopName,
                //                  goods = gr.Goods.Gname,
                //                  gr.DeduceScore
                //              }).OrderByDescending(x => x.ExchageTime).ToList();
                var query2 = _dbContext.DateScore.Where(x => x.HomeAddressUuid == hid && x.AddTime.Contains(time1.ToString("yyyy"))).Select(x=>new { 
                    x.Jan,x.Feb,x.Mar,x.Apr,x.May,x.Jun,x.Jul,x.Aug,x.Sep,x.Oct,x.Nov,x.Dec
                }).ToList();
                List< dynamic > data = new List<dynamic>() { query, temp, query2 };
                double can = GetAllScore(hid) - GetUsedScore(hid);//可用积分
                if (can < 0)
                    can = 0;
                var query3 = _dbContext.HomeAddress.FirstOrDefault(x=>x.HomeAddressUuid==hid);
                query3.Score = can;
                _dbContext.SaveChanges();
                data.Add(can);
                int zyzsc = GetAllzyzScore(uid);//服务积分
                data.Add(zyzsc);
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(data);
                return Ok(response);
            }
        }
        /// <summary>
        /// 获取所有垃圾厢房
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllGrabRoom(string guid)
        {
            using (_dbContext)
            {
                var query = _dbContext.GrabageRoom.Where(x => x.IsDelete != "1");
                if (Guid.TryParse(guid, out Guid guid1))
                {
                    query = query.Where(x => x.VillageId == guid1);
                    query = query.Where(x => x.State == "0");
                }
                //查询今天已被预订的厢房
                //var temp = _dbContext.VolunteerYy.Where(x => x.StartTime == DateTime.Now.ToShortDateString()).Select(x => new { id = x.GrabRoomUuid }).ToList();
                //query.Where(x => !temp.FindAll(x.GarbageRoomUuid.ToString()));
                var list = query.Select(x => new { x.GarbageRoomUuid, x.Ljname }).ToList();

                if (list.Count == 0)
                {
                    list.Add(new { GarbageRoomUuid = Guid.Empty, Ljname = "" });
                }

                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(list);
                return Ok(response);
            }
        }
        /// <summary>
        /// 获取所有社区
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllVillage()
        {
            using (_dbContext)
            {
                var query = (from gr in _dbContext.Village
                             where gr.IsDelete != "1"
                             select new
                             {
                                 gr.VillageUuid,
                                 gr.Vname,
                             }).ToList();
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(query);
                return Ok(response);
            }
        }
        /// <summary>
        ///志愿者预约
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ZYZYY(ZYZYYViewModel model)
        {
            using (_dbContext)
            {
                //先判断是不是志愿者
                int en = _dbContext.SystemUser.Where(x => x.SystemRoleUuid.Contains(AuthContextService.CurrentUser.ZYZ) && x.SystemUserUuid == model.userId).Count();
                if (en <= 0)
                {
                    //不是志愿者，但是是用户就添加志愿者身份
                    var ent = _dbContext.SystemUser.FirstOrDefault(x => x.SystemUserUuid == model.userId);
                    ent.SystemRoleUuid = ent.SystemRoleUuid.TrimEnd(',') + "," + AuthContextService.CurrentUser.ZYZ;
                    _dbContext.SaveChanges();
                }
                //判断是否以预约过厢房
                var demo = _dbContext.VolunteerYy.Where(x => x.VolunteerUuid == model.userId && x.StartTime == model.startdate && x.Ap == model.ap).ToList();
                if (demo.Count()<=0)
                {
                    //判断这个厢房有没有被预订
                    var temp = _dbContext.VolunteerYy.Where(x => x.GrabRoomUuid == model.grabRoomUUid && x.StartTime == model.startdate && x.Ap == model.ap).ToList();
                    if (temp.Count() <= 0)
                    {
                        var entity = new HaikanRefuseClassification.Api.Entities.VolunteerYy();
                        entity.VolunteerYyuuid = Guid.NewGuid();
                        entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        entity.VolunteerUuid = model.userId;
                        entity.GrabRoomUuid = model.grabRoomUUid;
                        entity.StartTime = model.startdate;
                        entity.Ap = model.ap;
                        _dbContext.VolunteerYy.Add(entity);
                        _dbContext.SaveChanges();
                        var response = ResponseModelFactory.CreateInstance;
                        response.SetSuccess("success");
                        return Ok(response);
                    }
                    else
                        return Ok("YYED");
                }
                else {
                    return Ok("YYNO");
                    
                }
            }
        }

        /// <summary>
        /// 消息时间轴
        /// </summary>
        /// <returns></returns>
        [HttpGet("{guid}")]
        public IActionResult ShowMessage(string guid)
        {
            using (_dbContext)
            {
                var query = _dbContext.Message.Where(x => x.SystemUserUuid == Guid.Parse(guid)).OrderByDescending(x => x.AddTime);
                var list = query.Select(x => new { x.Message1, x.AddTime }).ToList();
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(list);
                return Ok(response);
            }
        }

        /// <summary>
        /// 获取用户积分
        /// </summary>
        /// <param name="ownid"></param>
        /// <returns></returns>
        private static double GetUsedScore(Guid ownid)
        {
            using (RefuseClassificationContext cc = new RefuseClassificationContext())
            {
                var query = (from a in cc.GoodsExchange
                             where a.SystemUserUuid == ownid && a.IsDelete != "1"
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
        /// <summary>
        /// 获取积分列表
        /// </summary>
        /// <param name="ownid"></param>
        /// <returns></returns>
        private static double GetAllScore(Guid ownid)
        {
            using (RefuseClassificationContext cc = new RefuseClassificationContext())
            {
                var query = from a in cc.GrabageDisposal
                            where a.HomeAddressUuid == ownid && a.IsDelete != "1" && a.IsScore=="1"
                            select new
                            {
                                Score = a.ScoreSettingUu.Integral
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
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        public IActionResult GetUserInfo(Guid guid)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                //var entity = _dbContext.SystemUser.FirstOrDefault(x => x.SystemUserUuid == guid);
                var query = (from a in _dbContext.SystemUser
                            join b in _dbContext.Shop
                            on a.ShopUuid equals b.ShopUuid
                            select new
                            {
                                a,
                                b.State
                            }).ToList();
                var entity = query.FirstOrDefault(x => x.a.SystemUserUuid == guid);
                response.SetSuccess();
                response.SetData(entity);
                return Ok(response);
            }
        }
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult editUserInfo(SystemUser model)
        {
            var response = ResponseModelFactory.CreateInstance;

            using (_dbContext)
            {
                var entity = _dbContext.SystemUser.FirstOrDefault(x => x.SystemUserUuid == model.SystemUserUuid);
                entity.OldCard = model.OldCard;
                entity.Address = model.Address;
                entity.RealName = model.RealName;


                _dbContext.SaveChanges();
                response.SetSuccess("完善成功");
                return Ok(response);
            }
        }
        /// <summary>
        /// 商店信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Shoplist()
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var entity = _dbContext.Shop.ToArray();

                response.SetData(entity);
                return Ok(response);
            }
        }

        /// <summary>
        /// 可兑换商品信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult goodlist(Guid guid)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var entity = _dbContext.Goods.Where(x=>x.ShopId==guid).ToArray();

                response.SetData(entity);
                return Ok(response);
            }
        }
        /// <summary>
        /// 添加兑换记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult AddchangeGoods(dynamic model)
        {
            try
            {
                var response = ResponseModelFactory.CreateInstance;

                using (_dbContext)
                {
                    var entity = new GoodsExchange();
                    entity.StoreExchangeUuid = Guid.NewGuid();
                    entity.ShopId = model.ShopId;
                    entity.IsDelete = "0";
                    entity.SystemUserUuid = model.SystemUserUuid;
                    entity.ExchageTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    entity.DeduceScore = model.DeduceScore;
                    //entity.GoodsId = model.GoodsId;

                    var temp = _dbContext.HomeAddress.FirstOrDefault(x => x.HomeAddressUuid == entity.SystemUserUuid);
                    temp.Score = temp.Score - double.Parse(entity.DeduceScore);
                    _dbContext.GoodsExchange.Add(entity);
                    _dbContext.SaveChanges();


                    response.SetSuccess("兑换成功");
                    return Ok(response);
                }
            }
            catch (Exception es)
            {

                return Ok(es.Message);
            }
            
        }
        /// <summary>
        /// 获取用户二维码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetEWM(Guid id)
        {
            using (_dbContext)
            {
                var query = _dbContext.SystemUser.FirstOrDefault(x => x.SystemUserUuid == id);
                //var phone = query.Phone;
                //string old = query.OldCard;
                //string address = query.Address;
                var response = ResponseModelFactory.CreateInstance;
                response.SetData("u_"+query.SystemUserUuid.ToString());
                return Ok(response);
            }
        }
        /// <summary>
        /// 获取用户家庭码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetJiatinEWM(Guid id)
        {
            using (_dbContext)
            {
                //var query = _dbContext.SystemUser.FirstOrDefault(x => x.HomeAddressUuid == id);
                //var phone = query.Phone;
                //string old = query.OldCard;
                //string address = query.Address;
                var query1 = _dbContext.HomeAddress.FirstOrDefault(x=>x.HomeAddressUuid==id);
                var response = ResponseModelFactory.CreateInstance;
                response.SetData("h_" + query1.HomeAddressUuid.ToString()+","+query1.Address);
                return Ok(response);
            }
        }
        /// <summary>
        /// 确认预约再打卡
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="ap"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult CheckSubscribe(Guid guid,string ap)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var date = DateTime.Now.ToString("yyyy-MM-dd");
                var entity = _dbContext.VolunteerYy.FirstOrDefault(x => x.VolunteerUuid == guid && x.Ap == ap && x.StartTime.Contains(date));
                if (entity != null)
                {
                    response.SetSuccess();
                    return Ok(response);
                }
                else
                {
                    response.SetFailed("请先预约再打卡");
                    return Ok(response);
                }
            }
        }

        /// <summary>
        /// 生成二维码信息
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string ewm(string text)
        {
            Random r = new Random();
            string webRootPath = _hostingEnvironment.WebRootPath;
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            Bitmap bt;
            string enCodeString = text;
            QRCodeGenerator generator = new QRCodeGenerator();
            QRCodeData codeData = generator.CreateQrCode(text, QRCodeGenerator.ECCLevel.M, true);
            QRCoder.QRCode qrcode = new QRCoder.QRCode(codeData);
            Bitmap qrImage = qrcode.GetGraphic(15, Color.Black, Color.White, true);
            string filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + r.Next(1, 100) + ".jpg";
            string filepath= webRootPath + "\\UploadFiles\\EWM\\" + filename;
            qrImage.Save(filepath, System.Drawing.Imaging.ImageFormat.Jpeg);

            return filename;
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

    }
}