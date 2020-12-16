using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;
using HaikanRefuseClassification.Api.Entities;
using HaikanRefuseClassification.Api.Extensions;
using HaikanRefuseClassification.Api.Entities.Enums;
using HaikanRefuseClassification.Api.Extensions;
using HaikanRefuseClassification.Api.Extensions.AuthContext;
using HaikanRefuseClassification.Api.Extensions.CustomException;
using HaikanRefuseClassification.Api.Models.Response;
using HaikanRefuseClassification.Api.RequestPayload.Person;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace HaikanRefuseClassification.Api.Controllers.Api.V1.Person
{
    [Route("api/v1/Person/[controller]/[action]")]
    [ApiController]
    [CustomAuthorize]
    public class GoodsController : ControllerBase
    {
        private readonly RefuseClassificationContext _dbContext;
        public GoodsController(RefuseClassificationContext db) {
            _dbContext = db;
        }
        /// <summary>
        /// 志愿者管理(模糊查询)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult List(GoodsRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = from x in  _dbContext.Goods
                            select new
                            {
                                x.Gname,
                                x.GoodsId,
                                x.Shop.ShopName,
                                x.Price,
                                x.AddTime,
                                x.Shop,
                                state=x.State=="1"?"已兑换":"未兑换"
                            };
                //姓名筛选
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.Gname.Contains(payload.Kw));
                }
                //联系方式筛选
                if (!string.IsNullOrEmpty(payload.Kw2))
                {
                    query = query.Where(x => x.Shop.ShopName.Contains(payload.Kw2));
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
        /// 添加商品(保存)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Create(Goods model)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                Goods g = new Goods();
                g.Gname = model.Gname;
                if (_dbContext.Goods.Count(x => x.Gname == model.Gname&&x.ShopId==model.ShopId) > 0)
                {
                    response.SetFailed("商品名已存在");
                    return Ok(response);
                }
                g.Price = model.Price;
                g.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                g.AddPeople = AuthContextService.CurrentUser.LoginName;
                g.ShopId = model.ShopId;
                g.GoodsId = Guid.NewGuid();
                g.State = "0";
                _dbContext.Goods.Add(g);
                _dbContext.SaveChanges();
                response.SetSuccess();
                return Ok(response);
            }
        }
        /// <summary>
        /// 编辑(保存)
        /// </summary>
        /// <param name="guid">申请惟一编码</param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        public IActionResult Edit(Guid guid)
        {
            using (_dbContext)
            {
                var entity = _dbContext.Goods.FirstOrDefault(x => x.GoodsId == guid);
                
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(entity);
                return Ok(response);
            }
        }
        /// <summary>
        /// 保存编辑后的商品(保存)
        /// </summary>
        /// <param name="model">申请视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Edit(Goods model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            using (_dbContext)
            {
                var entity = _dbContext.Goods.FirstOrDefault(x => x.GoodsId == model.GoodsId);
                entity.Gname = model.Gname;
                entity.Price = model.Price;
                entity.ShopId = model.ShopId;
                _dbContext.SaveChanges();
                return Ok(response);
            }
        }
        /// <summary>
        /// 下拉框绑定
        /// </summary>
        /// <returns></returns>
        public IActionResult ShopList() {
            using (_dbContext) {
                var entity=_dbContext.Shop.Where(x => x.IsDelete != "1").ToList();
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(entity);
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
                var sql = string.Format("UPDATE SystemUser SET IsDeleted=@IsDeleted WHERE SystemUserUuid IN ({0})", parameterNames);
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
    }
}