namespace HaiKanTaskHousekeeperCore.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SupervisorsWork")]
    public partial class SupervisorsWork
    {
        [Key]
        public Guid SupervisorWordUUID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public Guid? SupervisorID { get; set; }

        [StringLength(255)]
        public string GrabageType { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AddTime { get; set; }

        public Guid? OwnerID { get; set; }

        [StringLength(255)]
        public string IsDelete { get; set; }

        public virtual SystemUser SystemUser { get; set; }

        public virtual SystemUser SystemUser1 { get; set; }
    }
}
