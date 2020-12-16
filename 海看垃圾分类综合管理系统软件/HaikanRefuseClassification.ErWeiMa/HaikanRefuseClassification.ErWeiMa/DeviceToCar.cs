namespace HaikanDemo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DeviceToCar")]
    public partial class DeviceToCar
    {
        [Key]
        public Guid DeviceToCarUUID { get; set; }

        [StringLength(50)]
        public string IMEI { get; set; }

        public Guid? CarUUID { get; set; }

        [StringLength(100)]
        public string AddTime { get; set; }

        [StringLength(100)]
        public string AddPerson { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public virtual Car Car { get; set; }
    }
}
