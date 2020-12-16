using System;
using System.Collections.Generic;

using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaiKan3.Utils;
using HaikanRefuseClassification.Api.Entities;
using HaikanRefuseClassification.Api.Extensions;
using HaikanRefuseClassification.Api.Entities.Enums;
using HaikanRefuseClassification.Api.Extensions;
using HaikanRefuseClassification.Api.Extensions.AuthContext;
using HaikanRefuseClassification.Api.Models.Response;
using HaikanRefuseClassification.Api.RequestPayload.SupervisorInspection;
using HaikanRefuseClassification.Api.ViewModels.Person;
using HaikanRefuseClassification.Api.ViewModels.SupervisorInspection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HaikanRefuseClassification.Api.Controllers.Api.V1.SupervisorInspection
{
    [Route("api/v1/supervisorInspection/[controller]/[action]")]
    [ApiController]
    public class SupervisorInspectionController : ControllerBase
    {
        private readonly RefuseClassificationContext _dbContext;
        private readonly IMapper _mapper;
        private IHostingEnvironment _hostEnv;
        public SupervisorInspectionController(RefuseClassificationContext dbContext, IMapper mapper, IHostingEnvironment hostingEnvironment)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _hostEnv = hostingEnvironment;
        }
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult List(SupervisorInspectionRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = _dbContext.SupervisorInspection.OrderByDescending(p=>p.AddTime).Select(x => new
                {
                    x.HomeAddressUu.Addresscode,
                    x.Picture,
                    x.AuditUuid,
                    x.GarbageSoring,
                    x.AddPeople,
                    x.AddTime,
                    x.IsDeleted,
                    x.Grade

                });
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.AddPeople.Contains(payload.Kw.Trim()));
                }
                if (payload.IsDeleted > CommonEnum.IsDeleted.All)
                {
                    query = query.Where(x => x.IsDeleted == Convert.ToInt32(((CommonEnum.IsDeleted)payload.IsDeleted)));
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
        /// 保存提交的督检
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateSupervisor(SupervisorInspectionViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var entity=_mapper.Map<SupervisorInspectionViewModel, Entities.SupervisorInspection>(model);

                entity.AuditUuid = Guid.NewGuid();   //添加人Uuid
                entity.HomeAddressUuid = Guid.Parse(model.HomeAddressUUID.ToString());   //家庭码
                entity.AddTime = (DateTime.Now).ToString();  //添加时间
                entity.GarbageSoring = model.GarbageSoring;  //垃圾分类列
                entity.Picture = model.Picture;  //图片
                entity.Grade = model.Grade;
                entity.AddPeople = model.AddPeople; //添加人
              //entity.AddPeople = AuthContextService.CurrentUser.LoginName; //添加人
                entity.IsDeleted = 0;
                _dbContext.SupervisorInspection.Add(entity);
                _dbContext.SaveChanges();
                response.SetSuccess();
                return Ok(response);
            }
        }
        /// <summary>
        /// 编辑菜单
        /// </summary>
        /// <param name="guid">菜单ID</param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        public IActionResult Detail(Guid guid)
        {
            using (_dbContext)
            {
                var entity = from home in _dbContext.HomeAddress
                             join sup in _dbContext.SupervisorInspection
                             on home.HomeAddressUuid equals sup.HomeAddressUuid
                             select new
                             {
                                 sup.AddPeople,
                                 sup.AddTime,
                                 sup.HomeAddressUuid,
                                 sup.AuditUuid,
                                 home.Addresscode,
                                 sup.Picture,
                                 sup.GarbageSoring
                             };
                var model = entity.FirstOrDefault(x => x.AuditUuid == guid);
                var response = ResponseModelFactory.CreateInstance;
                //if (model.ParentGuid.HasValue)
                //{
                //    var parent = _dbContext.DncMenu.FirstOrDefault(x => x.Guid == entity.ParentGuid);
                //    if (parent != null)
                //    {
                //        model.ParentName = parent.Name;
                //    }
                //}
                response.SetData(model);
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
                var sql = string.Format("UPDATE SupervisorInspection SET IsDeleted=@IsDeleted WHERE AuditUuid IN ({0})", parameterNames);
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
        /// 图片上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> UpLoadAsync()
        {
            var response = ResponseModelFactory.CreateInstance;
            //var check = System.IO.File.Exists(filename);

            var files = Request.Form.Files;
            if (files == null || files.Count() <= 0)
            {
                response.SetFailed("请上图片文件");
                return Ok(response);
            }
            //var paths = new List<string>();
            //var dataPaths = new List<string>();
            var allowType = new string[] { "image/jpeg", "image/png","image/jpg" };
            var message = "";
            try
            {
                if (files.Any(c => allowType.Contains(c.ContentType)))
                {

                    //foreach (var file in files)
                    //{
                    if (files[0].Length > 1024 * 1024 * 5)
                    {
                        message += files[0].FileName + "大小超过5M";
                        response.SetFailed(message);
                        return Ok(response);
                    }
                    int index = files[0].FileName.LastIndexOf('.');
                    string prefix = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + "_" + Guid.NewGuid().ToString();
                    string fname = prefix + files[0].FileName.Substring(index);
                    string strpath = Path.Combine("UploadFiles/logo", fname);
                    string path = Path.Combine(_hostEnv.WebRootPath, strpath);
                    //System.IO.File.Create(path);
                    var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    await files[0].CopyToAsync(stream);

                    stream.Close();
                    bool toCompression = false;
                    string fname2 = "";
                    string strpath2 = "";

                    if (files[0].Length > (1024 * 1024 * 1))
                    {
                        fname2 = prefix + "_cp" + files[0].FileName.Substring(index);
                        strpath2 = Path.Combine("UploadFiles/logo", fname2);
                        var dFile = Path.Combine(_hostEnv.WebRootPath, strpath2);
                        var num = 1024 * 1024 * 1.0 / files[0].Length;
                        int flag = (int)(num * 100);
                        if (flag < 50)
                        {
                            flag = (int)(flag * 1.7);
                        }
                        toCompression = PhotoCompression.CompressImage(path, dFile, flag, 1024);
                        if (toCompression)
                        {
                            if (System.IO.File.Exists(path))
                            {
                                FileInfo info = new FileInfo(path);
                                if (info.Attributes != FileAttributes.ReadOnly)
                                {
                                    System.IO.File.Delete(path);

                                }

                            }
                        }
                    }
                    else if (files[0].Length > (1024 * 1024 * 0.5) && files[0].Length <= (1024 * 1024 * 1))
                    {
                        fname2 = prefix + "_cp" + files[0].FileName.Substring(index);
                        strpath2 = Path.Combine("UploadFiles/logo", fname2);
                        var dFile = Path.Combine(_hostEnv.WebRootPath, strpath2);

                        int flag = 90;

                        toCompression = PhotoCompression.CompressImage(path, dFile, flag, 500);
                        if (toCompression)
                        {
                            if (System.IO.File.Exists(path))
                            {
                                FileInfo info = new FileInfo(path);
                                if (info.Attributes != FileAttributes.ReadOnly)
                                {
                                    System.IO.File.Delete(path);

                                }

                            }
                        }
                    }
                    if (toCompression)
                    {
                        response.SetData(new { strpath = strpath2, fname = fname2 });

                    }
                    else
                    {
                        response.SetData(new { strpath, fname });
                    }

                }
                if (message.Length > 0)
                {
                    response.SetFailed(message);
                }
                //response.SetData(new { paths, dataPaths });
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
