using HaikanRefuseClassification.Api.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.RequestPayload.SupervisorInspection
{
    public class SupervisorInspectionRequestPayload : RequestPayload
    {
        public CommonEnum.IsDeleted IsDeleted { get; set; }
    }
}
