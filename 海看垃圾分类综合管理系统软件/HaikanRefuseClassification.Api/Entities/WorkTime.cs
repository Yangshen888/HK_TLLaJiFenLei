using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class WorkTime
    {
        public Guid WorkTimeUuid { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string AddTime { get; set; }
        public string AddPeople { get; set; }
        public int? IsDeleted { get; set; }
        public int Id { get; set; }
    }
}
