namespace HaikanDemo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WingNoRecord")]
    public partial class WingNoRecord
    {
        public long? ID { get; set; }

        [StringLength(100)]
        public string times { get; set; }

        [Key]
        public Guid GarbageRoomUUID { get; set; }

        [StringLength(255)]
        public string LJName { get; set; }

        [StringLength(255)]
        public string VName { get; set; }

        public Guid? SystemUserUUID { get; set; }

        [StringLength(255)]
        public string ColckDate { get; set; }
    }
}
