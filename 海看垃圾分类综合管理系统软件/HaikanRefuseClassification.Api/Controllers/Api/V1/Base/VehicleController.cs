using AutoMapper;
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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace HaikanRefuseClassification.Api.Controllers.Api.V1.Base
{
    [Route("api/v1/Base/[controller]/[action]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly RefuseClassificationContext _dbContext;
        private readonly IMapper _mapper;
        //用来获取路径相关
        private IHostingEnvironment _hostingEnvironment;

        public VehicleController(RefuseClassificationContext dbContext, IMapper mapper, IHostingEnvironment hostingEnvironment)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// 车辆管理(模糊查询)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult List(VehicleRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = from cd in _dbContext.Car
                            where cd.IsDelete == 0
                            select new
                            {
                                cd.CarUuid,
                                cd.CarNum,
                                cd.BalanceId,
                                cd.CameraId,
                                cd.IsDelete,
                                cd.Id,
                                cd.Company,
                                cd.Street,
                                Ljname= GraRoom(cd.CarUuid,1),
                                WingID = GraRoom(cd.CarUuid,0),
                                GrabType = cd.GrabType == "0" ? "其他垃圾" : cd.GrabType == "1" ? "易腐垃圾" : cd.GrabType == "2" ? "有害垃圾" : cd.GrabType == "3" ? "可回收垃圾" : "",
                                //State = g.State == "1" ? "使用中" : g.State == "0" ? "暂停使用" : "",
                                //TypeName = GetGarbage(cd.CarUuid),
                                //Ljname = GetLj(cd.CarUuid),
                            };
                //车牌号筛选
                if (!string.IsNullOrEmpty(payload.kw))
                {
                    query = query.Where(x => x.CarNum.ToString().Contains(payload.kw));
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

        private static string GraRoom(Guid guid,int type)
        {
            using (RefuseClassificationContext cc = new RefuseClassificationContext())
            {
                string s = "";
                var groom = "";
                var query = (from a in cc.GrabageWeightSon
                    where a.GrabageRoomId != null &&
                          a.CarUuid== guid
                             group a by a.GrabageRoomId
                    into b
                    select new
                    {
                        b.Key
                    }).ToList();
                for (int i = 0; i < query.Count; i++)
                {
                    if (type == 1)
                    {
                         groom = cc.GrabageRoom.FirstOrDefault(x => x.GarbageRoomUuid == query[i].Key && x.IsDelete != "1").Ljname;
                    }
                    else
                    {
                        groom = cc.GrabageRoom.FirstOrDefault(x => x.GarbageRoomUuid == query[i].Key && x.IsDelete != "1").WingId;
                    }
                    
                    if (s=="")
                    {
                        s = groom;
                    }
                    else
                    {
                        s +="," + groom ;
                    }
                }
                return s;
            }
        }

        //收运垃圾类型
        private string GetGarbage(Guid ljid)
        {
            using (RefuseClassificationContext cc = new RefuseClassificationContext())
            {
                var query = (from a in cc.RubbishSyrecord
                             where a.CarUuid == ljid && a.IsDelete == "0"
                             select a).ToList();
                string s = "";
                foreach (var item in query)
                {
                    if (!string.IsNullOrEmpty(item.RubbishType))
                    {
                        s += item.RubbishType;
                    }
                }
                return s;
            }

        }
        //垃圾房
        private string GetLj(Guid ljid)
        {
            using (RefuseClassificationContext cc = new RefuseClassificationContext())
            {
                var query = (from a in cc.GrabageRoom
                             where a.CarId == ljid && a.IsDelete != "1"
                             select a).ToList();
                string s = "";
                foreach (var item in query)
                {
                    if (!string.IsNullOrEmpty(item.Ljname))
                    {
                        s += item.Ljname + "、";
                    }
                }
                return s.TrimEnd('、');
            }

        }
        //获取收运车辆下拉框
        [HttpPost]
        public IActionResult Huoquche()
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var query = _dbContext.Car.Where(x => x.IsDelete == 0).ToList();
                response.SetData(query);
                return Ok(response);
            }
        }
        /// <summary>
        /// 添加车辆信息管理(保存)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Create(VehicleViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;

            using (_dbContext)
            {
                var entity = new HaikanRefuseClassification.Api.Entities.Car();
                Guid cguid= Guid.NewGuid();
                entity.CarUuid = cguid;
                if (_dbContext.Car.Where(x => x.IsDelete == 0).Count(x => x.CarNum == model.CarNum) > 0)
                {
                    response.SetFailed("车牌号已存在");
                    return Ok(response);
                }
                entity.CarNum = model.CarNum;
                entity.CarType = model.CarType;
                entity.HolderId = model.HolderId;
                entity.HolderPhone = model.HolderPhone;
                entity.Brand = model.Brand;
                entity.Company = model.Company;
                entity.Street = model.Street;
                if (!string.IsNullOrEmpty(model.OnlyNum))
                {
                    var device = _dbContext.DeviceToCar.FirstOrDefault(x => x.Imei == model.OnlyNum);
                    if (device != null)
                    {
                        var iscar = _dbContext.Car.FirstOrDefault(x=>x.IsDelete!=1 && x.CarUuid== device.CarUuid);
                        if (iscar!=null)
                        {
                            response.SetFailed("该车载唯一编码已绑定");
                            return Ok(response);
                        }
                    }
                    entity.OnlyNum = model.OnlyNum;
                    var deviceTo = new DeviceToCar();
                    deviceTo.DeviceToCarUuid = Guid.NewGuid();
                    deviceTo.CarUuid = cguid;
                    deviceTo.Imei= model.OnlyNum;
                    deviceTo.AddPerson= AuthContextService.CurrentUser.DisplayName;
                    deviceTo.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
                if (!string.IsNullOrEmpty(entity.RegisterTime))
                {
                    entity.RegisterTime = DateTime.Parse(model.RegisterTime).ToString("yyyy-MM-dd");
                }
                else
                {
                    entity.RegisterTime = model.RegisterTime;
                }
                if (!string.IsNullOrEmpty(entity.AwardTime))
                {
                    entity.AwardTime = DateTime.Parse(model.AwardTime).ToString("yyyy-MM-dd");
                }
                else
                {
                    entity.AwardTime = model.RegisterTime;
                }
                entity.GrabType = model.GrabType;
                entity.Weight = model.Weight;
                entity.BalanceId = model.BalanceId;
                entity.CameraId = model.CameraId;
                // entity.RubbishTypeUuid = model.RubbishTypeUuid;
                entity.IsDelete = 0;
                _dbContext.Car.Add(entity);
                _dbContext.SaveChanges();
                response.SetSuccess("添加成功");
                return Ok(response);
            }
        }

        /// <summary>
        /// 编辑车辆信息管理(保存)
        /// </summary>
        /// <param name="guid">申请惟一编码</param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        public IActionResult Edit(Guid guid)
        {
            using (_dbContext)
            {
                var entity = _dbContext.Car.FirstOrDefault(x => x.CarUuid == guid);
                var query = _mapper.Map<HaikanRefuseClassification.Api.Entities.Car, VehicleEditView>(entity);
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(query);
                return Ok(response);
            }
        }
        /// <summary>
        /// 保存编辑后的车辆信息管理(保存)
        /// </summary>
        /// <param name="model">申请视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Edit(VehicleViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            using (_dbContext)
            {
                var entity = _dbContext.Car.FirstOrDefault(x => x.CarUuid == model.CarUuid);
                //entity.CarUuid = model.CarUuid;
                entity.CarNum = model.CarNum;
                entity.CarType = model.CarType;
                entity.HolderId = model.HolderId;
                entity.HolderPhone = model.HolderPhone;
                entity.Brand = model.Brand;
                entity.RegisterTime = model.RegisterTime;
                entity.AwardTime = model.AwardTime;
                entity.GrabType = model.GrabType;
                entity.Weight = model.Weight;
                entity.BalanceId = model.BalanceId;
                entity.Company = model.Company;
                entity.CameraId = model.CameraId;
                entity.Street = model.Street;
                if (!string.IsNullOrEmpty(model.OnlyNum))
                {
                    var device = _dbContext.DeviceToCar.FirstOrDefault(x => x.Imei == model.OnlyNum);
                    if (device != null)
                    {
                        var iscar = _dbContext.Car.FirstOrDefault(x => x.IsDelete != 1 && x.CarUuid == device.CarUuid && x.CarUuid!= entity.CarUuid);
                        if (iscar != null)
                        {
                            response.SetFailed("该车载唯一编码已绑定");
                            return Ok(response);
                        }
                    }
                    entity.OnlyNum = model.OnlyNum;
                    var deviceTo = new DeviceToCar();
                    deviceTo.DeviceToCarUuid = Guid.NewGuid();
                    deviceTo.CarUuid = entity.CarUuid;
                    deviceTo.Imei = model.OnlyNum;
                    deviceTo.AddPerson = AuthContextService.CurrentUser.DisplayName;
                    deviceTo.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    _dbContext.DeviceToCar.Add(deviceTo);
                }
                //entity.RubbishTypeUuid = model.RubbishTypeUuid;
                _dbContext.SaveChanges();
                return Ok(response);
            }
        }
        /// <summary>
        /// 车辆信息管理详情
        /// </summary>
        /// <param name="guid">申请惟一编码</param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        public IActionResult Detail(Guid guid)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var query = from cd in _dbContext.Car
                            where cd.CarUuid == guid
                            select new
                            {
                                cd.HolderId,
                                cd.HolderPhone,
                                cd.AddPeople,
                                cd.AddTime,
                                cd.AwardTime,
                                cd.BalanceId,
                                cd.Brand,
                                cd.CameraId,
                                cd.CarType,
                                cd.CarUuid,
                                cd.CarNum,
                                cd.IsDelete,
                                cd.Id,
                                cd.Company,
                                cd.Street,
                                cd.OnlyNum,
                                cd.Weight,
                                cd.RegisterTime,
                                GrabType = cd.GrabType == "0" ? "其他垃圾" : cd.GrabType == "1" ? "易腐垃圾" : cd.GrabType == "2" ? "有害垃圾" : cd.GrabType == "3" ? "可回收垃圾" : "",
                                //State = g.State == "1" ? "使用中" : g.State == "0" ? "暂停使用" : "",
                                //TypeName = GetGarbage(cd.CarUuid),
                                //Ljname = GetLj(cd.CarUuid),
                            };

                var query1 = query.ToList();
                response.SetData(query1);
                return Ok(response);
                //var entity = _dbContext.Car.FirstOrDefault(x => x.CarUuid == guid);
                //var query = _mapper.Map<HaikanRefuseClassification.Api.Entities.Car, VehicleShowViewModel>(entity);
                //var response = ResponseModelFactory.CreateInstance;
                //response.SetData(query);
                //return Ok(response);
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
                var sql = string.Format("UPDATE Car SET IsDelete=@IsDeleted WHERE CarUuid IN ({0})", parameterNames);
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
        /// 导入车辆信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UserInfoImport(IFormFile excelfile)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                DateTime beginTime = DateTime.Now;

                string sWebRootFolder = _hostingEnvironment.WebRootPath + "\\UploadFiles\\ImportUserInfoExcel";


                //var schoolinfo = _dbContext.SchoolInforManagement.AsQueryable();
                string uploadtitle = "车辆信息导入" + DateTime.Now.ToString("yyyyMMddHHmmss");
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
                        if (!dt.Columns.Contains("车牌号"))
                        {
                            response.SetFailed("无‘车牌号’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("车辆类型"))
                        {
                            response.SetFailed("无‘车辆类型’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("持有人"))
                        {
                            response.SetFailed("无‘持有人’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("持有人电话"))
                        {
                            response.SetFailed("无‘持有人电话’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("所属公司"))
                        {
                            response.SetFailed("无‘所属公司’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("品牌型号"))
                        {
                            response.SetFailed("无‘品牌型号’列");
                            return Ok(response);
                        }
                        //if (!dt.Columns.Contains("注册日期"))
                        //{
                        //    response.SetFailed("无‘注册日期’列");
                        //    return Ok(response);
                        //}
                        //if (!dt.Columns.Contains("发证日期"))
                        //{
                        //    response.SetFailed("无‘发证日期’列");
                        //    return Ok(response);
                        //}
                        //if (!dt.Columns.Contains("车辆重量"))
                        //{
                        //    response.SetFailed("无‘车辆重量’列");
                        //    return Ok(response);
                        //}
                        if (!dt.Columns.Contains("称重设备ID"))
                        {
                            response.SetFailed("无‘称重设备ID’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("车载录像机编号"))
                        {
                            response.SetFailed("无‘车载录像机编号’列");
                            return Ok(response);
                        }

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {

                            var entity = new Car();
                            entity.CarUuid = Guid.NewGuid();
                            if (!string.IsNullOrEmpty(dt.Rows[i]["车牌号"].ToString()))
                            {
                                var a = dt.Rows[i]["车牌号"].ToString();
                                var user = _dbContext.Car.Where(x=>x.IsDelete==0).FirstOrDefault(x => x.CarNum == dt.Rows[i]["车牌号"].ToString());

                                if (user == null)
                                {
                                    entity.CarNum = dt.Rows[i]["车牌号"].ToString();
                                }
                                else
                                {
                                    responsemsgrepeat += "<p style='color:orange'>" + "第" + (i + 2) + "行车牌号已存在" + "</p></br>";
                                    repeatcount++;
                                    continue;
                                }

                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行车牌号为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["车辆类型"].ToString()))
                            {

                                entity.CarType = dt.Rows[i]["车辆类型"].ToString();
                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行车辆类型为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["持有人"].ToString()))
                            {

                                entity.HolderId = dt.Rows[i]["持有人"].ToString();
                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行持有人为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["持有人电话"].ToString()))
                            {

                                entity.HolderPhone = dt.Rows[i]["持有人电话"].ToString();
                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行持有人电话为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["品牌型号"].ToString()))
                            {

                                entity.Brand = dt.Rows[i]["品牌型号"].ToString();
                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行品牌型号为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["注册日期"].ToString()))
                            {
                                entity.RegisterTime = dt.Rows[i]["注册日期"].ToString();
                            }
                            //else
                            //{
                            //    responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行注册日期为空" + "</p></br>";
                            //    defaultcount++;
                            //    continue;
                            //}
                            if (!string.IsNullOrEmpty(dt.Rows[i]["所属街道"].ToString()))
                            {
                                entity.Street = dt.Rows[i]["所属街道"].ToString();
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["发证日期"].ToString()))
                            {
                                entity.AwardTime = dt.Rows[i]["发证日期"].ToString();
                            }
                            //else
                            //{
                            //    responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行发证日期为空" + "</p></br>";
                            //    defaultcount++;
                            //    continue;
                            //}
                            if (!string.IsNullOrEmpty(dt.Rows[i]["收运垃圾类型"].ToString()))
                            {
                                entity.GrabType = dt.Rows[i]["收运垃圾类型"].ToString();
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["车辆重量"].ToString()))
                            {
                                entity.Weight = dt.Rows[i]["车辆重量"].ToString();
                            }

                            if (!string.IsNullOrEmpty(dt.Rows[i]["称重设备ID"].ToString()))
                            {

                                entity.BalanceId = dt.Rows[i]["称重设备ID"].ToString();
                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行称重设备ID为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["车载录像机编号"].ToString()))
                            {

                                entity.CameraId = dt.Rows[i]["车载录像机编号"].ToString();
                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行车载录像机编号为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            entity.Company= dt.Rows[i]["所属公司"].ToString();
                            entity.IsDelete = 0;
                            _dbContext.Car.Add(entity);
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
