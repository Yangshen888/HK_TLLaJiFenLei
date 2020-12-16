using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class Response
    {
        public Guid ProblemId { get; set; }
        public string Content { get; set; }
        public string AddTime { get; set; }
        public string AddPeople { get; set; }
        public Guid? SystemUserUuid { get; set; }
        public string IsDelete { get; set; }
    }
}
