namespace HaikanDemo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Supervisor_Volunteer
    {
        public long? ID { get; set; }

        [Key]
        public Guid AttendanceUUID { get; set; }

        [StringLength(255)]
        public string ColckDate { get; set; }

        public Guid? SystemUserUUID { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(255)]
        public string RealName { get; set; }

        [StringLength(255)]
        public string AP { get; set; }
    }
}
