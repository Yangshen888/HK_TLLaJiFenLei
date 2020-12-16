using AutoMapper;
using HaikanRefuseClassification.Api.Entities;
using HaikanRefuseClassification.Api.Entities.Enums;
using HaikanRefuseClassification.Api.Extensions;
using HaikanRefuseClassification.Api.Extensions.AuthContext;
using HaikanRefuseClassification.Api.Models.Response;
using HaikanRefuseClassification.Api.RequestPayload.Base;
using HaikanRefuseClassification.Api.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HaikanRefuseClassification.Api.RequestPayload.Grab;

using Microsoft.EntityFrameworkCore;
using HaikanRefuseClassification.Api.Controllers.Api.V1.GiMap;
using System.Net;
using System.ComponentModel;
using System.Text;
using System.IO;
using SmartCampusManagementWatchDog.Model;
using Newtonsoft.Json;
using System.Collections;
using Microsoft.Data.SqlClient;

namespace HaikanRefuseClassification.Api.Controllers.Api.V1.grab
{
    [Route("api/v1/grab/[controller]/[action]")]
    [ApiController]
    public class TravelController : ControllerBase
    {
        private readonly RefuseClassificationContext _dbContext;
        private readonly IMapper _mapper;
        public TravelController(RefuseClassificationContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// 车辆行驶记录查询
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult List(TravelRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = from a in _dbContext.Car
                            where  a.IsDelete != 1
                            select new
                            {
                                a.CarUuid,
                                a.CarNum,
                                a.BalanceId,
                                a.CameraId,
                                a.CarType,
                                a.Company,
                                GrabType = a.GrabType == "0" ? "其他垃圾" : a.GrabType == "1" ? "易腐垃圾" : a.GrabType == "2" ? "有害垃圾" : a.GrabType == "3" ? "可回收垃圾" : "",
                                a.Street
                            };
                //车牌号筛选
                if (!string.IsNullOrEmpty(payload.kw))
                {
                    query = query.Where(x => x.CarNum.Contains(payload.kw));
                }
                //分页
                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(list, totalCount);
                return Ok(response);
            }
        }
        
        /// <summary>
        /// 获取车辆信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetCarList()
        {
            using (_dbContext)
            {
                var query = _dbContext.Car.Where(x=>x.IsDelete!=1).ToList();
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(query);
                return Ok(response);
            }
        }

        [HttpPost]
        public IActionResult TimeList(TravelRequestPayload payload)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            //var datatime = payload.kw1.Split("T")[0];
            var datatime = Convert.ToDateTime(payload.kw1).ToString("yyyy-MM-dd");
            //var datatime1 = DateTime.Parse(datatime);
            //datatime1 = datatime1.AddDays(1);
            //var datatime2 = datatime1.ToString("yyyy/MM/dd");
            //var startTime = payload.kw2[0];
            //var startTime1 = startTime.Split("’")[0];
            //var startTime2 = startTime.Split("’")[1];
            //startTime = datatime2 + " " + startTime1 + ":" + startTime2 + ":00";
            //var endTime = payload.kw2[1];
            //var endTime1 = endTime.Split("’")[0];
            //var endTime2 = endTime.Split("’")[1];
            //endTime = datatime2 + " " + endTime1 + ":" + endTime2 + ":00";
            using (_dbContext)
            {
                var query = _dbContext.VehicleInfo.Where(x => x.CarUuid == Guid.Parse(payload.CarUuid) && x.Addtime.Contains(datatime)).OrderBy(x=>x.Addtime).ToList();

                response.SetData(query);
                return Ok(response);
            }
        }

        /// <summary>
        /// 获取垃圾箱房经纬度
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ListLonLat()
        {
            using (_dbContext)
            {
                using (_dbContext)
                {
                    List<dynamic> dat = new List<dynamic>();
                    var query = _dbContext.GrabageRoom.Where(x => x.IsDelete != "1" && x.Lat != "" && x.Lon != "" && x.Lat != null && x.Lon != null).Select(x => new
                    {
                        lon = MapController.GCJ2BD(x.Lon, x.Lat).lon.ToString(),
                        lat = MapController.GCJ2BD(x.Lon, x.Lat).lat.ToString(),
                        name = x.Ljname,
                        code = x.Id,
                        type = 1,
                    }).ToList();
                    foreach (var item in query)
                    {
                        dat.Add(item);
                    }
                    var response = ResponseModelFactory.CreateResultInstance;
                    response.SetData(query.ToList());
                    return Ok(response);
                }
            }
        }


        /// <summary>
        /// 车辆历史轨迹
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult CarList(string guid)
        {
            using (_dbContext)
            {
                var query = (from a in _dbContext.Car
                             where a.CarUuid == Guid.Parse(guid)
                             select new
                             {
                                 a.CarNum,
                                 a.HolderId,
                                 a.HolderPhone,
                                 a.Weight,
                                 a.CarUuid
                             }).ToList();
                //var entity = query.FirstOrDefault();
                //var entity = _dbContext.Attendance.FirstOrDefault(x => x.AttendanceUuid == model.AttendanceUuid);
                //var query = _mapper.Map<HaikanRefuseClassification.Api.Entities.Attendance, CensorShowViewModel>(entity);
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(query);
                return Ok(response);
            }
        }

