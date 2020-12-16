using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.ViewModels.Base
{
    public class CollegeCreateViewModel
    {
        public Guid VillageUuid { get; set; }
        public string Vname { get; set; }
        public string Sname { get; set; }
        public string Ljname { get; set; }
        public string Address { get; set; }
        public string AddPeople { get; set; }
        public string AddTime { get; set; }
        public int? IsDelete { get; set; }
        public Guid GarbageRoomUuid { get; set; }
    }
}
