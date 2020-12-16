namespace HaikanDemo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Message")]
    public partial class Message
    {
        [Key]
        public Guid MessageUUID { get; set; }

        public Guid? SystemUserUUID { get; set; }

        [Column("Message")]
        [StringLength(255)]
        public string Message1 { get; set; }

        [StringLength(100)]
        public string AddTime { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public virtual SystemUser SystemUser { get; set; }
    }
}
