using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class GrabageRoom
    {
        public GrabageRoom()
        {
            Attendance = new HashSet<Attendance>();
            GrabageDisposal = new HashSet<GrabageDisposal>();
            GrabageDisposalCopy1 = new HashSet<GrabageDisposalCopy1>();
            GrabageWeightSon = new HashSet<GrabageWeightSon>();
            GrabageWeighting = new HashSet<GrabageWeighting>();
            Question = new HashSet<Question>();
            RubbishSyrecord = new HashSet<RubbishSyrecord>();
            SystemUser = new HashSet<SystemUser>();
            VolunteerService = new HashSet<VolunteerService>();
            VolunteerYy = new HashSet<VolunteerYy>();
        }

        public int Id { get; set; }
        public Guid GarbageRoomUuid { get; set; }
        public string Posititon { get; set; }
        public string Ljname { get; set; }
        public Guid? VillageId { get; set; }
        public string Ewm { get; set; }
        public string AddTime { get; set; }
        public string AddPeople { get; set; }
        public Guid? SystemUserUuid { get; set; }
        public string State { get; set; }
        public Guid? CarId { get; set; }
        public string RottenRubbishPercent { get; set; }
        public string IsDelete { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Lon { get; set; }
        public string Lat { get; set; }
        public string Address { get; set; }
        public string Towns { get; set; }
        public string Facilityuuid { get; set; }
        public string MonitoringNum { get; set; }
        public string WingId { get; set; }

        public virtual Village Village { get; set; }
        public virtual ICollection<Attendance> Attendance { get; set; }
        public virtual ICollection<GrabageDisposal> GrabageDisposal { get; set; }
        public virtual ICollection<GrabageDisposalCopy1> GrabageDisposalCopy1 { get; set; }
        public virtual ICollection<GrabageWeightSon> GrabageWeightSon { get; set; }
        public virtual ICollection<GrabageWeighting> GrabageWeighting { get; set; }
        public virtual ICollection<Question> Question { get; set; }
        public virtual ICollection<RubbishSyrecord> RubbishSyrecord { get; set; }
        public virtual ICollection<SystemUser> SystemUser { get; set; }
        public virtual ICollection<VolunteerService> VolunteerService { get; set; }
        public virtual ICollection<VolunteerYy> VolunteerYy { get; set; }
    }
}
