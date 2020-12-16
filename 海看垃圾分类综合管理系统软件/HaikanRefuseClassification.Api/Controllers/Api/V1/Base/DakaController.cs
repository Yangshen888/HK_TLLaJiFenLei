using AutoMapper;
using HaikanRefuseClassification.Api.Entities;
using HaikanRefuseClassification.Api.Extensions;
using HaikanRefuseClassification.Api.RequestPayload.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.Controllers.Api.V1.Base
{
    [Route("api/v1/Base/[controller]/[action]")]
    [ApiController]
    public class DakaController : ControllerBase
    {
        private readonly RefuseClassificationContext _dbContext;
        private readonly IMapper _mapper;
        public DakaController(RefuseClassificationContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public IActionResult List(DakaRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = from g in _dbContext.GrabageRoom
                            join v in _dbContext.Village          //小区
                            on g.VillageId equals v.VillageUuid
                            where g.IsDelete == "0"
                            select new
                            {
                                g.GarbageRoomUuid,
                                g.Ljname,//垃圾箱
                                v.VillageUuid,        //社区
                                v.Vname,
                                Number = GetPeople(g.GarbageRoomUuid),
                            };
               var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(list, totalCount);
                return Ok(response);
            }
        }
        //异常打卡次数
        private string GetPeople(Guid id)
        {
            using (RefuseClassificationContext cc = new RefuseClassificationContext())
            {
                var query = (from a in cc.Attendance
                             where a.GarbageRoomUuid == id && a.IsDelete != "1" && (a.StartState == 0 || a.EndState == 0)
                             select a).ToList();
                return query.Count().ToString();
            }
        }
    }
}
