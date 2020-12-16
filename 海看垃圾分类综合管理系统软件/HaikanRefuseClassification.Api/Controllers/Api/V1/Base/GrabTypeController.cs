using AutoMapper;
using HaikanRefuseClassification.Api.Entities;
using HaikanRefuseClassification.Api.Entities.Enums;
using HaikanRefuseClassification.Api.Extensions;
using HaikanRefuseClassification.Api.Models.Response;
using HaikanRefuseClassification.Api.RequestPayload.Base;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HaikanRefuseClassification.Api.Extensions;
using HaikanRefuseClassification.Api.Extensions.AuthContext;
using HaikanRefuseClassification.Api.Extensions.CustomException;

namespace HaikanRefuseClassification.Api.Controllers.Api.V1.Base
{
    [Route("api/v1/base/[controller]/[action]")]
    [ApiController]
    public class GrabTypeController : ControllerBase
    {
        private readonly RefuseClassificationContext _dbContext;
        private readonly IMapper _mapper;
        private IHostingEnvironment _hostingEnvironment;

        public GrabTypeController(RefuseClassificationContext dbContext, IMapper mapper, IHostingEnvironment hostingEnvironment)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }
      
        [HttpPost]
        [CustomAuthorize]
        public IActionResult List(GrabTypeRequestRayload payload)
        {
            using (_dbContext)
            {
                var query = from cd in _dbContext.GrabType
                            where cd.IsDelete == "0"
                            select new
                            {
                                cd.GrabTypeUuid,
                                cd.GrabName,//名
                                cd.Id,
                                cd.AddPeopel,
                                cd.AddTime,
                                cd.Type,//类型
                            };
                //垃圾名
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.GrabName.Contains(payload.Kw));
                }
                //垃圾类型
                if (!string.IsNullOrEmpty(payload.kw1))
                {
                    query = query.Where(x => x.Type.Contains(payload.kw1));
                }
                if (payload.FirstSort != null)
                {
                    query = query.OrderByDescending(x => x.AddTime);
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
        /// 添加垃圾(保存)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize]
        [ProducesResponseType(200)]
        public IActionResult Create(dynamic model)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var entity = new HaikanRefuseClassification.Api.Entities.GrabType();
                entity.GrabTypeUuid = Guid.NewGuid();
                entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                entity.AddPeopel = AuthContextService.CurrentUser.DisplayName;
                entity.GrabName = model.name;
                entity.Type = model.type;
                entity.IsDelete = "0";
                _dbContext.GrabType.Add(entity);
                _dbContext.SaveChanges();
                response.SetSuccess();
                return Ok(response);
            }
        }
        /// <summary>
        /// 保存编辑后的住户信息管理(保存)
        /// </summary>
        /// <param name="model">申请视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize]
        [ProducesResponseType(200)]
        public IActionResult Edit(dynamic model)
        {
            Guid uid = model.grabTypeUuid;
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            using (_dbContext)
            {
                var entity = _dbContext.GrabType.FirstOrDefault(x => x.GrabTypeUuid == uid);
                entity.GrabName = model.name;
                entity.Type = model.type;
                _dbContext.SaveChanges();
                return Ok(response);
            }
        }

        /// <summary>
        /// 删除单个角色
        /// </summary>
        /// <param name="ids">角色ID,多个以逗号分隔</param>
        /// <returns></returns>
        [HttpGet("{ids}")]
        [CustomAuthorize]
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
        [HttpGet("{guid}")]
        public IActionResult GetEditData(string guid)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (!string.IsNullOrEmpty(guid))
            {
                using (_dbContext)
                {
                    var entity = (from a in _dbContext.GrabType
                                  where a.GrabTypeUuid == Guid.Parse(guid)
                                  select new
                                  {
                                      a.GrabTypeUuid,
                                      name = a.GrabName,
                                      type = a.Type
                                  }).ToList();
                    response.SetData(entity);
                    return Ok(response);
                }
            }
            else
                return null;
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
                var sql = string.Format("UPDATE grabtype SET IsDelete=@IsDelete WHERE grabtypeuuid IN ({0})", parameterNames);
                parameters.Add(new SqlParameter("@IsDelete", (int)isDeleted));
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
        [CustomAuthorize]
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
        /// app垃圾分类查询
        /// </summary>
        /// <param name="dd"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetListGroupByType(dynamic dd)
        {
            string kw = dd.kw;
            using (_dbContext)
            {
                var query1 = from cd in _dbContext.GrabType
                             where cd.IsDelete == "0" && cd.Type.Contains("回收")
                             select new
                             {
                                 cd.GrabName,//名
                             };
                var query2 = from cd in _dbContext.GrabType
                             where cd.IsDelete == "0" && cd.Type.Contains("有害")
                             select new
                             {
                                 cd.GrabName,//名
                             };
                var query3 = from cd in _dbContext.GrabType
                             where cd.IsDelete == "0" && cd.Type.Contains("易腐")
                             select new
                             {
                                 cd.GrabName,//名
                             };
                var query4 = from cd in _dbContext.GrabType
                             where cd.IsDelete == "0" && cd.Type.Contains("其他")
                             select new
                             {
                                 cd.GrabName,//名
                             };
                //垃圾名
                List<dynamic> data = new List<dynamic>();
                if (!string.IsNullOrEmpty(kw))
                {
                    data.Add(query1.Where(x => x.GrabName.Contains(kw)).ToList());
                    data.Add(query2.Where(x => x.GrabName.Contains(kw)).ToList());
                    data.Add(query3.Where(x => x.GrabName.Contains(kw)).ToList());
                    data.Add(query4.Where(x => x.GrabName.Contains(kw)).ToList());
                }
                else  
                {
                    data.Add(query1.ToList());
                    data.Add(query2.ToList());
                    data.Add(query3.ToList());
                    data.Add(query4.ToList());
                }
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(data);
                return Ok(response);
            }
        }


        /// <summary>
        /// 导入
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize]
        public IActionResult shopInfoImport(IFormFile excelfile)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                DateTime beginTime = DateTime.Now;

                string sWebRootFolder = _hostingEnvironment.WebRootPath + "\\UploadFiles\\ImportUserInfoExcel";


                //var schoolinfo = _dbContext.SchoolInforManagement.AsQueryable();
                string uploadtitle = "垃圾分类导入" + DateTime.Now.ToString("yyyyMMddHHmmss");
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
                        if (!dt.Columns.Contains("垃圾名称"))
                        {
                            response.SetFailed("无‘垃圾名称’列");
                            return Ok(response);
                        }

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {

                            var entity = new GrabType();
                            entity.GrabTypeUuid = Guid.NewGuid();
                            if (!string.IsNullOrEmpty(dt.Rows[i]["垃圾名称"].ToString()))
                            {
                                    entity.GrabName = dt.Rows[i]["垃圾名称"].ToString();
                                

                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行垃圾名称为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["垃圾类型"].ToString()))
                            {
                                string type = dt.Rows[i]["垃圾类型"].ToString();
                                if (type!="易腐垃圾"&&type!="其他垃圾"&&type!="可回收物"&&type!="有害垃圾")
                                {
                                    responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行垃圾类型不正确" + "</p></br>";
                                    defaultcount++;
                                    continue;
                                }
                                else
                                {
                                    entity.Type = dt.Rows[i]["垃圾类型"].ToString();
                                }
                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行垃圾类型为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            entity.AddPeopel = "超级管理员";
                            entity.IsDelete = "0";
                            entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                            _dbContext.GrabType.Add(entity);
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