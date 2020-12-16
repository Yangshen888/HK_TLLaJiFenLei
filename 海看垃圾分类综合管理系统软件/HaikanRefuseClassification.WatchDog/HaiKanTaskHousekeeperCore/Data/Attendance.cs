namespace HaiKanTaskHousekeeperCore.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Attendance")]
    public partial class Attendance
    {
        [Key]
        public Guid AttendanceUUID { get; set; }

        public Guid? SystemUserUUID { get; set; }

        [StringLength(255)]
        public string ColckDate { get; set; }

        [StringLength(255)]
        public string AMStartTime { get; set; }

        [StringLength(255)]
        public string AMStartPlace { get; set; }

        [StringLength(255)]
        public string AMEndTime { get; set; }

        [StringLength(255)]
        public string AMEndPlace { get; set; }

        public int? StartState { get; set; }

        public int? EndState { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(255)]
        public string IsDelete { get; set; }

        [StringLength(255)]
        public string Type { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        public Guid? GarbageRoomUUID { get; set; }

        [Column(" Volunteerevaluation ")]
        [StringLength(255)]
        public string C_Volunteerevaluation_ { get; set; }

        [StringLength(255)]
        public string PMStartTime { get; set; }

        [StringLength(255)]
        public string PMStartPlace { get; set; }

        [StringLength(255)]
        public string PMEndTime { get; set; }

        [StringLength(255)]
        public string PMEndPlace { get; set; }

        public virtual SystemUser SystemUser { get; set; }

        public virtual GrabageRoom GrabageRoom { get; set; }
    }
}
