using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class SupervisorInspection
    {
        public int Id { get; set; }
        public Guid AuditUuid { get; set; }
        public string GarbageSoring { get; set; }
        public string Picture { get; set; }
        public string AddTime { get; set; }
        public string AddPeople { get; set; }
        public Guid HomeAddressUuid { get; set; }
        public int? IsDeleted { get; set; }
        public string Grade { get; set; }

        public virtual HomeAddress HomeAddressUu { get; set; }
    }
}
