namespace HaikanDemo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RubbishSYRecord")]
    public partial class RubbishSYRecord
    {
        [Key]
        public Guid GarbageSYUUID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public Guid? GrabageRoomUUID { get; set; }

        public Guid? VillageUUID { get; set; }

        [StringLength(255)]
        public string SYTime { get; set; }

        [StringLength(255)]
        public string RubbishType { get; set; }

        [StringLength(255)]
        public string IsDelete { get; set; }

        [StringLength(255)]
        public string State { get; set; }

        [StringLength(255)]
        public string CorruptRubbishPercent { get; set; }

        public Guid? GrabageWeighingUUID { get; set; }

        public Guid? CarUUID { get; set; }

        public virtual GrabageRoom GrabageRoom { get; set; }

        public virtual GrabageWeighting GrabageWeighting { get; set; }

        public virtual Village Village { get; set; }
    }
}
