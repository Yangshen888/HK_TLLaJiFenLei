using AutoMapper;
using HaikanRefuseClassification.Api.Configurations;
using HaikanRefuseClassification.Api.Entities;
using HaikanRefuseClassification.Api.Entities.Enums;
using HaikanRefuseClassification.Api.Extensions;
using HaikanRefuseClassification.Api.Extensions.AuthContext;
using HaikanRefuseClassification.Api.Models.Response;
using HaikanRefuseClassification.Api.RequestPayload.Person;
using HaikanRefuseClassification.Api.ViewModels.Person;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NPOI.XWPF.UserModel;
using Spire.Doc.Documents;
using System;
using System.Collections.Generic;
using System.Data;

using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Haikan3.Utils;
using Spire.Doc;
using Spire.Doc.Fields;
using System.Drawing;
using HaikanRefuseClassification.Api.Extensions.CustomException;
using Microsoft.Data.SqlClient;
using Section = Spire.Doc.Section;
using Document = Spire.Doc.Document;
using System.Text.RegularExpressions;

namespace HaikanRefuseClassification.Api.Controllers.Api.V1.Person
{
    [Route("api/v1/Person/[controller]/[action]")]
    [ApiController]
    public class SupervisorController : ControllerBase
    {
        private readonly RefuseClassificationContext _dbContext;
        private readonly IMapper _mapper;
        private readonly MdDesEncrypt MdDesEncrypt;
        //用来获取路径相关
        private IHostingEnvironment _hostingEnvironment;

