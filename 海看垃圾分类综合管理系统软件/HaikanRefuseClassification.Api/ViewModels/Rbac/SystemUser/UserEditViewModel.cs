﻿using System;
using System.Collections.Generic;
using HaikanRefuseClassification.Api.Entities;
using HaikanRefuseClassification.Api.Entities.Enums;

namespace HaikanRefuseClassification.Api.ViewModels.Rbac.SystemUser
{
    /// <summary>
    /// 
    /// </summary>
    public class UserEditViewModel
    {
        public Guid SystemUserUuid { get; set; }
        public string LoginName { get; set; }
        public string RealName { get; set; }
        public string UserIdCard { get; set; }
        public string PassWord { get; set; }
        public int? UserType { get; set; }
        public Guid? DepartmentUuid { get; set; }
        public string AddTime { get; set; }
        public string AddPeople { get; set; }
        public string Phone { get; set; }
        public int? IsDeleted { get; set; }
        public string OldCard { get; set; }
        public List<string> SystemRoleUuid { get; set; }
        public Guid? ShopUuid { get; set; }
        public Guid? VillageId { get; set; }
        public string Streets { get; set; }
        public string Community { get; set; }
        public string Biotope { get; set; }
    }
}
