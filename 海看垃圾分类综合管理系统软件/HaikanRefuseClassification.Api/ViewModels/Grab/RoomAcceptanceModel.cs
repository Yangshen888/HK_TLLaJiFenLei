using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.ViewModels.Person
{
    public class RoomAcceptanceModel
    {
        public Guid GarbageRoomUUID { get; set; }
        public string CarNumber { get; set; }
        public string AddTime { get; set; }
        public string Weight { get; set; }
        public string Type { get; set; }
        public string Phone { get; set; }
        public string RecordType { get; set; }
        public string VName { get; set; }
        public string Address { get; set; }
        public string VillageID { get; set; }
    }
}
