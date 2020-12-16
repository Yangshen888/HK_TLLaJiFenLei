using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class DeviceToCar
    {
        public Guid DeviceToCarUuid { get; set; }
        public string Imei { get; set; }
        public Guid? CarUuid { get; set; }
        public string AddTime { get; set; }
        public string AddPerson { get; set; }
        public int Id { get; set; }

        public virtual Car CarUu { get; set; }
    }
}
