using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class CarMonitoring
    {
        public Guid CarMonitoringUuid { get; set; }
        public string MonitoringNum { get; set; }
        public Guid? CarUuid { get; set; }
        public string PalyType { get; set; }
        public string SvrPort { get; set; }
        public string Appkey { get; set; }
        public string AppSecret { get; set; }
        public string Time { get; set; }
        public string TimeSecret { get; set; }
        public string Httpsflag { get; set; }
        public string CamList { get; set; }
        public string Remark { get; set; }
        public string AddTime { get; set; }
        public int? IsDelete { get; set; }
        public int Id { get; set; }
        public string SvrIp { get; set; }
        public string Svrlp { get; set; }

        public virtual Car CarUu { get; set; }
    }
}
