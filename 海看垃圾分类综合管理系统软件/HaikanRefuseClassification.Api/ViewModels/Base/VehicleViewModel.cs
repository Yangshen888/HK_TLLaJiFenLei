using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.ViewModels.Base
{
    public class VehicleViewModel
    {
        public Guid CarUuid { get; set; }
        public string CarNum { get; set; }
        public string CarType { get; set; }
        public string AddPeople { get; set; }
        public string AddTime { get; set; }
        public int? IsDelete { get; set; }
        public string HolderId { get; set; }
        public string RegisterTime { get; set; }
        public string AwardTime { get; set; }
        public string Weight { get; set; }
        public string BalanceId { get; set; }
        public string Photo { get; set; }
        public string CameraId { get; set; }
        public Guid? RubbishTypeUuid { get; set; }
        public string Brand { get; set; }
        public string HolderPhone { get; set; }
        public string GrabType { get; set; }
        public string Company { get; set; }
        public string Street { get; set; }
        public string OnlyNum { get; set; }
    }
}
