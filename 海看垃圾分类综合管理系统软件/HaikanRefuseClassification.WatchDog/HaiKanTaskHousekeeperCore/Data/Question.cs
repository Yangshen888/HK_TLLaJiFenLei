namespace HaiKanTaskHousekeeperCore.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Question")]
    public partial class Question
    {
        [Key]
        public Guid QuestionUUID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(255)]
        public string QueType { get; set; }

        public Guid? QueRoomID { get; set; }

        [StringLength(255)]
        public string AddPeople { get; set; }

        [StringLength(255)]
        public string AddTime { get; set; }

        [StringLength(255)]
        public string IsDelete { get; set; }

        public Guid? CarUUID { get; set; }

        [StringLength(255)]
        public string Remarks { get; set; }

        public Guid? VillageUUID { get; set; }

        [StringLength(255)]
        public string estimate { get; set; }

        [StringLength(100)]
        public string esttime { get; set; }

        [StringLength(100)]
        public string estpeople { get; set; }

        public virtual Car Car { get; set; }

        public virtual GrabageRoom GrabageRoom { get; set; }

        public virtual Village Village { get; set; }
    }
}
