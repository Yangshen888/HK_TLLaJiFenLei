namespace HaikanDemo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RecycleInfo")]
    public partial class RecycleInfo
    {
        public long? ID { get; set; }

        [Key]
        public Guid GarbageRoomUUID { get; set; }

        [StringLength(255)]
        public string LJName { get; set; }

        [StringLength(255)]
        public string VName { get; set; }

        [StringLength(255)]
        public string Towns { get; set; }

        [StringLength(255)]
        public string Lon { get; set; }

        [StringLength(255)]
        public string Lat { get; set; }

        public Guid? GrabageRoomID { get; set; }

        [StringLength(255)]
        public string Type { get; set; }
    }
}
