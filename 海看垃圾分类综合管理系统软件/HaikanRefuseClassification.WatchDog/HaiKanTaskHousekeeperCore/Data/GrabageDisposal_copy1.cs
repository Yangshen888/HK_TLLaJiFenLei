namespace HaiKanTaskHousekeeperCore.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GrabageDisposal_copy1
    {
        [Key]
        public Guid GarbageDisposalUUID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public Guid? GrabageRoomID { get; set; }

        public Guid? SystemUserUUID { get; set; }

        [StringLength(20)]
        public string AddTime { get; set; }

        public Guid? ScoreSettingUUid { get; set; }

        [StringLength(255)]
        public string IsDelete { get; set; }

        [StringLength(20)]
        public string GrabType { get; set; }

        public Guid? SupervisorUUID { get; set; }

        [StringLength(255)]
        public string ScoreAddtime { get; set; }

        public virtual GrabageRoom GrabageRoom { get; set; }

        public virtual ScoreSetting ScoreSetting { get; set; }

        public virtual SystemUser SystemUser { get; set; }

        public virtual SystemUser SystemUser1 { get; set; }
    }
}