        /// <summary>
        /// 垃圾信息
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ljList(TravelRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = from a in _dbContext.GrabageRoom
                            join u in _dbContext.GrabageWeighting
                            on a.GarbageRoomUuid equals u.GrabageRoomId
                            join v in _dbContext.Village
                            on a.VillageId equals v.VillageUuid
                            where  a.IsDelete != "1"
                            && u.IsDelete != "1"
                            && v.IsDelete!="1"
                            select new
                            {
                                a.Ljname,
                                v.Vname,
                                u.Weight,
                                u.AddTime,
                                u.GrabageWeighingRecordUuid

                            };
                //分页
                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(list, totalCount);
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
                var sql = string.Format("UPDATE Car SET IsDelete=@IsDeleted WHERE CarUUID IN ({0})", parameterNames);
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

        [HttpGet]
        public IActionResult GetCarSite()
        {
            using (_dbContext)
            {
                var response = ResponseModelFactory.CreateResultInstance;

                #region 定位认证
                //发送的参数
                string url = "http://47.96.2.154/clbs/oauth/token";
                model1 user = new model1();
                user.client_id = "mobile_1";
                user.client_secret = "secret_1";
                user.grant_type = "password";
                user.scope = "read";
                user.username = "zhengfu_api";
                user.password = "lanhai123!@#";
                var strPostData = ToPaeameter(user);
                var strResponse = PostUrl(url, strPostData);
                TrackToken usermodel = JsonConvert.DeserializeObject<TrackToken>(strResponse);
                #endregion

                var car = _dbContext.Car.Where(x=>x.IsDelete!=1 && x.AddPeople=="狗").ToList();
                var date =new ArrayList();

                #region 获取定位
                //发送参数
                var name = string.Empty;
                var access_token = usermodel.value;
                for (int i = 0; i < car.Count; i++)
                {
                    name = car[i].CarNum;
                    var request = (HttpWebRequest)WebRequest.Create("http://47.96.2.154/clbs/swagger/m/monitoring/monitor/location/latest?name=" + name + "&access_token=" + access_token);
                    request.Method = "GET";
                    //结果返回
                    var responses = (HttpWebResponse)request.GetResponse();
                    //转字符串
                    var responseString = new StreamReader(responses.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                    //转换为json对象
                    Track commodel = JsonConvert.DeserializeObject<Track>(responseString);
                    if (!string.IsNullOrEmpty(commodel.data[0].longitude.ToString()) && !string.IsNullOrEmpty(commodel.data[0].latitude.ToString()))
                    {
                        Guid caruuid = car[i].CarUuid;
                        var log = ((double)commodel.data[0].longitude / (double)1000000).ToString();
                        var lat = ((double)commodel.data[0].latitude / (double)1000000).ToString();

                        #region 经纬度转换
                        var jwdrequest = (HttpWebRequest)WebRequest.Create("http://api.map.baidu.com/geoconv/v1/?coords=" + log + "," + lat + "&from=1&to=5&ak=nSxiPohfziUaCuONe4ViUP2N");
                        request.Method = "GET";
                        //结果返回
                        var jwdresponses = (HttpWebResponse)jwdrequest.GetResponse();
                        //转字符串
                        var jwdresponseString = new StreamReader(jwdresponses.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                        //转换为json对象
                        JWDBaidu jwdcommodel = JsonConvert.DeserializeObject<JWDBaidu>(jwdresponseString);
                        #endregion

                        if (jwdcommodel.status == 0)
                        {
                            var query = new
                            {
                                name,
                                lat=jwdcommodel.result[0].y.ToString(),
                                lon= jwdcommodel.result[0].x.ToString(),
                                cartype=car[i].CarType,
                                grabtype=car[i].GrabType
                            };
                            date.Add(query);
                        }
                    }
                }
                #endregion

                response.SetData(date);
                return Ok(response);
            }
        }

        public string PostUrl(string url, string postData)
        {
            string result = "";
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

                req.Method = "POST";

                req.ContentType = "application/x-www-form-urlencoded";

                req.Timeout = 800;//请求超时时间

                byte[] data = Encoding.UTF8.GetBytes(postData);

                req.ContentLength = data.Length;

                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(data, 0, data.Length);

                    reqStream.Close();
                }

                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

                Stream stream = resp.GetResponseStream();

                //获取响应内容
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                result = e.ToString();
            }

            return result;
        }

        /// <summary>
        /// 参数拼接Url
        /// </summary>
        /// <param name="source">要拼接的实体</param>
        /// <paramref name="IsStrUpper">是否开启首字母大写,默认小写</paramref>
        /// <returns>Url,</returns>
        public string ToPaeameter(object source)
        {
            var buff = new StringBuilder(string.Empty);
            if (source == null)
                throw new ArgumentNullException("source", "Unable to convert object to a dictionary. The source object is null.");
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(source))
            {
                object value = property.GetValue(source);
                if (value != null)
                {
                    buff.Append(WebUtility.UrlEncode(property.Name) + "=" + WebUtility.UrlEncode(value + "") + "&");
                }
            }
            return buff.ToString().Trim('&');
        }
    }
}