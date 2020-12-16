namespace HaikanDemo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Overallsituation")]
    public partial class Overallsituation
    {
        [Key]
        public Guid SetUUID { get; set; }

        [StringLength(255)]
        public string SetName { get; set; }

        public int? SetNumber { get; set; }

        public int HourScore { get; set; }
    }
}
