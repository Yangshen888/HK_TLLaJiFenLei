namespace HaiKanTaskHousekeeperCore.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ScoreSetting")]
    public partial class ScoreSetting
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ScoreSetting()
        {
            GrabageDisposal = new HashSet<GrabageDisposal>();
            GrabageDisposal_copy1 = new HashSet<GrabageDisposal_copy1>();
        }

        [Key]
        public Guid ScoreUUID { get; set; }

        [StringLength(255)]
        public string ScoreName { get; set; }

        public int? Integral { get; set; }

        [StringLength(255)]
        public string Addpeople { get; set; }

        [StringLength(255)]
        public string AddTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GrabageDisposal> GrabageDisposal { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GrabageDisposal_copy1> GrabageDisposal_copy1 { get; set; }
    }
}
