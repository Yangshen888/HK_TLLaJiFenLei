using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class Department
    {
        public Guid DepartmentUuid { get; set; }
        public string DepartmentName { get; set; }
        public string AddTime { get; set; }
        public string AddPeople { get; set; }
        public int? IsDeleted { get; set; }
        public int Id { get; set; }
    }
}
