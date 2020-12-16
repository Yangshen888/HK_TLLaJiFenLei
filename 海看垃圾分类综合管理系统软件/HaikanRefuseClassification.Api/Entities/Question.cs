using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class Question
    {
        public Guid QuestionUuid { get; set; }
        public int Id { get; set; }
        public string QueType { get; set; }
        public Guid? QueRoomId { get; set; }
        public string AddPeople { get; set; }
        public string AddTime { get; set; }
        public string IsDelete { get; set; }
        public Guid? CarUuid { get; set; }
        public string Remarks { get; set; }
        public Guid? VillageUuid { get; set; }
        public string Estimate { get; set; }
        public string Esttime { get; set; }
        public string Estpeople { get; set; }

        public virtual Car CarUu { get; set; }
        public virtual GrabageRoom QueRoom { get; set; }
        public virtual Village VillageUu { get; set; }
    }
}
