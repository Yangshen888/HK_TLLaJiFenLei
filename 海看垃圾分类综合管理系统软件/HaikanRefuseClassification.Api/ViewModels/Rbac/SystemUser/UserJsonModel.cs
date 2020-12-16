﻿using System;
using HaikanRefuseClassification.Api.Entities;
using HaikanRefuseClassification.Api.Entities.Enums;

namespace HaikanRefuseClassification.Api.ViewModels.Rbac.SystemUser
{
    /// <summary>
    /// 
    /// </summary>
    public class UserJsonModel
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
        public int? IsDeleted { get; set; }
    }
}
