namespace HaikanDemo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GoodsExchange")]
    public partial class GoodsExchange
    {
        [Key]
        public Guid StoreExchangeUUID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public Guid? ShopID { get; set; }

        public Guid? SystemUserUUID { get; set; }

        [StringLength(50)]
        public string ExchageTime { get; set; }

        [StringLength(255)]
        public string DeduceScore { get; set; }

        [StringLength(255)]
        public string IsDelete { get; set; }

        public Guid? GoodsID { get; set; }
    }
}
