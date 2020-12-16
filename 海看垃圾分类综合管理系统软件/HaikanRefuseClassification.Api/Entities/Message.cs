using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class Message
    {
        public Guid MessageUuid { get; set; }
        public Guid? SystemUserUuid { get; set; }
        public string Message1 { get; set; }
        public string AddTime { get; set; }
        public int Id { get; set; }

        public virtual SystemUser SystemUserUu { get; set; }
    }
}
