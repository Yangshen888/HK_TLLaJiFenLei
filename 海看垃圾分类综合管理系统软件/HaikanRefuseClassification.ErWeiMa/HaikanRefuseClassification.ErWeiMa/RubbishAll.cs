namespace HaikanDemo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RubbishAll")]
    public partial class RubbishAll
    {
        public long? ID { get; set; }

        [Key]
        public Guid VillageUUID { get; set; }

        public double? Num { get; set; }

        public int? Ye { get; set; }

        public int? Mon { get; set; }
    }
}
