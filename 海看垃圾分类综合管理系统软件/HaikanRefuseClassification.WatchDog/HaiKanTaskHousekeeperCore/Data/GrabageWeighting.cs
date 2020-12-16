namespace HaiKanTaskHousekeeperCore.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GrabageWeighting")]
    public partial class GrabageWeighting
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GrabageWeighting()
        {
            RubbishSYRecord = new HashSet<RubbishSYRecord>();
        }

        [Key]
        public Guid GrabageWeighingRecordUUID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public Guid? GrabageRoomID { get; set; }

        [StringLength(255)]
        public string CarNumber { get; set; }

        [StringLength(255)]
        public string AddTime { get; set; }

        [StringLength(255)]
        public string Weight { get; set; }

        [StringLength(255)]
        public string Type { get; set; }

        [StringLength(255)]
        public string RecordType { get; set; }

        [StringLength(255)]
        public string IsDelete { get; set; }

        public Guid? CarUUID { get; set; }

        [StringLength(255)]
        public string CheckedUser { get; set; }

        public int? Grade { get; set; }

        public int? IsCheck { get; set; }

        [StringLength(255)]
        public string WeightTime { get; set; }

        public Guid? WeightUUID { get; set; }

        public virtual Car Car { get; set; }

        public virtual GrabageRoom GrabageRoom { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RubbishSYRecord> RubbishSYRecord { get; set; }
    }
}