        public SupervisorController(RefuseClassificationContext dbContext, IMapper mapper, IOptions<MdDesEncrypt> mdDesEncrypt, IHostingEnvironment hostingEnvironment)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            MdDesEncrypt = mdDesEncrypt.Value;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// 督导员管理(模糊查询)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize]
        public IActionResult List(SupervisorRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = from cd in _dbContext.SystemUser
                            join gr in _dbContext.GrabageRoom
                            on cd.GrabageRoomId equals gr.GarbageRoomUuid
                            where cd.SystemRoleUuid.Contains(AuthContextService.CurrentUser.DDY)
                            && cd.IsDeleted == 0
                            select new
                            {
                                cd.SystemUserUuid,//督导员编号
                                cd.LoginName,
                                cd.PassWord,
                                cd.RealName,//督导员姓名
                                Sex = cd.Sex == "1" ? "男" : cd.Sex == "0" ? "女" : "",
                                cd.Phone,
                                cd.Id,
                                gr.Ljname,
                                gr.GarbageRoomUuid,
                                cd.VillageId,
                                gr.Village.Towns,
                                gr.Village.Vname,
                                WingID = gr.WingId,
                                cd.InTime,//入职时间
                                ZaiGang = cd.ZaiGang == "1" ? "是" : cd.ZaiGang == "0" ? "否" : ""  //在岗
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
                //姓名筛选
                if (!string.IsNullOrEmpty(payload.kw))
                {
                    query = query.Where(x => x.RealName.Contains(payload.kw));
                }
                //社区筛选
                if (!string.IsNullOrEmpty(payload.kw1) && payload.kw1 != "全部")
                {
                    query = query.Where(x => x.Vname == payload.kw1);
                }
                //厢房编号筛选
                if (!string.IsNullOrEmpty(payload.kw2))
                {
                    query = query.Where(x => x.Ljname.Contains(payload.kw2));
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
        /// <summary>
        /// 添加督导员管理(保存)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Create(SupervisorViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;

            if (model.garbageRoomUuid.ToString() == "")
            {
                response.SetFailed("请选择垃圾厢房");
                return Ok(response);
            }
            using (_dbContext)
            {
                //先判断是不是数据库里已经存在的用户
                if (_dbContext.SystemUser.Count(x => x.Phone == model.Phone) > 0)
                {
                    //已经是数据库里的用户就添加督导员身份
                    var ent = _dbContext.SystemUser.FirstOrDefault(x => x.Phone == model.Phone && x.IsDeleted == 0);
                    ent.SystemRoleUuid = ent.SystemRoleUuid.TrimEnd(',') + ",c678d6e6-1c04-47d3-8660-2e4457504ee9";
                    ent.RealName = model.RealName;
                    ent.Sex = model.Sex;
                    ent.GrabageRoomId = model.garbageRoomUuid;
                    var entitys = _dbContext.GrabageRoom.FirstOrDefault(x => x.GarbageRoomUuid == model.garbageRoomUuid);
                    ent.VillageId = entitys.VillageId;
                    if (model.InTime != "")
                    {
                        ent.InTime = DateTime.Parse(model.InTime).ToString("yyyy-MM-dd");//入职时间
                    }
                    else
                    {
                        ent.InTime = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    ent.ZaiGang = model.ZaiGang;
                    _dbContext.SaveChanges();
                }
                else
                {
                    if (_dbContext.SystemUser.Where(x => x.IsDeleted == 0).Count(x => x.Phone == model.Phone) > 0)
                    {
                        response.SetFailed("该用户已存在");
                        return Ok(response);
                    }
                    var entity = new HaikanRefuseClassification.Api.Entities.SystemUser();
                    entity.SystemUserUuid = Guid.NewGuid();
                    entity.RealName = model.RealName;//督导员姓名        
                    entity.Sex = model.Sex;
                    entity.Phone = model.Phone;
                    entity.GrabageRoomId = model.garbageRoomUuid;
                    if (model.InTime != "")
                    {
                        entity.InTime = DateTime.Parse(model.InTime).ToString("yyyy-MM-dd");//入职时间
                    }
                    else
                    {
                        entity.InTime = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    entity.AddPeople = AuthContextService.CurrentUser.DisplayName;//添加人
                    entity.SystemRoleUuid = "c678d6e6-1c04-47d3-8660-2e4457504ee9";
                    entity.UserType = 5;
                    entity.ZaiGang = model.ZaiGang;//在职默认1在岗
                    entity.IsDeleted = 0;
                    var entitys = _dbContext.GrabageRoom.FirstOrDefault(x => x.GarbageRoomUuid == model.garbageRoomUuid);
                    entity.VillageId = entitys.VillageId;
                    entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd");
                    _dbContext.SystemUser.Add(entity);
                    _dbContext.SaveChanges();
                }
                response.SetSuccess("添加成功");
                return Ok(response);
            }
        }
        /// <summary>
        /// 编辑督导员管理(保存)
        /// </summary>
        /// <param name="guid">申请惟一编码</param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        public IActionResult Edit(Guid guid)
        {
            using (_dbContext)
            {
                var query = from s in _dbContext.SystemUser
                            join g in _dbContext.GrabageRoom
                            on s.GrabageRoomId equals g.GarbageRoomUuid
                            where s.SystemUserUuid == guid
                            select new
                            {
                                s.RealName,
                                s.Phone,
                                s.Sex,
                                s.InTime,
                                s.ZaiGang,
                                g.Ljname,
                                garbageRoomUuid = s.GrabageRoomId,
                                s.SystemUserUuid
                            };
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(query.ToList());
                return Ok(response);
            }
        }
        /// <summary>
        /// 保存编辑后的督导员管理(保存)
        /// </summary>
        /// <param name="model">申请视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Edit(SupervisorViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            using (_dbContext)
            {
                var entity = _dbContext.SystemUser.FirstOrDefault(x => x.SystemUserUuid == model.SystemUserUuid);
                entity.RealName = model.RealName;//督导员姓名    
                entity.Sex = model.Sex;//性别
                entity.Phone = model.Phone;//联系方式
                entity.GrabageRoomId = model.garbageRoomUuid;
                if (model.InTime != null)
                {
                    entity.InTime = DateTime.Parse(model.InTime).ToString("yyyy-MM-dd");//入职时间
                }
                else
                {
                    entity.InTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
                var entitys = _dbContext.GrabageRoom.FirstOrDefault(x => x.GarbageRoomUuid == model.garbageRoomUuid);
                entity.VillageId = entitys.VillageId;
                entity.ZaiGang = model.ZaiGang;//是否在职
                //entity.SystemRoleUuid = model.SystemRoleUUid;
                _dbContext.SaveChanges();
                return Ok(response);
            }
        }
        /// <summary>
        /// 督导员管理详情
        /// </summary>
        /// <param name="guid">申请惟一编码</param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        public IActionResult Detail(Guid guid)
        {
            using (_dbContext)
            {
                var query = (from s in _dbContext.SystemUser
                             join g in _dbContext.GrabageRoom
                             on s.GrabageRoomId equals g.GarbageRoomUuid
                             where s.SystemUserUuid == guid
                             select new
                             {
                                 realName = s.RealName,
                                 phone = s.Phone,
                                 sex = s.Sex == "1" ? "男" : "女",
                                 inTime = s.InTime,
                                 zaiGang = s.ZaiGang == "1" ? "在岗" : "离岗",
                                 garbageRoomUuid = s.GrabageRoomId,
                                 g.Ljname
                             }).ToList();
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(query);
                return Ok(response);
            }
        }
        //获取督导员下拉框
        [HttpPost]
        public IActionResult Huoqupeople()
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var query = _dbContext.SystemUser.Where(x => x.SystemRoleUuid.Contains(AuthContextService.CurrentUser.DDY)).Where(x => x.IsDeleted == 0).ToList();
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
                var sql = string.Format("UPDATE SystemUser SET IsDeleted=@IsDeleted  WHERE SystemUserUuid IN ({0})", parameterNames);
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
        /// 导入用户信息
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
                string uploadtitle = "督导员信息导入" + DateTime.Now.ToString("yyyyMMddHHmmss");
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
                        if (!dt.Columns.Contains("督导员姓名"))
                        {
                            response.SetFailed("无‘督导员姓名’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("性别"))
                        {
                            response.SetFailed("无‘性别’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("联系方式"))
                        {
                            response.SetFailed("无‘联系方式’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("负责厢房"))
                        {
                            response.SetFailed("无‘负责厢房’列");
                            return Ok(response);
                        }
                        //if (!dt.Columns.Contains("入职时间"))
                        //{
                        //entity.AddPeople = AuthContextService.CurrentUser.DisplayName;//添加人
                        //entity.SystemRoleUuid = "c678d6e6-1c04-47d3-8660-2e4457504ee9";
                        //    response.SetFailed("无‘入职时间’列");
                        //    return Ok(response);
                        //}
                        if (!dt.Columns.Contains("是否在职"))
                        {
                            response.SetFailed("无‘是否在职’列");
                            return Ok(response);
                        }

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {

                            var entity = new SystemUser();
                            entity.SystemUserUuid = Guid.NewGuid();
                            if (!string.IsNullOrEmpty(dt.Rows[i]["督导员姓名"].ToString().Trim()))
                            {
                                var name = dt.Rows[i]["督导员姓名"].ToString().Trim();
                                var room = dt.Rows[i]["负责厢房"].ToString().Trim();
                                var users = _dbContext.GrabageRoom.FirstOrDefault(x => x.Ljname == room && x.IsDelete == "0");
                                if (users==null)
                                {
                                    responsemsgrepeat += "<p style='color:orange'>" + "第" + (i + 2) + "行负责厢房不存在" + "</p></br>";
                                    repeatcount++;
                                    continue;
                                }
                                else
                                {
                                    var user = _dbContext.SystemUser.Where(x => x.IsDeleted == 0).FirstOrDefault(x => x.RealName == name && x.VillageId == users.VillageId);

                                    if (user == null)
                                    {
                                        entity.RealName = dt.Rows[i]["督导员姓名"].ToString().Trim();
                                    }
                                    else
                                    {
                                        responsemsgrepeat += "<p style='color:orange'>" + "第" + (i + 2) + "行督导员姓名已存在" + "</p></br>";
                                        repeatcount++;
                                        continue;
                                    }
                                }
                                

                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行督导员姓名为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["负责厢房"].ToString().Trim()))
                            {
                                var a = dt.Rows[i]["负责厢房"].ToString().Trim();
                                var user = _dbContext.GrabageRoom.FirstOrDefault(x => x.Ljname == a && x.IsDelete == "0");

                                if (user == null)
                                {
                                    responsemsgrepeat += "<p style='color:orange'>" + "第" + (i + 2) + "行负责厢房不存在" + "</p></br>";
                                    repeatcount++;
                                    continue;
                                }
                                else
                                {
                                    entity.GrabageRoomId = user.GarbageRoomUuid;
                                    entity.VillageId = user.VillageId;
                                }

                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行负责厢房为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["性别"].ToString()))
                            {
                                var sex = dt.Rows[i]["性别"].ToString();
                                if (dt.Rows[i]["性别"].ToString() == "男")
                                {
                                    entity.Sex = "1";
                                }
                                else if (dt.Rows[i]["性别"].ToString() == "女")
                                {
                                    entity.Sex = "0";
                                }
                                else
                                {
                                    responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行性别不正确" + "</p></br>";
                                    defaultcount++;
                                    continue;
                                }
                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行性别为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["联系方式"].ToString()))
                            {
                                Regex reg = new Regex("^[1][3,4,5,7,8][0-9]{9}$");
                                if (reg.IsMatch(dt.Rows[i]["联系方式"].ToString()))
                                {
                                    entity.Phone = dt.Rows[i]["联系方式"].ToString();
                                }
                                else
                                {
                                    responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行联系方式格式不正确" + "</p></br>";
                                    defaultcount++;
                                    continue;
                                }
                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行联系方式为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["入职时间"].ToString()))
                            {
                                //string[] time = dt.Rows[i]["入职时间"].ToString().Split("-");
                                //entity.InTime = time[2] + "-" + time[1] + "-" + time[0];
                                //entity.InTime = dt.Rows[i]["入职时间"].ToString();
                                Regex reg = new Regex("^((((1[6-9]|[2-9]\\d)\\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\\d|3[01]))|(((1[6-9]|[2-9]\\d)\\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\\d|30))|(((1[6-9]|[2-9]\\d)\\d{2})-0?2-(0?[1-9]|1\\d|2[0-8]))|(((1[6-9]|[2-9]\\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$");
                                if (reg.IsMatch(dt.Rows[i]["入职时间"].ToString()))
                                {
                                    entity.InTime = dt.Rows[i]["入职时间"].ToString();
                                }
                                else
                                {
                                    responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行入职时间不正确" + "</p></br>";
                                    defaultcount++;
                                    continue;
                                }
                            }
                            //else
                            //{
                            //    responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行入职时间为空" + "</p></br>";
                            //    defaultcount++;
                            //    continue;
                            //}
                            if (!string.IsNullOrEmpty(dt.Rows[i]["是否在职"].ToString()))
                            {

                                var zaigang = dt.Rows[i]["是否在职"].ToString();
                                if (dt.Rows[i]["是否在职"].ToString() == "是")
                                {
                                    entity.ZaiGang = "1";
                                }
                                else
                                {
                                    entity.ZaiGang = "0";
                                }
                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行是否在职为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            entity.UserType = 5;
                            entity.IsDeleted = 0;
                            entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd");
                            entity.AddPeople = AuthContextService.CurrentUser.DisplayName;//添加人
                            entity.SystemRoleUuid = "c678d6e6-1c04-47d3-8660-2e4457504ee9";
                            _dbContext.SystemUser.Add(entity);
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
        /// 督导员赋分设备获取信息
        /// </summary>
        /// <param name="uuid">设备UUID</param>
        /// <returns></returns>
        [HttpPost("{uuid}")]
        public IActionResult GetInfo(string uuid)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var entity = _dbContext.GrabageRoom.Where(x => x.Facilityuuid == uuid);
                if (entity == null)
                {
                    response.SetFailed("该设备请先绑定对应的垃圾厢房");
                    return Ok(response);
                }
                else
                {
                    response.SetSuccess();
                    response.SetData(entity);
                    return Ok(response);
                }
            }

        }

        /// <summary>
        /// 获取默认赋分分数 好
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetScore()
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var entity = _dbContext.ScoreSetting.FirstOrDefault(x => x.ScoreName == "好");
                response.SetData(entity);
            }
            return Ok(response);
        }

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
                var entities = _dbContext.SystemUser.Where(x => ids.IndexOf(x.SystemUserUuid.ToString()) >= 0).ToList();
                for (int i = 0; i < entities.Count; i++)
                {
                    var pata = _hostingEnvironment.WebRootPath + EWM.GetEWM2("d_" + entities[i].SystemUserUuid.ToString(), _hostingEnvironment, entities[i].RealName);
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
