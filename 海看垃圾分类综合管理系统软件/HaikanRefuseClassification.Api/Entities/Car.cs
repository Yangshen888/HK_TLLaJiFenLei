using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class Car
    {
        public Car()
        {
            CarMonitoring = new HashSet<CarMonitoring>();
            DeviceToCar = new HashSet<DeviceToCar>();
            GrabageWeightSon = new HashSet<GrabageWeightSon>();
            GrabageWeighting = new HashSet<GrabageWeighting>();
            Question = new HashSet<Question>();
        }

        public Guid CarUuid { get; set; }
        public string CarNum { get; set; }
        public string CarType { get; set; }
        public string AddPeople { get; set; }
        public string AddTime { get; set; }
        public int? IsDelete { get; set; }
        public int Id { get; set; }
        public string HolderId { get; set; }
        public string RegisterTime { get; set; }
        public string AwardTime { get; set; }
        public string Weight { get; set; }
        public string BalanceId { get; set; }
        public string Photo { get; set; }
        public string CameraId { get; set; }
        public string Brand { get; set; }
        public string HolderPhone { get; set; }
        public string GrabType { get; set; }
        public string Company { get; set; }
        public string Street { get; set; }
        public string OnlyNum { get; set; }
        public string GrabageRoomId { get; set; }

        public virtual ICollection<CarMonitoring> CarMonitoring { get; set; }
        public virtual ICollection<DeviceToCar> DeviceToCar { get; set; }
        public virtual ICollection<GrabageWeightSon> GrabageWeightSon { get; set; }
        public virtual ICollection<GrabageWeighting> GrabageWeighting { get; set; }
        public virtual ICollection<Question> Question { get; set; }
    }
}
