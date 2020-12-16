namespace HaiKanTaskHousekeeperCore.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Shop")]
    public partial class Shop
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Shop()
        {
            Goods = new HashSet<Goods>();
            GoodsExchange = new HashSet<GoodsExchange>();
            SystemUser = new HashSet<SystemUser>();
        }

        [Key]
        public Guid ShopUUID { get; set; }

        [StringLength(255)]
        public string Shopowner { get; set; }

        [StringLength(255)]
        public string Phone { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        [StringLength(255)]
        public string State { get; set; }

        [StringLength(255)]
        public string AddPeople { get; set; }

        [StringLength(20)]
        public string Addtime { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(255)]
        public string IsDelete { get; set; }

        [StringLength(255)]
        public string ShopName { get; set; }

        [StringLength(255)]
        public string Lat { get; set; }

        [StringLength(255)]
        public string Lon { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Goods> Goods { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GoodsExchange> GoodsExchange { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SystemUser> SystemUser { get; set; }
    }
}
