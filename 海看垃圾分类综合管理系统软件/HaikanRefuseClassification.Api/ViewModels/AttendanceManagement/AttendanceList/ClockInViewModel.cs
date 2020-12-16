using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.ViewModels.AttendanceManagement.AttendanceList
{
    public class ClockInViewModel
    {
        public string AmstartTime { get; set; }
        public string AmstartPlace { get; set; }
        public string AmendTime { get; set; }
        public string AmendPlace { get; set; }
        public Guid? UserUUID { get; set; }
        public string UserName { get; set; }
        public string Type { get; set; }
        public Guid? GarbageRoomUuid { get; set; }

        public string PmstartTime { get; set; }
        public string PmstartPlace { get; set; }
        public string PmendTime { get; set; }
        public string PmendPlace { get; set; }
        public string TimeState { get; set; }


    }
}
