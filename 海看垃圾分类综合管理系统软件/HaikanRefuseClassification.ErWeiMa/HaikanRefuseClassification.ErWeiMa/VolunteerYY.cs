namespace HaikanDemo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VolunteerYY")]
    public partial class VolunteerYY
    {
        [Key]
        public Guid VolunteerYYUUID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public Guid? VolunteerUUid { get; set; }

        public Guid? GrabRoomUUid { get; set; }

        [StringLength(20)]
        public string AddTime { get; set; }

        [StringLength(255)]
        public string AddPeople { get; set; }

        [StringLength(20)]
        public string StartTime { get; set; }

        [StringLength(255)]
        public string AP { get; set; }

        public virtual GrabageRoom GrabageRoom { get; set; }

        public virtual SystemUser SystemUser { get; set; }
    }
}
