namespace HaikanDemo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GrabType")]
    public partial class GrabType
    {
        [Key]
        public Guid GrabTypeUUid { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(255)]
        public string GrabName { get; set; }

        [StringLength(255)]
        public string IsDelete { get; set; }

        [StringLength(20)]
        public string AddTime { get; set; }

        [StringLength(255)]
        public string AddPeopel { get; set; }

        [StringLength(255)]
        public string Type { get; set; }
    }
}
