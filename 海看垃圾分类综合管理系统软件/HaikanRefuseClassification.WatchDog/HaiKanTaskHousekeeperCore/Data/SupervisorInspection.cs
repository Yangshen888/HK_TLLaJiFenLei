namespace HaiKanTaskHousekeeperCore.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SupervisorInspection")]
    public partial class SupervisorInspection
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Key]
        public Guid AuditUuid { get; set; }

        public string GarbageSoring { get; set; }

        public string Picture { get; set; }

        [StringLength(255)]
        public string AddTime { get; set; }

        [StringLength(255)]
        public string AddPeople { get; set; }

        public Guid HomeAddressUUID { get; set; }

        public int? IsDeleted { get; set; }

        [StringLength(255)]
        public string Grade { get; set; }

        public virtual HomeAddress HomeAddress { get; set; }
    }
}
