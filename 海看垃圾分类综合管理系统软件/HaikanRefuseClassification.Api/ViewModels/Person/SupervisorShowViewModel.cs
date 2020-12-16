using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.ViewModels.Person
{
    public class SupervisorShowViewModel
    {
        public Guid SupervisorUuid { get; set; }
        public string RealName { get; set; }
        public string Sex { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string AddPeople { get; set; }
        public string AddTime { get; set; }
        public string ZaiGang { get; set; }
        public string LoginName { get; set; }
        public string PassWord { get; set; }
        public string InTime { get; set; }
        public string SystemRoleUUid { get; set; }
    }
}
