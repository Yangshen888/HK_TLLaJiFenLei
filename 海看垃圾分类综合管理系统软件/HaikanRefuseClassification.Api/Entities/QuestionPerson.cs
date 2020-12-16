using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class QuestionPerson
    {
        public Guid QuestionPersonUuid { get; set; }
        public Guid? HomeUserUuid { get; set; }
        public string ProblemType { get; set; }
        public string ProblemContent { get; set; }
        public string AddTime { get; set; }
        public string Remarks { get; set; }
        public string Name { get; set; }
        public string Estimate { get; set; }
        public string Esttime { get; set; }
        public string Estpeople { get; set; }
        public int Id { get; set; }

        public virtual SystemUser HomeUserUu { get; set; }
    }
}
