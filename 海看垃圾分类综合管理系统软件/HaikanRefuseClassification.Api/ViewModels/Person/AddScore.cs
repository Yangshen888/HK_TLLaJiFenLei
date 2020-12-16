using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.ViewModels.Person
{
    public class AddScore
    {
        public Guid SystemUserUuid { get; set; }
        public string LoginName { get; set; }
        public string RealName { get; set; }
        public string UserIdCard { get; set; }
        public int? UserType { get; set; }
        public Guid? DepartmentUuid { get; set; }
        public string AddTime { get; set; }
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
        public string Streets { get; set; }
        public string Types { get; set; }
        public string Address { get; set; }
        public string GrabageRoomNum { get; set; }
        public string UserUUid { get; set; }
        public string Score { get; set; }
        public string AddPeople { get; set; }
        public Guid? HomeAddressUuid { get; set; }

    }
}
