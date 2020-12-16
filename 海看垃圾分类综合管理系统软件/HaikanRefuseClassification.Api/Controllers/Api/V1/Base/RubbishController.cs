using AutoMapper;
using Haikan3.Utils;
using HaikanRefuseClassification.Api.Configurations;
using HaikanRefuseClassification.Api.Entities;
using HaikanRefuseClassification.Api.Entities.Enums;
using HaikanRefuseClassification.Api.Extensions;
using HaikanRefuseClassification.Api.Extensions.AuthContext;
using HaikanRefuseClassification.Api.Models.Response;
using HaikanRefuseClassification.Api.RequestPayload.Base;
using HaikanRefuseClassification.Api.ViewModels.Base;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NPOI.XWPF.UserModel;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using Microsoft.Data.SqlClient;
using Document = Spire.Doc.Document;
using Microsoft.Extensions.Logging;
using HaikanRefuseClassification.Api.Extensions.CustomException;

namespace HaikanRefuseClassification.Api.Controllers.Api.V1.Base
{
    [Route("api/v1/Base/[controller]/[action]")]
    [ApiController]
    public class RubbishController : ControllerBase
    {
        private readonly RefuseClassificationContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        //用来获取路径相关
        private IHostingEnvironment _hostingEnvironment;
        public RubbishController(RefuseClassificationContext dbContext, IMapper mapper, IOptions<MdDesEncrypt> mdDesEncrypt, IHostingEnvironment hostingEnvironment, ILogger<TestController> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }
        /// <summary>
        /// 垃圾箱房管理(模糊查询)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize]
        public IActionResult List(RubbishRequestPayload payload)
        {

            using (_dbContext)
            {
                var query = from g in _dbContext.GrabageRoom      //垃圾
                            join v in _dbContext.Village          //小区
                            on g.VillageId equals v.VillageUuid
                            //join u in _dbContext.SystemUser      
                            //on g.SystemUserUuid equals u.SystemUserUuid
                            where g.IsDelete == "0"
                            select new
                            {
                                g.GarbageRoomUuid,  //垃圾编号
                                g.Ljname,//垃圾箱
                                State = g.State == "0" ? "使用中" : g.State == "1" ? "暂停使用" : "",
                                Stateid = g.State,
                                g.StartTime,
                                g.EndTime,
                                g.Lon,
                                g.Lat,
                                v.Towns,
                                WingID=g.WingId,
                                RealName= GraRoom(g.GarbageRoomUuid,1),
                                CarNum = GraRoom(g.GarbageRoomUuid, 0),
                                //u.SystemUserUuid,     //督导员
                                //u.RealName,             //督导员姓名
                                v.VillageUuid,        //社区
                                v.Vname,           //社区名
                                //v.Address,
                                g.Id,
                                g.Facilityuuid    //设备id
                                //c.CarUuid,            //车辆编号
                                //c.CarNum            //车牌号
                            };
                _logger.LogError(message: "12111111111");
                if (AuthContextService.CurrentUser.Streets != null && AuthContextService.CurrentUser.Streets == "" && AuthContextService.CurrentUser.Community != "")
                {
                    var Community = AuthContextService.CurrentUser.Community.Split(',');
                    query = query.Where(x => Community.Contains(x.Vname));
                }
                else if (AuthContextService.CurrentUser.Streets != null && AuthContextService.CurrentUser.Streets != "" && AuthContextService.CurrentUser.Community == "")
                {
                    var Streets = AuthContextService.CurrentUser.Streets.Split(',');
                    query = query.Where(x => Streets.Contains(x.Towns));
                }
                else if (AuthContextService.CurrentUser.Streets != null && AuthContextService.CurrentUser.Streets != "" && AuthContextService.CurrentUser.Community != "")
                {
                    var Streets = AuthContextService.CurrentUser.Streets.Split(',');
                    var Community = AuthContextService.CurrentUser.Community.Split(',');
                    query = query.Where(x => Streets.Contains(x.Towns) || Community.Contains(x.Vname));
                }
                //街道筛选
                if (!string.IsNullOrEmpty(payload.street))
                {
                    query = query.Where(x => x.Towns == payload.street);
                }
                //所在社区筛选
                if (!string.IsNullOrEmpty(payload.ccmmunity))
                {
                    query = query.Where(x => x.Vname == payload.ccmmunity);
                }
                //箱房编号筛选
                if (!string.IsNullOrEmpty(payload.kw))
                {
                    query = query.Where(x => x.Ljname.ToString().Contains(payload.kw));
                }
                //状态筛选
                if (!string.IsNullOrEmpty(payload.kw2))
                {
                    if (payload.kw2 != "")
                        query = query.Where(x => x.Stateid.ToString() == payload.kw2);
                }
                if (payload.FirstSort != null)
                {
                    query = query.OrderByDescending(x => x.Id);
                }
                //分页
                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(list, totalCount);
                return Ok(response);
            }
        }


