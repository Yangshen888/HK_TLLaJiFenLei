namespace HaiKanTaskHousekeeperCore.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Response")]
    public partial class Response
    {
        [Key]
        public Guid ProblemID { get; set; }

        [StringLength(255)]
        public string Content { get; set; }

        [StringLength(255)]
        public string AddTime { get; set; }

        [StringLength(255)]
        public string AddPeople { get; set; }

        public Guid? SystemUserUUID { get; set; }

        [StringLength(255)]
        public string IsDelete { get; set; }
    }
}
