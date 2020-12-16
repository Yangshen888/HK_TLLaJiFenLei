namespace HaiKanTaskHousekeeperCore.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Goods
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Goods()
        {
            GoodsExchange = new HashSet<GoodsExchange>();
        }

        public Guid GoodsID { get; set; }

        [StringLength(255)]
        public string GName { get; set; }

        [StringLength(225)]
        public string Price { get; set; }

        [StringLength(20)]
        public string AddTime { get; set; }

        [StringLength(255)]
        public string AddPeople { get; set; }

        public Guid? ShopID { get; set; }

        [StringLength(255)]
        public string State { get; set; }

        public virtual Shop Shop { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GoodsExchange> GoodsExchange { get; set; }
    }
}
