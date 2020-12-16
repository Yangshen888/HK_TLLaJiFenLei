using HaikanRefuseClassification.Api.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.RequestPayload.AttendanceManagement.WorkTime
{
    public class WorkTimeRequestPayload : RequestPayload
    {

        /// <summary>
        /// 是否已被删除
        /// </summary>
        public CommonEnum.IsDeleted IsDeleted { get; set; }
    }
}
