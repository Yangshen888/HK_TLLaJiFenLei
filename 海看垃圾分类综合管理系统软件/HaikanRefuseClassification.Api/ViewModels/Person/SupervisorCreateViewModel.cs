using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.ViewModels.Person
{
    public class SupervisorCreateViewModel
    {
        public Guid SupervisorUuid { get; set; }
        public string Sname { get; set; }
        public string Vname { get; set; }
        public string Sex { get; set; }
        public string Phone { get; set; }
        public Guid GrabageRoomId { get; set; }
        public string Address { get; set; }
        public Guid VillageId { get; set; }     
        public string AddPeople { get; set; }
        public string AddTime { get; set; }
        public string ZaiGang { get; set; }
        public Guid GarbageRoomUuid { get; set; }
        public string LoginName { get; set; }
        public string PassWord { get; set; }
        public string Ljname { get; set; }
        public string InTime { get; set; }
    }
}
