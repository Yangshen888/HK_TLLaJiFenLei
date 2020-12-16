namespace HaiKanTaskHousekeeperCore.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OWNSource")]
    public partial class OWNSource
    {
        public long? ID { get; set; }

        [Key]
        [Column(Order = 0)]
        public Guid HomeAddressUuid { get; set; }

        public string Address { get; set; }

        [StringLength(255)]
        public string Town { get; set; }

        [StringLength(255)]
        public string Ccmmunity { get; set; }

        [StringLength(255)]
        public string Resregion { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int used { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int alls { get; set; }

        public int? can { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LastScore { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NewScore { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int state { get; set; }
    }
}
