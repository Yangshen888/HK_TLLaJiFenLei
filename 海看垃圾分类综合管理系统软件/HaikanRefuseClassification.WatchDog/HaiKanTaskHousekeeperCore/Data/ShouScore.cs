namespace HaiKanTaskHousekeeperCore.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ShouScore")]
    public partial class ShouScore
    {
        [Key]
        [Column(Order = 0)]
        public Guid ShopUUID { get; set; }

        [StringLength(255)]
        public string ShopName { get; set; }

        [StringLength(255)]
        public string Shopowner { get; set; }

        [StringLength(255)]
        public string Phone { get; set; }

        [StringLength(255)]
        public string AddPeople { get; set; }

        [StringLength(255)]
        public string State { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [StringLength(255)]
        public string Lat { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        [StringLength(255)]
        public string Lon { get; set; }

        [StringLength(20)]
        public string Addtime { get; set; }

        public double? Score { get; set; }
    }
}
