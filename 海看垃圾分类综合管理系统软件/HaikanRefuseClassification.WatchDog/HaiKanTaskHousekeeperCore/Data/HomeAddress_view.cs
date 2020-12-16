namespace HaiKanTaskHousekeeperCore.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class HomeAddress_view
    {
        public long? ID { get; set; }

        [StringLength(255)]
        public string LoginName { get; set; }

        [Key]
        [Column(Order = 0)]
        public Guid SystemUserUUID { get; set; }

        public string Address { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid HomeAddressUUID { get; set; }

        [StringLength(255)]
        public string town { get; set; }

        [StringLength(255)]
        public string ccmmunity { get; set; }

        public int? per { get; set; }
    }
}
