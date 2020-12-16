using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class Driver
    {
        public Guid DriverUuid { get; set; }
        public string DriverName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string AddPeople { get; set; }
        public string AddTime { get; set; }
        public int? IsDelete { get; set; }
        public int Id { get; set; }
    }
}
