using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AutoMapper;
using HaikanRefuseClassification.Api.Entities;
using HaikanRefuseClassification.Api.Extensions;
using HaikanRefuseClassification.Api.ViewModels.CarApp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HaikanRefuseClassification.Api.Controllers.Api.V1.CarApp
{
    [Route("api/v1/carapp/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class CarAppController : ControllerBase
    {
        private readonly RefuseClassificationContext _dbContext;
        private readonly IMapper _mapper;

        public CarAppController(RefuseClassificationContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        [HttpPost]
        public IActionResult List(RequestPayload.RequestPayload payload)
        {
            using (_dbContext)
            {
                //var query = from gr in _dbContext.GrabageRoom
                //            join v in _dbContext.Village
                //            on gr.VillageId equals v.VillageUuid
                //            where gr.IsDelete == "0"
                //            select new
                //            {
                //                gr.Posititon,
                //                gr.Ljname,
                //                v.Vname,
                //                gr.SystemUser.FirstOrDefault(x => x.SystemUserUuid == gr.SystemUserUuid).RealName,

                //            };
                var response =
            ResponseModelFactory.CreateResultInstance;
                //response.SetData(list, totalCount);
                return Ok(response);
            }
        }

        /// <summary>
        /// 获取当前请求的车牌号
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{imei}")]
        public IActionResult CarInfo(string imei)
        {

            using (_dbContext)
            {
                var response = ResponseModelFactory.CreateResultInstance;
                if (!string.IsNullOrEmpty(imei))
                {
                    var query = from dc in _dbContext.DeviceToCar
                                join c in _dbContext.Car
                                on dc.CarUuid equals c.CarUuid
                                where c.IsDelete == 0 && dc.Imei == imei
                                select new
                                {
                                    dc.DeviceToCarUuid,
                                    dc.Imei,
                                    dc.CarUuid,
                                    c.CarNum,
                                };
                    var entity = query.FirstOrDefault();


                    //  DeviceToCar query2 = _dbContext.DeviceToCar.FirstOrDefault(x => x.Imei == imei);
                    //var CarNum = query2.CarUu.CarNum;
                    //DeviceCarModel model = new DeviceCarModel(query2,CarNum);
                    response.SetData(entity);
                    return Ok(response);

                }
                else
                {
                    response.SetFailed();
                    return Ok(response);
                }
                //var query=_dbContext.Car

                //response.SetData(list, totalCount);

            }
        }

        /// <summary>
        /// 问题反馈
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult AddQuestion(Question model)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                model.QuestionUuid = Guid.NewGuid();
                model.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
                model.IsDelete = "0";
                _dbContext.Question.Add(model);
                _dbContext.SaveChanges();
                response.SetSuccess();
                return Ok(response);
            }
        }

        /// <summary>
        /// 获取称重记录
        /// </summary>
        /// <param name="imei"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetWeighRecord(dynamic models)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                string imei = models.imei;
                string latitude = models.latitude;
                string longitude = models.longitude;
                string guuid="";
                int isgroom = 0;
                if (string.IsNullOrEmpty(imei))
                {
                    response.SetFailed("IMEI信息为空");
                    return Ok(response);
                }
                var entityroom = _dbContext.GrabageRoom.Where(x=>x.IsDelete!="1").ToList();
                foreach (var item in entityroom)
                {
                    if (!string.IsNullOrEmpty(item.Lat) && !string.IsNullOrEmpty(item.Lon))
                    {
                        var result = GetDistance(Convert.ToDouble(longitude), Convert.ToDouble(latitude), Convert.ToDouble(item.Lon),  Convert.ToDouble(item.Lat));
                        if (result<=20)
                        {
                            isgroom = 1;
                            guuid = item.GarbageRoomUuid.ToString();
                            break;
                        }
                    }
                }
                if (isgroom==0)
                {
                    response.SetFailed("距离箱房位置超20米！");
                    return Ok(response);
                }
                var query = from dc in _dbContext.DeviceToCar
                            join c in _dbContext.Car
                            on dc.CarUuid equals c.CarUuid
                            join gw in _dbContext.GrabageWeightSon
                            on c.CarUuid equals gw.CarUuid
                            where dc.Imei == imei && c.IsDelete == 0 && gw.IsCheck == 0 && gw.WeightTime.Substring(0, 10) == DateTime.Now.ToString("yyyy-MM-dd")
                            select new
                            {
                                gw.GrabageWeighingRecordUuid,
                                gw.GrabageRoomId,
                                gw.GrabageRoom.Village.Vname,
                                gw.AddTime,
                                gw.WeightTime,
                                gw.Weight,
                                gw.GrabageRoom.Ljname,
                                gw.GrabageRoom.VillageId,
                                //gw.Type,
                            };
                query = query.OrderByDescending(x => x.WeightTime);
                var entity = query.Take(1).FirstOrDefault();
                if (entity == null)
                {
                    response.SetFailed("已确认称重记录");
                    return Ok(response);
                }
                var gwquery = _dbContext.GrabageWeightSon.FirstOrDefault(x=>x.GrabageWeighingRecordUuid==entity.GrabageWeighingRecordUuid);
                if (gwquery.GrabageRoomId==null)
                {
                    gwquery.GrabageRoomId = Guid.Parse(guuid);
                    _dbContext.SaveChanges();
                }
                else
                {
                    guuid = entity.GrabageRoomId.ToString();
                }
                var list = query.ToList();
                WeighDataModel model = new WeighDataModel();
                model.GrabageWeighingRecordUuid = entity.GrabageWeighingRecordUuid;
                model.GrabageRoomId = Guid.Parse(guuid);
                model.Vname = entity.Vname;
                model.WeighData = new List<Array>();
                model.Ljname = entity.Ljname;
                model.VillageId = entity.VillageId;
                //model.Type = entity.Type;
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].GrabageRoomId != Guid.Parse(guuid))
                    {
                        break;
                    }
                    string[] arr = new string[3];
                    arr[0] = list[i].AddTime;
                    arr[1] = list[i].Weight;
                    arr[2] = list[i].GrabageWeighingRecordUuid.ToString();
                    model.WeighData.Add(arr);
                }
                response.SetData(model);
                return Ok(response);
            }


        }

        /*          
        * @param lng1第一个经度
        * @param lat1 第一个纬度
        * @param lng2第二个经度
        * @param lat2第二个纬度         
        * @return 两个经纬度的距离(单位千米)
        */
        public static double GetDistance(double lng1, double lat1, double lng2, double lat2)
        {
            double earth_radius = 6378.137;//地球半径,单位千米
            double radLat1 = lat1 * Math.PI / 180.0;
            double radLat2 = lat2 * Math.PI / 180.0;
            double radLng1 = lng1 * Math.PI / 180.0;
            double radLng2 = lng2 * Math.PI / 180.0;
            double a = radLat1 - radLat2;
            double b = radLng1 - radLng2;

            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
                    Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * earth_radius;
            s = Math.Round(s * 10000) / 10000;
            return s;
        }


        [HttpGet]
        public IActionResult GetAllGrabRoom(string guid)
        {
            using (_dbContext)
            {
                var query = _dbContext.GrabageRoom.Where(x => x.IsDelete != "1");
                if (Guid.TryParse(guid, out Guid guid1))
                {
                    query = query.Where(x => x.VillageId == guid1);
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
        /// 确认称重记录
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CheckWeighRecord(CheckWeighRecordModel model)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                int num = 0;
                for (int i = 0; i < model.WeighData.Count; i++)
                {
                    var entity = _dbContext.GrabageWeightSon.FirstOrDefault(x => x.GrabageWeighingRecordUuid == Guid.Parse(model.WeighData[i][2]));
                    entity.GrabageRoomId = model.Room;
                    //entity.Type = model.Type;
                    entity.CheckedUser = model.CheckedUser;
                    entity.AddTime = model.WeighData[i][0];
                    entity.Weight = model.WeighData[i][1];
                    entity.Grade = model.Grade;
                    entity.IsCheck = 1;
                    num += _dbContext.SaveChanges();
                }
                if (num > 0)
                {
                    response.SetSuccess("确认成功");

                }
                else
                {
                    response.SetFailed("确认失败");
                }
                return Ok(response);
            }
            //response.SetData(list);

        }

        /// <summary>
        /// 车载APP实时地图数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult RecycleInfo(string qs)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var query = _dbContext.RecycleInfo.Where(x => x.Lat != null && x.Lon != null).OrderBy(x => x.GarbageRoomUuid
                );
                var list = query.ToList();
                response.SetData(list);
                return Ok(response);
            }
        }
    }
}
