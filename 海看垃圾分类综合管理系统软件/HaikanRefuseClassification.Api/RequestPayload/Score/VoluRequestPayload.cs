using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaikanRefuseClassification.Api.Entities.Enums;

namespace HaikanRefuseClassification.Api.RequestPayload.Score
{
    public class VoluRequestPayload : RequestPayload
    {
        public string kw1 { get; set; }
        public string[] kw2 { get; set; }
        public string vuuid { get; set; }
        /// <summary>
        /// 是否已被删除
        /// </summary>
        public CommonEnum.IsDeleted IsDeleted { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public CommonEnum.Status Status { get; set; }
    }
}
