using System;
using System.Collections.Generic;

using System.IO;
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
using Newtonsoft.Json;

namespace HaikanRefuseClassification.Api.Controllers.Api.V1.Monitoring
{
    [Route("api/v1/Monitoring/[controller]/[action]")]
    [ApiController]
    public class GMonitoringController : ControllerBase
    {
        private readonly RefuseClassificationContext _dbContext;
        private readonly IMapper _mapper;
        private readonly MdDesEncrypt MdDesEncrypt;
        //用来获取路径相关
        private IHostingEnvironment _hostingEnvironment;

        public GMonitoringController(RefuseClassificationContext dbContext, IMapper mapper, IOptions<MdDesEncrypt> mdDesEncrypt, IHostingEnvironment hostingEnvironment)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            MdDesEncrypt = mdDesEncrypt.Value;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// 厢房监控列表（厢房名称模糊查询）
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GrabageList(MonitoringRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = from m in _dbContext.GrabageMonitoring
                            join c in _dbContext.GrabageRoom 
                            on m.GarbageRoomUuid equals c.GarbageRoomUuid into gr from g in gr.DefaultIfEmpty()
                            where g.IsDelete!= "1" && m.IsDelete != 1
                            select new
                            {
                                m.Id,
                                m.GarbageRoomUuid,
                                m.MonitoringNum,
                                m.Remark,
                                m.AddTime,
                                m.JiankongName,
                                m.RegionId,
                                m.Latitude,
                                m.Longitude,
                                Ljname=g.Ljname,
                                m.GrabageMonitoringUuid,
                                m.VideoUrl
                                //c.VillageId
                            };
                ////街道筛选
                //if (!string.IsNullOrEmpty(payload.street))
                //{
                //    query = query.Where(x => x.JiankongName.Contains(payload.street));
                //}
                ////社区筛选
                //if (!string.IsNullOrEmpty(payload.ccmmunity))
                //{
                //    query = query.Where(x => x.JiankongName.Contains(payload.ccmmunity));
                //}
                //车牌号筛选
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.JiankongName.Contains(payload.Kw));
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
        /// 获取厢房下拉框
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GrabageNums()
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var query = _dbContext.GrabageRoom.Where(x => x.IsDelete == "0").ToList();
                response.SetData(query);
                return Ok(response);
            }
        }

        /// <summary>
        /// 添加厢房监控信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult AddGrabageMonit(dynamic model)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var entity = new HaikanRefuseClassification.Api.Entities.GrabageMonitoring();
                entity.GrabageMonitoringUuid = Guid.NewGuid();
                entity.MonitoringNum = model.monitoringNum;
                entity.GarbageRoomUuid = model.garbageRoomUuid;
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
                var entity2 = _dbContext.GrabageRoom.FirstOrDefault(x => x.GarbageRoomUuid == entity.GarbageRoomUuid);
                entity2.MonitoringNum = entity2.MonitoringNum+","+ entity.MonitoringNum;
                _dbContext.GrabageMonitoring.Add(entity);
                _dbContext.SaveChanges();
                response.SetSuccess("添加成功");
                return Ok(response);
            }
        }

        /// <summary>
        /// 获取厢房监控编辑信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        public IActionResult GetGrabageMonit(Guid guid)
        {
            using (_dbContext)
            {
                var entity = _dbContext.GrabageMonitoring.FirstOrDefault(x => x.GrabageMonitoringUuid == guid);
                var query = from m in _dbContext.GrabageMonitoring
                            join c in _dbContext.GrabageRoom
                            on m.GarbageRoomUuid equals c.GarbageRoomUuid into gr from g in gr.DefaultIfEmpty()
                            where m.GarbageRoomUuid == entity.GarbageRoomUuid
                            select new
                            {
                                m.MonitoringNum,
                                m.GarbageRoomUuid,
                                Ljname=g.Ljname,
                                m.AddTime,
                                m.Appkey,
                                m.AppSecret,
                                m.CamList,
                                m.GrabageMonitoringUuid,
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
        /// 编辑厢房监控信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult EditGrabageMonit(dynamic model)
        {

            string guid = model.grabageMonitoringUuid;
            string MonitoringNum = model.monitoringNum;
            string GarbageRoomUuid = model.garbageRoomUuid;
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
                var entity = _dbContext.GrabageMonitoring.FirstOrDefault(x => x.GrabageMonitoringUuid == Guid.Parse(guid));
                entity.MonitoringNum = model.monitoringNum;
                entity.GarbageRoomUuid = model.garbageRoomUuid;
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
                var entity2 = _dbContext.GrabageRoom.FirstOrDefault(x => x.GarbageRoomUuid == entity.GarbageRoomUuid);
                if (entity2!=null)
                {
                    entity2.MonitoringNum = entity.MonitoringNum;
                }
                //else
                //{
                //    var entity3 = _dbContext.GrabageRoom.FirstOrDefault(x => x.GarbageRoomUuid == GraRoomUuids);
                //    entity3.MonitoringNum = null;
                //}
                _dbContext.SaveChanges();
                response.SetSuccess("修改成功");
                return Ok(response);
            }
        }

        /// <summary>
        /// 删除厢房监控
        /// </summary>
        /// <param name="isDeleted"></param>
        /// <param name="ids">厢房ID字符串,多个以逗号隔开</param>
        /// <returns></returns>
        private ResponseModel UpdateIsDelete(CommonEnum.IsDeleted isDeleted, string ids)
        {
            using (_dbContext)
            {
                var parameters = ids.Split(",").Select((id, index) => new SqlParameter(string.Format("@p{0}", index), id)).ToList();
                foreach (var item in parameters)
                {
                    var query1 = _dbContext.GrabageMonitoring.Where(x => x.GrabageMonitoringUuid == Guid.Parse(item.Value.ToString())).ToList();
                    foreach (var item1 in query1)
                    {
                        var query = _dbContext.GrabageRoom.FirstOrDefault(x => x.GarbageRoomUuid == item1.GarbageRoomUuid);
                        query.MonitoringNum = null;
                        _dbContext.SaveChanges();
                    }
                }
                var parameterNames = string.Join(", ", parameters.Select(p => p.ParameterName));
                var sql = string.Format("UPDATE GrabageMonitoring SET IsDelete=@IsDeleted WHERE GrabageMonitoringUuid IN ({0})", parameterNames);
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

        /// <summary>
        /// 导入
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult shopInfoImport(IFormFile excelfile)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                DateTime beginTime = DateTime.Now;

                string sWebRootFolder = _hostingEnvironment.WebRootPath + "\\UploadFiles\\ImportUserInfoExcel";


                //var schoolinfo = _dbContext.SchoolInforManagement.AsQueryable();
                string uploadtitle = "箱房监控信息导入" + DateTime.Now.ToString("yyyyMMddHHmmss");
                string sFileName = $"{uploadtitle}.xlsx";
                FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
                //string conStr = ConnectionStrings.DefaultConnection;
                string responsemsgsuccess = "";
                string responsemsgrepeat = "";
                string responsemsgdefault = "";
                int successcount = 0;
                int repeatcount = 0;
                int defaultcount = 0;
                string today = DateTime.Now.ToString("yyyy-MM-dd");
                try
                {
                    //把excelfile中的数据复制到file中
                    using (FileStream fs = new FileStream(file.ToString(), FileMode.Create)) //初始化一个指定路径和创建模式的FileStream
                    {
                        excelfile.CopyTo(fs);
                        fs.Flush();  //清空stream的缓存，并且把缓存中的数据输出到file
                    }
                    System.Data.DataTable dt = Haikan3.Utils.ExcelTools.ExcelToDataTable(file.ToString(), "Sheet1", true);

                    if (dt == null || dt.Rows.Count == 0)
                    {
                        response.SetFailed("表格无数据");
                        return Ok(response);
                    }
                    else
                    {
                        if (!dt.Columns.Contains("region_id"))
                        {
                            response.SetFailed("无‘region_id’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("index_code"))
                        {
                            response.SetFailed("无‘index_code’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("name"))
                        {
                            response.SetFailed("无‘name’列");
                            return Ok(response);
                        }
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {

                            var entity = new GrabageMonitoring();
                            entity.GrabageMonitoringUuid = Guid.NewGuid();
                            if (!string.IsNullOrEmpty(dt.Rows[i]["region_id"].ToString()))
                            {
                                string cc = "";
                                cc = dt.Rows[i]["region_id"].ToString();
                                entity.RegionId = Convert.ToInt32(cc);
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["index_code"].ToString()))
                            {
                                var a = dt.Rows[i]["index_code"].ToString();
                                var user = _dbContext.GrabageMonitoring.FirstOrDefault(x => x.MonitoringNum == dt.Rows[i]["index_code"].ToString());

                                if (user == null)
                                {
                                    entity.MonitoringNum = dt.Rows[i]["index_code"].ToString();
                                }
                                else
                                {
                                    responsemsgrepeat += "<p style='color:orange'>" + "第" + (i + 2) + "行index_code已存在" + "</p></br>";
                                    repeatcount++;
                                    continue;
                                }

                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行index_code为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }

                            if (!string.IsNullOrEmpty(dt.Rows[i]["name"].ToString()))
                            {
                                entity.JiankongName = dt.Rows[i]["name"].ToString();
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["longitude"].ToString()))
                            {
                                entity.Longitude = dt.Rows[i]["longitude"].ToString();
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["latitude"].ToString()))
                            {
                                entity.Latitude = dt.Rows[i]["latitude"].ToString();
                            }
 
                            entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd");
                            _dbContext.GrabageMonitoring.Add(entity);
                            _dbContext.SaveChanges();
                            successcount++;
                        }
                    }
                    responsemsgsuccess = "<p style='color:green'>导入成功:" + successcount + "条</p></br>" + responsemsgsuccess;
                    responsemsgrepeat = "<p style='color:orange'>重复需手动修改数据:" + repeatcount + "条</p></br>" + responsemsgrepeat;
                    responsemsgdefault = "<p style='color:red'>导入失败:" + defaultcount + "条</p></br>" + responsemsgdefault;


                    DateTime endTime = DateTime.Now;
                    TimeSpan useTime = endTime - beginTime;
                    string taketime = "导入时间" + useTime.TotalSeconds.ToString() + "秒  ";
                    response.SetData(JsonConvert.DeserializeObject(JsonConvert.SerializeObject(new
                    {
                        time = taketime,
                        successmsg = responsemsgsuccess
                        ,
                        repeatmsg = responsemsgrepeat,
                        defaultmsg = responsemsgdefault
                    })));
                    return Ok(response);
                }
                catch (Exception ex)
                {

                    response.SetFailed(ex.Message);
                    return Ok(response);
                }
            }
        }

    }
}