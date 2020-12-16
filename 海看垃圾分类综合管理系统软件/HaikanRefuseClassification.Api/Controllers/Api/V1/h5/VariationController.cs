using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaikanRefuseClassification.Api.Entities;
using HaikanRefuseClassification.Api.Extensions;
using HaikanRefuseClassification.Api.ViewModels.h5;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HaikanRefuseClassification.Api.Controllers.Api.V1.h5
{
    [Route("api/v1/h5/[controller]/[action]")]
    [ApiController]
    public class VariationController : ControllerBase
    {
        private readonly RefuseClassificationContext _dbContext;
        private readonly IMapper _mapper;
        public VariationController(RefuseClassificationContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult login(login model)
        {
            using (_dbContext)
            {
                var response = ResponseModelFactory.CreateResultInstance;
                //var query = _dbContext.SystemUser.Where();
                return Ok(response);
            }
        }
    }
}
