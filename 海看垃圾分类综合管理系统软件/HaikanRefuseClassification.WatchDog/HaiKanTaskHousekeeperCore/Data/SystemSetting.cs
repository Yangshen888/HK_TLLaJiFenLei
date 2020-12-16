namespace HaiKanTaskHousekeeperCore.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SystemSetting")]
    public partial class SystemSetting
    {
        [Key]
        public Guid ClobalUuid { get; set; }

        [StringLength(255)]
        public string ClobalName { get; set; }

        [StringLength(255)]
        public string ClobalSuo { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AddTime { get; set; }

        [StringLength(255)]
        public string AddPeople { get; set; }

        public int? IsDeleted { get; set; }
    }
}
