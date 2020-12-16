using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class GrabageDisposalCopy1
    {
        public Guid GarbageDisposalUuid { get; set; }
        public int Id { get; set; }
        public Guid? GrabageRoomId { get; set; }
        public Guid? SystemUserUuid { get; set; }
        public string AddTime { get; set; }
        public Guid? ScoreSettingUuid { get; set; }
        public string IsDelete { get; set; }
        public string GrabType { get; set; }
        public Guid? SupervisorUuid { get; set; }
        public string ScoreAddtime { get; set; }

        public virtual GrabageRoom GrabageRoom { get; set; }
        public virtual ScoreSetting ScoreSettingUu { get; set; }
        public virtual SystemUser SupervisorUu { get; set; }
        public virtual SystemUser SystemUserUu { get; set; }
    }
}
