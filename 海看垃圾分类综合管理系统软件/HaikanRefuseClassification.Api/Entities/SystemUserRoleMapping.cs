﻿using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class SystemUserRoleMapping
    {
        public Guid SystemUserUuid { get; set; }
        public Guid SystemRoleUuid { get; set; }
        public string AddTime { get; set; }
        public string AddPeople { get; set; }
        public int Id { get; set; }
    }
}
