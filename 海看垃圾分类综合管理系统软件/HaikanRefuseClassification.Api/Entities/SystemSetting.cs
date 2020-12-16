using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class SystemSetting
    {
        public Guid ClobalUuid { get; set; }
        public string ClobalName { get; set; }
        public string ClobalSuo { get; set; }
        public DateTime? AddTime { get; set; }
        public string AddPeople { get; set; }
        public int? IsDeleted { get; set; }
        public string GlobalLogo { get; set; }
    }
}
