using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class Village
    {
        public Village()
        {
            GrabageRoom = new HashSet<GrabageRoom>();
            Question = new HashSet<Question>();
            RubbishSyrecord = new HashSet<RubbishSyrecord>();
            SystemUser = new HashSet<SystemUser>();
            VolunteerService = new HashSet<VolunteerService>();
        }

        public Guid VillageUuid { get; set; }
        public string Vname { get; set; }
        public string Address { get; set; }
        public string Addpeople { get; set; }
        public string AddTime { get; set; }
        public string IsDelete { get; set; }
        public int Id { get; set; }
        public string Towns { get; set; }
        public int Exchange { get; set; }
        public int DisNum { get; set; }

        public virtual ICollection<GrabageRoom> GrabageRoom { get; set; }
        public virtual ICollection<Question> Question { get; set; }
        public virtual ICollection<RubbishSyrecord> RubbishSyrecord { get; set; }
        public virtual ICollection<SystemUser> SystemUser { get; set; }
        public virtual ICollection<VolunteerService> VolunteerService { get; set; }
    }
}
