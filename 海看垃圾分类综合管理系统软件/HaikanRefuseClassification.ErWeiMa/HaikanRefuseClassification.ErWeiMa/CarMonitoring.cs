namespace HaikanDemo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CarMonitoring")]
    public partial class CarMonitoring
    {
        [Key]
        public Guid CarMonitoringUUID { get; set; }

        [StringLength(255)]
        public string MonitoringNum { get; set; }

        public Guid? CarUUID { get; set; }

        [StringLength(255)]
        public string PalyType { get; set; }

        [StringLength(255)]
        public string SvrIp { get; set; }

        [StringLength(255)]
        public string SvrPort { get; set; }

        [StringLength(255)]
        public string appkey { get; set; }

        [StringLength(255)]
        public string appSecret { get; set; }

        [StringLength(255)]
        public string time { get; set; }

        [StringLength(255)]
        public string timeSecret { get; set; }

        [StringLength(255)]
        public string httpsflag { get; set; }

        [StringLength(255)]
        public string CamList { get; set; }

        [StringLength(255)]
        public string remark { get; set; }

        [StringLength(255)]
        public string AddTime { get; set; }

        public int? IsDelete { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public virtual Car Car { get; set; }
    }
}
