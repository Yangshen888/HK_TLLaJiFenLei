using AutoMapper;
using HaikanRefuseClassification.Api.Entities;
using HaikanRefuseClassification.Api.Entities.Enums;
using HaikanRefuseClassification.Api.Extensions;
using HaikanRefuseClassification.Api.Extensions.DataAccess;
using HaikanRefuseClassification.Api.RequestPayload.Rbac.Log;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.Controllers.Api.V1.Rbac
{
    [Route("api/v1/rbac/[controller]/[action]")]
    [ApiController]
    public class SystemjournalController : Controller
    {
        private readonly RefuseClassificationContext _dbContext;
        private readonly IMapper _mapper;
#pragma warning disable CS0618 // 类型或成员已过时
        private IHostingEnvironment _hostEnv;
#pragma warning restore CS0618 // 类型或成员已过时  
        public SystemjournalController(RefuseClassificationContext dbContext, IMapper mapper, IHostingEnvironment ihhostingEnvironment)

        {
            _dbContext = dbContext;
            _mapper = mapper;
            _hostEnv = ihhostingEnvironment;
        }
        #region 查看日志
        /// <summary>
        /// 查看日志
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult List(SerchLogRequestPayload payload)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var query = from l in _dbContext.SystemLog
                            where l.IsDelete != 1
                            orderby l.OperationTime descending
                            select new
                            {
                                l.AddPeople,
                                l.AddTime,
                                l.Id,
                                l.Ipaddress,
                                l.IsDelete,
                                OperationTime = l.OperationTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                                l.SystemLogUuid,
                                Type=l.Type=="1"? "开始查询商品兑换信息":l.Type=="2"? "开始执行数据操作":"结束执行数据操作",
                                l.UserName
                            };
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.UserName.Contains(payload.Kw));
                }
                if (!string.IsNullOrEmpty(payload.OperationTime))
                {
                    query = query.Where(x => x.OperationTime.ToString().Contains(payload.OperationTime));
                }
                if (payload.IsDeleted > CommonEnum.IsDeleted.All)
                {
                    query = query.Where(x => (CommonEnum.IsDeleted)x.IsDelete == payload.IsDeleted);
                }
                if (payload.FirstSort != null)
                {
                    query = query.OrderBy(payload.FirstSort.Field, payload.FirstSort.Direct == "DESC");
                }
                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();
                response.SetData(list, totalCount);
                return Ok(response);
            }
        }
        #endregion

        #region 根据日志id查看日志
        [HttpGet("{guid}")]
        public IActionResult GetByid(Guid guid)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var query = from l in _dbContext.SystemLog where l.SystemLogUuid==guid select new
                {
                    l.AddPeople,
                    l.AddTime,
                    l.Id,
                    l.Ipaddress,
                    l.IsDelete,
                    l.OperationContent,
                    OperationTime = l.OperationTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                    l.SystemLogUuid,
                    Type = l.Type == "1" ? "开始查询商品兑换信息" : l.Type == "2" ? "开始执行数据操作" : "结束执行数据操作",
                    l.UserName
                };
                response.SetData(query.ToList());
                return Ok(response);
            }
        }
        #endregion

    }
}
