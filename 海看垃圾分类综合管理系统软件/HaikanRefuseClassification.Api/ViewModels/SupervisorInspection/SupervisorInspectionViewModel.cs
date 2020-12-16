using HaikanRefuseClassification.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.ViewModels.SupervisorInspection
{
    public class SupervisorInspectionViewModel
    {

        public Guid AuditUuid { get; set; }

        public string GarbageSoring { get; set; }
        public string Picture { get; set; }
        public string AddTime { get; set; }
        public int IsDeleted { get; set; }
        public string AddPeople { get; set; }
        public Guid HomeAddressUUID { get; set; }
        public string Grade { get; set; }

 
    }

}
