using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaikanRefuseClassification.Api.Entities;

namespace HaikanRefuseClassification.Api.ViewModels.CarApp
{
    public class DeviceCarModel
    {
        public DeviceCarModel() { }
        public DeviceCarModel(DeviceToCar car,string num)
        {
            this.DeviceToCarUuid = car.DeviceToCarUuid;
            this.CarUuid = car.CarUuid;
            this.Imei = car.Imei;
            this.CarNum = num;
        }
        public Guid DeviceToCarUuid { get; set; }
        public string Imei { get; set; }
        public Guid? CarUuid { get; set; }
        public string CarNum { get; set; }
        //public string CarType { get; set; }
    }
}
