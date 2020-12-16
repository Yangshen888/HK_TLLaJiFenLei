using HaikanRefuseClassification.Api.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.RequestPayload.AttendanceManagement.AttendanceList
{
    public class AttendanceListRequestPayload : RequestPayload
    {
        public string[] time { get; set; }
        public string DepartmentUuid { get; set; }
    }
}
