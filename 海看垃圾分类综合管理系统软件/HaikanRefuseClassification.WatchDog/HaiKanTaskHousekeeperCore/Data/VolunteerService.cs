namespace HaiKanTaskHousekeeperCore.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VolunteerService")]
    public partial class VolunteerService
    {
        [Key]
        public Guid VolunteerServiceUUID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public Guid? SystemUserUUID { get; set; }

        public Guid? GrabageRoomID { get; set; }

        [StringLength(50)]
        public string INTime { get; set; }

        [StringLength(50)]
        public string OUTTime { get; set; }

        [StringLength(255)]
        public string Score { get; set; }

        [StringLength(255)]
        public string AddPeople { get; set; }

        public Guid? VillageID { get; set; }

        [StringLength(255)]
        public string IsDelete { get; set; }

        public Guid? SupervisorUUid { get; set; }

        [StringLength(255)]
        public string ScoreAddtime { get; set; }

        public virtual GrabageRoom GrabageRoom { get; set; }

        public virtual SystemUser SystemUser { get; set; }

        public virtual SystemUser SystemUser1 { get; set; }

        public virtual Village Village { get; set; }
    }
}
