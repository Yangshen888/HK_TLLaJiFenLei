using HaikanRefuseClassification.Api.Configurations;
using HaikanRefuseClassification.Api.Entities;
using HaikanRefuseClassification.Api.Entities.Enums;
using HaikanRefuseClassification.Api.Extensions;
using HaikanRefuseClassification.Api.Extensions.AuthContext;
using HaikanRefuseClassification.Api.Extensions.CustomException;
using HaikanRefuseClassification.Api.Models.Response;
using HaikanRefuseClassification.Api.RequestPayload.Person;
using HaikanRefuseClassification.Api.ViewModels.App;
using HaikanRefuseClassification.Api.ViewModels.Person;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.Controllers.Api.V1.GiMap
{
    [Route("api/v1/GiMap/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class MapController : ControllerBase
    {
        private readonly RefuseClassificationContext _dbContext;
        public MapController(RefuseClassificationContext db)
        {
            _dbContext = db;
        }
        /// <summary>
        /// 获取标点 垃圾厢房 商店
        /// </summary>
        /// <returns></returns>
        public IActionResult getAllMarker()
        {
            using (_dbContext)
            {
                List<dynamic> dat = new List<dynamic>();
                //var grabroom = _dbContext.GrabageRoom.Where(x =>x.State=="0"&&x.IsDelete != "1" && x.Lat != "" && x.Lon != ""&&x.Lat!=null&&x.Lon!=null).Select(x => new
                //{
                //    lng = MapController.GCJ2BD(x.Lon,x.Lat).lon.ToString(),
                //    lat = MapController.GCJ2BD(x.Lon, x.Lat).lat.ToString(),
                //    name = x.Ljname,
                //    code = x.Id,
                //    type = 1,
                //}).ToList() ;
                //foreach (var item in grabroom)
                //{
                //    dat.Add(item);
                //}
                var rooms = _dbContext.RecycleInfo.Where(x => x.Lat != null && x.Lon != null).OrderBy(x => x.GarbageRoomUuid
                ).ToList();
                var shop = _dbContext.Shop.Where(x => x.IsDelete != "1" && x.Lat != "" && x.Lon != "" && x.Lat != null && x.Lon != null).Select(x => new
                {
                    x.Id,
                    lng = MapController.GCJ2BD(x.Lon, x.Lat).lon.ToString(),
                    lat = MapController.GCJ2BD(x.Lon, x.Lat).lat.ToString(),
                    name =x.ShopName,
                    code=x.Id,
                    type = 2
                }).ToList();
                foreach (var item in shop)
                {
                    dat.Add(item);
                }
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(new { dat, rooms });
                return Ok(response);
            }
        }

        /// <summary>
        /// 获取地图点位信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetAddress(GetAddressModel model)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            if (model.Lat == null || model.Lon == null || string.IsNullOrEmpty(model.Key))
            {
                response.SetFailed();
                response.SetData(null);
                return Ok(response);
            }
            var code = "0";
            //先根据用户请求的uri构造请求地址
            string serviceUrl = "https://apis.map.qq.com/ws/geocoder/v1/?location=" + model.Lat+","+model.Lon + "&key=" + model.Key;
            //string serviceUrl = "https://apis.map.qq.com/ws/geocoder/v1/";
            //创建Web访问对象
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(serviceUrl);
            //把用户传过来的数据转成“UTF-8”的字节流
            //byte[] buf = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(data);

            //myRequest.Method = "GET";
            //myRequest.ContentLength = buf.Length;
            myRequest.ContentType = "application/json";
            //myRequest.MaximumAutomaticRedirections = 1;
            //myRequest.AllowAutoRedirect = true;
            //发送请求
            //Stream stream = myRequest.GetRequestStream();
            //stream.Write(buf, 0, buf.Length);
            //stream.Close();

            //获取接口返回值
            //通过Web访问对象获取响应内容
            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
            //通过响应内容流创建StreamReader对象，因为StreamReader更高级更快
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            //string returnXml = HttpUtility.UrlDecode(reader.ReadToEnd());//如果有编码问题就用这个方法
            string returnJson = reader.ReadToEnd();//利用StreamReader就可以从响应内容从头读到尾
            code = myResponse.StatusCode.ToString();
            reader.Close();
            myResponse.Close();
            var data = JsonConvert.DeserializeObject(returnJson);
            if (code == "OK"&& data!=null)
            {
                response.SetSuccess();
                response.SetData(JsonConvert.DeserializeObject(returnJson));
                return Ok(response);
            }
            else
            {
                response.SetFailed();
                response.SetData(null);
                return Ok(response);
            }
            
        }


        /// <summary>
        /// bd09->gcj02
        /// </summary>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <returns></returns>
        public static Point BD2GCJ(string longitude, string latitude)
        {
            
            double x = Convert.ToDouble(longitude) - 0.0065, y = Convert.ToDouble(latitude) - 0.006;
            double z = Math.Sqrt(x * x + y * y) - 0.00002 * Math.Sin(y * Math.PI);
            double theta = Math.Atan2(y, x) - 0.000003 * Math.Cos(x * Math.PI);

            double lng = z * Math.Cos(theta);//lng
            double lat = z * Math.Sin(theta);//lat
            return new Point(lat, lng);
        }

        /// <summary>
        /// gcj02->bd09
        /// </summary>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <returns></returns>
        public static Point GCJ2BD(string longitude, string latitude)
        {
            double x = Convert.ToDouble(longitude), y = Convert.ToDouble(latitude);
            double z = Math.Sqrt(x * x + y * y) + 0.00002 * Math.Sin(y * Math.PI);
            double theta = Math.Atan2(y, x) + 0.000003 * Math.Cos(x * Math.PI);
            double tempLon = z * Math.Cos(theta) + 0.0065;
            double tempLat = z * Math.Sin(theta) + 0.006;
            return new Point(tempLat, tempLon);

        }
    }

    public class Point
    {
        public Point() { }
        public Point(double lat,double lon) 
        {
            this.lat = lat;
            this.lon = lon;
        }
        public double lon { get; set; }
        public double lat { get; set; }
    }
}