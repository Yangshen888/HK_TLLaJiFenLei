using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class HomeUser
    {
        public Guid HomeUserUuid { get; set; }
        public string LoginName { get; set; }
        public string RealName { get; set; }
        public string UserIdCard { get; set; }
        public string PassWord { get; set; }
        public int? UserType { get; set; }
        public string AddTime { get; set; }
        public string AddPeople { get; set; }
        public int? IsDeleted { get; set; }
        public int Id { get; set; }
        public string ZaiGang { get; set; }
        public string Wechat { get; set; }
        public string Phone { get; set; }
        public string Sex { get; set; }
        public string SystemRoleUuid { get; set; }
        public Guid? ShopUuid { get; set; }
        public Guid? HomeAddressUuid { get; set; }
    }
}
