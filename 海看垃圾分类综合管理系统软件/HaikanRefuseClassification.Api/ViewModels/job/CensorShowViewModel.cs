using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.ViewModels.job
{
    public class CensorShowViewModel
    {
        public Guid AttendanceUuid { get; set; }
        public Guid? SystemUserUUID { get; set; }
        public string ColckDate { get; set; }
        public string AMStartTime { get; set; }
        public string AMStartPlace { get; set; }
        public string AMEndTime { get; set; }
        public string AMEndPlace { get; set; }
        public string StartState { get; set; }
        public string EndState { get; set; }
        public string IsDelete { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public Guid? GarbageRoomUUID { get; set; }
        public string Volunteerevaluation { get; set; }
        public string PMStartTime { get; set; }
        public string PMStartPlace { get; set; }
        public string PMEndTime { get; set; }
        public string PMEndPlace { get; set; }
        public string satend { get; set; }
    }
}
