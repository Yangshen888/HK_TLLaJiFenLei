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
using System;
using System.Collections.Generic;
using System.Data;

using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HaikanRefuseClassification.Api.Extensions;
using Microsoft.Data.SqlClient;

namespace HaikanRefuseClassification.Api.Controllers.Api.V1.Person
{
    [Route("api/v1/Person/[controller]/[action]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly RefuseClassificationContext _dbContext;
        private readonly IMapper _mapper;
        private readonly MdDesEncrypt MdDesEncrypt;
        //用来获取路径相关
        private IHostingEnvironment _hostingEnvironment;

        public ShopController(RefuseClassificationContext dbContext, IMapper mapper, IOptions<MdDesEncrypt> mdDesEncrypt, IHostingEnvironment hostingEnvironment)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            MdDesEncrypt = mdDesEncrypt.Value;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// 商店管理(模糊查询)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult List(HaikanRefuseClassification.Api.RequestPayload.RequestPayload payload)
        {
            using (_dbContext)
            {
                var query = from cd in _dbContext.ShouScore
                            select new
                            {
                                cd.ShopUuid,
                                cd.Shopowner,
                                cd.ShopName,
                                cd.Address,
                                state = cd.State == "1" ? "正在营业" : cd.State == "2" ? "暂停营业" : "",
                                cd.Phone,
                                cd.Id,
                                cd.Addtime,
                                Score = cd.Score == null ? 0 : cd.Score,
                            };

 
                //店名筛选
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.ShopName.Contains(payload.Kw));
                }
                query = query.OrderByDescending(x => x.Id);

                //分页
                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(list,totalCount); 
                return Ok(response);
            }
        }


            private string getGoods(Guid id)
        {
            using (RefuseClassificationContext cc = new RefuseClassificationContext())
            {
                string s = "";
                var req = cc.Goods.Where(x => x.ShopId == id && x.State != "1");
                if (req == null)
                    return "";
                foreach (var item in req)
                {
                    s += item.Gname + ",";
                }
                return s.TrimEnd(',');
            }

        }

        
        /// <summary>
        /// 判断积分
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult DbScore(dynamic model)
        {
            var response = ResponseModelFactory.CreateInstance;
            Guid uuid = model.shopUuid;
            using (_dbContext)
            {
                var query = _dbContext.ShouScore.FirstOrDefault(x => x.ShopUuid == uuid);
                response.SetData(query);
                return Ok(response);
            }

        }


        /// <summary>
        /// 扣除积分
        /// </summary>
        /// <param name="guid">申请惟一编码</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult EditShopScore(dynamic model)
        {
            var response = ResponseModelFactory.CreateInstance;
            var exchageTime = DateTime.Now.ToLocalTime().ToString();
            var StoreExchangeUuid = Guid.NewGuid();
            var score ="-" + model.score;
            Guid uuid = model.shopUuid;
            using (_dbContext)
            {
                var entity = new HaikanRefuseClassification.Api.Entities.GoodsExchange();
                entity.StoreExchangeUuid = StoreExchangeUuid;
                entity.ShopId = uuid;
                entity.ExchageTime = exchageTime;
                entity.DeduceScore = score;
                entity.IsDelete = "0";
                _dbContext.GoodsExchange.Add(entity);
                _dbContext.SaveChanges();
                response.SetSuccess();
                return Ok(response);
            }
        }

