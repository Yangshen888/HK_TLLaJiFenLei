using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaikanRefuseClassification.Api.Configurations;
using HaikanRefuseClassification.Api.Entities;
using HaikanRefuseClassification.Api.Entities.Enums;
using HaikanRefuseClassification.Api.Extensions;
using HaikanRefuseClassification.Api.Models.Response;
using HaikanRefuseClassification.Api.RequestPayload.Monitoring;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HaikanRefuseClassification.Api.Controllers.Api.V1.Monitoring
{
    [Route("api/v1/Monitoring/[controller]/[action]")]
    [ApiController]
    public class MonitoringController : ControllerBase
    {
        private readonly RefuseClassificationContext _dbContext;
        private readonly IMapper _mapper;
        private readonly MdDesEncrypt MdDesEncrypt;
        //用来获取路径相关
        private IHostingEnvironment _hostingEnvironment;

        public MonitoringController(RefuseClassificationContext dbContext, IMapper mapper, IOptions<MdDesEncrypt> mdDesEncrypt, IHostingEnvironment hostingEnvironment)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            MdDesEncrypt = mdDesEncrypt.Value;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// 车辆监控列表（车牌号模糊查询）
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CarList(MonitoringRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = from m in _dbContext.CarMonitoring
                            join c in _dbContext.Car
                            on m.CarUuid equals c.CarUuid
                            where c.IsDelete != 1 && m.IsDelete != 1
                            select new
                            {
                                m.CarUuid,
                                m.MonitoringNum,
                                m.Remark,
                                m.AddTime,
                                c.CarNum,
                                m.CarMonitoringUuid
                            };
                //车牌号筛选
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.CarNum.Contains(payload.Kw));
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
        /// 获取车辆下拉框
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CarNums()
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var query = _dbContext.Car.Where(x => x.IsDelete == 0 ).ToList();
                response.SetData(query);
                return Ok(response);
            }
        }

        /// <summary>
        /// 添加车辆监控信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult AddCarMonit(dynamic model)
        {
            try
            {
                var response = ResponseModelFactory.CreateInstance;
                using (_dbContext)
                {
                    var entity = new HaikanRefuseClassification.Api.Entities.CarMonitoring();
                    entity.CarMonitoringUuid = Guid.NewGuid();
                    entity.MonitoringNum = model.monitoringNum;
                    entity.CarUuid = model.carUuid;
                    entity.PalyType = model.palyType;
                    entity.SvrIp = model.svrIp;
                    entity.SvrPort = model.svrPort;
                    entity.Appkey = model.appkey;
                    entity.AppSecret = model.appSecret;
                    entity.Time = model.time;
                    entity.TimeSecret = model.timeSecret;
                    entity.Httpsflag = model.httpsflag;
                    entity.CamList = model.camList;
                    entity.Remark = model.remark;
                    entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd");
                    entity.IsDelete = 0;
                    var entity2 = _dbContext.Car.FirstOrDefault(x => x.CarUuid == entity.CarUuid);
                    entity2.CameraId = entity.MonitoringNum;
                    _dbContext.CarMonitoring.Add(entity);
                    _dbContext.SaveChanges();
                    response.SetSuccess("添加成功");
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                return  Ok(ex);
            }
            
        }

        /// <summary>
        /// 获取车辆监控编辑信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        public IActionResult GetCarMonit(Guid guid)
        {
            using (_dbContext)
            {
                var entity = _dbContext.CarMonitoring.FirstOrDefault(x => x.CarMonitoringUuid == guid);
                var query = from m in _dbContext.CarMonitoring
                            join c in _dbContext.Car
                            on m.CarUuid equals c.CarUuid
                            where c.CarUuid == entity.CarUuid
                            && m.CarMonitoringUuid==entity.CarMonitoringUuid
                            select new
                            {
                                m.MonitoringNum,
                                m.CarUuid,
                                c.CarNum,
                                m.AddTime,
                                m.Appkey,
                                m.AppSecret,
                                m.CamList,
                                m.CarMonitoringUuid,
                                m.Httpsflag,
                                m.PalyType,
                                m.Remark,
                                m.SvrIp,
                                m.SvrPort,
                                m.Time,
                                m.TimeSecret
                            };
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(query.ToList());
                return Ok(response);
            }
        }

        /// <summary>
        /// 编辑车辆监控信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult EditCarMonit(dynamic model)
        {

            string guid = model.carMonitoringUuid;
            string MonitoringNum = model.monitoringNum;
            string CarUuid = model.carUuid;
            string PalyType = model.palyType;
            string SvrIp = model.svrIp;
            string SvrPort = model.svrPort;
            string Appkey = model.appkey;
            string AppSecret = model.appSecret;
            string Time = model.time;
            string TimeSecret = model.timeSecret;
            string Httpsflag = model.httpsflag;
            string CamList = model.camList;
            string Remark = model.remark;
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            using (_dbContext)
            {
                var entity = _dbContext.CarMonitoring.FirstOrDefault(x => x.CarMonitoringUuid == Guid.Parse(guid));
                Guid carUuid = entity.CarUuid.Value;
                entity.MonitoringNum = model.monitoringNum;
                entity.CarUuid = model.carUuid;
                entity.PalyType = model.palyType;
                entity.SvrIp = model.svrIp;
                entity.SvrPort = model.svrPort;
                entity.Appkey = model.appkey;
                entity.AppSecret = model.appSecret;
                entity.Time = model.time;
                entity.TimeSecret = model.timeSecret;
                entity.Httpsflag = model.httpsflag;
                entity.CamList = model.camList;
                entity.Remark = model.remark;
                //entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd");
                //entity.IsDelete = 0;
                var entity2 = _dbContext.Car.FirstOrDefault(x => x.CarUuid == entity.CarUuid);
                if (entity2.CarUuid == carUuid)
                {
                    entity2.CameraId = entity.MonitoringNum;
                }
                else
                {
                    var entity3 = _dbContext.Car.FirstOrDefault(x => x.CarUuid == carUuid);
                    entity3.CameraId = null;
                }
                _dbContext.SaveChanges();
                response.SetSuccess("修改成功");
                return Ok(response);
            }
        }

        /// <summary>
        /// 删除车辆监控
        /// </summary>
        /// <param name="isDeleted"></param>
        /// <param name="ids">车辆ID字符串,多个以逗号隔开</param>
        /// <returns></returns>
        private ResponseModel UpdateIsDelete(CommonEnum.IsDeleted isDeleted, string ids)
        {
            using (_dbContext)
            {
                var parameters = ids.Split(",").Select((id, index) => new SqlParameter(string.Format("@p{0}", index), id)).ToList();
                foreach (var item in parameters)
                {
                     var query1 = _dbContext.CarMonitoring.Where(x => x.CarMonitoringUuid == Guid.Parse(item.Value.ToString())).ToList();
                    foreach (var item1 in query1)
                    {
                        var query = _dbContext.Car.FirstOrDefault(x => x.CarUuid == item1.CarUuid);
                        query.CameraId = null;
                        _dbContext.SaveChanges();
                    }
                }
                var parameterNames = string.Join(", ", parameters.Select(p => p.ParameterName));
                var sql = string.Format("UPDATE CarMonitoring SET IsDelete=@IsDeleted WHERE CarMonitoringUUID IN ({0})", parameterNames);
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
        /// <param name="ids">厢房ID,多个以逗号分隔</param>
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
    }
}