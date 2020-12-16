using HaikanRefuseClassification.Api.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.RequestPayload.Material
{
    public class ConsumableRequestPayload : RequestPayload
    {
        /// <summary>
        /// 是否已被删除
        /// </summary>
        public CommonEnum.IsDeleted IsDeleted { get; set; }
        public string materialType { get; set; }
        public string materialName { get; set; }
        public string auditState { get; set; }
        public string[] time { get; set; }
    }
}
