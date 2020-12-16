namespace HaikanDemo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VehicleInfo")]
    public partial class VehicleInfo
    {
        [Key]
        [Column("VehicleInfo")]
        public Guid VehicleInfo1 { get; set; }

        public Guid? CarUUID { get; set; }

        [StringLength(255)]
        public string Addtime { get; set; }

        [StringLength(255)]
        public string Lon { get; set; }

        [StringLength(255)]
        public string Lat { get; set; }
    }
}