        // 添加商店
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Create(dynamic model)
        {
            var response = ResponseModelFactory.CreateInstance;
            string shopName = model.shopName;
            string shopowner = model.shopowner;
            string address = model.address;
            string state = model.state;
            string phone = model.phone;
            string lat = model.lat;
            string lon = model.lon;
            using (_dbContext)
            {
                if (_dbContext.Shop.Count(x => x.ShopName == shopName) > 0)
                {
                    response.SetFailed("商店名已存在");
                    return Ok(response);
                }
                var user = _dbContext.SystemUser.FirstOrDefault(x => x.Phone == phone);
                if (user==null)
                {
                    response.SetFailed("不存在此手机号的微信用户");
                    return Ok(response);
                }
                if (!user.SystemRoleUuid.Contains(AuthContextService.CurrentUser.SJ))
                {
                    user.SystemRoleUuid += "," + AuthContextService.CurrentUser.SJ;
                }
                var shop = _dbContext.Shop.FirstOrDefault(x=>x.Phone==user.Phone);
                if (shop!=null)
                {
                    response.SetFailed("此用户已注册商店");
                    return Ok(response);
                }
                var entity = new HaikanRefuseClassification.Api.Entities.Shop();
                entity.ShopUuid = Guid.NewGuid();
                user.ShopUuid = entity.ShopUuid;
                entity.ShopName = shopName;
                entity.Shopowner = shopowner;
                entity.Address = address;
                entity.State = state;
                entity.Phone = phone;
                entity.AddPeople = AuthContextService.CurrentUser.DisplayName;//添加人
                entity.Addtime = DateTime.Now.ToString("yyyy-MM-dd");
                entity.IsDelete = "0";
                entity.Lat = lat;
                entity.Lon = lon;
                _dbContext.Shop.Add(entity);
                _dbContext.SaveChanges();
                response.SetSuccess();
                return Ok(response);
            }
        }
        /// <summary>
        /// 获取编辑数据
        /// </summary>
        /// <param name="guid">申请惟一编码</param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        public IActionResult GetEdit(Guid guid)
        {
            using (_dbContext)
            {
                var entity = _dbContext.Shop.FirstOrDefault(x => x.ShopUuid == guid);
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(entity);
                return Ok(response);
            }
        }
        /// <summary>
        /// 保存编辑(保存)
        /// </summary>
        /// <param name="model">申请视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Edit(dynamic model)
        {
            string shopName = model.shopName;
            string uuid = model.shopUuid;
            string shopowner = model.shopowner;
            string address = model.address;
            string state = model.state;
            string phone = model.phone;
            string lat = model.lat;
            string lon = model.lon;
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            using (_dbContext)
            {
                
                var user = _dbContext.SystemUser.FirstOrDefault(x => x.Phone == phone);
                if (user == null)
                {
                    response.SetFailed("不存在此手机号的微信用户");
                    return Ok(response);
                }
                if (!user.SystemRoleUuid.Contains(AuthContextService.CurrentUser.SJ))
                {
                    user.SystemRoleUuid += "," + AuthContextService.CurrentUser.SJ;
                }
                var entity = _dbContext.Shop.FirstOrDefault(x => x.ShopUuid == Guid.Parse(uuid));
                if (entity.ShopName != shopName)
                {
                    if (_dbContext.Shop.Count(x => x.ShopName == shopName && x.ShopUuid.ToString()!= uuid) > 0)
                    {
                        response.SetFailed("商店名已存在");
                        return Ok(response);
                    }
                }
                var shop = _dbContext.Shop.FirstOrDefault(x => x.Phone == user.Phone && x.ShopUuid.ToString() != uuid);
                if (shop != null)
                {
                    response.SetFailed("此用户已注册商店");
                    return Ok(response);
                }
                entity.ShopName = shopName;
                entity.Shopowner = shopowner;
                entity.Address = address;
                entity.State = state;
                entity.Phone = phone;
                entity.Lat =lat ;
                entity.Lon = lon;
                _dbContext.SaveChanges();
                return Ok(response);
            }
        }
        /// <summary>
        /// 商店详情
        /// </summary>
        /// <param name="guid">申请惟一编码</param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        public IActionResult Detail(Guid guid)
        {
            using (_dbContext)
            {

                var query = (from cd in _dbContext.Shop
                             where cd.IsDelete != "1" && cd.ShopUuid == guid
                             select new
                             {
                                 cd.ShopUuid,
                                 cd.Shopowner,
                                 cd.ShopName,
                                 cd.Address,
                                 state = cd.State == "1" ? "正在营业" : cd.State == "2" ? "暂停营业" : "",
                                 cd.Phone,
                                 cd.Id,
                                 goods = getGoods(cd.ShopUuid),
                                 cd.Addtime,//
                                 cd.AddPeople,//
                                 cd.Lat,
                                 cd.Lon
                             }).ToList();
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
                var sql = string.Format("UPDATE shop SET IsDelete=@IsDelete WHERE shopUuid IN ({0})", parameterNames);
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

        [HttpPost]
        public IActionResult GoodsList(Guid id,RequestPayload.RequestPayload payload)
        {
            using (_dbContext) {
                var query = _dbContext.Goods.Where(x => x.ShopId == id);
                //分页
                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(list, totalCount);
                return Ok(response);
            }
        }



        /// <summary>
        /// 导入商店信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult shopImport(IFormFile excelfile)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                DateTime beginTime = DateTime.Now;

                string sWebRootFolder = _hostingEnvironment.WebRootPath + "\\UploadFiles\\ImportUserInfoExcel";
                string uploadtitle = "商店信息导入" + DateTime.Now.ToString("yyyyMMddHHmmss");
                string sFileName = $"{uploadtitle}.xlsx";
                FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
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
                        if (!dt.Columns.Contains("商店名称"))
                        {
                            response.SetFailed("无‘商店名称’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("商店地址"))
                        {
                            response.SetFailed("无‘商店地址’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("商店负责人"))
                        {
                            response.SetFailed("无‘商店负责人’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("联系方式"))
                        {
                            response.SetFailed("无‘联系方式’列");
                            return Ok(response);
                        }

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {

                            var entity = new Shop();
                            entity.ShopUuid = Guid.NewGuid();
                            bool check = false;
                            SystemUser su ;
                            if (!string.IsNullOrEmpty(dt.Rows[i]["商店名称"].ToString()))
                            {
                                var a = dt.Rows[i]["商店名称"].ToString();
                                var user = _dbContext.Shop.FirstOrDefault(x => x.ShopName == dt.Rows[i]["商店名称"].ToString());

                                if (user == null)
                                {
                                    entity.ShopName = dt.Rows[i]["商店名称"].ToString();
                                }
                                else
                                {
                                    responsemsgrepeat += "<p style='color:orange'>" + "第" + (i + 2) + "行商店名称已存在" + "</p></br>";
                                    repeatcount++;
                                    continue;
                                }

                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行商店名称为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["商店地址"].ToString()))
                            {
                                    entity.Address = dt.Rows[i]["商店地址"].ToString();
                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行商店地址为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["商店负责人"].ToString()))
                            {
                                    entity.Shopowner = dt.Rows[i]["商店负责人"].ToString();
                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行商店负责人为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["联系方式"].ToString()))
                            {
                                
                                entity.Phone = dt.Rows[i]["联系方式"].ToString();
                                su = _dbContext.SystemUser.FirstOrDefault(x => x.Phone == entity.Phone);
                                if (su == null)
                                {
                                    responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "不存在此手机号的微信用户" + "</p></br>";
                                    defaultcount++;
                                    continue;
                                }
                                else
                                {
                                    check = true;
                                }

                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行联系方式为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }

                            if (!string.IsNullOrEmpty(dt.Rows[i]["经度(最多精确到六位)"].ToString()))
                            {
                                if (double.Parse(dt.Rows[i]["经度(最多精确到六位)"].ToString()) > 180.999999)
                                {
                                    response.SetFailed("经度范围在-180.999999~180.999999");
                                    return Ok(response);
                                }
                                else
                                {
                                    entity.Lon = dt.Rows[i]["经度(最多精确到六位)"].ToString();
                                }

                            }

                            if (!string.IsNullOrEmpty(dt.Rows[i]["纬度(最多精确到六位)"].ToString()))
                            {
                                if (double.Parse(dt.Rows[i]["纬度(最多精确到六位)"].ToString()) > 180.999999)
                                {
                                    response.SetFailed("经度范围在-90.999999~90.999999");
                                    return Ok(response);
                                }
                                else
                                {
                                    entity.Lat = dt.Rows[i]["纬度(最多精确到六位)"].ToString();
                                }
                                
                            }
                            if (check)
                            {
                                if (!su.SystemRoleUuid.Contains(AuthContextService.CurrentUser.SJ))
                                {
                                    su.SystemRoleUuid += "," + AuthContextService.CurrentUser.SJ;
                                    
                                }

                            }
                            var shop = _dbContext.Shop.FirstOrDefault(x => x.Phone == entity.Phone);
                            if (shop != null)
                            {
                                response.SetFailed("此用户已注册商店");
                                return Ok(response);
                            }
                            su.ShopUuid = entity.ShopUuid;
                            _dbContext.Shop.Add(entity);
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