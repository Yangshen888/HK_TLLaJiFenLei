namespace HaikanDemo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GrabageDisposal")]
    public partial class GrabageDisposal
    {
        [Key]
        public Guid GarbageDisposalUUID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public Guid? GrabageRoomID { get; set; }

        public Guid? SystemUserUUID { get; set; }

        [StringLength(20)]
        public string AddTime { get; set; }

        public Guid? ScoreSettingUUid { get; set; }

        [StringLength(255)]
        public string IsDelete { get; set; }

        [StringLength(20)]
        public string GrabType { get; set; }

        public Guid? SupervisorUUID { get; set; }

        [StringLength(255)]
        public string ScoreAddtime { get; set; }

        public Guid? HomeAddressUUID { get; set; }

        [StringLength(20)]
        public string IsScore { get; set; }

        [StringLength(255)]
        public string MarkType { get; set; }
    }
}
