namespace HaikanDemo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GrabageMonitoring")]
    public partial class GrabageMonitoring
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Key]
        public Guid GrabageMonitoringUUID { get; set; }

        [StringLength(255)]
        public string MonitoringNum { get; set; }

        public Guid? GarbageRoomUUID { get; set; }

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

        public int? RegionId { get; set; }

        [StringLength(255)]
        public string jiankongName { get; set; }

        [StringLength(255)]
        public string longitude { get; set; }

        [StringLength(255)]
        public string latitude { get; set; }

        public string VideoUrl { get; set; }

        public virtual GrabageRoom GrabageRoom { get; set; }

        public virtual Region_info Region_info { get; set; }
    }
}
