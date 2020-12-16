using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class Attendance
    {
        public Guid AttendanceUuid { get; set; }
        public Guid? SystemUserUuid { get; set; }
        public string ColckDate { get; set; }
        public string AmstartTime { get; set; }
        public string AmstartPlace { get; set; }
        public string AmendTime { get; set; }
        public string AmendPlace { get; set; }
        public int? StartState { get; set; }
        public int? EndState { get; set; }
        public int Id { get; set; }
        public string IsDelete { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public Guid? GarbageRoomUuid { get; set; }
        public string Volunteerevaluation { get; set; }
        public string PmstartTime { get; set; }
        public string PmstartPlace { get; set; }
        public string PmendTime { get; set; }
        public string PmendPlace { get; set; }

        public virtual GrabageRoom GarbageRoomUu { get; set; }
        public virtual SystemUser SystemUserUu { get; set; }
    }
}
