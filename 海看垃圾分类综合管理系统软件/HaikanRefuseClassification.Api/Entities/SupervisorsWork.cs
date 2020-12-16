using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class SupervisorsWork
    {
        public Guid SupervisorWordUuid { get; set; }
        public int Id { get; set; }
        public Guid? SupervisorId { get; set; }
        public string GrabageType { get; set; }
        public DateTime? AddTime { get; set; }
        public Guid? OwnerId { get; set; }
        public string IsDelete { get; set; }

        public virtual SystemUser Owner { get; set; }
        public virtual SystemUser Supervisor { get; set; }
    }
}
