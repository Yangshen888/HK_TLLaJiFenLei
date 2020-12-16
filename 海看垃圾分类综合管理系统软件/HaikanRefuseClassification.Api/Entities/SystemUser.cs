using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class SystemUser
    {
        public SystemUser()
        {
            Attendance = new HashSet<Attendance>();
            GrabageDisposalCopy1SupervisorUu = new HashSet<GrabageDisposalCopy1>();
            GrabageDisposalCopy1SystemUserUu = new HashSet<GrabageDisposalCopy1>();
            GrabageDisposalSupervisorUu = new HashSet<GrabageDisposal>();
            GrabageDisposalSystemUserUu = new HashSet<GrabageDisposal>();
            Message = new HashSet<Message>();
            QuestionPerson = new HashSet<QuestionPerson>();
            SupervisorsWorkOwner = new HashSet<SupervisorsWork>();
            SupervisorsWorkSupervisor = new HashSet<SupervisorsWork>();
            VolunteerServiceSupervisorUu = new HashSet<VolunteerService>();
            VolunteerServiceSystemUserUu = new HashSet<VolunteerService>();
            VolunteerYy = new HashSet<VolunteerYy>();
        }

        public Guid SystemUserUuid { get; set; }
        public string LoginName { get; set; }
        public string RealName { get; set; }
        public string UserIdCard { get; set; }
        public string PassWord { get; set; }
        public int? UserType { get; set; }
        public Guid? DepartmentUuid { get; set; }
        public string AddTime { get; set; }
        public string AddPeople { get; set; }
        public int? IsDeleted { get; set; }
        public string ManageDepartment { get; set; }
        public int Id { get; set; }
        public Guid? GrabageRoomId { get; set; }
        public Guid? VillageId { get; set; }
        public string ZaiGang { get; set; }
        public string Wechat { get; set; }
        public string Phone { get; set; }
        public string OldCard { get; set; }
        public string Sex { get; set; }
        public string InTime { get; set; }
        public string Types { get; set; }
        public string Address { get; set; }
        public string SystemRoleUuid { get; set; }
        public string ProblemContent { get; set; }
        public string ProblemType { get; set; }
        public string Ewm { get; set; }
        public string Remarks { get; set; }
        public Guid? ShopUuid { get; set; }
        public Guid? HomeAddressUuid { get; set; }
        public string IdcardMd5 { get; set; }
        public string Streets { get; set; }
        public string Community { get; set; }
        public string Biotope { get; set; }

        public virtual GrabageRoom GrabageRoom { get; set; }
        public virtual HomeAddress HomeAddressUu { get; set; }
        public virtual Shop ShopUu { get; set; }
        public virtual Village Village { get; set; }
        public virtual ICollection<Attendance> Attendance { get; set; }
        public virtual ICollection<GrabageDisposalCopy1> GrabageDisposalCopy1SupervisorUu { get; set; }
        public virtual ICollection<GrabageDisposalCopy1> GrabageDisposalCopy1SystemUserUu { get; set; }
        public virtual ICollection<GrabageDisposal> GrabageDisposalSupervisorUu { get; set; }
        public virtual ICollection<GrabageDisposal> GrabageDisposalSystemUserUu { get; set; }
        public virtual ICollection<Message> Message { get; set; }
        public virtual ICollection<QuestionPerson> QuestionPerson { get; set; }
        public virtual ICollection<SupervisorsWork> SupervisorsWorkOwner { get; set; }
        public virtual ICollection<SupervisorsWork> SupervisorsWorkSupervisor { get; set; }
        public virtual ICollection<VolunteerService> VolunteerServiceSupervisorUu { get; set; }
        public virtual ICollection<VolunteerService> VolunteerServiceSystemUserUu { get; set; }
        public virtual ICollection<VolunteerYy> VolunteerYy { get; set; }
    }
}
