namespace HaiKanTaskHousekeeperCore.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CarWeightView")]
    public partial class CarWeightView
    {
        public long? ID { get; set; }

        [StringLength(255)]
        public string CarNumber { get; set; }

        [StringLength(255)]
        public string GrabType { get; set; }

        [StringLength(255)]
        public string Street { get; set; }

        [Key]
        public Guid CarUUID { get; set; }

        [StringLength(16)]
        public string WTime { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Weight { get; set; }
    }
}
