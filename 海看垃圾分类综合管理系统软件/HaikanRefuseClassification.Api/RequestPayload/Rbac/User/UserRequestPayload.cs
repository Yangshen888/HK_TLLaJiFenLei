﻿using HaikanRefuseClassification.Api.Entities;
using HaikanRefuseClassification.Api.Entities.Enums;

namespace HaikanRefuseClassification.Api.RequestPayload.Rbac.User
{
    /// <summary>
    /// 
    /// </summary>
    public class UserRequestPayload : RequestPayload
    {
        /// <summary>
        /// 是否已被删除
        /// </summary>
        public CommonEnum.IsDeleted IsDeleted { get; set; }
        /// <summary>
        /// 用户状态
        /// </summary>
       // public UserStatus Status { get; set; }
    }
}
