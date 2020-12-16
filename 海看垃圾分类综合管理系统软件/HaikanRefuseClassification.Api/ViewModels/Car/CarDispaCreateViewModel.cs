using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.ViewModels.Car
{
    public class CarDispaCreateViewModel
    {
        
        public Guid? SystemUserUuid { get; set; }
        public string ApplyTime { get; set; }
        public Guid? DepartmentUuid { get; set; }
        public string ApplyReason { get; set; }
        public string AddPeople { get; set; }
    }
}
