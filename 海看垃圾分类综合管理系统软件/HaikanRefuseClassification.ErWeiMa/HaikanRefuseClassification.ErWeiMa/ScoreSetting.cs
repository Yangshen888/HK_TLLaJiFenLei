namespace HaikanDemo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ScoreSetting")]
    public partial class ScoreSetting
    {
        [Key]
        public Guid ScoreUUID { get; set; }

        [StringLength(255)]
        public string ScoreName { get; set; }

        public int? Integral { get; set; }

        [StringLength(255)]
        public string Addpeople { get; set; }

        [StringLength(255)]
        public string AddTime { get; set; }
    }
}