        private static string GraRoom(Guid guid, int type)
        {
            using (RefuseClassificationContext cc = new RefuseClassificationContext())
            {
                //_logger.LogError(message: "66666666666");
                string s = "";
                var groom = "";

                if (type == 1)
                {
                    var ddquery = (from a in cc.SystemUser
                        where a.GrabageRoomId == guid
                        select new
                        {
                            a.SystemUserUuid
                        }).ToList();
                    for (int i = 0; i < ddquery.Count; i++)
                    {
                        groom = cc.SystemUser.FirstOrDefault(x => x.SystemUserUuid == ddquery[i].SystemUserUuid && x.IsDeleted != 1)?.RealName;
                    }
                        
                }
                else
                {
                    var query = (from a in cc.GrabageWeightSon
                        where a.GrabageRoomId == guid
                        group a by a.CarUuid
                        into b
                        select new
                        {
                            b.Key
                        }).ToList();
                    for (int i = 0; i < query.Count; i++)
                    {
                        groom = cc.Car.FirstOrDefault(x => x.CarUuid == query[i].Key && x.IsDelete != 1)?.CarNum;
                    }
                }

                if (s == "")
                {
                    s = groom;
                }
                else
                {
                    s += "," + groom;
                }

                //_logger.LogError(message: "888888888");
                return s;
            }
        }

        /// <summary>
        /// 添加垃圾箱房管理(保存)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize]
        [ProducesResponseType(200)]
        public IActionResult Create(RubbishViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;

            using (_dbContext)
            {
                var entity = new HaikanRefuseClassification.Api.Entities.GrabageRoom();
                entity.GarbageRoomUuid = Guid.NewGuid();
                if (_dbContext.GrabageRoom.Where(x => x.IsDelete == "0").Count(x => x.Ljname == model.Ljname) > 0)
                {
                    response.SetFailed("垃圾箱房名字已存在");
                    return Ok(response);
                }
                //VillageId = _dbContext.Village.FirstOrDefault(x => x.Vname == model.Vname);
                entity.VillageId = model.villageUuid;//社区id
                entity.Ljname = model.Ljname;
                entity.SystemUserUuid = model.SystemUserUuid;//督导员id
                entity.StartTime = model.StartTime;
                entity.EndTime = model.EndTime;
                entity.Lon = model.Lon;
                entity.Lat = model.Lat;
                entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd");
                entity.AddPeople = AuthContextService.CurrentUser.DisplayName;
                entity.CarId = model.CarId;
                entity.IsDelete = "0";
                entity.State = "0";
                entity.WingId = model.WingId;
                entity.Facilityuuid = model.Facilityuuid;
                _dbContext.GrabageRoom.Add(entity);
                _dbContext.SaveChanges();
                response.SetSuccess("添加成功");
                return Ok(response);
            }
        }
        /// <summary>
        /// 编辑垃圾箱房管理(保存)
        /// </summary>
        /// <param name="guid">申请惟一编码</param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [CustomAuthorize]
        [ProducesResponseType(200)]
        public IActionResult Edit(Guid guid)
        {
            using (_dbContext)
            {
                var entity = _dbContext.GrabageRoom.FirstOrDefault(x => x.GarbageRoomUuid == guid);
                var query = _mapper.Map<HaikanRefuseClassification.Api.Entities.GrabageRoom, RubbishEditView>(entity);
                //所在社区
                query.Vname = _dbContext.Village.FirstOrDefault(x => x.VillageUuid == entity.VillageId).Vname;
                //query.Address = _dbContext.Village.FirstOrDefault(x => x.VillageUuid == entity.VillageId).Address;
                query.Towns = _dbContext.Village.FirstOrDefault(x => x.VillageUuid == entity.VillageId).Towns;
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(query);
                return Ok(response);
            }
        }


