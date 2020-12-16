using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class VolunteerService
    {
        public Guid VolunteerServiceUuid { get; set; }
        public int Id { get; set; }
        public Guid? SystemUserUuid { get; set; }
        public Guid? GrabageRoomId { get; set; }
        public string Intime { get; set; }
        public string Outtime { get; set; }
        public string Score { get; set; }
        public string AddPeople { get; set; }
        public Guid? VillageId { get; set; }
        public string IsDelete { get; set; }
        public Guid? SupervisorUuid { get; set; }
        public string ScoreAddtime { get; set; }

        public virtual GrabageRoom GrabageRoom { get; set; }
        public virtual SystemUser SupervisorUu { get; set; }
        public virtual SystemUser SystemUserUu { get; set; }
        public virtual Village Village { get; set; }
    }
}
