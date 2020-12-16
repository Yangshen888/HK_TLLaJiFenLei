using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class VolunteerYy
    {
        public Guid VolunteerYyuuid { get; set; }
        public int Id { get; set; }
        public Guid? VolunteerUuid { get; set; }
        public Guid? GrabRoomUuid { get; set; }
        public string AddTime { get; set; }
        public string AddPeople { get; set; }
        public string StartTime { get; set; }
        public string Ap { get; set; }

        public virtual GrabageRoom GrabRoomUu { get; set; }
        public virtual SystemUser VolunteerUu { get; set; }
    }
}
