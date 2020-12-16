using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class PerishRubbishView
    {
        public string Type { get; set; }
        public string Weight { get; set; }
        public Guid GrabageWeighingRecordUuid { get; set; }
        public Guid? GrabageRoomId { get; set; }
        public string Ljname { get; set; }
        public string Towns { get; set; }
        public string Vname { get; set; }
    }
}