        //根据乡镇(街道)获取社区
        [HttpPost]
        public IActionResult Huoqushequ()
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var query = from v in _dbContext.Village
                            select new
                            {
                                v.Vname,
                                v.Towns,
                                v.VillageUuid,
                                v.Address
                            };
                var entiy = query.ToList();
                response.SetData(entiy);
                return Ok(response);
            }
        }



        //获取垃圾箱下拉框
        [HttpPost]
        public IActionResult Huoqu()
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var query = from g in _dbContext.GrabageRoom
                            join v in _dbContext.Village
                            on g.VillageId equals v.VillageUuid
                            where g.IsDelete == "0"
                            select new
                            {
                                v.Vname,
                                v.Towns,
                                g.GarbageRoomUuid,
                                g.Ljname
                            };
                if (AuthContextService.CurrentUser.Streets != null && AuthContextService.CurrentUser.Streets == "" && AuthContextService.CurrentUser.Community != "")
                {
                    var Community = AuthContextService.CurrentUser.Community.Split(',');
                    query = query.Where(x => Community.Contains(x.Vname));
                }
                else if (AuthContextService.CurrentUser.Streets != null && AuthContextService.CurrentUser.Streets != "" && AuthContextService.CurrentUser.Community == "")
                {
                    var Streets = AuthContextService.CurrentUser.Streets.Split(',');
                    query = query.Where(x => Streets.Contains(x.Towns));
                }
                else if (AuthContextService.CurrentUser.Streets != null && AuthContextService.CurrentUser.Streets != "" && AuthContextService.CurrentUser.Community != "")
                {
                    var Streets = AuthContextService.CurrentUser.Streets.Split(',');
                    var Community = AuthContextService.CurrentUser.Community.Split(',');
                    query = query.Where(x => Streets.Contains(x.Towns) || Community.Contains(x.Vname));
                }
                response.SetData(query.ToList());
                return Ok(response);
            }
        }

        //获取垃圾箱下拉框
        [HttpGet]
        public IActionResult Huoqu1(string guid)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var query = _dbContext.GrabageRoom.Where(x => x.IsDelete == "0" && x.VillageId == Guid.Parse(guid)).ToList();
                response.SetData(query);
                return Ok(response);
            }
        }
        /// <summary>
        /// 保存编辑后的垃圾箱房管理(保存)
        /// </summary>
        /// <param name="model">申请视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize]
        [ProducesResponseType(200)]
        public IActionResult Edit(RubbishViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            using (_dbContext)
            {
                var entity = _dbContext.GrabageRoom.FirstOrDefault(x => x.GarbageRoomUuid == model.GarbageRoomUuid);
                entity.GarbageRoomUuid = model.GarbageRoomUuid;
                entity.VillageId = model.villageUuid;
                entity.Ljname = model.Ljname;
                entity.SystemUserUuid = model.SystemUserUuid;
                entity.StartTime = model.StartTime;
                entity.EndTime = model.EndTime;
                entity.Lon = model.Lon;
                entity.Lat = model.Lat;
                entity.State = model.State;
                entity.AddTime = model.AddTime;
                entity.CarId = model.CarId;
                entity.WingId = model.WingId;
                entity.Facilityuuid = model.Facilityuuid;
                _dbContext.SaveChanges();
                return Ok(response);
            }
        }
        /// <summary>
        /// 垃圾箱房管理详情
        /// </summary>
        /// <param name="guid">申请惟一编码</param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        public IActionResult Detail(Guid guid)
        {
            using (_dbContext)
            {
                var entity = _dbContext.GrabageRoom.FirstOrDefault(x => x.GarbageRoomUuid == guid);
                var query = _mapper.Map<HaikanRefuseClassification.Api.Entities.GrabageRoom, RubbishShowViewModel>(entity);
                //督导员
                //query.RealName = _dbContext.SystemUser.FirstOrDefault(x => x.SystemUserUuid == entity.SystemUserUuid).RealName;
                //所在社区
                query.Vname = _dbContext.Village.FirstOrDefault(x => x.VillageUuid == entity.VillageId).Vname;
                //query.Address = _dbContext.Village.FirstOrDefault(x => x.VillageUuid == entity.VillageId).Address;
                query.Towns = _dbContext.Village.FirstOrDefault(x => x.VillageUuid == entity.VillageId).Towns;
                //收运车辆
                //query.CarNum = _dbContext.Car.FirstOrDefault(x => x.CarUuid == entity.CarId).CarNum;
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(query);
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
                var sql = string.Format("UPDATE GrabageRoom SET IsDelete=@IsDeleted WHERE GarbageRoomUuid IN ({0})", parameterNames);
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
        /// <summary>
        /// 导入
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult RubbishInfoImport(IFormFile excelfile)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                DateTime beginTime = DateTime.Now;

                string sWebRootFolder = _hostingEnvironment.WebRootPath + "\\UploadFiles\\ImportUserInfoExcel";


                //var schoolinfo = _dbContext.SchoolInforManagement.AsQueryable();
                string uploadtitle = "垃圾箱房信息导入" + DateTime.Now.ToString("yyyyMMddHHmmss");
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
                    DataTable dt = Haikan3.Utils.ExcelTools.ExcelToDataTable(file.ToString(), "Sheet1", true);

                    if (dt == null || dt.Rows.Count == 0)
                    {
                        response.SetFailed("表格无数据");
                        return Ok(response);
                    }
                    else
                    {
                        if (!dt.Columns.Contains("垃圾箱房名字"))
                        {
                            response.SetFailed("无‘垃圾箱房名字’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("所在乡镇(街道)"))
                        {
                            response.SetFailed("无‘所在乡镇(街道)’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("所在社区"))
                        {
                            response.SetFailed("无‘所在社区’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("当前状态"))
                        {
                            response.SetFailed("无‘当前状态’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("经度"))
                        {
                            response.SetFailed("无‘经度’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("纬度"))
                        {
                            response.SetFailed("无‘纬度’列");
                            return Ok(response);
                        }

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {

                            var entity = new GrabageRoom();
                            entity.GarbageRoomUuid = Guid.NewGuid();
                            if (!string.IsNullOrEmpty(dt.Rows[i]["垃圾箱房名字"].ToString()))
                            {
                                var a = dt.Rows[i]["垃圾箱房名字"].ToString();
                                var user = _dbContext.GrabageRoom.Where(x => x.IsDelete == "0").FirstOrDefault(x => x.Ljname == dt.Rows[i]["垃圾箱房名字"].ToString());

                                if (user == null)
                                {
                                    entity.Ljname = dt.Rows[i]["垃圾箱房名字"].ToString();
                                }
                                else
                                {
                                    responsemsgrepeat += "<p style='color:orange'>" + "第" + (i + 2) + "行垃圾箱房名字已存在" + "</p></br>";
                                    repeatcount++;
                                    continue;
                                }

                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行垃圾箱房名字为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["所在乡镇(街道)"].ToString()))
                            {
                                var depart = _dbContext.Village.FirstOrDefault(x => x.Towns == dt.Rows[i]["所在乡镇(街道)"].ToString());
                                if (depart == null)
                                {
                                    responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行所在乡镇(街道)不存在" + "</p>";
                                    continue;
                                }
                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行所在乡镇(街道)为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["所在社区"].ToString()) || dt.Rows[i]["所在乡镇(街道)"].ToString().Contains("镇"))
                            {
                                var depart = _dbContext.Village.FirstOrDefault(x => x.Vname == dt.Rows[i]["所在社区"].ToString());
                                if (depart != null)
                                {
                                    entity.VillageId = depart.VillageUuid;
                                }
                                else if (dt.Rows[i]["所在乡镇(街道)"].ToString().Contains("镇"))
                                {
                                    var departs = _dbContext.Village.FirstOrDefault(x => x.Towns == dt.Rows[i]["所在乡镇(街道)"].ToString());
                                    if (departs != null)
                                    {
                                        entity.VillageId = departs.VillageUuid;
                                    }
                                }
                                else
                                {
                                    responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行所在社区不存在" + "</p>";
                                    continue;
                                }
                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行所在社区为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }

                            if (!string.IsNullOrEmpty(dt.Rows[i]["设备号"].ToString()))
                            {
                                entity.Facilityuuid = dt.Rows[i]["设备号"].ToString();

                            }
                            //else
                            //{
                            //    responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行设备号为空" + "</p></br>";
                            //    defaultcount++;
                            //    continue;
                            //}
                            if (!string.IsNullOrEmpty(dt.Rows[i]["当前状态"].ToString()))
                            {
                                var sex = dt.Rows[i]["当前状态"].ToString();
                                if (dt.Rows[i]["当前状态"].ToString() == "使用中")
                                {
                                    entity.State = "0";
                                }
                                else
                                {
                                    entity.State = "1";
                                }
                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行当前状态为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["经度"].ToString()))
                            {
                                if (double.Parse(dt.Rows[i]["经度"].ToString()) > 180.999999)
                                {
                                    response.SetFailed("经度范围在-180.999999~180.999999");
                                    return Ok(response);
                                }
                                else
                                {
                                    entity.Lon = dt.Rows[i]["经度"].ToString();
                                }
                            }

                            if (!string.IsNullOrEmpty(dt.Rows[i]["纬度"].ToString()))
                            {
                                if (double.Parse(dt.Rows[i]["纬度"].ToString()) > 180.999999)
                                {
                                    response.SetFailed("经度范围在-90.999999~90.999999");
                                    return Ok(response);
                                }
                                else
                                {
                                    entity.Lat = dt.Rows[i]["纬度"].ToString();
                                }

                            }

                            entity.IsDelete = "0";
                            entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd");
                            entity.AddPeople = AuthContextService.CurrentUser.DisplayName;//添加人
                            _dbContext.GrabageRoom.Add(entity);
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
        /// <summary>
        /// 获取全部垃圾厢房点
        /// </summary>
        /// <returns></returns>
        public IActionResult AllList()
        {

            using (_dbContext)
            {
                var query = (from g in _dbContext.GrabageRoom      //垃圾
                             where g.IsDelete == "0"
                             select new
                             {
                                 g.Id,
                                 callout =
                                 new
                                 {
                                     Content = g.Ljname,
                                     borderRadius = 2,
                                     display = "BYCLICK",
                                     textAlign = "center",
                                     bgColor = "white",
                                     color = "black",
                                     padding = 10,
                                 },
                                 longitude = getFloat(g.Lon),
                                 latitude = getFloat(g.Lat)
                             }).ToList();

                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(query);
                return Ok(response);
            }
        }
        private static float getFloat(string num)
        {
            float a = 0;
            if (!string.IsNullOrEmpty(num))
            {
                a = float.Parse(num);
            }
            return a;
        }

        //垃圾厢房二维码导出
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult Batchs(string command, string ids)
        {
            var response = ResponseModelFactory.CreateInstance;
            string sWebRootFolder = _hostingEnvironment.WebRootPath + "\\UploadFiles\\ImportUserInfoWord\\";
            var timeInfo = DateTime.Now.ToString("yyyyMMddHHmmss");
            var wordName = timeInfo + "address.docx";
            string wordUrl = sWebRootFolder + wordName;
            MemoryStream ms = new MemoryStream();
            XWPFDocument m_Docx = new XWPFDocument();
            m_Docx.Write(ms);
            ms.Flush();
            SaveToFile(ms, wordUrl);
            List<string> list = new List<string>();


            using (_dbContext)
            {
                //document111.LoadFromFile(sWebRootFolder + "test.docx");
                Document document111 = new Document();
                document111.LoadFromFile(wordUrl);
                Section section = document111.Sections[0];
                document111.Watermark = null;
                section.Paragraphs[0].AppendBookmarkStart("picture");
                section.Paragraphs[0].AppendBookmarkEnd("picture");
                var parameters = ids.Split(",").Select((id, index) => new SqlParameter(string.Format("@p{0}", index), id)).ToList();
                var parameterNames = string.Join(", ", parameters.Select(p => p.ParameterName));
                var entities = _dbContext.GrabageRoom.Where(x => ids.IndexOf(x.GarbageRoomUuid.ToString()) >= 0).ToList();
                for (int i = 0; i < entities.Count; i++)
                {
                    var pata = _hostingEnvironment.WebRootPath + EWM.GetEWM2("d_" + entities[i].GarbageRoomUuid.ToString(), _hostingEnvironment, entities[i].Ljname);
                    //实例化BookmarksNavigator类,指定需要添加图片的书签“”
                    BookmarksNavigator bn = new BookmarksNavigator(document111);
                    bn.MoveToBookmark("picture", true, true);
                    //添加段落，加载图片并插入到段落
                    Section section0 = document111.AddSection();
                    Spire.Doc.Documents.Paragraph paragraph = section0.AddParagraph();
                    Image image = Image.FromFile(pata);

                    //DocPicture picture = paragraph.AppendPicture(image);
                    DocPicture picture = document111.Sections[0].Paragraphs[0].AppendPicture(image);
                    picture.Width = 160;
                    picture.Height = 180;
                    //picture.HorizontalPosition = 100.0f;
                    //picture.VerticalPosition = 100.0f;
                    bn.InsertParagraph(paragraph);
                    document111.Sections.Remove(section0);
                    //string output = sWebRootFolder + "test.docx";
                    document111.SaveToFile(wordUrl, FileFormat.Docx);

                }
                list.Add(wordUrl);
                //关闭进程
                document111.Dispose();
                var time = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                var check = ZIP.CompressMulti(list, _hostingEnvironment.WebRootPath + "\\UploadFiles\\EWM\\" + time, false);
                if (check)
                {
                    response.SetSuccess("导出成功");
                    response.SetData("\\UploadFiles\\EWM\\" + time + ".zip");
                }
                else
                {
                    response.SetFailed("导出失败");
                }
                return Ok(response);
            }
        }

        public static void SaveToFile(MemoryStream ms, string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                byte[] data = ms.ToArray();

                fs.Write(data, 0, data.Length);
                fs.Flush();
                data = null;
            }
        }
    }
}
