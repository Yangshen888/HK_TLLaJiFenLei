namespace HaikanDemo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WorkTime")]
    public partial class WorkTime
    {
        [Key]
        public Guid WorkTimeUUID { get; set; }

        [StringLength(255)]
        public string StartTime { get; set; }

        [StringLength(255)]
        public string EndTime { get; set; }

        [StringLength(255)]
        public string AddTime { get; set; }

        [StringLength(255)]
        public string AddPeople { get; set; }

        public int? IsDeleted { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
    }
}
