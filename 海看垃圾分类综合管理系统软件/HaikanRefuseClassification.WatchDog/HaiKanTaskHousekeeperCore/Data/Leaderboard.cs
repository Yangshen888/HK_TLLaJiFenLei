namespace HaiKanTaskHousekeeperCore.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Leaderboard")]
    public partial class Leaderboard
    {
        public long? ID { get; set; }

        [Key]
        public Guid SystemUserUUID { get; set; }

        public Guid? HomeAddressUUID { get; set; }

        [StringLength(255)]
        public string LoginName { get; set; }

        public string Address { get; set; }

        [StringLength(255)]
        public string town { get; set; }

        [StringLength(255)]
        public string ccmmunity { get; set; }

        public int? zyzsc { get; set; }
    }
}
