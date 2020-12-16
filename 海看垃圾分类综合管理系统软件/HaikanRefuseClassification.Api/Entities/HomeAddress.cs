using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class HomeAddress
    {
        public HomeAddress()
        {
            DateScore = new HashSet<DateScore>();
            GrabageDisposal = new HashSet<GrabageDisposal>();
            SupervisorInspection = new HashSet<SupervisorInspection>();
            SystemUser = new HashSet<SystemUser>();
        }

        public Guid HomeAddressUuid { get; set; }
        public double? Score { get; set; }
        public string Address { get; set; }
        public int Id { get; set; }
        public string Addresscode { get; set; }
        public string Town { get; set; }
        public string Ccmmunity { get; set; }
        public string Squad { get; set; }
        public string Village { get; set; }
        public string Szone { get; set; }
        public string Street { get; set; }
        public string Door { get; set; }
        public string Resregion { get; set; }
        public string Building { get; set; }
        public string BuildingNum { get; set; }
        public string Unit { get; set; }
        public string Floor { get; set; }
        public string Room { get; set; }
        public int? RoomFloor { get; set; }
        public int Jan { get; set; }
        public int Feb { get; set; }
        public int Mar { get; set; }
        public int Apr { get; set; }
        public int May { get; set; }
        public int Jun { get; set; }
        public int Jul { get; set; }
        public int Aug { get; set; }
        public int Sep { get; set; }
        public int Oct { get; set; }
        public int Nov { get; set; }
        public int Dec { get; set; }
        public int LastDec { get; set; }

        public virtual ICollection<DateScore> DateScore { get; set; }
        public virtual ICollection<GrabageDisposal> GrabageDisposal { get; set; }
        public virtual ICollection<SupervisorInspection> SupervisorInspection { get; set; }
        public virtual ICollection<SystemUser> SystemUser { get; set; }
    }
}
