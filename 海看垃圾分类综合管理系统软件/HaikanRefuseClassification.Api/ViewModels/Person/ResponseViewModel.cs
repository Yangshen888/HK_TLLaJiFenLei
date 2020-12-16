using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.ViewModels.Person
{
    public class ResponseViewModel
    {
        public string AddTime { get; set; }
        public string AddPeople { get; set; }
        public Guid SystemUserUuid { get; set; }
        public string ProblemContent { get; set; }
        public string ProblemType { get; set; }
        public string LoginName { get; set; }
        public string Remarks { get; set; }
    }
}
