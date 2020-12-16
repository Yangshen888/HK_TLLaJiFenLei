using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.ViewModels.CarApp
{
    public class WeighDataModel
    {
        public Guid GrabageWeighingRecordUuid { get; set; }
        public Guid? GrabageRoomId { get; set; }
        public string Vname { get; set; }
        public List<Array> WeighData { get; set; }
        public string Ljname { get; set; }
        public Guid? VillageId { get; set; }
        public string Type { get; set; }
    }
}
