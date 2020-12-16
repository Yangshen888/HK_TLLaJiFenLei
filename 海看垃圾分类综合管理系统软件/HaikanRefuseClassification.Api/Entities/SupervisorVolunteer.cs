using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class SupervisorVolunteer
    {
        public long? Id { get; set; }
        public Guid AttendanceUuid { get; set; }
        public string ColckDate { get; set; }
        public Guid? SystemUserUuid { get; set; }
        public string Name { get; set; }
        public string RealName { get; set; }
        public string Ap { get; set; }
    }
}
