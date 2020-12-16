namespace HaiKanTaskHousekeeperCore.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RecordTime")]
    public partial class RecordTime
    {
        [StringLength(100)]
        public string times { get; set; }

        [Key]
        public Guid GarbageRoomUUID { get; set; }
    }
}
